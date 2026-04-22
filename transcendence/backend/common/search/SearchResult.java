package transcendence.backend.common.search;

import java.util.Map;

public record SearchResult(
    String sql,
    Map<String, Object> parameters,
    int limit
) {}