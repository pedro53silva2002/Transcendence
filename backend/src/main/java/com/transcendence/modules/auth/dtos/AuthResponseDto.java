package com.transcendence.modules.auth.dtos;

public record AuthResponseDto(
        String accessToken,
        String refreshToken,
        long   accessTokenExpiresInSeconds,
        PublicUserDto user
) {}
