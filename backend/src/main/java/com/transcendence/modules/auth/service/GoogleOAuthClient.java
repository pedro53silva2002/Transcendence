package com.transcendence.modules.auth.service;

import com.transcendence.modules.auth.dtos.GoogleTokenResponse;
import com.transcendence.modules.auth.dtos.GoogleUserInfoResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Component;
import org.springframework.util.LinkedMultiValueMap;
import org.springframework.util.MultiValueMap;
import org.springframework.web.client.RestClient;
import org.springframework.web.client.RestClientResponseException;
import org.springframework.web.util.UriComponentsBuilder;

/**
 * HTTP client for Google's OAuth 2.0 / OpenID Connect endpoints.
 *
 * This class knows ONE thing: how to talk to Google. It does not know about
 * users, the database, JWTs, or the application's authentication policy.
 * Its only callers are AuthService.
 *
 * Reference: https://developers.google.com/identity/protocols/oauth2/web-server
 */
@Component
public class GoogleOAuthClient {

    private final RestClient restClient;
    private final String     clientId;
    private final String     clientSecret;
    private final String     redirectUri;
    private final String     authUri;
    private final String     tokenUri;
    private final String     userInfoUri;
    private final String     scopes;

    /**
     * Pulls the Google OAuth config from application.yml (google.oauth.*) and
     * builds a shared RestClient. RestClient.Builder is autoconfigured by
     * spring-boot-starter-web — no extra dependency needed.
     */
    public GoogleOAuthClient(
            RestClient.Builder restClientBuilder,
            @Value("${google.oauth.client-id}")     String clientId,
            @Value("${google.oauth.client-secret}") String clientSecret,
            @Value("${google.oauth.redirect-uri}")  String redirectUri,
            @Value("${google.oauth.auth-uri}")      String authUri,
            @Value("${google.oauth.token-uri}")     String tokenUri,
            @Value("${google.oauth.userinfo-uri}")  String userInfoUri,
            @Value("${google.oauth.scopes}")        String scopes) {
        this.restClient   = restClientBuilder.build();
        this.clientId     = clientId;
        this.clientSecret = clientSecret;
        this.redirectUri  = redirectUri;
        this.authUri      = authUri;
        this.tokenUri     = tokenUri;
        this.userInfoUri  = userInfoUri;
        this.scopes       = scopes;
    }

    /**
     * Build the URL the user's browser must visit to start Google consent.
     *
     * Implementation hint — append query params to authUri:
     *   client_id      = clientId
     *   redirect_uri   = redirectUri
     *   response_type  = "code"
     *   scope          = scopes (e.g. "openid email profile")
     *   state          = state         (CSRF nonce supplied by the caller)
     *   access_type    = "offline"     (so Google returns a refresh_token)
     *   prompt         = "consent"     (forces account chooser every time)
     *
     * Use UriComponentsBuilder.fromUriString(authUri).queryParam(...).build().toUriString()
     * so values are properly URL-encoded.
     *
     * Reference: https://developers.google.com/identity/protocols/oauth2/web-server#creatingclient
     *
     * @param state CSRF-protecting nonce (already stored in Redis by AuthService)
     * @return fully-formed Google authorization URL
     */
    public String buildAuthorizationUrl(String state) {
        return UriComponentsBuilder.fromUriString(authUri)
                .queryParam("client_id", clientId)
                .queryParam("redirect_uri", redirectUri)
                .queryParam("response_type", "code")
                .queryParam("scope", scopes)
                .queryParam("state", state)
                .queryParam("access_type", "offline")
                .queryParam("prompt", "consent")
                .build()
                .toUriString();
    }

    /**
     * Exchange the short-lived authorization code from Google's redirect for
     * tokens (access_token, id_token, refresh_token).
     *
     * Implementation hint:
     *   POST {tokenUri}
     *   Content-Type: application/x-www-form-urlencoded
     *   body params:
     *     code          = code
     *     client_id     = clientId
     *     client_secret = clientSecret
     *     redirect_uri  = redirectUri      (must EXACTLY match the one used in the auth URL)
     *     grant_type    = "authorization_code"
     *
     *   restClient.post().uri(tokenUri)
     *             .contentType(MediaType.APPLICATION_FORM_URLENCODED)
     *             .body(formParams)
     *             .retrieve()
     *             .body(GoogleTokenResponse.class);
     *
     * On HTTP 4xx → throw InvalidCredentialsException("google rejected the code")
     * On HTTP 5xx → throw ThirdPartyUnavailableException("google token endpoint")
     * (Note: those DomainException subclasses are package-private right now;
     * either make them public or fall back to IllegalStateException for the stub.)
     *
     * Reference: https://developers.google.com/identity/protocols/oauth2/web-server#exchange-authorization-code
     *
     * @param code the 'code' query parameter Google appended to the redirect
     * @return Google's token response
     */
    public GoogleTokenResponse exchangeCodeForTokens(String code) {
        MultiValueMap<String, String> form = new LinkedMultiValueMap<>();
        form.add("code", code);
        form.add("client_id", clientId);
        form.add("client_secret", clientSecret);
        form.add("redirect_uri", redirectUri);
        form.add("grant_type", "authorization_code");

        try {
            return restClient.post()
                    .uri(tokenUri)
                    .contentType(MediaType.APPLICATION_FORM_URLENCODED)
                    .body(form)
                    .retrieve()
                    .body(GoogleTokenResponse.class);
        }
        catch (RestClientResponseException ex) {
            if (ex.getStatusCode().is4xxClientError()) {
                throw new IllegalStateException("google rejected the code", ex);
            }
            if (ex.getStatusCode().is5xxServerError()) {
                throw new IllegalStateException("google token endpoint", ex);
            }
            throw ex;
        }
    }

    /**
     * Fetch the authenticated user's profile from Google's UserInfo endpoint.
     *
     * Implementation hint:
     *   GET {userInfoUri}
     *   Authorization: Bearer {accessToken}
     *
     *   restClient.get().uri(userInfoUri)
     *             .header(HttpHeaders.AUTHORIZATION, "Bearer " + accessToken)
     *             .retrieve()
     *             .body(GoogleUserInfoResponse.class);
     *
     * The 'sub' field in the response is Google's stable, never-changing user
     * id — store it as oauth_id. Never use 'email' as the OAuth identity key;
     * users can change their primary email at Google.
     *
     * Reference: https://developers.google.com/identity/openid-connect/openid-connect#obtainuserinfo
     *
     * @param accessToken the access_token returned by exchangeCodeForTokens()
     * @return Google's user profile
     */
    public GoogleUserInfoResponse fetchUserInfo(String accessToken) {
        return restClient.get()
                .uri(userInfoUri)
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + accessToken)
                .retrieve()
                .body(GoogleUserInfoResponse.class);
    }
}
