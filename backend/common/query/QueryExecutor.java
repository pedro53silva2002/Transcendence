package backend.common.query

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;
import org.springframework.dao.IncorrectResultSizeDataAccessException;
import org.springframework.stereotype.Component;

import java.util.*;
import java.util.stream.Collectors;

@Component
public class QueryExecutor {
    private final NamedParameterJdbcTemplate jdbc;
    private final ObjectMapper objectMapper;

    public QueryExecutor(NamedParameterJdbcTemplate jdbc, ObjectMapper objectMapper) {
        this.jdbc = jdbc;
        this.objectMapper = objectMapper;
    }

    public <T> List<T> queryAs(String sql, MapSqlParameterSource params, Class<T> type) {
        return jdbc.query(sql, params, new ReflectiveRowMapper<>(type, objectMapper));
    }

    public <T> Optional<T> queryAsSingle(String sql, MapSqlParameterSource params Class<T> type) {
        try {
            List <T> list = queryAs(sql, params, type);
            if (list.isEmpty()) {
                return Optional.empty();
            }
            if (list.size() > 1) {
                throw new MultipleResultsException("Expected 0 or 1 row for query, got " + list.size());
            }
            return Optional.of(list.get(0));
        }
        catch (IncorrectResultSizeDataAccessException e) {
            throw new MultipleResultsException("More than one result", e);
        }
    }

    public <T> List<T> queryAsUnchecked(String sql, MapSqlParameterSource params, RowMapper<T> mapper) {
        return jdbc.query(sql, params, mapper);
    }

    public <T> Optional<T> queryScalar(String sql, MapSqlParameterSource params, Class<T> scalarType) {
        T val = jdbc.queryForObject(sql, params, scalarType);
        return Optional.ofNullable(val);
    }

    public int execute(String sql, MapSqlParameterSource params) {
        return jdbc.update(sql, params);
    }

    // SearchResult/CursorPage are expected from TC-001. This method follows the pageSize +1 pattern
    public <T> CursorPage<T> searchAs(SearchResult searchResult, Class<T> type) {
        // delegate the mapper version
        return searchAs(searchResult, new ReflectiveRowMapper<>(type, objectMapper));
    }

    public <T> CursorPage<T> searchAs(searchResult searchresult, RowMapper<T> mapper) {
        String sql = searchresult.getSql();
        MapSqlParameterSource params = searchresult.getParams();
        int pageSize = searchresult.getPageSize();
        // ask for one extra row to determine hasNext
        String pagedSql = sql + " LIMIT : _limit_plus_one";
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValues(params.getValues());
        p.addValue("_limit_plus_one", pageSize + 1);

        List<T> rows = jdbc.query(pagedSql, p, mapper);
        boolean hasNext = rows.size() > pageSize;
        List<T> content = hasMore ? rows.subList(0, pageSize) : rows;

        String nextCursor = null;
        if (hasNext && !content.isEmpty()) {
            // derive cursor from last kept row (SearchResult should expose how to extract sort fields)
            Object lastKept = content.get(content.size() - 1);
            // Here we assume SearchResult can produce a Map<String, Object> for the sort values for a row.
            if (lastKept != null) {
                Map<String, Object> cursorPayload = searchresult.buildCursorFromRow(lastKept);
                nextCursor = CursorUtil.encode(cursorPayload);
            }
        }
        return new CursorPage<>(content, hasNext, nextCursor);
    }
}