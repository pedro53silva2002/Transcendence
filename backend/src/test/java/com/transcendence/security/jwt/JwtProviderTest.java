package com.transcendence.security.jwt;

import com.transcendence.common.services.jwtservice.JwtProvider;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.UUID;

import static org.assertj.core.api.Assertions.assertThat;

public class JwtProviderTest {
    private static final String SECRET = "test-secret-key-that-is-at-least-32-chars!!";
    private static final long TTL_MINUTES = 2;
    private static final long REFRESH_TTL_DAYS = 7;

    private JwtProvider jwtProvider;

    @BeforeEach
    void setUp() {
        jwtProvider = new JwtProvider(SECRET, TTL_MINUTES, REFRESH_TTL_DAYS);
    }

    // Token Generations

    @Test
    void generateAccessToken_returnsNonNullToken() {
        String token = jwtProvider.generateAccessToken(UUID.randomUUID(), "USER");

        assertThat(token).isNotNull().isNotEmpty();
    }

    @Test
    void generateAccessToken_tokenHasThreeDotSeparatedParts() {
        String token = jwtProvider.generateAccessToken(UUID.randomUUID(), "USER");

        assertThat(token.split("\\.")).hasSize(3);
    }

    // Token Validations

    @Test
    void validateToken_returnsTrueForValidToken() {
        String token = jwtProvider.generateAccessToken(UUID.randomUUID(), "ADMIN");
        
        assertThat(jwtProvider.validateToken(token)).isTrue();
    }

    @Test
    void validateToken_returnsFalseForExpiredToken() throws InterruptedException {
        JwtProvider shortJwt = new JwtProvider(SECRET, 0, REFRESH_TTL_DAYS);

        String token = shortJwt.generateAccessToken(UUID.randomUUID(), "USER");

        Thread.sleep(10);

        assertThat(shortJwt.validateToken(token)).isFalse();
    }

    @Test
    void validateToken_returnsFalseForTamperedSignature() {
        String token = jwtProvider.generateAccessToken(UUID.randomUUID(), "USER");
        // Replace last character with a different one — if it's 'X' use 'Y', otherwise use 'X'
        char lastChar = token.charAt(token.length() - 1);
        char replacement = (lastChar == 'X') ? 'Y' : 'X';
        String tampered = token.substring(0, token.length() - 1) + replacement;

        assertThat(jwtProvider.validateToken(tampered)).isFalse();
    }

    @Test
    void validateToken_returnsFalseForRandomString() {
        assertThat(jwtProvider.validateToken("not.a.token")).isFalse();
    }

    @Test
    void validateToken_returnsFalseForNullToken() {
        assertThat(jwtProvider.validateToken(null)).isFalse();
    }

    @Test
    void validateToken_returnsFalseForEmptyString() {
        assertThat(jwtProvider.validateToken("")).isFalse();
    }

    // Test getters

    @Test
    void getUserIdFromToken_returnsCorrectUserId() {
        UUID userId = UUID.randomUUID();
        String token = jwtProvider.generateAccessToken(userId, "ADMIN");

        assertThat(jwtProvider.getUserIdFromToken(token)).isEqualTo(userId);
    }

    @Test
    void getRoleFromToken_returnsCorrectRole() {
        String token = jwtProvider.generateAccessToken(UUID.randomUUID(), "ADMIN");

        assertThat(jwtProvider.getRoleFromToken(token)).isEqualTo("ADMIN");
    }

    @Test
    void generateRefreshToken_returnsValidUUIDString() {
        String refreshToken = jwtProvider.generateRefreshToken();

        assertThat(refreshToken).isNotNull();
        UUID.fromString(refreshToken);
    }
}
