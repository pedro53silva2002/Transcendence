package com.transcendence.common.services.user;

import jakarta.persistence.*;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import java.time.OffsetDateTime;
import java.util.Collection;
import java.util.List;
import java.util.UUID;

/**
 * Represents a user in the database and in Spring Security.
 *
 * This class does two things at once:
 *
 * 1. JPA ENTITY — maps this Java class to the "users" table in the "auth" schema.
 *    Each field annotated with @Column maps to a database column.
 *    Hibernate (the JPA implementation) uses this mapping to read/write rows automatically.
 *
 * 2. SPRING SECURITY UserDetails — by implementing UserDetails, Spring Security
 *    knows how to use this class during authentication. When a user logs in,
 *    Spring calls UserDetailsService.loadUserByUsername() which returns this object,
 *    and then uses the getPassword() and getAuthorities() methods to verify credentials.
 *
 * Database location: schema "auth", table "users"
 */
@Entity // Tells JPA/Hibernate this class is a database entity (maps to a table)
@Table(name = "users", schema = "auth") // Specifies the exact table and schema name
public class User implements UserDetails {

    /**
     * Primary key — maps to the "id" column (UUID in Postgres).
     *
     * GenerationType.UUID tells Hibernate 6+ to generate a random (v4) UUID
     * in Java before the INSERT is sent. The DB column has
     * DEFAULT gen_random_uuid() as a safety net, but Hibernate's value takes
     * precedence, so the source of truth is the application — predictable
     * and unit-testable without a DB roundtrip.
     */
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    // Each user must have a unique email — enforced at both DB level (unique = true)
    // and application level. Cannot be null.
    @Column(unique = true, nullable = false)
    private String email;

    // Each user must have a unique username — same constraints as email.
    @Column(unique = true, nullable = false)
    private String username;

    // The Java field is named 'passwordHash' but the DB column is 'password_hash'.
    // @Column(name = ...) bridges that naming difference.
    // We never store plain-text passwords — always BCrypt hashed.
    @Column(name = "password_hash")
    private String passwordHash;

    // Display name is what other users see (e.g. "Pedro Silva").
    // Cannot be null — every user must have one.
    @Column(name = "display_name", nullable = false)
    private String displayName;

    // Optional short bio text shown on the user's profile. Can be null.
    private String bio;

    // URL pointing to the user's profile photo (stored in MinIO/S3, not in the DB directly).
    // Can be null if the user hasn't uploaded a photo.
    @Column(name = "profile_photo_url")
    private String profilePhotoUrl;

    // OAuth fields — only populated when the user signed up via Google/GitHub/etc.
    // Both are null for users who registered with email + password.
    @Column(name = "oauth_provider")
    private String oauthProvider; // e.g. "google", "github"

    @Column(name = "oauth_id")
    private String oauthId; // the unique ID from the OAuth provider

    // Automatically set when the row is first inserted.
    // 'updatable = false' means Hibernate will never change this value after creation.
    @Column(name = "created_at", nullable = false, updatable = false)
    private OffsetDateTime createdAt;

    // Updated every time the user's record is modified.
    @Column(name = "updated_at", nullable = false)
    private OffsetDateTime updatedAt;

    // ─── Constructors ─────────────────────────────────────────────────────────
    //
    // JPA requires a no-arg constructor to instantiate entities via reflection.
    // It is 'protected' (not 'public') because application code should never
    // build a half-empty User by hand — use the parameterized constructor below.

    protected User() {}

