package backend.common.search;

import java.util.Map;

/**
 * The generated output of the query builder, ready for execution.
 *
 * @param sql        the generated SQL string with named parameters
 * @param parameters map of parameter names to their converted values
 * @param limit      the number of records to fetch (pageSize + 1 for hasNext check)
 */
public record SearchResult(
    String sql,
    Map<String, Object> parameters,
    int limit
) {}