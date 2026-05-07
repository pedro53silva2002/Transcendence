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

/**
 * Central security configuration for the entire application.
 *
 * This class is the "security brain" — it tells Spring Security:
 *   - Which routes are public and which require a JWT token
 *   - How to handle unauthenticated (401) and unauthorized (403) requests
 *   - Which filter to use for JWT validation
 *   - Which origins (frontends) are allowed to make requests (CORS)
 *   - How to encode/verify passwords (BCrypt)
 *   - How to load users from the database during authentication
 *
 * Spring Security works as a chain of filters that every HTTP request passes through.
 * This class configures that chain.
 */
@Configuration   // Tells Spring this class contains bean definitions (@Bean methods)
@EnableWebSecurity // Activates Spring Security and disables Boot's default auto-configuration
public class SecurityConfig {

    // These are the dependencies this class needs — all injected via constructor.
    // 'final' means they can only be set once (in the constructor) and never changed.
    private final JwtAuthFilter jwtAuthFilter;          // our custom JWT validation filter
    private final AuthenticationEntryPoint authEntryPoint;   // handles 401 Unauthorized responses
    private final AccessDeniedHandler accessDeniedHandler;   // handles 403 Forbidden responses
    private final UserDetailsService userDetailsService;     // loads users from the database

    // @Value reads a property from application.yml at startup and injects it here.
    // The syntax ${cors.allowed-origins} means: look for 'cors.allowed-origins' in config.
    // Example application.yml value: cors.allowed-origins: "http://localhost:4200"
    @Value("${cors.allowed-origins}")
    private String allowedOrigins;

    /**
     * Constructor injection — Spring automatically finds and injects the matching beans.
     * This is preferred over @Autowired on fields because it makes dependencies explicit
     * and makes the class easier to unit test.
     */
    public SecurityConfig(JwtAuthFilter jwtAuthFilter,
                          AuthenticationEntryPoint authEntryPoint,
                          AccessDeniedHandler accessDeniedHandler,
                          UserDetailsService userDetailsService) {
        this.jwtAuthFilter = jwtAuthFilter;
        this.authEntryPoint = authEntryPoint;
        this.accessDeniedHandler = accessDeniedHandler;
        this.userDetailsService = userDetailsService;
    }

    /**
     * Defines the security filter chain — the core of Spring Security configuration.
     *
     * Every HTTP request passes through this chain in order. Here we configure:
     *   1. CSRF disabled (safe for JWT APIs — CSRF only matters for session-cookie auth)
     *   2. Stateless sessions (no server-side session storage — each request must carry its JWT)
     *   3. Route access rules (which endpoints are public vs protected)
     *   4. Custom error handlers (JSON responses instead of HTML error pages)
     *   5. JWT filter registration (runs before Spring's default auth filter)
     *   6. CORS configuration (which frontend origins are allowed)
     *
     * @param http Spring's builder object for configuring HTTP security
     * @return the fully configured SecurityFilterChain
     */
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {

        // CSRF (Cross-Site Request Forgery) protection is only needed for apps that use
        // session cookies for authentication. Since we use JWT (stateless), CSRF is irrelevant.
        http.csrf(csrf -> csrf.disable());

        // Make Spring Security fully stateless — it will never create or use an HttpSession.
        // Each request must prove who it is by sending a valid JWT in the Authorization header.
        http.sessionManagement(session -> session
                .sessionCreationPolicy(SessionCreationPolicy.STATELESS));

        // Define access rules for each route:
        // - /api/auth is public (login, register) — no JWT needed
        // - Everything else requires a valid JWT token
        http.authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/auth").permitAll()
                .anyRequest().authenticated()
        );

        // Register our custom error handlers:
        // - authEntryPoint  → called when there's no/invalid JWT (returns 401 JSON)
        // - accessDeniedHandler → called when JWT is valid but user lacks permission (returns 403 JSON)
        http.exceptionHandling(ex -> ex
                .authenticationEntryPoint(authEntryPoint)
                .accessDeniedHandler(accessDeniedHandler)
        );

