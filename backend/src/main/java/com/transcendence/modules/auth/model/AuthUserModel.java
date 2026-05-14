package com.transcendence.modules.auth.model;

import backend.common.query.QueryExecutor;
import com.transcendence.modules.auth.dtos.UserDto;
import common.services.user.User;
import common.services.user.UserRepository;
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
        throw new UnsupportedOperationException("TODO: implement with QueryExecutor");
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
        throw new UnsupportedOperationException("TODO: implement with QueryExecutor");
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
        throw new UnsupportedOperationException("TODO: implement with QueryExecutor");
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
        throw new UnsupportedOperationException("TODO: implement with UserRepository.save");
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
        throw new UnsupportedOperationException("TODO: implement with UserRepository.findById + save");
    }

    /**
     * Delete a user by id. Not used by the OAuth login flow itself; included
     * here for completeness of CRUD (account deletion / GDPR).
     *
     * Implementation hint: userRepository.deleteById(userId);
     */
    public void delete(UUID userId) {
        throw new UnsupportedOperationException("TODO: implement with UserRepository.deleteById");
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
        throw new UnsupportedOperationException("TODO: field-by-field mapping from entity to UserDto");
    }
}
