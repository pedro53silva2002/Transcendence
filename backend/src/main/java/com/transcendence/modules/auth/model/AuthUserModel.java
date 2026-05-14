package com.transcendence.modules.auth.model;

import backend.common.query.QueryExecutor;
import com.transcendence.modules.auth.dtos.UserDto;
import common.services.user.User;
import common.services.user.UserRepository;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.stereotype.Component;

import java.util.Optional;
import java.util.UUID;

/**
 * Data access layer for the auth module.
 *
 * Onion-architecture rule: this is the ONLY place that touches the database
 * for auth operations. The service and router layers never see SQL, never
 * see the JPA entity, and never call the repository directly.
 *
 * CRUD split:
 *   - READS  → QueryExecutor (raw SQL), returns UserDto
 *   - WRITES → UserRepository (Hibernate/JPA), returns UserDto via toDto()
 *
 * The JPA entity (User) is intentionally never exposed beyond this class.
 * Service-layer code receives UserDto only.
 */
@Component
public class AuthUserModel {

    private final UserRepository userRepository;
    private final QueryExecutor  queryExecutor;

    public AuthUserModel(UserRepository userRepository, QueryExecutor queryExecutor) {
        this.userRepository = userRepository;
        this.queryExecutor  = queryExecutor;
    }

    // ─── READS (QueryExecutor + raw SQL) ──────────────────────────────────────

    /**
     * Primary lookup for OAuth login: "has this Google account ever signed in here?"
     *
     * Implementation hint:
     *   SELECT id, email, username, display_name, bio, profile_photo_url,
     *          oauth_provider, oauth_id, created_at, updated_at
     *   FROM auth.users
     *   WHERE oauth_provider = :provider AND oauth_id = :oauthId
     *   LIMIT 1
     *
     * Use queryExecutor.queryAsSingle(sql, params, UserDto.class).
     *
     * @param provider e.g. "google"
     * @param oauthId  the provider's stable user id (Google's "sub" claim)
     * @return matching user as UserDto, or empty if no row matches
     */
    public Optional<UserDto> findByOauthId(String provider, String oauthId) {
        String sql = """
                SELECT id,
                       email,
                       username,
                       display_name,
                       bio,
                       profile_photo_url,
                       oauth_provider,
                       oauth_id,
                       created_at,
                       updated_at
                  FROM auth.users
                 WHERE oauth_provider = :provider
                   AND oauth_id = :oauthId
                 LIMIT 1
                """;
        MapSqlParameterSource params = new MapSqlParameterSource()
                .addValue("provider", provider)
                .addValue("oauthId", oauthId);
        return queryExecutor.queryAsSingle(sql, params, UserDto.class);
    }

    /**
     * Lookup by email — used during OAuth signup to decide between "link to
     * existing email/password account" and "create new account".
     *
     * Implementation hint: same SELECT shape as findByOauthId, WHERE email = :email.
     *
     * @param email exact-match email (case-sensitive at DB level; lowercase before passing if needed)
     * @return matching user as UserDto, or empty if no row matches
     */
    public Optional<UserDto> findByEmail(String email) {
        // Normalize lookup by comparing lowercase email to avoid casing mismatches
        String sql = """
                SELECT id,
                       email,
                       username,
                       display_name,
                       bio,
                       profile_photo_url,
                       oauth_provider,
                       oauth_id,
                       created_at,
                       updated_at
                  FROM auth.users
                 WHERE lower(email) = :emailLower
                 LIMIT 1
                """;
        MapSqlParameterSource params = new MapSqlParameterSource()
                .addValue("emailLower", email == null ? null : email.trim().toLowerCase());
        return queryExecutor.queryAsSingle(sql, params, UserDto.class);
    }

    /**
     * Username uniqueness check, called before INSERT.
     *
     * Implementation hint:
     *   queryExecutor.queryScalar(
     *       "SELECT EXISTS(SELECT 1 FROM auth.users WHERE username = :u)",
     *       params, Boolean.class).orElse(false);
     *
     * @param username candidate username
     * @return true if a user with that username already exists
     */
    public boolean existsByUsername(String username) {
        String sql = """
                SELECT EXISTS(
                    SELECT 1
                      FROM auth.users
                     WHERE username = :username
                )
                """;
        MapSqlParameterSource params = new MapSqlParameterSource()
                .addValue("username", username);
        return queryExecutor.queryScalar(sql, params, Boolean.class)
                .orElse(false);
    }

