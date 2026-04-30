package common.services.authcontext;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Service;
import org.springframework.security.authentication.AnonymousAuthenticationToken;

import java.util.UUID;

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
     * Returns the full UserDetails of the authenticated user.
     *
     * @throws UnauthorizedException if not authenticated or principal is not UserDetails
     */
    public UserDetails getUserDetails() {
        Object principal = getAuthentication().getPrincipal();
        if (principal instanceof UserDetails userDetails) {
            return userDetails;
        }
        throw new UnauthorizedException("Principal is not a UserDetails instance");
    }

    /**
     * Returns the full AuthenticatedUser principal.
     *
     * @throws UnauthorizedException if not authenticated or principal is not AuthenticatedUser
     */
    public AuthenticatedUser getCurrentUser() {
        Object principal = getAuthentication().getPrincipal();
        if (principal instanceof AuthenticatedUser user) {
            return user;
        }
        throw new UnauthorizedException("Principal is not an AuthenticatedUser instance");
    }

    /**
     * Returns the authenticated user's UUID.
     *
     * @throws UnauthorizedException if not authenticated
     */
    public UUID getCurrentUserId() {
        return getCurrentUser().getId();
    }

    /**
     * Returns true if there is a non-anonymous authenticated principal in the context.
     */
    public boolean isAuthenticated() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        return authentication != null
                && authentication.isAuthenticated()
                && !(authentication instanceof AnonymousAuthenticationToken);
                //!(authentication.getPrincipal() instanceof String s && s.equals("anonymousUser"));
    }

    /**
     * Returns true if the current user has the specified role (e.g. "ROLE_ADMIN").
     * Returns false if not authenticated.
     */
    public boolean hasRole(String role) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || !authentication.isAuthenticated()) {
            return false;
        }
        return authentication.getAuthorities().stream()
                .anyMatch(a -> a.getAuthority().equals(role));
    }

    /**
     * Throws UnauthorizedException if not authenticated.
     * Use as a guard at the top of service methods.
     *
     * @throws UnauthorizedException if not authenticated
     */
    public void requireAuthentication() {
        if (!isAuthenticated()) {
            throw new UnauthorizedException("Authentication required");
        }
    }
}