package com.transcendence.modules.auth.service;

import com.transcendence.modules.auth.dtos.AuthResponseDto;
import com.transcendence.modules.auth.dtos.GoogleAuthUrlResponse;
import com.transcendence.modules.auth.dtos.GoogleUserInfoResponse;
import com.transcendence.modules.auth.dtos.PublicUserDto;
import com.transcendence.modules.auth.dtos.UserDto;
import com.transcendence.modules.auth.model.AuthUserModel;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Service;
import com.transcendence.common.services.jwtservice.JwtProvider;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.time.Duration;
import java.util.UUID;

/**
 * Orchestrates the OAuth login flow:
 *
 *   - generates the CSRF state and the Google authorization URL (startGoogleLogin)
 *   - on callback, validates state, exchanges the code, fetches the profile,
 *     upserts the user via AuthUserModel, and issues local JWTs (handleGoogleCallback)
 *
 * All HTTP calls to Google go through GoogleOAuthClient.
 * All database access goes through AuthUserModel.
 * AuthService itself contains only the business rules.
 */
@Service
public class AuthService {

    private static final Logger log = LoggerFactory.getLogger(AuthService.class);

    private final GoogleOAuthClient    googleOAuthClient;
    private final AuthUserModel        authUserModel;
    private final JwtProvider          jwtProvider;
    private final StringRedisTemplate  redisTemplate;
    private final long                 accessTokenTtlMinutes;

    public AuthService(
            GoogleOAuthClient googleOAuthClient,
            AuthUserModel authUserModel,
            JwtProvider jwtProvider,
            StringRedisTemplate redisTemplate,
            @Value("${jwt.access-token-ttl-minutes}") long accessTokenTtlMinutes) {
        this.googleOAuthClient     = googleOAuthClient;
        this.authUserModel         = authUserModel;
        this.jwtProvider           = jwtProvider;
        this.redisTemplate         = redisTemplate;
        this.accessTokenTtlMinutes = accessTokenTtlMinutes;
    }

    /**
     * Step 1 of the OAuth flow — the FE calls this when the user clicks
     * "Login with Google". We generate a CSRF nonce, persist it in Redis
     * with a short TTL, and return the Google consent URL.
     *
     * Implementation hint:
     *   1. String state = UUID.randomUUID().toString();
     *   2. redisTemplate.opsForValue().set(
     *          "oauth:state:" + state, "1", Duration.ofMinutes(10));
     *   3. String url = googleOAuthClient.buildAuthorizationUrl(state);
     *   4. return new GoogleAuthUrlResponse(url, state);
     *
     * @return the URL the FE should redirect the browser to + the state
     *         (state is exposed for debugging; the FE does not need to use it)
     */
    public GoogleAuthUrlResponse startGoogleLogin() {
        String state = UUID.randomUUID().toString();
        redisTemplate.opsForValue().set(
                "oauth:state:" + state,
                "1",
                Duration.ofMinutes(10));

        log.info("OAuth state created: {}", state);

        String url = googleOAuthClient.buildAuthorizationUrl(state);
        return new GoogleAuthUrlResponse(url, state);
    }

