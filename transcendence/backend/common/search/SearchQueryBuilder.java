package transcendence.backend.common.search;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.math.BigDecimal;
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.*;
import java.util.Base64;
import java.util.stream.Collectors;

/**
 * Builder class for constructing SQL search queries based on provided field mappings and search criteria.
 * It generates the SQL string, query parameters, and limit for pagination using cursor-based pagination.
 * 
 * Features:
 * - Dynamic filter validation and SQL clause generation
 * - Type-safe parameter conversion
 * - Cursor-based keyset pagination for efficient large datasets
 * - Support for multiple sort fields with tiebreaker
 * - ILIKE escaping for LIKE/CONTAINS operations
 */
public class SearchQueryBuilder {
    private final String tableName;
    private final String baseQuery;
    private final Map<String, FieldMapping> allowedFields = new LinkedHashMap<>();
    private final List<String> fixedConditions = new ArrayList<>();
    private final Map<String, Object> fixedParameters = new HashMap<>();
    private static final ObjectMapper objectMapper = new ObjectMapper();

    public SearchQueryBuilder(String tableName, String baseQuery) {
        this.tableName = tableName;
        this.baseQuery = baseQuery;
    }

    // Configuration Methods

    /**
     * Registers a field that can be filtered and/or sorted.
     * @param name logical field name (used in SearchPayload)
     * @param column database column name
     * @param type Java type for conversion (String, Integer, LocalDate, etc.)
     * @param filterable whether this field can be used in filters
     * @param sortable whether this field can be used for sorting
     * @return this builder for chaining
     */
    public SearchQueryBuilder field(String name, String column, Class<?> type, boolean filterable, boolean sortable) {
        if (allowedFields.containsKey(name)) {
            throw new IllegalArgumentException("Field already defined: " + name);
        }
        allowedFields.put(name, new FieldMapping(column, type, filterable, sortable));
        return this;
    }

    /**
     * Adds a fixed WHERE condition that is always applied to the query.
     * @param sql SQL fragment (e.g., "status = :status")
     * @param paramName parameter name for the value, null if no parameter
     * @param value parameter value, null if not needed
     * @return this builder for chaining
     */
    public SearchQueryBuilder fixedCondition(String sql, String paramName, Object value) {
        fixedConditions.add(sql);
        if (paramName != null)
            fixedParameters.put(paramName, value);
        return this;
    }

    /**
     * Builds a complete SearchResult from a SearchPayload.
     * 
     * @param payload contains filters, sort criteria, and pagination info
     * @return SearchResult with SQL, parameters, and limit
     * @throws IllegalArgumentException if filter field is not filterable or sort field is not sortable
     */
    public SearchResult build(SearchPayload payload) {
        Map<String, Object> params = new HashMap<>(fixedParameters);
        List<String> whereClauses = new ArrayList<>(fixedConditions);
        
        // Process filters
        if (payload.getFilters() != null && !payload.getFilters().isEmpty()) {
            for (FilterCriteria filter : payload.getFilters()) {
                String filterClause = buildAndValidateFilterClause(filter, params);
                whereClauses.add(filterClause);
            }
        }
        
        // Build WHERE clause
        String whereClause = whereClauses.isEmpty() 
            ? "" 
            : " WHERE " + String.join(" AND ", whereClauses);
        
        // Process sorts (keyset pagination)
        String orderByClause = "";
        int limit = payload.getPage().getPageSize() + 1; // +1 to detect if there are more records
        
        if (payload.getSort() != null && !payload.getSort().isEmpty()) {
            orderByClause = buildOrderByClause(payload.getSort());
            
            // Add keyset clause if cursor exists
            if (payload.getPage().getEncodedCursor() != null) {
                String keysetClause = buildKeysetClause(
                    decodeCursor(payload.getPage().getEncodedCursor()),
                    payload.getSort(),
                    params
                );
                whereClause = whereClause.isEmpty() 
                    ? " WHERE " + keysetClause 
                    : whereClause + " AND " + keysetClause;
            }
        } else {
            // Default sort by primary key if none specified
            orderByClause = " ORDER BY id ASC";
        }
        
        // Construct final SQL
        String sql = baseQuery + whereClause + orderByClause + " LIMIT " + limit;
        
        return new SearchResult(sql, params, limit);
    }

    /**
     * Builds and validates a single filter clause, converting the field name to column name
     * and generating the appropriate SQL based on the operator.
     * 
     * @param filter the filter criteria with column (field name), operator, and value(s)
     * @param params parameter map to accumulate named parameters
     * @return SQL fragment for this filter (e.g., "username ILIKE :username_filter_0")
     * @throws IllegalArgumentException if field doesn't exist or isn't filterable
     */
    private String buildAndValidateFilterClause(FilterCriteria filter, Map<String, Object> params) {
        String fieldName = filter.getColumn();
        
        if (!allowedFields.containsKey(fieldName)) {
            throw new IllegalArgumentException("Unknown field: " + fieldName);
        }
        
        FieldMapping mapping = allowedFields.get(fieldName);
        if (!mapping.filterable()) {
            throw new IllegalArgumentException("Field not filterable: " + fieldName);
        }
        
        String column = mapping.column();
        FilterOperator operator = filter.getOperator();
        Object value = filter.getValue();
        Object valueTo = filter.getValueTo();
        
        return buildFilterClause(column, operator, value, valueTo, mapping.type(), params);
    }

