package com.transcendence.common.search;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.time.Instant;
import java.time.LocalDate;
import java.util.*;

import static org.assertj.core.api.Assertions.*;

@DisplayName("SearchQueryBuilder Tests")
class SearchQueryBuilderTest {

    private SearchQueryBuilder builder;

    @BeforeEach
    void setUp() {
        builder = new SearchQueryBuilder(
            "users",
            "SELECT u.id, u.username, u.email, u.created_at FROM users u"
        );
        
        builder
            .field("id", "u.id", Long.class, true, true)
            .field("username", "u.username", String.class, true, true)
            .field("email", "u.email", String.class, true, true)
            .field("createdAt", "u.created_at", Instant.class, true, true)
            .field("active", "u.active", Boolean.class, true, true);
    }

    // ==================== Field Registration Tests ====================

    @Test
    @DisplayName("Should register field successfully")
    void testFieldRegistration() {
        Map<String, FieldMapping> fields = builder.getAllowedFields();
        
        assertThat(fields).containsKeys("id", "username", "email", "createdAt", "active");
        assertThat(fields.get("username").column()).isEqualTo("u.username");
        assertThat(fields.get("username").type()).isEqualTo(String.class);
        assertThat(fields.get("username").filterable()).isTrue();
        assertThat(fields.get("username").sortable()).isTrue();
    }

    @Test
    @DisplayName("Should throw exception when registering duplicate field")
    void testDuplicateFieldRegistration() {
        assertThatThrownBy(() -> builder.field("username", "u.username", String.class, true, true))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Field already defined");
    }

    @Test
    @DisplayName("Should register non-filterable and non-sortable fields")
    void testFieldWithLimitedPermissions() {
        SearchQueryBuilder customBuilder = new SearchQueryBuilder("test", "SELECT * FROM test");
        customBuilder.field("secret", "t.secret", String.class, false, false);
        
        Map<String, FieldMapping> fields = customBuilder.getAllowedFields();
        assertThat(fields.get("secret").filterable()).isFalse();
        assertThat(fields.get("secret").sortable()).isFalse();
    }

    // ==================== Fixed Conditions Tests ====================

    @Test
    @DisplayName("Should add fixed condition without parameters")
    void testFixedConditionWithoutParam() {
        builder.fixedCondition("u.deleted_at IS NULL", null, null);
        
        SearchResult result = builder.build(null);
        assertThat(result.sql()).contains("u.deleted_at IS NULL");
    }

    @Test
    @DisplayName("Should add fixed condition with parameters")
    void testFixedConditionWithParam() {
        builder.fixedCondition("u.tenant_id = :tenantId", "tenantId", 123L);
        
        SearchResult result = builder.build(null);
        assertThat(result.sql()).contains("u.tenant_id = :tenantId");
        assertThat(result.parameters()).containsEntry("tenantId", 123L);
    }

    // ==================== Build with Null Payload Tests ====================

    @Test
    @DisplayName("Should handle null payload with default sort by id")
    void testBuildWithNullPayload() {
        SearchResult result = builder.build(null);
        
        assertThat(result.sql()).contains("ORDER BY id ASC");
        assertThat(result.sql()).contains("LIMIT 21"); // pageSize (20) + 1
        assertThat(result.sortFields()).containsExactly("id");
    }

