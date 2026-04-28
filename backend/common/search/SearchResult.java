package transcendence.backend.common.search;

import java.util.Map;

/**
 * A Java record is a concise way to define an immutable data carrier.
 * It automatically provides a constructor, accessor methods, and
 * implementations of equals, hashCode, and toString.
 *
 * This record represents the result of a search query construction,
 * containing the generated SQL string, the associated query parameters,
 * and the maximum number of results to return (limit).
 */
public record SearchResult(
    String sql,
    Map<String, Object> parameters,
    int limit
) {}