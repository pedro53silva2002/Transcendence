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

    public SearchQueryBuilder1 field(String name, String column, Class<?> type, boolean filterable, boolean sortable) {
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
        if (paramName != null) {
            fixedParameters.put(paramName, value);
        }
        return this;
    }

    /**
     * Constrói a query SQL final a partir do SearchPayload.
     *
     * Passos internos:
     * 1. Validar filtros contra allowedFields
     * 2. Construir WHERE (fixas + dinâmicas)
     * 3. Construir ORDER BY (com tiebreaker)
     * 4. Aplicar keyset pagination (se cursor presente)
     * 5. Aplicar LIMIT (size + 1)
     * 6. Retornar SearchResult
     *
     * @param payload O SearchPayload do frontend
     * @return SearchResult com SQL, params e limit
     * @throws IllegalArgumentException se um campo não existir em allowedFields
     * @throws IllegalArgumentException se um campo não for sortable
     */
    public SearchResult build(SearchPayload payload) {
        SearchPayload effectivePayload = payload == null ? new SearchPayload() : payload;
        CursorPageRequest page = effectivePayload.getPage() == null ? new CursorPageRequest() : effectivePayload.getPage();

        Map<String, Object> params = new HashMap<>(fixedParameters);
        List<String> whereClauses = new ArrayList<>(fixedConditions);

        if (effectivePayload.getFilters() != null && !effectivePayload.getFilters().isEmpty()) {
            for (FilterCriteria filter : effectivePayload.getFilters()) {
                whereClauses.add(buildAndValidateFilterClause(filter, params));
            }
        }

        String orderByClause;
        List<SortCriteria> sorts = effectivePayload.getSort();
        if (sorts != null && !sorts.isEmpty()) {
            orderByClause = buildOrderByClause(sorts);
        } else {
            orderByClause = " ORDER BY id ASC";
        }

        if (page.getEncodedCursor() != null && !page.getEncodedCursor().isBlank()) {
            CursorData cursor = decodeCursor(page.getEncodedCursor());
            String keysetClause = buildKeysetClause(
                cursor,
                sorts == null ? List.of(new SortCriteria("id", SortDirection.ASC)) : sorts,
                params
            );
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
            case GT, AFTER -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " > :" + paramName;
            }
            case GTE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " >= :" + paramName;
            }
            case LT, BEFORE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " < :" + paramName;
            }
            case LTE -> {
                params.put(paramName, convertValue(value, targetType));
                yield column + " <= :" + paramName;
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
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, "%" + escaped + "%");
                yield column + " ILIKE :" + paramName + " ESCAPE '\\\\'";
            }
            case STARTS_WITH -> {
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, escaped + "%");
                yield column + " ILIKE :" + paramName + " ESCAPE '\\\\'";
            }
            case ENDS_WITH -> {
                String escaped = escapeForLike(String.valueOf(value));
                params.put(paramName, "%" + escaped);
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

    private Object normalizeToCollection(Object value, String column, FilterOperator operator) {
        if (value == null) {
            throw new IllegalArgumentException(operator + " requires a non-null collection for column: " + column);
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
        throw new IllegalArgumentException(operator + " requires a list/array for column: " + column);
    }

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
            String dir = direction == SortDirection.ASC ? "ASC" : "DESC";
            orderClauses.add(mapping.column() + " " + dir);
            if ("id".equalsIgnoreCase(mapping.column())) {
                includesId = true;
            }
        }

        if (!includesId) {
            orderClauses.add("id ASC");
        }

        return " ORDER BY " + String.join(", ", orderClauses);
    }

    private String buildKeysetClause(CursorData cursor, List<SortCriteria> sorts, Map<String, Object> params) {
        if (cursor == null || cursor.getData() == null || cursor.getData().isEmpty()) {
            return "1=1";
        }

        List<SortCriteria> effectiveSorts = (sorts == null || sorts.isEmpty())
            ? List.of(new SortCriteria("id", SortDirection.ASC))
            : sorts;

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

    private Object convertValue(Object value, Class<?> targetType) {
        if (value == null || targetType == null || targetType == Object.class || targetType.isInstance(value)) {
            return value;
        }

        try {
            return switch (targetType.getSimpleName()) {
                case "String" -> String.valueOf(value);
                case "Integer" -> value instanceof Number n ? n.intValue() : Integer.parseInt(String.valueOf(value));
                case "Long" -> value instanceof Number n ? n.longValue() : Long.parseLong(String.valueOf(value));
                case "Double" -> value instanceof Number n ? n.doubleValue() : Double.parseDouble(String.valueOf(value));
                case "Float" -> value instanceof Number n ? n.floatValue() : Float.parseFloat(String.valueOf(value));
                case "Boolean" -> value instanceof Boolean b ? b : Boolean.parseBoolean(String.valueOf(value));
                case "BigDecimal" -> value instanceof BigDecimal bd ? bd : new BigDecimal(String.valueOf(value));
                case "LocalDate" -> value instanceof LocalDate d ? d : LocalDate.parse(String.valueOf(value));
                case "LocalTime" -> value instanceof LocalTime t ? t : LocalTime.parse(String.valueOf(value));
                case "Instant" -> value instanceof Instant i ? i : Instant.parse(String.valueOf(value));
                default -> value;
            };
        } catch (Exception e) {
            throw new IllegalArgumentException("Cannot convert value '" + value + "' to type " + targetType.getSimpleName(), e);
        }
    }

    private String escapeForLike(String value) {
        if (value == null) {
            return "";
        }
        return value
            .replace("\\", "\\\\")
            .replace("%", "\\%")
            .replace("_", "\\_");
    }

    private CursorData decodeCursor(String cursor) {
        try {
            byte[] bytes = Base64.getUrlDecoder().decode(cursor);
            return objectMapper.readValue(bytes, CursorData.class);
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid cursor", e);
        }
    }

    public static String encodeCursor(Map<String, Object> values) {
        try {
            byte[] bytes = objectMapper.writeValueAsBytes(new CursorData(values));
            return Base64.getUrlEncoder().withoutPadding().encodeToString(bytes);
        } catch (Exception e) {
            throw new IllegalArgumentException("Failed to encode cursor", e);
        }
    }

    private String generateParamName(String column, Map<String, Object> params) {
        String base = column.replace('.', '_') + "_param";
        String current = base;
        int i = 0;
        while (params.containsKey(current)) {
            i++;
            current = base + "_" + i;
        }
        return current;
    }
}
}