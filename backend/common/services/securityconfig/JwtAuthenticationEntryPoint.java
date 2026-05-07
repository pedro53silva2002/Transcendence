package common.services.securityconfig;

import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.stereotype.Component;
import java.io.IOException;
import java.time.Instant;

/**
 * Handles HTTP 401 Unauthorized responses for unauthenticated requests.
 *
 * There are two types of security errors:
 *   - 401 Unauthorized → user is NOT logged in / has no valid JWT (handled HERE)
 *   - 403 Forbidden    → user IS logged in but lacks the required role (handled by JwtAccessDeniedHandler)
 *
 * Example: a request with no Authorization header hitting a protected endpoint → 401.
 *
 * By default, Spring Security returns an HTML error page for 401 responses.
 * We implement AuthenticationEntryPoint to override that behavior and return
 * a clean JSON response instead — which is what a REST API client expects.
 *
 * This class is wired into SecurityConfig via the exceptionHandling() configuration.
 */
@Component // Registers this class as a Spring bean so it can be injected into SecurityConfig
public class JwtAuthenticationEntryPoint implements AuthenticationEntryPoint {

    // ObjectMapper converts Java objects to JSON strings.
    // We inject it (rather than creating it with 'new') so Spring reuses
    // the same configured instance across the whole application.
    private final ObjectMapper objectMapper;

    // Constructor injection — Spring automatically provides the ObjectMapper bean here.
    // This is the preferred way to inject dependencies in Spring (over @Autowired on fields).
    public JwtAuthenticationEntryPoint(ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
    }

    /**
     * Called automatically by Spring Security when a request reaches a protected route
     * without a valid JWT token (or with no token at all).
     *
     * The name "commence" comes from the AuthenticationEntryPoint interface —
     * it means "start the authentication process". In our case, we don't redirect
     * to a login page (that's for web apps); we just return a 401 JSON response
     * because this is a stateless REST API.
     *
     * @param request        the incoming HTTP request
     * @param response       the HTTP response we are building to send back
     * @param authException  the exception Spring threw when no authentication was found
     *                       (we don't use it directly, but it's required by the interface)
     */
    @Override
    public void commence(HttpServletRequest request, HttpServletResponse response,
                         AuthenticationException authException) throws IOException {

        // 401 Unauthorized — the request has no valid JWT so we can't identify the user.
        // SC_UNAUTHORIZED is just the constant 401 defined in HttpServletResponse.
        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);

        // Tell the browser/client that the response body is JSON, not HTML.
        // Without this, the client might misinterpret the response format.
        response.setContentType("application/json");

        // Build the error payload using our SecurityErrorResponse record.
        // This creates an object like: { "error": "Authentication required", "timestamp": "2024-..." }
        SecurityErrorResponse error = new SecurityErrorResponse("Authentication required", Instant.now());

        // Serialize the SecurityErrorResponse object to JSON and write it directly
        // into the HTTP response body. The client will receive this as the response.
        objectMapper.writeValue(response.getWriter(), error);
    }
}