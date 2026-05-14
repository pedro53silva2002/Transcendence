package com.transcendence.modules.auth.router;

import com.transcendence.modules.auth.dtos.AuthResponseDto;
import com.transcendence.modules.auth.dtos.GoogleAuthUrlResponse;
import com.transcendence.modules.auth.service.AuthService;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.net.URI;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;

/**
 * HTTP layer for authentication endpoints.
 *
 * Responsibilities of this layer (and ONLY these):
 *   - parsing query/body parameters into typed values
 *   - validation that's purely syntactic (presence, format)
 *   - mapping success/error to HTTP status codes
 *   - choosing the response shape (JSON body, redirect, cookie)
 *
 * Not the router's job:
 *   - calling Google directly        (→ GoogleOAuthClient)
 *   - touching the database          (→ AuthUserModel)
 *   - generating JWTs                (→ JwtProvider via AuthService)
 *   - any business decisions         (→ AuthService)
 *
 * Domain exceptions thrown by AuthService propagate up to your
 * GlobalExceptionHandler, which renders them as JSON error responses.
 */
@RestController
@RequestMapping("/api/auth")
public class AuthController {

    private final AuthService authService;
    private final String      frontendSuccessUri;
    private final long        refreshTokenTtlDays;

    public AuthController(
            AuthService authService,
            @Value("${frontend.oauth-success-uri}") String frontendSuccessUri,
            @Value("${jwt.refresh-token-ttl-days}") long refreshTokenTtlDays) {
        this.authService        = authService;
        this.frontendSuccessUri = frontendSuccessUri;
        this.refreshTokenTtlDays = refreshTokenTtlDays;
    }

    /**
     * GET /api/auth/google/url
     *
     * The FE calls this on "Login with Google" click. The response carries
     * the Google consent URL; the FE does:
     *
     *   const res = await fetch('/api/auth/google/url').then(r => r.json());
     *   window.location.assign(res.authorizationUrl);
     *
     * No business logic in this method — just forward to the service.
     */
    @GetMapping("/google/url")
    public GoogleAuthUrlResponse startGoogleLogin() {
        return authService.startGoogleLogin();
    }

    /**
     * GET /api/auth/google/callback?code=...&state=...
     *
     * This is the redirect_uri registered with Google. When the user finishes
     * consent on Google's domain, the browser is sent here mid-navigation —
     * which means the response must be a REDIRECT, not a JSON body.
     *
     * Recommended strategy: redirect to the FE with tokens in the URL fragment.
     *
     * Implementation hint:
     *   1. AuthResponseDto auth = authService.handleGoogleCallback(code, state);
     *   2. String target = frontendSuccessUri
     *          + "#access_token="  + URLEncoder.encode(auth.accessToken(),  UTF_8)
     *          + "&refresh_token=" + URLEncoder.encode(auth.refreshToken(), UTF_8)
     *          + "&expires_in="    + auth.accessTokenExpiresInSeconds();
     *   3. return ResponseEntity.status(HttpStatus.FOUND)
     *                           .location(URI.create(target))
     *                           .build();
     *
     * Why the URL fragment (#...) and not query params (?...)?
     *   - fragments are NOT sent to the server in subsequent requests
     *   - most reverse proxies and access logs do NOT capture fragments
     *   - the FE reads them via window.location.hash and immediately scrubs
     *     them with history.replaceState(...)
     *
     * Alternative (more secure but more wiring): set access_token and
     * refresh_token as HttpOnly; Secure; SameSite=Lax cookies and redirect to
     * a plain FE URL. Requires JwtAuthFilter to also read the cookie when
     * the Authorization header is absent. Pick one strategy and stick with it.
     *
     * Any DomainException thrown by AuthService (invalid state, Google
     * rejected the code, unverified email) propagates to your global handler,
     * which should return an HTML error page or redirect to a FE error route
     * — NOT a JSON body, since the browser is mid-navigation.
     */
    @GetMapping("/google/callback")
    public ResponseEntity<Void> googleCallback(
            @RequestParam("code")  String code,
            @RequestParam("state") String state) {
        AuthResponseDto auth = authService.handleGoogleCallback(code, state);

        // Set HttpOnly cookies for access and refresh tokens; redirect to FE
        // without exposing tokens in the URL. Cookies: HttpOnly; Secure;
        // SameSite=Lax; path=/; maxAge as appropriate.
        var accessCookie = org.springframework.http.ResponseCookie.from("access_token", auth.accessToken())
                .httpOnly(true)
                .secure(true)
                .sameSite("Lax")
                .path("/")
                .maxAge(auth.accessTokenExpiresInSeconds())
                .build();

        long refreshMaxAge = Math.max(0, refreshTokenTtlDays * 24L * 60L * 60L);
        var refreshCookie = org.springframework.http.ResponseCookie.from("refresh_token", auth.refreshToken())
                .httpOnly(true)
                .secure(true)
                .sameSite("Lax")
                .path("/")
                .maxAge(refreshMaxAge)
                .build();

        return ResponseEntity.status(HttpStatus.FOUND)
                .header(org.springframework.http.HttpHeaders.SET_COOKIE, accessCookie.toString())
                .header(org.springframework.http.HttpHeaders.SET_COOKIE, refreshCookie.toString())
                .location(URI.create(frontendSuccessUri))
                .build();
    }
}