    /**
     * Step 2 of the OAuth flow — Google redirected the browser to our
     * /callback endpoint with ?code=...&state=.... The router calls this.
     *
     * Implementation sequence:
     *   1. Validate state — Boolean deleted = redisTemplate.delete("oauth:state:" + state);
     *      if Boolean.TRUE != deleted → throw InvalidTokenException("oauth state").
     *      Deleting (not just reading) makes the state single-use.
     *
     *   2. GoogleTokenResponse tokens = googleOAuthClient.exchangeCodeForTokens(code);
     *
     *   3. GoogleUserInfoResponse info = googleOAuthClient.fetchUserInfo(tokens.accessToken());
     *
     *   4. if (!Boolean.TRUE.equals(info.emailVerified()))
     *          throw InvalidCredentialsException("google email not verified");
     *
     *   5. Upsert via the model (no SQL knowledge here):
     *      UserDto user = authUserModel.findByOauthId("google", info.sub())
     *          .or(() -> authUserModel.findByEmail(info.email())
     *              .map(existing -> authUserModel.linkOauthToExistingUser(
     *                  existing.getId(), "google", info.sub())))
     *          .orElseGet(() -> authUserModel.create(
     *              info.email(),
     *              generateUniqueUsername(info),
     *              null,                       // OAuth user has no password
     *              info.name(),
     *              info.picture(),
     *              "google",
     *              info.sub()));
     *
     *   6. String access  = jwtProvider.generateAccessToken(user.getId(), "USER");
     *      String refresh = jwtProvider.generateRefreshToken();
     *      // persist refresh wherever you decide (Redis key or auth.refresh_tokens table)
     *
     *   7. return new AuthResponseDto(access, refresh,
     *                                 accessTokenTtlMinutes * 60,
     *                                 toPublic(user));
     *
     * @param code  Google's authorization code from the callback query string
     * @param state the state nonce echoed back by Google — must match Redis
     * @return tokens + the public-safe user payload
     */
    public AuthResponseDto handleGoogleCallback(String code, String state) {
        Boolean deleted = redisTemplate.delete("oauth:state:" + state);
        log.info("OAuth state validation for {} -> deleted={}", state, deleted);
        if (!Boolean.TRUE.equals(deleted)) {
            throw new IllegalStateException("invalid or expired oauth state");
        }

        var tokens = googleOAuthClient.exchangeCodeForTokens(code);
        GoogleUserInfoResponse info = googleOAuthClient.fetchUserInfo(tokens.accessToken());

        if (!Boolean.TRUE.equals(info.emailVerified())) {
            throw new IllegalStateException("google email not verified");
        }

        // Upsert policy: prefer existing OAuth-linked user; if the email already
        // exists as a local account (non-oauth), we reject and ask FE to show an
        // explicit error (no silent merge). Otherwise create a new OAuth user.
        UserDto user = authUserModel.findByOauthId("google", info.sub())
                .or(() -> {
                    var byEmail = authUserModel.findByEmail(info.email());
                    if (byEmail.isPresent()) {
                        String provider = byEmail.get().getOauthProvider();
                        // If provider is 'none' (local account), reject per policy.
                        if (provider == null || provider.equalsIgnoreCase("none")) {
                            throw new com.transcendence.modules.auth.exceptions.OAuthConflictException(
                                    "an account with this email already exists; please sign in with your existing credentials and link Google from your profile");
                        }
                        // provider indicates OAuth (e.g., 'google') — link and continue
                        UserDto linked = authUserModel.linkOauthToExistingUser(byEmail.get().getId(), "google", info.sub());
                        return java.util.Optional.of(linked);
                    }
                    return java.util.Optional.empty();
                })
                .orElseGet(() -> authUserModel.create(
                        info.email(),
                        generateUniqueUsername(info),
                        null,
                        info.name(),
                        info.picture(),
                        "google",
                        info.sub()));

        String access = jwtProvider.generateAccessToken(user.getId(), "USER");
        String refresh = jwtProvider.generateRefreshToken();

        return new AuthResponseDto(
                access,
                refresh,
                accessTokenTtlMinutes * 60,
                toPublic(user));
    }

    /**
     * Pick a username that doesn't collide with auth.users.username.
     *
     * Implementation hint:
     *   String base = info.email().split("@")[0]
     *                              .toLowerCase()
     *                              .replaceAll("[^a-z0-9_.-]", "");
     *   String candidate = base;
     *   int i = 1;
     *   while (authUserModel.existsByUsername(candidate)) {
     *       candidate = base + i++;
     *       if (i > 100) throw new ConflictException("could not pick username");
     *   }
     *   return candidate;
     *
     * The retry cap defends against pathological collision loops.
     *
     * @param info Google's profile payload
     * @return a username unique against the users table at this moment
     */
    private String generateUniqueUsername(GoogleUserInfoResponse info) {
        String base = info.email()
                .split("@")[0]
                .toLowerCase()
                .replaceAll("[^a-z0-9_.-]", "");

        String candidate = base;
        int i = 1;
        while (authUserModel.existsByUsername(candidate)) {
            candidate = base + i++;
            if (i > 100) {
                throw new IllegalStateException("could not pick username");
            }
        }
        return candidate;
    }

    /**
     * Strip internal fields (oauth_provider, oauth_id, timestamps) before
     * the DTO leaves the server. UserDto is for internal use; PublicUserDto
     * is what the FE sees.
     *
     * Implementation hint:
     *   return new PublicUserDto(
     *       u.getId(),
     *       u.getEmail(),
     *       u.getUsername(),
     *       u.getDisplayName(),
     *       u.getBio(),
     *       u.getProfilePhotoUrl());
     */
    private PublicUserDto toPublic(UserDto u) {
        return new PublicUserDto(
                u.getId(),
                u.getEmail(),
                u.getUsername(),
                u.getDisplayName(),
                u.getBio(),
                u.getProfilePhotoUrl());
    }
}