    /**
     * Generates SQL fragment for a filter condition based on operator.
     * 
     * @param column database column name
     * @param operator filter operator (EQ, CONTAINS, BETWEEN, etc.)
     * @param value primary value
     * @param valueTo secondary value (for BETWEEN)
     * @param targetType target type for conversion
     * @param params parameter map
     * @return SQL fragment (e.g., "username ILIKE :username_0")
     */
    private String buildFilterClause(String column, FilterOperator operator, Object value, 
                                     Object valueTo, Class<?> targetType, Map<String, Object> params) {
        String paramName = generateParamName(column, params);
        
        return switch (operator) {
            case EQ -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " = :" + paramName;
            }
            case NEQ -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " <> :" + paramName;
            }
            case GT -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " > :" + paramName;
            }
            case GTE -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " >= :" + paramName;
            }
            case LT -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " < :" + paramName;
            }
            case LTE -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " <= :" + paramName;
            }
            case BEFORE -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " < :" + paramName;
            }
            case AFTER -> {
                Object converted = convertValue(value, targetType);
                params.put(paramName, converted);
                yield column + " > :" + paramName;
            }
            case BETWEEN -> {
                Object convertedFrom = convertValue(value, targetType);
                Object convertedTo = convertValue(valueTo, targetType);
                String paramNameTo = generateParamName(column + "_to", params);
                params.put(paramName, convertedFrom);
                params.put(paramNameTo, convertedTo);
                yield column + " BETWEEN :" + paramName + " AND :" + paramNameTo;
            }
            case CONTAINS -> {
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, "%" + escaped + "%");
                yield column + " ILIKE :" + paramName;
            }
            case STARTS_WITH -> {
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, escaped + "%");
                yield column + " ILIKE :" + paramName;
            }
            case ENDS_WITH -> {
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, "%" + escaped);
                yield column + " ILIKE :" + paramName;
            }
            case IS_NULL -> column + " IS NULL";
            case IS_NOT_NULL -> column + " IS NOT NULL";
            case IN -> {
                params.put(paramName, value);
                yield column + " = ANY(:" + paramName + ")";
            }
            case NOT_IN -> {
                params.put(paramName, value);
                yield column + " <> ALL(:" + paramName + ")";
            }
        };
    }

    /**
     * Builds the ORDER BY clause from sort criteria, adding id as a tiebreaker.
     * 
     * @param sorts list of sort criteria
     * @return SQL ORDER BY clause (e.g., " ORDER BY username ASC, id ASC")
     * @throws IllegalArgumentException if a sort field doesn't exist or isn't sortable
     */
    private String buildOrderByClause(List<SortCriteria> sorts) {
        List<String> orderClauses = new ArrayList<>();
        
        for (SortCriteria sort : sorts) {
            String fieldName = sort.getColumn();
            
            if (!allowedFields.containsKey(fieldName)) {
                throw new IllegalArgumentException("Unknown field for sort: " + fieldName);
            }
            
            FieldMapping mapping = allowedFields.get(fieldName);
            if (!mapping.sortable()) {
                throw new IllegalArgumentException("Field not sortable: " + fieldName);
            }
            
            String column = mapping.column();
            String direction = sort.getDirection() == SortDirection.ASC ? "ASC" : "DESC";
            orderClauses.add(column + " " + direction);
        }
        
        // Add tiebreaker on id to ensure consistent ordering
        orderClauses.add("id ASC");
        
        return " ORDER BY " + String.join(", ", orderClauses);
    }

    /**
     * Builds keyset pagination clause for cursor-based pagination.
     * Constructs a WHERE condition that retrieves rows after the cursor position.
     * 
     * For example, with sort by (username ASC, id ASC):
     *   (username, id) > (:cursor_username, :cursor_id)
     * 
     * @param cursor decoded cursor data containing column values
     * @param sorts sort criteria that define the order
     * @param params parameter map to accumulate cursor values
     * @return SQL fragment (e.g., "(username, id) > (:cursor_username, :cursor_id)")
     */
    private String buildKeysetClause(CursorData cursor, List<SortCriteria> sorts, Map<String, Object> params) {
        if (cursor == null || cursor.getData() == null || cursor.getData().isEmpty()) {
            return "1=1"; // No cursor, return all rows
        }
        
        List<String> columns = new ArrayList<>();
        List<String> paramNames = new ArrayList<>();
        
        for (SortCriteria sort : sorts) {
            String fieldName = sort.getColumn();
            FieldMapping mapping = allowedFields.get(fieldName);
            String column = mapping.column();
            
            columns.add(column);
            String paramName = "cursor_" + fieldName;
            paramNames.add(":" + paramName);
            
            // Add cursor value to parameters
            Object cursorValue = cursor.getData().get(fieldName);
            if (cursorValue != null) {
                params.put(paramName, convertValue(cursorValue, mapping.type()));
            }
        }
        
        // Build comparison: (col1, col2, ...) > (:cursor_col1, :cursor_col2, ...)
        String columnList = "(" + String.join(", ", columns) + ")";
        String paramList = "(" + String.join(", ", paramNames) + ")";
        
        return columnList + " > " + paramList;
    }

    /**
     * Converts a value to the target type with proper error handling.
     * 
     * @param value the value to convert
     * @param targetType the target Java type
     * @return converted value
     * @throws IllegalArgumentException if conversion fails
     */
    private Object convertValue(Object value, Class<?> targetType) {
        if (value == null) {
            return null;
        }
        
        if (targetType == null || targetType == Object.class || value.getClass() == targetType) {
            return value;
        }
        
        try {
            return switch (targetType.getSimpleName()) {
                case "String" -> String.valueOf(value);
                case "Integer" -> {
                    if (value instanceof Number) yield ((Number) value).intValue();
                    yield Integer.parseInt(String.valueOf(value));
                }
                case "Long" -> {
                    if (value instanceof Number) yield ((Number) value).longValue();
                    yield Long.parseLong(String.valueOf(value));
                }
                case "Double" -> {
                    if (value instanceof Number) yield ((Number) value).doubleValue();
                    yield Double.parseDouble(String.valueOf(value));
                }
                case "Float" -> {
                    if (value instanceof Number) yield ((Number) value).floatValue();
                    yield Float.parseFloat(String.valueOf(value));
                }
                case "Boolean" -> {
                    if (value instanceof Boolean) yield value;
                    yield Boolean.parseBoolean(String.valueOf(value));
                }
                case "BigDecimal" -> {
                    if (value instanceof BigDecimal) yield value;
                    yield new BigDecimal(String.valueOf(value));
                }
                case "LocalDate" -> {
                    if (value instanceof LocalDate) yield value;
                    yield LocalDate.parse(String.valueOf(value));
                }
                case "LocalTime" -> {
                    if (value instanceof LocalTime) yield value;
                    yield LocalTime.parse(String.valueOf(value));
                }
                case "Instant" -> {
                    if (value instanceof Instant) yield value;
                    yield Instant.parse(String.valueOf(value));
                }
                default -> value;
            };
        } catch (Exception e) {
            throw new IllegalArgumentException(
                "Cannot convert value '" + value + "' to type " + targetType.getSimpleName(), e
            );
        }
    }

    /**
     * Escapes special characters for LIKE patterns in PostgreSQL ILIKE.
     * Escapes: backslash, percent, underscore
     * 
     * @param value the string to escape
     * @return escaped string safe for ILIKE patterns
     */
    private String escapeForLike(String value) {
        if (value == null) {
            return "";
        }
        return value
            .replace("\\", "\\\\")
            .replace("%", "\\%")
            .replace("_", "\\_");
    }

    /**
     * Decodes a Base64-encoded cursor string into CursorData.
     * 
     * @param encodedCursor the Base64-encoded cursor
     * @return decoded CursorData, or null if decoding fails
     */
    private CursorData decodeCursor(String encodedCursor) {
        if (encodedCursor == null || encodedCursor.isEmpty()) {
            return null;
        }
        
        try {
            byte[] decodedBytes = Base64.getUrlDecoder().decode(encodedCursor);
            return objectMapper.readValue(decodedBytes, CursorData.class);
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid cursor: " + encodedCursor, e);
        }
    }

    /**
     * Encodes a map of cursor values into a Base64-encoded cursor string.
     * 
     * @param values map of column names to values (typically from the last row)
     * @return Base64-encoded cursor string
     */
    public static String encodeCursor(Map<String, Object> values) {
        if (values == null || values.isEmpty()) {
            return null;
        }
        
        try {
            CursorData cursorData = new CursorData(values);
            byte[] jsonBytes = objectMapper.writeValueAsBytes(cursorData);
            return Base64.getUrlEncoder().withoutPadding().encodeToString(jsonBytes);
        } catch (Exception e) {
            throw new IllegalArgumentException("Failed to encode cursor", e);
        }
    }

    /**
     * Generates a unique parameter name based on column and existing parameters.
     * Avoids collisions by appending an index if needed.
     * 
     * @param column the column name
     * @param params existing parameters map
     * @return unique parameter name
     */
    private String generateParamName(String column, Map<String, Object> params) {
        String baseName = column.replace(".", "_") + "_param";
        String paramName = baseName;
        int index = 0;
        
        while (params.containsKey(paramName)) {
            paramName = baseName + "_" + (index++);
        }
        
        return paramName;
    }
}