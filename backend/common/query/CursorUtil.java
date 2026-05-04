package backend.common.query;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.util.Base64;
import java.util.Map;

/**
 * Small helper to encode/decode a cursor map as base64(JSON)
 */
public final class CursorUtil {
    private static final ObjectMapper M = new ObjectMapper();
    private CursorUtil() {};

    public static String encode(Map<String,Object> payload) {
        try {
            byte[] json = M.writeValueAsBytes(payload);
            return Base64.getUrlEncoder().withoutPadding().encodeToString(json);
        }
        catch (Exception e) {
            throw new RuntimeException("Failed to encode cursor", e);
        }
    }

    public static Map<String,Object> decode(String cursor) {
        try {
            byte[] json = Base64.getUrlDecoder().decode(cursor);
            return M.readValue(json, Map.class);
        }
        catch (Exception e) {
            throw new RuntimeException("Failed to decode cursor", e);
        }
    }
}