package com.transcendence.modules.auth.exceptions;

import com.transcendence.common.services.globalExceptionHandler.DomainException;

/**
 * Thrown when an OAuth flow would collide with an existing local account
 * and the policy is to reject (do not auto-link or merge).
 */
public class OAuthConflictException extends DomainException {

    public OAuthConflictException(String message) {
        super("OAUTH_CONFLICT", message);
    }
}

