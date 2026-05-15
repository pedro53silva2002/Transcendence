import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import com.transcendence.common.services.securityconfig.JwtAuthenticationEntryPoint;
import com.transcendence.common.services.securityconfig.JwtAccessDeniedHandler;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.core.AuthenticationException;
import java.io.PrintWriter;
import java.io.StringWriter;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SecurityConfigTest {

    // ─── JwtAuthenticationEntryPoint ─────────────────────────────────────────

    @Nested
    class JwtAuthenticationEntryPointTests {

        private JwtAuthenticationEntryPoint entryPoint;

        @BeforeEach
        void setUp() {
            ObjectMapper objectMapper = new ObjectMapper();
            objectMapper.registerModule(new JavaTimeModule());
            entryPoint = new JwtAuthenticationEntryPoint(objectMapper);
        }

        @Test
        void commence_setsStatus401() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AuthenticationException exception = mock(AuthenticationException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            entryPoint.commence(request, response, exception);
            verify(response).setStatus(HttpServletResponse.SC_UNAUTHORIZED);
        }

        @Test
        void commence_setsContentTypeJson() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AuthenticationException exception = mock(AuthenticationException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            entryPoint.commence(request, response, exception);
            verify(response).setContentType("application/json");
        }

        @Test
        void commence_writesAuthenticationRequiredMessage() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AuthenticationException exception = mock(AuthenticationException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            entryPoint.commence(request, response, exception);
            assertTrue(stringWriter.toString().contains("Authentication required"));
        }
    }

    // ─── JwtAccessDeniedHandler ───────────────────────────────────────────────

    @Nested
    class JwtAccessDeniedHandlerTests {

        private JwtAccessDeniedHandler handler;

        @BeforeEach
        void setUp() {
            ObjectMapper objectMapper = new ObjectMapper();
            objectMapper.registerModule(new JavaTimeModule());
            handler = new JwtAccessDeniedHandler(objectMapper);
        }

        @Test
        void handle_setsStatus403() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AccessDeniedException exception = mock(AccessDeniedException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            handler.handle(request, response, exception);
            verify(response).setStatus(HttpServletResponse.SC_FORBIDDEN);
        }

        @Test
        void handle_setsContentTypeJson() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AccessDeniedException exception = mock(AccessDeniedException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            handler.handle(request, response, exception);
            verify(response).setContentType("application/json");
        }

        @Test
        void handle_writesAccessDeniedMessage() throws Exception {
            HttpServletRequest request = mock(HttpServletRequest.class);
            HttpServletResponse response = mock(HttpServletResponse.class);
            AccessDeniedException exception = mock(AccessDeniedException.class);
            StringWriter stringWriter = new StringWriter();
            when(response.getWriter()).thenReturn(new PrintWriter(stringWriter));
            handler.handle(request, response, exception);
            assertTrue(stringWriter.toString().contains("Access denied"));
        }
    }
}