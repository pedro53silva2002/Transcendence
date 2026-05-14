package com.transcendence.modules.auth.dtos;

import java.util.UUID;

public record PublicUserDto(
        UUID id,
        String email,
        String username,
        String displayName,
        String bio,
        String profilePhotoUrl
) {}