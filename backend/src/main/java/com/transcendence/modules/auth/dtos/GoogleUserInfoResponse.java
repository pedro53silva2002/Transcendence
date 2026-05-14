package com.transcendence.modules.auth.dtos;

import com.fasterxml.jackson.annotation.JsonProperty;

public record GoogleUserInfoResponse(
        String sub,                                // Google's stable user ID → maps to oauth_id
        String email,
        @JsonProperty("email_verified") Boolean emailVerified,
        String name,                               // → maps to display_name
        @JsonProperty("given_name")   String givenName,
        @JsonProperty("family_name")  String familyName,
        String picture,                            // → maps to profile_photo_url
        String locale
) {}
