package common.services.securityconfig;

import services.jwtservice.JwtAuthFilter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.security.web.access.AccessDeniedHandler;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.CorsConfigurationSource;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;
import org.springframework.beans.factory.annotation.Value;
import java.util.List;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.core.userdetails.UserDetailsService;

// Marks this class as a source of Spring bean definitions
@Configuration
// Enables Spring Security's web security support and provides the Spring MVC integration
// Also disables Spring Boot's default auto-configured security
@EnableWebSecurity
public class SecurityConfig {

    // Fields are final because they should never change after construction
    private final JwtAuthFilter jwtAuthFilter;
    private final AuthenticationEntryPoint authEntryPoint; // handles 401 - unauthenticated
    private final AccessDeniedHandler accessDeniedHandler; // handles 403 - unauthorized
    private final UserDetailsService userDetailsService;

    // @Value tells Spring to inject a value from config at startup
    @Value("${cors.allowed-origins}")
    // the field that holds the value once injected
    private String allowedOrigins;
    // Spring sees this constructor and automatically injects the matching beans
    // This is constructor injection - preferred over @Autowired on fields
    public SecurityConfig(JwtAuthFilter jwtAuthFilter,
                          AuthenticationEntryPoint authEntryPoint,
                          AccessDeniedHandler accessDeniedHandler,
                          UserDetailsService userDetailsService) {
        this.jwtAuthFilter = jwtAuthFilter;
        this.authEntryPoint = authEntryPoint;
        this.accessDeniedHandler = accessDeniedHandler;
        this.userDetailsService = userDetailsService;
    }

    // @Bean tells Spring to register the return value as a bean in the application context
    // Spring Security will automatically detect and use this SecurityFilterChain
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {

        // Disable CSRF protection - safe here because we use JWT (stateless, no session cookies)
        http.csrf(csrf -> csrf.disable());

        // Tell Spring never to create or use an HttpSession
        // Each request must authenticate itself via JWT - nothing is remembered between requests
        http.sessionManagement(session -> session
                .sessionCreationPolicy(SessionCreationPolicy.STATELESS));

        // Define which routes are public and which require authentication
        http.authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/auth").permitAll()  // auth endpoints are public
                .anyRequest().authenticated()              // everything else requires a valid JWT
        );

        // Wire the 401 and 403 handlers
        // authEntryPoint fires when request is unauthenticated (no/invalid JWT)
        // accessDeniedHandler fires when request is authenticated but lacks the required role/permission
        http.exceptionHandling(ex -> ex
                .authenticationEntryPoint(authEntryPoint)
                .accessDeniedHandler(accessDeniedHandler)
        );

        // Register JwtAuthFilter to run BEFORE Spring's default username/password filter
        // This way the JWT is validated first and the security context is set before Spring checks it
        http.addFilterBefore(jwtAuthFilter, UsernamePasswordAuthenticationFilter.class);

        // Tell Spring Security to use our CORS config for all incoming requests
        http.cors(cors -> cors.configurationSource(corsConfigurationSource()));

        // Build and return the configured SecurityFilterChain
        // This is what Spring Security actually uses to protect your app
        return http.build();
    }

    @Bean
    public CorsConfigurationSource corsConfigurationSource() {
        CorsConfiguration config = new CorsConfiguration();

        // The origin(s) allowed to make requests to your API
        // Comes from application.yml via @Value
        config.setAllowedOrigins(List.of(allowedOrigins));

        // Which HTTP methods are allowed from the browser
        config.setAllowedMethods(List.of("GET", "POST", "PUT", "DELETE", "OPTIONS"));

        // Which headers the browser is allowed to send
        // Authorization — carries the JWT token
        // Content-Type — tells the server the format of the request body
        config.setAllowedHeaders(List.of("Authorization", "Content-Type"));

        // Allow the browser to send credentials (JWT in Authorization header)
        // Must be true if your frontend sends the Authorization header
        config.setAllowCredentials(true);

        // UrlBasedCorsConfigurationSource maps CORS config to specific URL patterns
        UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();

        // Register the config for ALL routes in your app
        // "/**" means every endpoint will use this CORS configuration
        source.registerCorsConfiguration("/**", config);

        return source;
    }

    @Bean
    public BCryptPasswordEncoder passwordEncoder() {
       return new BCryptPasswordEncoder(12);
    }

    // Wires together UserDetailsService and BCryptPasswordEncoder
    // Spring uses this to load the user and verify the password during login
    @Bean
    public DaoAuthenticationProvider authenticationProvider() {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider();
        provider.setUserDetailsService(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder());
        return provider;
    }

    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
        return config.getAuthenticationManager();
    }

}