        // Insert our JwtAuthFilter into the filter chain BEFORE Spring's default
        // UsernamePasswordAuthenticationFilter. This ensures:
        //   1. The JWT is extracted and validated first
        //   2. The SecurityContext is populated with the user's identity
        //   3. Spring's authorization checks then have access to that identity
        http.addFilterBefore(jwtAuthFilter, UsernamePasswordAuthenticationFilter.class);

        // Apply our CORS configuration to all requests.
        // Without this, the browser would block requests from the Angular frontend.
        http.cors(cors -> cors.configurationSource(corsConfigurationSource()));

        return http.build();
    }

    /**
     * Configures Cross-Origin Resource Sharing (CORS).
     *
     * CORS is a browser security feature that blocks requests from one origin (e.g. localhost:4200)
     * to a different origin (e.g. localhost:8080) unless the server explicitly allows it.
     *
     * This config tells the browser: "requests from our Angular frontend are allowed".
     * The allowed origin comes from application.yml so it can be changed per environment
     * without touching the code.
     *
     * IMPORTANT: Never use "*" (wildcard) as allowed origin when allowCredentials is true —
     * browsers will block it, and it's a security risk.
     */
    @Bean
    public CorsConfigurationSource corsConfigurationSource() {
        CorsConfiguration config = new CorsConfiguration();

        // Only allow requests from our configured frontend origin (e.g. http://localhost:4200).
        // In production this would be the deployed frontend URL.
        config.setAllowedOrigins(List.of(allowedOrigins));

        // Allow these HTTP methods from the browser.
        // OPTIONS is required for CORS preflight requests (browser sends this before POST/PUT/DELETE).
        config.setAllowedMethods(List.of("GET", "POST", "PUT", "DELETE", "OPTIONS"));

        // Allow the browser to send these headers with requests.
        // Authorization carries the JWT token; Content-Type describes the request body format.
        config.setAllowedHeaders(List.of("Authorization", "Content-Type"));

        // Allow the browser to send credentials (in our case, the JWT in the Authorization header).
        // This must be true for the frontend to include the Authorization header.
        config.setAllowCredentials(true);

        // UrlBasedCorsConfigurationSource lets us apply different CORS configs to different URL patterns.
        // We use "/**" to apply the same config to every endpoint in the application.
        UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        source.registerCorsConfiguration("/**", config);

        return source;
    }

    /**
     * Creates a BCryptPasswordEncoder bean for hashing and verifying passwords.
     *
     * BCrypt is a one-way hashing algorithm — you can never reverse a hash back to plaintext.
     * The strength parameter (12) controls how many rounds of hashing are performed.
     * Higher = more secure but slower. Strength 12 adds ~300ms per login — this is intentional,
     * it makes brute-force attacks much harder.
     *
     * This bean is injected into DaoAuthenticationProvider and any service that needs
     * to hash a password before saving it to the database.
     */
    @Bean
    public BCryptPasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder(12);
    }

    /**
     * Wires together the user loader and password encoder for Spring's authentication system.
     *
     * DaoAuthenticationProvider is Spring's built-in class that handles login:
     *   1. It calls userDetailsService.loadUserByUsername(email) to fetch the user from the DB
     *   2. It uses passwordEncoder to verify the submitted password against the stored hash
     *   3. If both match, authentication succeeds
     *
     * This is used during the login endpoint when the user submits their credentials.
     */
    @Bean
    public DaoAuthenticationProvider authenticationProvider() {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider();
        provider.setUserDetailsService(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder());
        return provider;
    }

    /**
     * Exposes Spring's AuthenticationManager as a bean so it can be injected elsewhere.
     *
     * AuthenticationManager is the entry point for triggering authentication programmatically.
     * It's used in the login endpoint: when the user submits email + password,
     * the controller calls authenticationManager.authenticate(...) which internally
     * uses the DaoAuthenticationProvider configured above.
     *
     * Without exposing it as a @Bean, it would not be injectable into other classes.
     */
    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
        return config.getAuthenticationManager();
    }
}