    /**
     * Constructor used for creating a new user via either authentication path:
     *
     *  - Email/password signup → pass a BCrypt 'passwordHash'; leave
     *                            'oauthProvider' and 'oauthId' as null.
     *  - OAuth signup (Google, …) → pass 'oauthProvider' and 'oauthId';
     *                               leave 'passwordHash' as null.
     *
     * The DB schema permits all three (passwordHash, oauthProvider, oauthId)
     * to be null individually, which is what makes a single shared constructor
     * possible. The decision about which fields to fill belongs to the caller
     * (the auth service).
     *
     * The 'id', 'createdAt', and 'updatedAt' fields are populated by Hibernate
     * (id) and the @PrePersist lifecycle hook (timestamps) when the entity is saved.
     */
    public User(String email,
                String username,
                String passwordHash,
                String displayName,
                String profilePhotoUrl,
                String oauthProvider,
                String oauthId) {
        this.email = email;
        this.username = username;
        this.passwordHash = passwordHash;
        this.displayName = displayName;
        this.profilePhotoUrl = profilePhotoUrl;
        this.oauthProvider = oauthProvider;
        this.oauthId = oauthId;
    }

    // ─── Getters ──────────────────────────────────────────────────────────────
    //
    // Note: there is no public 'getUsername()' for the username field — that
    // method is reserved by Spring Security's UserDetails contract and already
    // returns the email (the login identifier). Use 'getUsernameHandle()' to
    // read the actual username column.

    public UUID          getId()              { return id; }
    public String        getEmail()           { return email; }
    public String        getUsernameHandle()  { return username; }
    public String        getDisplayName()     { return displayName; }
    public String        getBio()             { return bio; }
    public String        getProfilePhotoUrl() { return profilePhotoUrl; }
    public String        getOauthProvider()   { return oauthProvider; }
    public String        getOauthId()         { return oauthId; }
    public OffsetDateTime getCreatedAt()      { return createdAt; }
    public OffsetDateTime getUpdatedAt()      { return updatedAt; }

    // ─── Setters ──────────────────────────────────────────────────────────────
    //
    // Only fields that are legitimately mutable have setters.
    //  - 'id'                 → set by Hibernate on INSERT, never by us
    //  - 'createdAt'          → set once on INSERT (@PrePersist), never changes
    //  - 'updatedAt'          → managed by @PreUpdate, not by callers
    //  - 'email' / 'username' → currently treated as immutable identities; add a
    //                           setter here only if you implement an "edit email"
    //                           or "rename" feature with the correct uniqueness
    //                           and re-verification handling.

    public void setPasswordHash(String passwordHash)       { this.passwordHash = passwordHash; }
    public void setDisplayName(String displayName)         { this.displayName = displayName; }
    public void setBio(String bio)                         { this.bio = bio; }
    public void setProfilePhotoUrl(String profilePhotoUrl) { this.profilePhotoUrl = profilePhotoUrl; }
    public void setOauthProvider(String oauthProvider)     { this.oauthProvider = oauthProvider; }
    public void setOauthId(String oauthId)                 { this.oauthId = oauthId; }

    @PrePersist
    private void onCreate() {
        OffsetDateTime now = OffsetDateTime.now();
        this.createdAt = now;
        this.updatedAt = now;
    }

    @PreUpdate
    private void onUpdate() {
        this.updatedAt = OffsetDateTime.now();
    }

    // ─── UserDetails interface methods ────────────────────────────────────────
    //
    // Spring Security calls these methods during authentication and authorization.
    // We must implement all of them because we implement UserDetails.

    // Spring uses getUsername() to identify the user — we return email because
    // that's what users log in with (not their display username).
    @Override
    public String getUsername() { return email; }

    // Returns the stored BCrypt hash — Spring Security compares this against
    // the submitted password using BCryptPasswordEncoder.
    @Override
    public String getPassword() { return passwordHash; }

    // Returns the roles/permissions this user has.
    // We return an empty list for now — role management will be added later.
    // An empty list means the user is authenticated but has no special roles.
    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() { return List.of(); }

    // The following four methods are account status checks used by Spring Security.
    // We return true for all of them — meaning all accounts are always considered
    // active and valid. If you later add account suspension or password expiry
    // features, these would return false for affected accounts.
    @Override public boolean isAccountNonExpired()     { return true; } // account never expires
    @Override public boolean isAccountNonLocked()      { return true; } // account is never locked
    @Override public boolean isCredentialsNonExpired() { return true; } // password never expires
    @Override public boolean isEnabled()               { return true; } // account is always active
}
