package com.transcendence.common.services.authcontext;

import com.transcendence.common.services.user.User;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.Collection;
import java.util.List;
import java.util.UUID;

public class AuthenticatedUser implements UserDetails {

    private final UUID id;
    private final String role;

    public AuthenticatedUser(UUID id, String role) {
        this.id = id;
        this.role = role;
    }

    public UUID getId()      { return id; }

    @Override public String getUsername()              { return id.toString(); }
    @Override public String getPassword()              { return null; }
    @Override public boolean isAccountNonExpired()     { return true; }
    @Override public boolean isAccountNonLocked()      { return true; }
    @Override public boolean isCredentialsNonExpired() { return true; }
    @Override public boolean isEnabled()               { return true; }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        return List.of(() -> role);
    }

}
