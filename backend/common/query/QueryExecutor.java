package backend.common.query;

import backend.common.search.SearchQueryBuilder;
import backend.common.search.SearchResult;
import backend.common.search.CursorPage;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.dao.IncorrectResultSizeDataAccessException;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;
import org.springframework.stereotype.Component;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

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

    public <T> Optional<T> queryAsSingle(String sql, MapSqlParameterSource params, Class<T> type) {
        try {
            List<T> list = queryAs(sql, params, type);
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

    public <T> CursorPage<T> searchAs(SearchResult searchResult, RowMapper<T> mapper) {
        String sql = searchResult.sql();
        MapSqlParameterSource params = new MapSqlParameterSource();
        if (searchResult.parameters() != null) {
            params.addValues(searchResult.parameters());
        }

        List<T> rows = jdbc.query(sql, params, mapper);
        int pageSize = Math.max(0, searchResult.limit() - 1);
        boolean hasNext = pageSize > 0 && rows.size() > pageSize;
        List<T> content = hasNext ? rows.subList(0, pageSize) : rows;

        String nextCursor = null;
        if (hasNext && !content.isEmpty()) {
            Object lastKept = content.get(content.size() - 1);
            Map<String, Object> cursorValues = buildCursorValues(lastKept, searchResult.sortFields());
            if (!cursorValues.isEmpty()) {
                nextCursor = SearchQueryBuilder.encodeCursor(cursorValues);
            }
        }

        return new CursorPage<>(content, nextCursor, hasNext, content.size());
    }

    private Map<String, Object> buildCursorValues(Object row, List<String> sortFields) {
        Map<String, Object> values = new HashMap<>();
        if (row == null || sortFields == null || sortFields.isEmpty()) {
            return values;
        }

        for (String fieldName : sortFields) {
            if (fieldName == null || fieldName.isBlank()) {
                continue;
            }
            Object fieldValue = readPropertyValue(row, fieldName);
            if (fieldValue != null || hasProperty(row, fieldName)) {
                values.put(fieldName, fieldValue);
            }
        }

        return values;
    }

    private boolean hasProperty(Object row, String fieldName) {
        return findField(row.getClass(), fieldName) != null || findGetter(row.getClass(), fieldName) != null;
    }

    private Object readPropertyValue(Object row, String fieldName) {
        Method getter = findGetter(row.getClass(), fieldName);
        if (getter != null) {
            try {
                return getter.invoke(row);
            }
            catch (Exception e) {
                return null;
            }
        }

        Field field = findField(row.getClass(), fieldName);
        if (field != null) {
            try {
                field.setAccessible(true);
                return field.get(row);
            }
            catch (Exception e) {
                return null;
            }
        }

        return null;
    }

    private Field findField(Class<?> type, String fieldName) {
        Class<?> current = type;
        while (current != null && current != Object.class) {
            try {
                return current.getDeclaredField(fieldName);
            }
            catch (NoSuchFieldException ignored) {
                current = current.getSuperclass();
            }
        }
        return null;
    }

    private Method findGetter(Class<?> type, String fieldName) {
        String capitalized = capitalize(fieldName);
        List<String> candidates = new ArrayList<>();
        candidates.add("get" + capitalized);
        candidates.add("is" + capitalized);

        for (String name : candidates) {
            try {
                return type.getMethod(name);
            }
            catch (NoSuchMethodException ignored) {
            }
        }
        return null;
    }

    private String capitalize(String value) {
        if (value == null || value.isEmpty()) {
            return value;
        }
        return Character.toUpperCase(value.charAt(0)) + value.substring(1);
    }
}