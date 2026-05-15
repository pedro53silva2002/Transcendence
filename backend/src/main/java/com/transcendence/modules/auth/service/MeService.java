package com.transcendence.modules.auth.dtos;

import backend.common.services.globalExceptionHandler.NotFoundException;
import common.services.authcontext.AuthenticatedUserService;
import common.services.controller.dtos.MeResponse;
import common.services.user.UserRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.UUID;

@Service
public class MeService {
  private final UserRepository userRepository;
  private final AuthenticatedUserService authContext;

  public MeService(UserRepository userRepository, AuthenticatedUserService authContext)
  {
    this.userRepository = userRepository;
    this.authContext = authContext;
  }

  @Transactional(readOnly = true)
  public MeResponse getMe() {
    UUID userId = authContext.getCurrentUserId();

    return userRepository.findByUuid(userId).map(MeResponse::from).orElseThrow(() -> new NotFoundException("User not found"));
  }
}
