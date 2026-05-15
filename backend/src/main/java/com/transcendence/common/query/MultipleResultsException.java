package com.transcendence.common.query;

/**
 * It's a custom exception thrown when you expect 1 row but get more than 1.
 * super is used to call the constructor of the parent class (RuntimeException) to set the error message and/or cause.
 */
public class MultipleResultsException extends RuntimeException {
    public MultipleResultsException(String message) {
        super(message);
    }
    public MultipleResultsException(String message, Throwable cause) {
        super(message, cause);
    }
}