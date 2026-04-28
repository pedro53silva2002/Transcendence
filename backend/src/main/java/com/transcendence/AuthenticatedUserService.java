package backend.common.src.main.java.com.transcendence;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Service;
//import transcendence.backend.common.exceptions.UnauthorizedException;

@Service
public class AuthenticatedUserService {

    /**
     * Returns the Authentication object from the security context.
     *
     * @throws UnauthorizedException if no authenticated user is found
     */
    public Authentication getAuthentication() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();

        if (authentication == null || !authentication.isAuthenticated()) {
            throw new UnauthorizedException("No authenticated user in security context");
        }

        return authentication;
    }

    /**
     * Returns the username (subject) of the currently authenticated user.
     *
     * @throws UnauthorizedException if no authenticated user is found
     */
    public String getUsername() {
        Authentication authentication = getAuthentication();
        Object principal = authentication.getPrincipal();

        if (principal instanceof UserDetails userDetails) {
            return userDetails.getUsername();
        }

        return principal.toString(); // fallback for plain JWT subject string
    }

    /**
     * Returns the full UserDetails of the authenticated user.
     * Use this when you need roles/authorities, not just the username.
     *
     * @throws UnauthorizedException if no authenticated user is found
     * @throws UnauthorizedException if principal is not a UserDetails instance
     */
    public UserDetails getUserDetails() {
        Object principal = getAuthentication().getPrincipal();

        if (principal instanceof UserDetails userDetails) {
            return userDetails;
        }

        throw new UnauthorizedException("Principal is not a UserDetails instance");
    }

    /**
     * Returns true if the current user has the given role.
     * Role should be passed without the "ROLE_" prefix (e.g. "ADMIN").
     */
    public boolean hasRole(String role) {
        return getAuthentication().getAuthorities().stream()
                .anyMatch(a -> a.getAuthority().equals("ROLE_" + role));
    }
}