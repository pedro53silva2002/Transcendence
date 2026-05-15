package com.transcendence.common.services.securityconfig;

import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.web.access.AccessDeniedHandler;
import org.springframework.stereotype.Component;
import java.io.IOException;
import java.time.Instant;

/**
 * Handles HTTP 403 Forbidden responses for authenticated users who lack permission.
 *
 * There are two types of security errors:
 *   - 401 Unauthorized → user is NOT logged in (handled by JwtAuthenticationEntryPoint)
 *   - 403 Forbidden    → user IS logged in, but doesn't have the required role/permission (handled HERE)
 *
 * Example: a ROLE_USER trying to access an admin-only endpoint → 403.
 *
 * Spring Security automatically calls this handler when an authenticated user
 * is denied access to a resource. We override the default behavior (which returns
 * an HTML error page) to return a clean JSON response instead.
 */
@Component // Registers this class as a Spring bean so it can be injected into SecurityConfig
public class JwtAccessDeniedHandler implements AccessDeniedHandler {

    // ObjectMapper converts Java objects to JSON strings.
    // We inject it (rather than creating it with 'new') so Spring reuses
    // the same configured instance across the whole application.
    private final ObjectMapper objectMapper;

    // Constructor injection — Spring automatically provides the ObjectMapper bean here.
    // This is the preferred way to inject dependencies in Spring (over @Autowired on fields).
    public JwtAccessDeniedHandler(ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
    }

    /**
     * Called automatically by Spring Security when an authenticated user tries to access
     * a resource they don't have permission for.
     *
     * @param request        the incoming HTTP request
     * @param response       the HTTP response we are building to send back
     * @param authException  the exception Spring threw when access was denied (we don't use it
     *                       directly, but it's required by the interface)
     */
    @Override
    public void handle(HttpServletRequest request, HttpServletResponse response,
                       AccessDeniedException authException) throws IOException {

        // 403 Forbidden — the user is authenticated but not authorized for this resource.
        // SC_FORBIDDEN is just the constant 403 defined in HttpServletResponse.
        response.setStatus(HttpServletResponse.SC_FORBIDDEN);

        // Tell the browser/client that the response body is JSON, not HTML.
        // Without this, the client might misinterpret the response format.
        response.setContentType("application/json");

        // Build the error payload using our SecurityErrorResponse record.
        // This creates an object like: { "error": "Access denied", "timestamp": "2024-..." }
        SecurityErrorResponse error = new SecurityErrorResponse("Access denied", Instant.now());

        // Serialize the SecurityErrorResponse object to JSON and write it directly
        // into the HTTP response body. The client will receive this as the response.
        objectMapper.writeValue(response.getWriter(), error);
    }
}