package common.services.securityconfig;

import java.time.Instant;

/**
 * Represents the JSON error response body returned by the security handlers.
 *
 * This is a Java 'record' — a special class introduced in Java 16 that is
 * perfect for simple data containers. It automatically generates:
 *   - A constructor with all fields
 *   - Getters for all fields (error() and timestamp())
 *   - equals(), hashCode(), and toString() methods
 *
 * When serialized to JSON by ObjectMapper, it produces:
 * {
 *     "error": "Authentication required",
 *     "timestamp": "2024-03-15T10:30:00Z"
 * }
 *
 * This record is used by both:
 *   - JwtAuthenticationEntryPoint → 401 responses ("Authentication required")
 *   - JwtAccessDeniedHandler      → 403 responses ("Access denied")
 *
 * The timestamp field is useful for debugging — it tells you exactly when
 * the error occurred without needing to check server logs.
 */
public record SecurityErrorResponse(
        String error,     // human-readable error message, e.g. "Authentication required"
        Instant timestamp // exact UTC moment the error was generated, e.g. "2024-03-15T10:30:00Z"
) {}