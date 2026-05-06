package backend.common.query;

/**
 * It's a custom exception thrown when the mapper fails to convert data from database to Java
 * (e.g., invalid JSON, invalid enum value, type mismatch).
 */
public class DataMappingException extends RuntimeException {
    
    public DataMappingException(String message, Throwable cause) {
        super(message, cause);
    }
}