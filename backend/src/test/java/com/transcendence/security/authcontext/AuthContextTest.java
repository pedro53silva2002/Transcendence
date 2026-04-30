package com.transcendence;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.authentication.AnonymousAuthenticationToken;
import org.springframework.security.core.authority.SimpleGrantedAuthority;

import java.util.List;
import java.util.UUID;

import static org.junit.jupiter.api.Assertions.*;

class AuthenticatedUserServiceTest {

    private AuthenticatedUserService service;
    private static final UUID TEST_UUID = UUID.randomUUID();

    @BeforeEach
    void setUp() {
        service = new AuthenticatedUserService();
        SecurityContextHolder.clearContext();
    }

    // ─── getAuthentication ────────────────────────────────────────────────────

    @Test
    void getAuthentication_whenUserIsAuthenticated_returnsAuthentication() {
        setAuthenticatedUser(TEST_UUID, "ROLE_ADMIN");
        assertNotNull(service.getAuthentication());
    }

    @Test
    void getAuthentication_whenNoUser_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.getAuthentication());
    }

    // ─── getCurrentUser ───────────────────────────────────────────────────────

    @Test
    void getCurrentUser_whenPrincipalIsAuthenticatedUser_returnsUser() {
        setAuthenticatedUser(TEST_UUID, "ROLE_USER");
        AuthenticatedUser user = service.getCurrentUser();
        assertNotNull(user);
        assertEquals(TEST_UUID, user.getId());
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
        setAuthenticatedUser(TEST_UUID, "ROLE_USER");
        assertEquals(TEST_UUID, service.getCurrentUserId());
    }

    @Test
    void getCurrentUserId_whenNotAuthenticated_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.getCurrentUserId());
    }

    // ─── isAuthenticated ──────────────────────────────────────────────────────

    @Test
    void isAuthenticated_whenUserIsAuthenticated_returnsTrue() {
        setAuthenticatedUser(TEST_UUID, "ROLE_USER");
        assertTrue(service.isAuthenticated());
    }

    @Test
    void isAuthenticated_whenNoUser_returnsFalse() {
        assertFalse(service.isAuthenticated());
    }

    @Test
    void isAuthenticated_whenAnonymousAuthenticationToken_returnsFalse() {
        AnonymousAuthenticationToken auth = new AnonymousAuthenticationToken(
                "key", "anonymousUser", List.of(new SimpleGrantedAuthority("ROLE_ANONYMOUS"))
        );
        SecurityContextHolder.getContext().setAuthentication(auth);
        assertFalse(service.isAuthenticated());
    }

    // ─── hasRole ──────────────────────────────────────────────────────────────

    @Test
    void hasRole_whenUserHasRole_returnsTrue() {
        setAuthenticatedUser(TEST_UUID, "ROLE_ADMIN");
        assertTrue(service.hasRole("ROLE_ADMIN"));
    }

    @Test
    void hasRole_whenUserDoesNotHaveRole_returnsFalse() {
        setAuthenticatedUser(TEST_UUID, "ROLE_USER");
        assertFalse(service.hasRole("ROLE_ADMIN"));
    }

    @Test
    void hasRole_whenNotAuthenticated_returnsFalse() {
        assertFalse(service.hasRole("ROLE_ADMIN"));
    }

    // ─── requireAuthentication ────────────────────────────────────────────────

    @Test
    void requireAuthentication_whenAuthenticated_doesNotThrow() {
        setAuthenticatedUser(TEST_UUID, "ROLE_USER");
        assertDoesNotThrow(() -> service.requireAuthentication());
    }

    @Test
    void requireAuthentication_whenNotAuthenticated_throwsUnauthorizedException() {
        assertThrows(UnauthorizedException.class, () -> service.requireAuthentication());
    }

    // ─── Helper ───────────────────────────────────────────────────────────────

    private void setAuthenticatedUser(UUID id, String role) {
        AuthenticatedUser principal = new AuthenticatedUser(id, role);
        UsernamePasswordAuthenticationToken auth =
                new UsernamePasswordAuthenticationToken(principal, null, principal.getAuthorities());
        SecurityContextHolder.getContext().setAuthentication(auth);
    }
}