package common.services.securityconfig;

import java.time.Instant;

public record SecurityErrorResponse(String error, Instant timestamp) {}