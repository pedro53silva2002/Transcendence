package backend.common.query;

/**
 * It's a custom exception thrown when you expect 1 row but get more than 1.
 */
public class MultipleResultsException extends RuntimeException {
    public MultipleResultsException(String message) {
        super(message);
    }
    public MultipleResultsException(String message, Throwable cause) {
        super(message, cause);
    }
}