    @Test
    @DisplayName("Should handle null page in payload")
    void testBuildWithNullPage() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(null);
        payload.setSort(null);
        payload.setPage(null);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY id ASC");
    }

    // ==================== Filter Tests ====================

    @Test
    @DisplayName("Should build EQ filter clause")
    void testEqualFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.EQ, "john_doe")));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username = :");
        assertThat(result.parameters().values()).contains("john_doe");
    }

    @Test
    @DisplayName("Should build NEQ filter clause")
    void testNotEqualFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.NEQ, "admin")));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username <> :");
    }

    @Test
    @DisplayName("Should build GT filter clause")
    void testGreaterThanFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.GT, 100L)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id > :");
        assertThat(result.parameters().values()).contains(100L);
    }

    @Test
    @DisplayName("Should build GTE filter clause")
    void testGreaterThanEqualFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.GTE, 50L)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id >= :");
    }

    @Test
    @DisplayName("Should build LT filter clause")
    void testLessThanFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.LT, 200L)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id < :");
    }

    @Test
    @DisplayName("Should build LTE filter clause")
    void testLessThanEqualFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.LTE, 150L)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id <= :");
    }

    @Test
    @DisplayName("Should build BETWEEN filter clause")
    void testBetweenFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.BETWEEN, 10L, 100L)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id BETWEEN :");
        assertThat(result.sql()).contains("AND :");
        assertThat(result.parameters().values()).contains(10L, 100L);
    }

    @Test
    @DisplayName("Should throw exception for BETWEEN without valueTo")
    void testBetweenFilterMissingValueTo() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.BETWEEN, 10L)));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("BETWEEN requires valueTo");
    }

    @Test
    @DisplayName("Should build CONTAINS filter clause with ILIKE")
    void testContainsFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.CONTAINS, "john")));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username ILIKE");
        assertThat(result.sql()).contains("ESCAPE");
        Object paramValue = result.parameters().values().stream().findFirst().orElse(null);
        assertThat(paramValue).asString().contains("%john%");
    }

    @Test
    @DisplayName("Should build STARTS_WITH filter clause")
    void testStartsWithFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.STARTS_WITH, "admin")));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username ILIKE");
    }

    @Test
    @DisplayName("Should build ENDS_WITH filter clause")
    void testEndsWithFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("email", FilterOperator.ENDS_WITH, "@example.com")));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.email ILIKE");
    }

    @Test
    @DisplayName("Should build IS_NULL filter clause")
    void testIsNullFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.IS_NULL, null)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username IS NULL");
    }

    @Test
    @DisplayName("Should build IS_NOT_NULL filter clause")
    void testIsNotNullFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.IS_NOT_NULL, null)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username IS NOT NULL");
    }

    @Test
    @DisplayName("Should build IN filter clause with list")
    void testInFilterWithList() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.IN, List.of(1L, 2L, 3L))));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id = ANY(:") ;
        assertThat(result.parameters().values()).contains(List.of(1L, 2L, 3L));
    }

    @Test
    @DisplayName("Should build IN filter clause with array")
    void testInFilterWithArray() {
        SearchPayload payload = new SearchPayload();
        Long[] ids = {1L, 2L, 3L};
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.IN, ids)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id = ANY(:") ;
    }

    @Test
    @DisplayName("Should throw exception for IN filter with null value")
    void testInFilterWithNullValue() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.IN, null)));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("IN requires a non-null collection");
    }

    @Test
    @DisplayName("Should build NOT_IN filter clause")
    void testNotInFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.NOT_IN, List.of(5L, 6L))));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.id <> ALL(:") ;
    }

    @Test
    @DisplayName("Should throw exception for null filter")
    void testNullFilter() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(Collections.singletonList(null));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Filter cannot be null");
    }

    @Test
    @DisplayName("Should throw exception for filter with empty column")
    void testFilterWithEmptyColumn() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("", FilterOperator.EQ, "value")));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Filter field cannot be empty");
    }

    @Test
    @DisplayName("Should throw exception for unknown filter field")
    void testFilterWithUnknownField() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("unknownField", FilterOperator.EQ, "value")));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Unknown field");
    }

    @Test
    @DisplayName("Should throw exception for filtering non-filterable field")
    void testFilterNonFilterableField() {
        SearchQueryBuilder customBuilder = new SearchQueryBuilder("test", "SELECT * FROM test");
        customBuilder.field("secret", "t.secret", String.class, false, true);
        
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("secret", FilterOperator.EQ, "value")));
        
        assertThatThrownBy(() -> customBuilder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("not filterable");
    }

    @Test
    @DisplayName("Should escape special ILIKE characters")
    void testEscapeForLikeCharacters() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.CONTAINS, "john_50%")));
        
        SearchResult result = builder.build(payload);
        
        Object paramValue = result.parameters().values().stream().findFirst().orElse(null);
        assertThat(paramValue).asString().contains("\\%").contains("\\_");
    }

    // ==================== Sort Tests ====================

    @Test
    @DisplayName("Should build ORDER BY with single sort field")
    void testSingleSort() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria("username", SortDirection.ASC)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY u.username ASC");
        assertThat(result.sortFields()).containsExactly("username", "id");
    }

    @Test
    @DisplayName("Should build ORDER BY with multiple sort fields")
    void testMultipleSort() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(
            new SortCriteria("username", SortDirection.ASC),
            new SortCriteria("email", SortDirection.DESC)
        ));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY u.username ASC, u.email DESC");
    }

    @Test
    @DisplayName("Should add id as tiebreaker only if not already present")
    void testIdTiebreakerNotDuplicated() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria("id", SortDirection.ASC)));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY u.id ASC");
        assertThat(result.sortFields()).containsExactly("id");
        assertThat(result.sortFields()).doesNotHaveDuplicates();
    }

    @Test
    @DisplayName("Should throw exception for null sort field")
    void testSortWithNullField() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria(null, SortDirection.ASC)));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Sort field cannot be empty");
    }

    @Test
    @DisplayName("Should throw exception for unknown sort field")
    void testSortWithUnknownField() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria("unknownField", SortDirection.ASC)));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Unknown field for sort");
    }

    @Test
    @DisplayName("Should throw exception for non-sortable field")
    void testSortNonSortableField() {
        SearchQueryBuilder customBuilder = new SearchQueryBuilder("test", "SELECT * FROM test");
        customBuilder.field("data", "t.data", String.class, true, false);
        
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria("data", SortDirection.ASC)));
        
        assertThatThrownBy(() -> customBuilder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("not sortable");
    }

    @Test
    @DisplayName("Should handle default sort direction (ASC)")
    void testDefaultSortDirection() {
        SearchPayload payload = new SearchPayload();
        SortCriteria sort = new SortCriteria("username", null);
        payload.setSort(List.of(sort));
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("u.username ASC");
    }

    // ==================== Cursor Pagination Tests ====================

    @Test
    @DisplayName("Should encode and decode cursor correctly")
    void testCursorEncodeDecode() {
        Map<String, Object> cursorValues = new HashMap<>();
        cursorValues.put("username", "john_doe");
        cursorValues.put("id", 42L);
        
        String encoded = SearchQueryBuilder.encodeCursor(cursorValues);
        assertThat(encoded).isNotNull().isNotEmpty();
    }

    @Test
    @DisplayName("Should return null for empty cursor values")
    void testEncodeCursorEmpty() {
        String encoded = SearchQueryBuilder.encodeCursor(new HashMap<>());
        assertThat(encoded).isNull();
    }

    @Test
    @DisplayName("Should return null for null cursor values")
    void testEncodeCursorNull() {
        String encoded = SearchQueryBuilder.encodeCursor(null);
        assertThat(encoded).isNull();
    }

    @Test
    @DisplayName("Should throw exception for invalid cursor")
    void testInvalidCursor() {
        SearchPayload payload = new SearchPayload();
        CursorPageRequest page = new CursorPageRequest();
        page.setEncodedCursor("invalid-base64-!!!!");
        payload.setPage(page);
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Invalid cursor");
    }

    @Test
    @DisplayName("Should handle pagination with cursor")
    void testPaginationWithCursor() {
        Map<String, Object> cursorValues = new HashMap<>();
        cursorValues.put("username", "john_doe");
        cursorValues.put("id", 42L);
        String cursor = SearchQueryBuilder.encodeCursor(cursorValues);
        
        SearchPayload payload = new SearchPayload();
        payload.setSort(List.of(new SortCriteria("username", SortDirection.ASC)));
        CursorPageRequest page = new CursorPageRequest();
        page.setEncodedCursor(cursor);
        page.setPageSize(10);
        payload.setPage(page);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("(u.username, id) >");
        assertThat(result.parameters()).containsKeys("cursor_username", "cursor_id");
    }

    // ==================== Type Conversion Tests ====================

    @Test
    @DisplayName("Should convert String type correctly")
    void testTypeConversionString() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("username", FilterOperator.EQ, 123)));
        
        SearchResult result = builder.build(payload);
        
        Object value = result.parameters().values().stream().findFirst().orElse(null);
        assertThat(value).isInstanceOf(String.class).isEqualTo("123");
    }

    @Test
    @DisplayName("Should convert Integer type correctly")
    void testTypeConversionInteger() {
        SearchQueryBuilder customBuilder = new SearchQueryBuilder("test", "SELECT * FROM test");
        customBuilder.field("count", "t.count", Integer.class, true, true);
        
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("count", FilterOperator.EQ, "42")));
        
        SearchResult result = customBuilder.build(payload);
        
        Object value = result.parameters().values().stream().findFirst().orElse(null);
        assertThat(value).isInstanceOf(Integer.class).isEqualTo(42);
    }

    @Test
    @DisplayName("Should convert Boolean type correctly")
    void testTypeConversionBoolean() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("active", FilterOperator.EQ, "true")));
        
        SearchResult result = builder.build(payload);
        
        Object value = result.parameters().values().stream().findFirst().orElse(null);
        assertThat(value).isInstanceOf(Boolean.class).isEqualTo(true);
    }

    @Test
    @DisplayName("Should throw exception for invalid type conversion")
    void testTypeConversionFailure() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(new FilterCriteria("id", FilterOperator.EQ, "not_a_number")));
        
        assertThatThrownBy(() -> builder.build(payload))
            .isInstanceOf(IllegalArgumentException.class)
            .hasMessageContaining("Cannot convert");
    }

    // ==================== Pagination Limit Tests ====================

    @Test
    @DisplayName("Should limit page size to maximum of 100")
    void testMaximumPageSize() {
        SearchPayload payload = new SearchPayload();
        CursorPageRequest page = new CursorPageRequest();
        page.setPageSize(500);
        payload.setPage(page);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("LIMIT 101");
    }

    @Test
    @DisplayName("Should use default page size of 20 if not specified")
    void testDefaultPageSize() {
        SearchPayload payload = new SearchPayload();
        CursorPageRequest page = new CursorPageRequest();
        page.setPageSize(0);
        payload.setPage(page);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("LIMIT 21");
    }

    @Test
    @DisplayName("Should apply LIMIT pageSize + 1 for hasNext detection")
    void testLimitPlusOneForPagination() {
        SearchPayload payload = new SearchPayload();
        CursorPageRequest page = new CursorPageRequest();
        page.setPageSize(15);
        payload.setPage(page);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("LIMIT 16");
    }

    // ==================== Combined Filter and Sort Tests ====================

    @Test
    @DisplayName("Should combine multiple filters and sorts correctly")
    void testComplexQuery() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of(
            new FilterCriteria("username", FilterOperator.CONTAINS, "john"),
            new FilterCriteria("active", FilterOperator.EQ, true)
        ));
        payload.setSort(List.of(
            new SortCriteria("createdAt", SortDirection.DESC),
            new SortCriteria("username", SortDirection.ASC)
        ));
        CursorPageRequest page = new CursorPageRequest();
        page.setPageSize(50);
        payload.setPage(page);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql())
            .contains("WHERE")
            .contains("u.username ILIKE")
            .contains("u.active =")
            .contains("AND")
            .contains("ORDER BY u.created_at DESC, u.username ASC");
        assertThat(result.sql()).contains("LIMIT 51");
    }

    // ==================== Edge Cases ====================

    @Test
    @DisplayName("Should handle empty filter list")
    void testEmptyFilterList() {
        SearchPayload payload = new SearchPayload();
        payload.setFilters(List.of());
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY");
    }

    @Test
    @DisplayName("Should handle null sort list")
    void testNullSortList() {
        SearchPayload payload = new SearchPayload();
        payload.setSort(null);
        
        SearchResult result = builder.build(payload);
        
        assertThat(result.sql()).contains("ORDER BY id ASC");
    }

    @Test
    @DisplayName("Should build base query with no conditions")
    void testMinimalQuery() {
        SearchResult result = builder.build(null);
        
        assertThat(result.sql())
            .startsWith("SELECT u.id, u.username, u.email, u.created_at FROM users u")
            .contains("ORDER BY")
            .contains("LIMIT");
    }
}
