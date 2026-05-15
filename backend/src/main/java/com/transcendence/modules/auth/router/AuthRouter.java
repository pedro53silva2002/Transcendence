package com.transcendence.modules.auth.router;

import common.services.controller.dtos.MeResponse;
import common.services.me.MeService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("api/auth")
public class AuthRouter {
  private final MeService meService;

  public AuthRouter(MeService meService) {
    this.meService = meService;
  }

  @GetMapping("/me")
  public ResponseEntity<MeResponse> me() {
    return ResponseEntity.ok(meService.getMe());
  }
}
