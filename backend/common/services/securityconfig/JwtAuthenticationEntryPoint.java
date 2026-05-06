package common.services.securityconfig;

import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.stereotype.Component;
import java.io.IOException;
import java.time.Instant;

@Component
public class JwtAuthenticationEntryPoint implements AuthenticationEntryPoint {

    private final ObjectMapper objectMapper;

    // ObjectMapper is Spring Boot's JSON serializer — injected, not instantiated
    public JwtAuthenticationEntryPoint(ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
    }

    // Spring calls this automatically when an unauthenticated request hits a protected route
    @Override
    public void commence(HttpServletRequest request, HttpServletResponse response,
                         AuthenticationException authException) throws IOException {

        // Set the HTTP status code to 401
        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);

        // Tell the client the response body is JSON
        response.setContentType("application/json");

        // Build the error body
        SecurityErrorResponse error = new SecurityErrorResponse("Authentication required", Instant.now());

        // Serialize the record to JSON and write it to the response
        objectMapper.writeValue(response.getWriter(), error);
    }
}