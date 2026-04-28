package backend.common.src.main.java.com.transcendence;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.List;
import java.util.UUID;

import static org.junit.jupiter.api.Assertions.*;

class AuthenticatedUserServiceTest {

    // ─── Helper ───────────────────────────────────────────────────────────────

    private void setAuthenticatedUser(UUID id, String email, String role) {
        AuthenticatedUser principal = new AuthenticatedUser(id, email, role);  // ✅ correct class
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken(principal, null, principal.getAuthorities());
        SecurityContextHolder.getContext().setAuthentication(auth);
    }

    private AuthenticatedUserService service;

    private static final UUID TEST_UUID = UUID.randomUUID();
    private static final String TEST_EMAIL = "pedro@transcendence.com";

    @BeforeEach
    void setUp() {
        service = new AuthenticatedUserService();
        SecurityContextHolder.clearContext();
    }

    // ─── getAuthentication ────────────────────────────────────────────────────

    @Test
    void getAuthentication_whenUserIsAuthenticated_returnsAuthentication() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_ADMIN");
        assertNotNull(service.getAuthentication());
    }

    @Test
    void getAuthentication_whenNoUser_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.getAuthentication());
    }

    // ─── getUsername ──────────────────────────────────────────────────────────

    @Test
    void getUsername_whenPrincipalIsAuthenticatedUser_returnsEmail() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertEquals(TEST_EMAIL, service.getUsername());
    }

    @Test
    void getUsername_whenPrincipalIsString_returnsString() {
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken("some-uuid-string", null, List.of());
        SecurityContextHolder.getContext().setAuthentication(auth);
        assertEquals("some-uuid-string", service.getUsername());
    }

    // ─── getUserDetails ───────────────────────────────────────────────────────

    @Test
    void getUserDetails_whenPrincipalIsUserDetails_returnsUserDetails() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        UserDetails userDetails = service.getUserDetails();
        assertEquals(TEST_EMAIL, userDetails.getUsername());
    }

    @Test
    void getUserDetails_whenPrincipalIsString_throwsUnauthorizedException() {
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken("some-uuid-string", null, List.of());
        SecurityContextHolder.getContext().setAuthentication(auth);
        assertThrows(UnauthorizedException.class, () -> service.getUserDetails());
    }

    // ─── getCurrentUser ───────────────────────────────────────────────────────

    @Test
    void getCurrentUser_whenPrincipalIsAuthenticatedUser_returnsUser() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        AuthenticatedUserService user = service.getCurrentUser();
        assertNotNull(user);
        assertEquals(TEST_UUID, user.getId());
        assertEquals(TEST_EMAIL, user.getEmail());
    }

    @Test
    void getCurrentUser_whenPrincipalIsNotAuthenticatedUser_throwsUnauthorizedException() {
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken("some-uuid-string", null, List.of());
        SecurityContextHolder.getContext().setAuthentication(auth);
        assertThrows(UnauthorizedException.class, () -> service.getCurrentUser());
    }

    // ─── getCurrentUserId ─────────────────────────────────────────────────────

    @Test
    void getCurrentUserId_whenAuthenticated_returnsUUID() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertEquals(TEST_UUID, service.getCurrentUserId());
    }

    @Test
    void getCurrentUserId_whenNotAuthenticated_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.getCurrentUserId());
    }

    // ─── getCurrentEmail ──────────────────────────────────────────────────────

    @Test
    void getCurrentEmail_whenAuthenticated_returnsEmail() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertEquals(TEST_EMAIL, service.getCurrentEmail());
    }

    @Test
    void getCurrentEmail_whenNotAuthenticated_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.getCurrentEmail());
    }

    // ─── isAuthenticated ──────────────────────────────────────────────────────

    @Test
    void isAuthenticated_whenUserIsAuthenticated_returnsTrue() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertTrue(service.isAuthenticated());
    }

    @Test
    void isAuthenticated_whenNoUser_returnsFalse() {
        assertFalse(service.isAuthenticated());
    }

    @Test
    void isAuthenticated_whenAnonymousUser_returnsFalse() {
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken("anonymousUser", null, List.of());
        SecurityContextHolder.getContext().setAuthentication(auth);
        assertFalse(service.isAuthenticated());
    }

    // ─── hasRole ──────────────────────────────────────────────────────────────

    @Test
    void hasRole_whenUserHasRole_returnsTrue() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_ADMIN");
        assertTrue(service.hasRole("ROLE_ADMIN"));
    }

    @Test
    void hasRole_whenUserDoesNotHaveRole_returnsFalse() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertFalse(service.hasRole("ROLE_ADMIN"));
    }

    @Test
    void hasRole_whenNotAuthenticated_returnsFalse() {
        assertFalse(service.hasRole("ROLE_ADMIN"));
    }

    // ─── requireAuthentication ────────────────────────────────────────────────

    @Test
    void requireAuthentication_whenAuthenticated_doesNotThrow() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        assertDoesNotThrow(() -> service.requireAuthentication());
    }

    @Test
    void requireAuthentication_whenNotAuthenticated_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.requireAuthentication());
    }

    @Test
    void getCurrentUser_whenPrincipalIsAuthenticatedUser_returnsUser() {
        setAuthenticatedUser(TEST_UUID, TEST_EMAIL, "ROLE_USER");
        AuthenticatedUser user = service.getCurrentUser();  // ✅ correct class
        assertNotNull(user);
        assertEquals(TEST_UUID, user.getId());
        assertEquals(TEST_EMAIL, user.getEmail());
    }

    // ─── Helper ───────────────────────────────────────────────────────────────

    private void setAuthenticatedUser(UUID id, String email, String role) {
        AuthenticatedUser principal = new AuthenticatedUser(id, email, role);  // ✅ correct class
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken(principal, null, principal.getAuthorities());
        SecurityContextHolder.getContext().setAuthentication(auth);
    }
}