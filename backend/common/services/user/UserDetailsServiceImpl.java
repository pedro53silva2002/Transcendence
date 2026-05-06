package common.services.user;

import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;


// @Service marks this as a Spring bean so it can be injected into SecurityConfig
@Service
public class UserDetailsServiceImpl implements UserDetailsService {

    // We need the repository to query the database
    private final UserRepository userRepository;

    // Constructor injection — same pattern as SecurityConfig
    public UserDetailsServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    // Spring calls this automatically during authentication
    // The "username" parameter is whatever getUsername() returns on your User entity
    // In our case that's email, so Spring will pass the email here
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        // Try to find the user by email
        // If not found, throw UsernameNotFoundException — Spring expects this specific exception
        return userRepository.findByEmail(username)
                .orElseThrow(() -> new UsernameNotFoundException("User not found: " + username));
    }
}

