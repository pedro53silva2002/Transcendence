package common.services.user;

import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

/**
 * Implements Spring Security's UserDetailsService interface.
 *
 * Spring Security needs a way to load a user from the database during authentication.
 * This class is the bridge between Spring Security and our database — it tells
 * Spring "here's how to find a user by their login identifier".
 *
 * How it fits into the login flow:
 *   1. User submits email + password to the login endpoint
 *   2. The controller calls authenticationManager.authenticate(...)
 *   3. Spring Security internally calls loadUserByUsername(email) on this class
 *   4. We query the database and return the User object
 *   5. Spring Security then uses User.getPassword() + BCrypt to verify the password
 *   6. If correct, authentication succeeds and a JWT is generated
 *
 * This class is wired into DaoAuthenticationProvider inside SecurityConfig.
 */
@Service // Registers this class as a Spring bean so it can be injected into SecurityConfig
public class UserDetailsServiceImpl implements UserDetailsService {

    // The repository that handles all database queries for the users table.
    // 'final' because it should never be reassigned after construction.
    private final UserRepository userRepository;

    /**
     * Constructor injection — Spring automatically provides the UserRepository bean here.
     * This is preferred over @Autowired on fields because dependencies are explicit
     * and the class is easier to unit test.
     */
    public UserDetailsServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    /**
     * Loads a user from the database by their login identifier.
     *
     * Spring Security calls this method automatically during authentication.
     * Despite the parameter being named "username", it receives whatever value
     * the user submitted as their login identifier — in our case, their email,
     * because User.getUsername() returns email.
     *
     * The returned UserDetails object is then used by Spring Security to:
     *   - Verify the submitted password against the stored BCrypt hash
     *   - Check account status (locked, expired, enabled)
     *   - Load the user's roles/authorities for authorization
     *
     * @param username the email submitted by the user during login
     * @return the User entity if found (which implements UserDetails)
     * @throws UsernameNotFoundException if no user exists with that email.
     *         Spring Security catches this specific exception — do not change it
     *         to a generic exception or authentication will break.
     */
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        // Query the database for a user with this email.
        // findByEmail() returns an Optional<User> — either the user exists or it doesn't.
        // orElseThrow() unwraps the Optional: returns the User if present,
        // or throws UsernameNotFoundException if not found.
        return userRepository.findByEmail(username)
                .orElseThrow(() -> new UsernameNotFoundException("User not found: " + username));
    }
}