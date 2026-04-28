package transcendence.backend.common.search;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.math.BigDecimal;
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Builder class for constructing SQL search queries based on provided field mappings and search criteria.
 * It generates the SQL string, query parameters, and limit for pagination using cursor-based pagination.
 *
 * Features:
 * - Dynamic filter validation and SQL clause generation
 * - Type-safe parameter conversion
 * - Cursor-based keyset pagination for efficient large datasets
 * - Support for multiple sort fields with tiebreaker (avoids duplicate id tiebreaker)
 * - ILIKE escaping for LIKE/CONTAINS operations
 * - Null-safe build(): handles null payload and null page
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

    /**
     * Registers a field that can be filtered and/or sorted.
     *
     * @param name       logical field name (used in SearchPayload)
     * @param column     database column name (e.g., "u.username")
     * @param type       Java type for value conversion (String, Integer, LocalDate, etc.)
     * @param filterable whether this field can be used in filters
     * @param sortable   whether this field can be used for sorting
     * @return this builder for chaining
     * @throws IllegalArgumentException if the field name is already registered
     */
    public SearchQueryBuilder field(String name, String column, Class<?> type,
                                    boolean filterable, boolean sortable) {
        if (allowedFields.containsKey(name)) {
            throw new IllegalArgumentException("Field already defined: " + name);
        }
        allowedFields.put(name, new FieldMapping(column, type, filterable, sortable));
        return this;
    }

    /**
     * Adds a fixed WHERE condition that is always applied to the query,
     * regardless of the SearchPayload (e.g., tenant isolation, soft-delete filter).
     *
     * @param sql       SQL fragment (e.g., "u.deleted_at IS NULL")
     * @param paramName named parameter key, or null if the condition has no parameter
     * @param value     parameter value, or null if paramName is null
     * @return this builder for chaining
     */
    public SearchQueryBuilder fixedCondition(String sql, String paramName, Object value) {
        fixedConditions.add(sql);
        if (paramName != null) {
            fixedParameters.put(paramName, value);
        }
        return this;
    }

    /**
     * Builds the complete SearchResult from a SearchPayload.
     *
     * Internal steps:
     * 1. Null-safe defaults for payload and page
     * 2. Validate and translate filters into WHERE clauses
     * 3. Build ORDER BY (with safe tiebreaker — only adds id if not already present)
     * 4. Apply keyset pagination clause if cursor is present
     * 5. Apply LIMIT (pageSize + 1 to detect next page)
     * 6. Return SearchResult with SQL, params, and limit
     *
     * @param payload the SearchPayload from the frontend (may be null)
     * @return SearchResult with SQL, parameters, and limit
     * @throws IllegalArgumentException if a filter or sort field is unknown/not allowed
     */
    public SearchResult build(SearchPayload payload) {
        SearchPayload effectivePayload = (payload == null) ? new SearchPayload() : payload;
        CursorPageRequest page = (effectivePayload.getPage() == null)
                ? new CursorPageRequest() : effectivePayload.getPage();

        Map<String, Object> params = new HashMap<>(fixedParameters);
        List<String> whereClauses = new ArrayList<>(fixedConditions);

        if (effectivePayload.getFilters() != null && !effectivePayload.getFilters().isEmpty()) {
            for (FilterCriteria filter : effectivePayload.getFilters()) {
                whereClauses.add(buildAndValidateFilterClause(filter, params));
            }
        }

        List<SortCriteria> sorts = effectivePayload.getSort();
        String orderByClause;
        if (sorts != null && !sorts.isEmpty()) {
            orderByClause = buildOrderByClause(sorts);
        }
        else {
            orderByClause = " ORDER BY id ASC";
            sorts = List.of(new SortCriteria("id", SortDirection.ASC));
        }

        if (page.getEncodedCursor() != null && !page.getEncodedCursor().isBlank()) {
            CursorData cursor = decodeCursor(page.getEncodedCursor());
            String keysetClause = buildKeysetClause(cursor, sorts, params);
            if (!"1=1".equals(keysetClause)) {
                whereClauses.add(keysetClause);
            }
        }

        String whereClause = whereClauses.isEmpty() ? "" : " WHERE " + String.join(" AND ", whereClauses);

        int requestedSize = page.getPageSize() <= 0 ? 20 : page.getPageSize();
        int size = Math.min(requestedSize, 100);
        int sqlLimit = size + 1;

        String sql = baseQuery + whereClause + orderByClause + " LIMIT " + sqlLimit;
        return new SearchResult(sql, params, sqlLimit);
    }

    /**
     * Validates a FilterCriteria against the allowedFields map and builds the SQL clause.
     *
     * @param filter the filter to validate and translate
     * @param params parameter accumulator
     * @return SQL fragment (e.g., "u.username ILIKE :u_username_param")
     * @throws IllegalArgumentException if filter is null, field is unknown, or field is not filterable
     */
    private String buildAndValidateFilterClause(FilterCriteria filter, Map<String, Object> params) {
        if (filter == null) {
            throw new IllegalArgumentException("Filter cannot be null");
        }
        String fieldName = filter.getColumn();
        if (fieldName == null || fieldName.isBlank()) {
            throw new IllegalArgumentException("Filter field cannot be empty");
        }

        FieldMapping mapping = allowedFields.get(fieldName);
        if (mapping == null) {
            throw new IllegalArgumentException("Unknown field: " + fieldName);
        }
        if (!mapping.filterable()) {
            throw new IllegalArgumentException("Field not filterable: " + fieldName);
        }
        if (filter.getOperator() == null) {
            throw new IllegalArgumentException("Filter operator cannot be null for field: " + fieldName);
        }

        return buildFilterClause(
            mapping.column(),
            filter.getOperator(),
            filter.getValue(),
            filter.getValueTo(),
            mapping.type(),
            params
        );
    }

    /**
     * Generates the SQL fragment for a single filter condition.
     *
     * Operators:
     * - EQ / NEQ / GT / GTE / LT / LTE: standard comparisons
     * - AFTER / BEFORE: semantic aliases for GT / LT (useful for date fields)
     * - BETWEEN: inclusive range, requires valueTo
     * - CONTAINS / STARTS_WITH / ENDS_WITH: ILIKE with escape
     * - IS_NULL / IS_NOT_NULL: no parameter needed
     * - IN / NOT_IN: list comparison via ANY / ALL
     *
     * @param column     database column name
     * @param operator   filter operator
     * @param value      primary value
     * @param valueTo    secondary value (BETWEEN only)
     * @param targetType Java type for conversion
     * @param params     parameter accumulator
     * @return SQL fragment (e.g., "u.created_at BETWEEN :u_created_at_param AND :u_created_at_to_param")
     */
    private String buildFilterClause(String column, FilterOperator operator, Object value,
                                     Object valueTo, Class<?> targetType, Map<String, Object> params) {
        String paramName = generateParamName(column, params);

        return switch (operator) {
            case EQ -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " = :" + paramName;
            }
            case NEQ -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " <> :" + paramName;
            }
            case GT -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " > :" + paramName;
            }
            case GTE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " >= :" + paramName;
            }
            case LT -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " < :" + paramName;
            }
            case LTE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " <= :" + paramName;
            }
            case AFTER -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " > :" + paramName;
            }
            case BEFORE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " < :" + paramName;
            }
            case BETWEEN -> {
                if (valueTo == null) {
                    throw new IllegalArgumentException("BETWEEN requires valueTo for column: " + column);
                }
                String paramNameTo = generateParamName(column + "_to", params);
                params.put(paramName, convertValue(value, targetType));
                params.put(paramNameTo, convertValue(valueTo, targetType));
                yield column + " BETWEEN :" + paramName + " AND :" + paramNameTo;
            }
            case CONTAINS -> {
                params.put(paramName, "%" + escapeForLike(String.valueOf(value)) + "%");
                yield column + " ILIKE :" + paramName + " ESCAPE '\\\\'";
            }
            case STARTS_WITH -> {
                params.put(paramName, escapeForLike(String.valueOf(value)) + "%");
                yield column + " ILIKE :" + paramName + " ESCAPE '\\\\'";
            }
            case ENDS_WITH -> {
                params.put(paramName, "%" + escapeForLike(String.valueOf(value)));
                yield column + " ILIKE :" + paramName + " ESCAPE '\\\\'";
            }
            case IS_NULL -> column + " IS NULL";
            case IS_NOT_NULL -> column + " IS NOT NULL";
            case IN -> {
                params.put(paramName, normalizeToCollection(value, column, operator));
                yield column + " = ANY(:" + paramName + ")";
            }
            case NOT_IN -> {
                params.put(paramName, normalizeToCollection(value, column, operator));
                yield column + " <> ALL(:" + paramName + ")";
            }
        };
    }

    /**
     * Normalizes a value to a List for use with IN / NOT_IN operators.
     *
     * @param value    must be a List or array
     * @param column   column name (for error messages)
     * @param operator operator (for error messages)
     * @return List of values
     * @throws IllegalArgumentException if value is null or not a list/array
     */
    private Object normalizeToCollection(Object value, String column, FilterOperator operator) {
        if (value == null) {
            throw new IllegalArgumentException(
                    operator + " requires a non-null collection for column: " + column);
        }
        if (value instanceof List<?> list) {
            return list;
        }
        if (value.getClass().isArray()) {
            int length = java.lang.reflect.Array.getLength(value);
            List<Object> list = new ArrayList<>(length);
            for (int i = 0; i < length; i++) {
                list.add(java.lang.reflect.Array.get(value, i));
            }
            return list;
        }
        throw new IllegalArgumentException(
                operator + " requires a list/array for column: " + column);
    }

    /**
     * Builds the ORDER BY clause from the provided sort criteria.
     * Always appends "id ASC" as a tiebreaker — but only if id is not already present.
     *
     * @param sorts list of sort criteria (must not be null or empty)
     * @return SQL ORDER BY clause (e.g., " ORDER BY u.username ASC, id ASC")
     * @throws IllegalArgumentException if a sort field is unknown or not sortable
     */
    private String buildOrderByClause(List<SortCriteria> sorts) {
        List<String> orderClauses = new ArrayList<>();
        boolean includesId = false;

        for (SortCriteria sort : sorts) {
            if (sort == null || sort.getColumn() == null || sort.getColumn().isBlank()) {
                throw new IllegalArgumentException("Sort field cannot be empty");
            }

            String fieldName = sort.getColumn();
            FieldMapping mapping = allowedFields.get(fieldName);
            if (mapping == null) {
                throw new IllegalArgumentException("Unknown field for sort: " + fieldName);
            }
            if (!mapping.sortable()) {
                throw new IllegalArgumentException("Field not sortable: " + fieldName);
            }

            SortDirection direction = sort.getDirection() == null ? SortDirection.ASC : sort.getDirection();
            orderClauses.add(mapping.column() + " " + (direction == SortDirection.ASC ? "ASC" : "DESC"));
            
            if ("id".equalsIgnoreCase(mapping.column())) {
                includesId = true;
            }
        }

        if (!includesId) {
            orderClauses.add("id ASC");
        }

        return " ORDER BY " + String.join(", ", orderClauses);
    }

    /**
     * Builds the keyset pagination WHERE clause for cursor-based pagination.
     *
     * Example with sort by (username ASC, id ASC):
     *   (u.username, id) > (:cursor_username, :cursor_id)
     *
     * @param cursor decoded cursor data containing last-seen column values
     * @param sorts  sort criteria that define the order
     * @param params parameter accumulator for cursor values
     * @return SQL keyset clause, or "1=1" if cursor is empty/invalid
     */
    private String buildKeysetClause(CursorData cursor, List<SortCriteria> sorts,
                                     Map<String, Object> params) {
        if (cursor == null || cursor.getData() == null || cursor.getData().isEmpty()) {
            return "1=1";
        }

        List<String> columns = new ArrayList<>();
        List<String> paramRefs = new ArrayList<>();

        for (SortCriteria sort : effectiveSorts) {
            FieldMapping mapping = allowedFields.get(sort.getColumn());
            if (mapping == null) {
                throw new IllegalArgumentException("Unknown field in cursor sorting: " + sort.getColumn());
            }

            String cursorKey = sort.getColumn();
            if (!cursor.getData().containsKey(cursorKey)) {
                return "1=1";
            }

            String paramName = "cursor_" + cursorKey;
            columns.add(mapping.column());
            paramRefs.add(":" + paramName);
            params.put(paramName, convertValue(cursor.getData().get(cursorKey), mapping.type()));
        }

        return "(" + String.join(", ", columns) + ") > (" + String.join(", ", paramRefs) + ")";
    }

    /**
     * Decodes a Base64-encoded cursor string into CursorData.
     *
     * @param encodedCursor Base64url-encoded cursor (from previous response)
     * @return decoded CursorData
     * @throws IllegalArgumentException if the cursor is malformed or cannot be decoded
     */
    private CursorData decodeCursor(String encodedCursor) {
        if (encodedCursor == null || encodedCursor.isBlank()) {
            return null;
        }
        try {
            byte[] bytes = Base64.getUrlDecoder().decode(encodedCursor);
            return objectMapper.readValue(bytes, CursorData.class);
        }
        catch (Exception e) {
            throw new IllegalArgumentException("Invalid cursor: " + encodedCursor, e);
        }
    }

    /**
     * Encodes a map of cursor values (typically the last row's sort fields) into
     * a Base64url-encoded cursor string to be sent to the frontend.
     *
     * @param values map of field names to their last-seen values
     * @return Base64url-encoded cursor string, or null if values is null/empty
     */
    public static String encodeCursor(Map<String, Object> values) {
        if (values == null || values.isEmpty()) {
            return null;
        }
        try {
            byte[] bytes = objectMapper.writeValueAsBytes(new CursorData(values));
            return Base64.getUrlEncoder().withoutPadding().encodeToString(bytes);
        }
        catch (Exception e) {
            throw new IllegalArgumentException("Failed to encode cursor", e);
        }
    }

    /**
     * Converts a raw value (typically deserialized from JSON) to the required Java type.
     * Supports: String, Integer, Long, Double, Float, Boolean, BigDecimal,
     *           LocalDate, LocalTime, Instant.
     *
     * @param value      the value to convert
     * @param targetType the target Java type
     * @return converted value, or original value if no conversion is needed
     * @throws IllegalArgumentException if conversion fails
     */
    private Object convertValue(Object value, Class<?> targetType) {
        if (value == null || targetType == null || targetType == Object.class
                || targetType.isInstance(value)) {
            return value;
        }

        try {
            return switch (targetType.getSimpleName()) {
                case "String"     -> String.valueOf(value);
                case "Integer"    -> value instanceof Number n ? n.intValue()    : Integer.parseInt(String.valueOf(value));
                case "Long"       -> value instanceof Number n ? n.longValue()   : Long.parseLong(String.valueOf(value));
                case "Double"     -> value instanceof Number n ? n.doubleValue() : Double.parseDouble(String.valueOf(value));
                case "Float"      -> value instanceof Number n ? n.floatValue()  : Float.parseFloat(String.valueOf(value));
                case "Boolean"    -> value instanceof Boolean b ? b              : Boolean.parseBoolean(String.valueOf(value));
                case "BigDecimal" -> value instanceof BigDecimal bd ? bd         : new BigDecimal(String.valueOf(value));
                case "LocalDate"  -> value instanceof LocalDate d ? d            : LocalDate.parse(String.valueOf(value));
                case "LocalTime"  -> value instanceof LocalTime t ? t            : LocalTime.parse(String.valueOf(value));
                case "Instant"    -> value instanceof Instant i ? i              : Instant.parse(String.valueOf(value));
                default           -> value;
            };
        }
        catch (Exception e) {
            throw new IllegalArgumentException("Cannot convert value '" + value + "' to type " + targetType.getSimpleName(), e);
        }
    }

    /**
     * Escapes special ILIKE characters in a string value to prevent wildcard injection.
     * Escapes: backslash → \\, percent → \%, underscore → \_
     *
     * @param value the raw string from the user
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
     * Generates a unique named parameter key for a column, avoiding collisions
     * with existing keys by appending an incrementing index.
     *
     * @param column the column name (dots replaced with underscores)
     * @param params the current parameter map
     * @return unique parameter name (e.g., "u_username_param", "u_username_param_1")
     */
    private String generateParamName(String column, Map<String, Object> params) {
        String base = column.replace('.', '_') + "_param";
        String current = base;
        int i = 0;
        while (params.containsKey(current)) {
            current = base + "_" + (i++);
        }
        return current;
    }
}
