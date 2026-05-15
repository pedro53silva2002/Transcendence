package com.transcendence.common.services.jwtservice;

import io.jsonwebtoken.*;
import io.jsonwebtoken.security.Keys;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import javax.crypto.SecretKey;
import java.nio.charset.StandardCharsets;
import java.util.Date;
import java.util.Optional;
import java.util.UUID;

@Component
public class JwtProvider {
    private final SecretKey key;
    private final String secret;
    private final long accessTokenTtlMinutes;
    private final long refreshTokenTtlDays;

    // Use the same values that we created in application.yml
    // The jwt encryption will use the secret to create their encryption
    // The access is valid for 15 minutes. After that, we do another validation
    // After 7 days, the user needs to login again.
    public JwtProvider(
        @Value("${jwt.secret}") String secret,
        @Value("${jwt.access-token-ttl-minutes}") long accessTtlMinutes,
        @Value("${jwt.refresh-token-ttl-days}") long refreshTtlDays
    ) {
        this.secret = secret;
        this.key = Keys.hmacShaKeyFor(secret.getBytes(StandardCharsets.UTF_8));
        // Conversão para ms, uma vez que o jwt token utiliza ms
        this.accessTokenTtlMinutes = accessTtlMinutes * 60 * 1000;
        this.refreshTokenTtlDays = refreshTtlDays * 24 * 60 * 60 * 1000;
    }

    @jakarta.annotation.PostConstruct
    private void validate() {
        if (secret.length() < 32)
            throw new IllegalStateException("jwt: secret must be at least 32 characters.");
        if (accessTokenTtlMinutes <= 0)
                throw new IllegalStateException("jwt: access-token-ttl-minutes must be at least 1.");
    }

    // Generate the encrypted JWT Token
    public String generateAccessToken(UUID userId, String platformRole)
    {
        String jwtId = UUID.randomUUID().toString();
        Date now = new Date();
        Date exp = new Date(now.getTime() + accessTokenTtlMinutes);

        return Jwts.builder()
                .subject(userId.toString())
                .id(jwtId)
                .claim("role", platformRole)
                .issuedAt(now)
                .expiration(exp)
                .signWith(key)
                .compact();
    }

    public String generateRefreshToken()
    {
        return UUID.randomUUID().toString();
    }

    public boolean validateToken(String token)
    {
        return getValidatedClaims(token).isPresent();
    }

    /**
     * Parses and validates the token in a single pass.
     * Prefer this over validateToken + individual getters to avoid reparsing.
     */
    public Optional<Claims> getValidatedClaims(String token)
    {
        try {
            Claims claims = Jwts.parser()
                .verifyWith(key)
                .build()
                .parseSignedClaims(token)
                .getPayload();
            return Optional.of(claims);
        }
        catch (JwtException | IllegalArgumentException e)
        {
            return Optional.empty();
        }
    }

    public UUID getUserIdFromToken(String token)
    {
        return UUID.fromString(parseClaims(token).getSubject());
    }

    public String getJwtIdFromToken(String token)
    {
        return parseClaims(token).getId();
    }

    public String getRoleFromToken(String token)
    {
        return parseClaims(token).get("role", String.class);
    }

    public long getRemainingTtlMinutes(String token)
    {
        Date exp = parseClaims(token).getExpiration();
        return Math.max(0, (exp.getTime() - System.currentTimeMillis()) / 60_000);
    }

    private Claims parseClaims(String token)
    {
        return Jwts.parser()
                .verifyWith(key)
                .build()
                .parseSignedClaims(token)
                .getPayload();
    }
}