    // ─── WRITES (UserRepository + Hibernate) ──────────────────────────────────

    /**
     * Insert a new user. Single entry point used by both signup paths:
     *
     *   - Email/password: pass a BCrypt 'passwordHash', leave oauthProvider/oauthId null.
     *   - OAuth:          pass oauthProvider + oauthId, leave passwordHash null.
     *
     * Implementation hint:
     *   User entity = new User(email, username, passwordHash, displayName,
     *                          profilePhotoUrl, oauthProvider, oauthId);
     *   User saved  = userRepository.save(entity);
     *   return toDto(saved);
     *
     * Hibernate populates id (GenerationType.UUID) and the timestamps
     * (@PrePersist hook — still pending on User.java).
     *
     * @return the newly inserted row as a UserDto
     */
    public UserDto create(String email,
                          String username,
                          String passwordHash,
                          String displayName,
                          String profilePhotoUrl,
                          String oauthProvider,
                          String oauthId) {
        String normalizedEmail = email == null ? null : email.trim().toLowerCase();
        User entity = new User(
                normalizedEmail,
                username,
                passwordHash,
                displayName,
                profilePhotoUrl,
                oauthProvider,
                oauthId);
        User saved = userRepository.save(entity);
        return toDto(saved);
    }


    /**
     * Check whether the user with the given email has a local password set.
     * Keeps the password existence check inside the model layer to avoid
     * exposing the raw hash to upper layers.
     *
     * @param email user email
     * @return true if a non-null password_hash exists for that email
     */
    public boolean hasLocalPassword(String email) {
        String sql = "SELECT password_hash IS NOT NULL FROM auth.users WHERE email = :email LIMIT 1";
        MapSqlParameterSource params = new MapSqlParameterSource().addValue("email", email);
        return queryExecutor.queryScalar(sql, params, Boolean.class).orElse(false);
    }

    /**
     * Attach an OAuth identity to an existing user (e.g. user previously
     * registered with email/password and now signs in with Google using the
     * same email).
     *
     * Implementation hint:
     *   User entity = userRepository.findById(userId)
     *       .orElseThrow(() -> new IllegalStateException("user " + userId));
     *   entity.setOauthProvider(oauthProvider);
     *   entity.setOauthId(oauthId);
     *   User saved = userRepository.save(entity);
     *   return toDto(saved);
     *
     * The findById here is a JPA-managed load required for the update —
     * it's part of the write transaction, not a free-form read.
     *
     * @param userId        the existing user's id
     * @param oauthProvider e.g. "google"
     * @param oauthId       the provider's stable user id
     * @return the updated row as a UserDto
     */
    public UserDto linkOauthToExistingUser(UUID userId, String oauthProvider, String oauthId) {
        User entity = userRepository.findById(userId)
                .orElseThrow(() -> new IllegalStateException("user " + userId));
        entity.setOauthProvider(oauthProvider);
        entity.setOauthId(oauthId);
        User saved = userRepository.save(entity);
        return toDto(saved);
    }

    /**
     * Delete a user by id. Not used by the OAuth login flow itself; included
     * here for completeness of CRUD (account deletion / GDPR).
     *
     * Implementation hint: userRepository.deleteById(userId);
     */
    public void delete(UUID userId) {
        userRepository.deleteById(userId);
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    /**
     * Convert the JPA entity to UserDto so the entity never leaks out of the
     * model layer. Called by create() and linkOauthToExistingUser() after save().
     *
     * Note on timestamps: User uses OffsetDateTime, UserDto uses Instant —
     * call entity.getCreatedAt().toInstant() and getUpdatedAt().toInstant().
     */
    private UserDto toDto(User entity) {
        UserDto dto = new UserDto();
        dto.setId(entity.getId());
        dto.setEmail(entity.getEmail());
        dto.setUsername(entity.getUsernameHandle());
        dto.setDisplayName(entity.getDisplayName());
        dto.setBio(entity.getBio());
        dto.setProfilePhotoUrl(entity.getProfilePhotoUrl());
        dto.setOauthProvider(entity.getOauthProvider());
        dto.setOauthId(entity.getOauthId());

        if (entity.getCreatedAt() != null) {
            dto.setCreatedAt(entity.getCreatedAt().toInstant());
        }
        if (entity.getUpdatedAt() != null) {
            dto.setUpdatedAt(entity.getUpdatedAt().toInstant());
        }

        return dto;
    }
}
