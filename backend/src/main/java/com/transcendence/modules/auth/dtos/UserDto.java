package com.transcendence.modules.auth.dtos;

import java.time.Instant;
import java.util.UUID;

/**
 * Internal representation of a user, used between the model and service layers.
 *
 * Why a plain class (and not a record)?
 *   ReflectiveRowMapper instantiates results via the no-arg constructor and
 *   then assigns each column with either a setter or direct field access.
 *   Java records have no no-arg constructor, so they cannot be used as the
 *   target type of QueryExecutor.queryAs(...). Hence this is a mutable POJO.
 *
 * Why Instant (and not OffsetDateTime)?
 *   ReflectiveRowMapper maps TIMESTAMPTZ → Instant out of the box. The JPA
 *   entity (User.java) uses OffsetDateTime; the model converts between the
 *   two when mapping entity ↔ DTO. Both represent the same absolute moment
 *   in time, just with/without an explicit offset.
 *
 * Never leak this DTO to the frontend — it contains oauth_provider / oauth_id
 * which are internal-only. Map to PublicUserDto before returning from the router.
 */
public class UserDto {

    private UUID    id;
    private String  email;
    private String  username;
    private String  displayName;
    private String  bio;
    private String  profilePhotoUrl;
    private String  oauthProvider;
    private String  oauthId;
    private Instant createdAt;
    private Instant updatedAt;

    public UserDto() {}

    public UUID    getId()              { return id; }
    public String  getEmail()           { return email; }
    public String  getUsername()        { return username; }
    public String  getDisplayName()     { return displayName; }
    public String  getBio()             { return bio; }
    public String  getProfilePhotoUrl() { return profilePhotoUrl; }
    public String  getOauthProvider()   { return oauthProvider; }
    public String  getOauthId()         { return oauthId; }
    public Instant getCreatedAt()       { return createdAt; }
    public Instant getUpdatedAt()       { return updatedAt; }

    public void setId(UUID id)                               { this.id = id; }
    public void setEmail(String email)                       { this.email = email; }
    public void setUsername(String username)                 { this.username = username; }
    public void setDisplayName(String displayName)           { this.displayName = displayName; }
    public void setBio(String bio)                           { this.bio = bio; }
    public void setProfilePhotoUrl(String profilePhotoUrl)   { this.profilePhotoUrl = profilePhotoUrl; }
    public void setOauthProvider(String oauthProvider)       { this.oauthProvider = oauthProvider; }
    public void setOauthId(String oauthId)                   { this.oauthId = oauthId; }
    public void setCreatedAt(Instant createdAt)              { this.createdAt = createdAt; }
    public void setUpdatedAt(Instant updatedAt)              { this.updatedAt = updatedAt; }
}
