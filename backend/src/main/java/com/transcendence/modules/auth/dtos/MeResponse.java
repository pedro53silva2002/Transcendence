package com.transcendence.modules.auth.dtos;

import common.services.user.User;

public record MeResponse(String username, String displayName, String email, String profilePhotoUrl)
{
  public static MeResponse from(User user) {
    return new MeResponse(
      user.getUsername(),
      user.getDisplayName(),
      user.getEmail(),
      user.getProphilePhotoUrl()
    );
  }
}
