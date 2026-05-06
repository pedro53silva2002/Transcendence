package common.services.securityconfig;

import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.web.access.AccessDeniedHandler;
import org.springframework.stereotype.Component;
import java.io.IOException;
import java.time.Instant;

@Component
public class JwtAccessDeniedHandler implements AccessDeniedHandler {

    private final ObjectMapper objectMapper;

    // ObjectMapper is Spring Boot's JSON serializer — injected, not instantiated
    public JwtAccessDeniedHandler(ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
    }

    // Spring calls this automatically when an unauthenticated request hits a protected route
    @Override
    public void handle(HttpServletRequest request, HttpServletResponse response,
                       AccessDeniedException authException) throws IOException {

        // Set the HTTP status code to 401
        response.setStatus(HttpServletResponse.SC_FORBIDDEN);

        // Tell the client the response body is JSON
        response.setContentType("application/json");

        // Build the error body
        SecurityErrorResponse error = new SecurityErrorResponse("Access denied", Instant.now());

        // Serialize the record to JSON and write it to the response
        objectMapper.writeValue(response.getWriter(), error);
    }
}