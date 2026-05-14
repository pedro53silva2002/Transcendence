package com.transcendence.modules.auth.dtos;

/**
 * Response returned from GET /api/auth/google/url.
 *
 * The frontend reads 'authorizationUrl' and navigates the browser to it
 * (typically: window.location.assign(response.authorizationUrl)).
 *
 * 'state' is the CSRF-protection nonce that the backend has stored in Redis
 * with a short TTL. Google will echo it back as a query parameter on the
 * /callback redirect, and the backend will validate it then. The frontend
 * does not need to use 'state' for anything itself — it is exposed only for
 * debugging/observability.
 */
public record GoogleAuthUrlResponse(
        String authorizationUrl,
        String state
) {}
