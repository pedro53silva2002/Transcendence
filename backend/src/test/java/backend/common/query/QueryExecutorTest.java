package com.transcendence.common.query;

import com.transcendence.common.search.CursorPage;
import com.transcendence.common.search.SearchResult;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.*;

import static org.assertj.core.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
@DisplayName("QueryExecutor Tests")
class QueryExecutorTest {

    @Mock
    private NamedParameterJdbcTemplate jdbc;

    private QueryExecutor queryExecutor;
    private ObjectMapper objectMapper;

    // Test data class
    public static class User {
        private Long id;
        private String username;
        private String email;

        public User() {}
        public User(Long id, String username, String email) {
            this.id = id;
            this.username = username;
            this.email = email;
        }

        public Long getId() { return id; }
        public void setId(Long id) { this.id = id; }
        public String getUsername() { return username; }
        public void setUsername(String username) { this.username = username; }
        public String getEmail() { return email; }
        public void setEmail(String email) { this.email = email; }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            User user = (User) o;
            return Objects.equals(id, user.id) &&
                    Objects.equals(username, user.username) &&
                    Objects.equals(email, user.email);
        }

        @Override
        public int hashCode() {
            return Objects.hash(id, username, email);
        }
    }

    @BeforeEach
    void setUp() {
        objectMapper = new ObjectMapper();
        queryExecutor = new QueryExecutor(jdbc, objectMapper);
    }

    // ==================== queryAs Tests ====================

    @Test
    @DisplayName("Should return list of entities from query")
    void testQueryAsReturnsList() {
        User user1 = new User(1L, "john_doe", "john@example.com");
        User user2 = new User(2L, "jane_doe", "jane@example.com");
        List<User> expected = List.of(user1, user2);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(expected);

        SearchResult searchResult = new SearchResult("SELECT * FROM users", new HashMap<>(), 20, List.of("id"));
        MapSqlParameterSource params = new MapSqlParameterSource();
        List<User> result = queryExecutor.queryAs("SELECT * FROM users", params, User.class);

        assertThat(result).isEqualTo(expected);
        verify(jdbc, times(1)).query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class));
    }

    @Test
    @DisplayName("Should return empty list when no results")
    void testQueryAsEmptyResult() {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(new ArrayList<>());

        MapSqlParameterSource params = new MapSqlParameterSource();
        List<User> result = queryExecutor.queryAs("SELECT * FROM users WHERE id > 1000", params, User.class);

        assertThat(result).isEmpty();
    }

    @Test
    @DisplayName("Should use ReflectiveRowMapper for queryAs")
    void testQueryAsUsesReflectiveRowMapper() {
        List<User> expected = new ArrayList<>();
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(expected);

        MapSqlParameterSource params = new MapSqlParameterSource();
        queryExecutor.queryAs("SELECT * FROM users", params, User.class);

        ArgumentCaptor<RowMapper<?>> mapperCaptor = ArgumentCaptor.forClass(RowMapper.class);
        verify(jdbc).query(anyString(), any(MapSqlParameterSource.class), mapperCaptor.capture());

        assertThat(mapperCaptor.getValue()).isNotNull();
    }

    // ==================== queryAsSingle Tests ====================

    @Test
    @DisplayName("Should return single entity as Optional")
    void testQueryAsSingleReturnsOptional() {
        User expected = new User(1L, "john_doe", "john@example.com");
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(expected));

        MapSqlParameterSource params = new MapSqlParameterSource();
        Optional<User> result = queryExecutor.queryAsSingle("SELECT * FROM users WHERE id = :id", params, User.class);

        assertThat(result).isPresent().contains(expected);
    }

    @Test
    @DisplayName("Should return empty Optional when no result")
    void testQueryAsSingleEmptyResult() {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(new ArrayList<>());

        MapSqlParameterSource params = new MapSqlParameterSource();
        Optional<User> result = queryExecutor.queryAsSingle("SELECT * FROM users WHERE id = :id", params, User.class);

        assertThat(result).isEmpty();
    }

    @Test
    @DisplayName("Should throw exception when multiple results returned")
    void testQueryAsSingleThrowsWhenMultiple() {
        User user1 = new User(1L, "john_doe", "john@example.com");
        User user2 = new User(2L, "jane_doe", "jane@example.com");
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user1, user2));

        MapSqlParameterSource params = new MapSqlParameterSource();
        assertThatThrownBy(() -> queryExecutor.queryAsSingle("SELECT * FROM users", params, User.class))
            .isInstanceOf(MultipleResultsException.class)
            .hasMessageContaining("Expected 0 or 1 row");
    }

    // ==================== queryAsUnchecked Tests ====================

    @Test
    @DisplayName("Should execute query with custom RowMapper")
    void testQueryAsUnchecked() {
        User user1 = new User(1L, "john_doe", "john@example.com");
        User user2 = new User(2L, "jane_doe", "jane@example.com");
        List<User> expected = List.of(user1, user2);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(expected);

        MapSqlParameterSource params = new MapSqlParameterSource();
        RowMapper<User> mapper = (rs, rowNum) -> new User(1L, "test", "test@example.com");
        List<User> result = queryExecutor.queryAsUnchecked("SELECT * FROM users", params, mapper);

        assertThat(result).isEqualTo(expected);
    }

    // ==================== queryScalar Tests ====================

    @Test
    @DisplayName("Should return scalar value as Optional")
    void testQueryScalarReturnsOptional() {
        when(jdbc.queryForObject(anyString(), any(MapSqlParameterSource.class), eq(Long.class)))
            .thenReturn(42L);

        MapSqlParameterSource params = new MapSqlParameterSource();
        Optional<Long> result = queryExecutor.queryScalar("SELECT COUNT(*) FROM users", params, Long.class);

        assertThat(result).isPresent().contains(42L);
    }

    @Test
    @DisplayName("Should return empty Optional when scalar is null")
    void testQueryScalarNullResult() {
        when(jdbc.queryForObject(anyString(), any(MapSqlParameterSource.class), eq(String.class)))
            .thenReturn(null);

        MapSqlParameterSource params = new MapSqlParameterSource();
        Optional<String> result = queryExecutor.queryScalar("SELECT MAX(name) FROM users", params, String.class);

        assertThat(result).isEmpty();
    }

    @Test
    @DisplayName("Should work with different scalar types")
    void testQueryScalarWithDifferentTypes() {
        when(jdbc.queryForObject(anyString(), any(MapSqlParameterSource.class), eq(Integer.class)))
            .thenReturn(100);

        MapSqlParameterSource params = new MapSqlParameterSource();
        Optional<Integer> result = queryExecutor.queryScalar("SELECT COUNT(*) FROM users", params, Integer.class);

        assertThat(result).isPresent().contains(100);
    }

    // ==================== execute Tests ====================

    @Test
    @DisplayName("Should execute update statement")
    void testExecuteUpdate() {
        when(jdbc.update(anyString(), any(MapSqlParameterSource.class))).thenReturn(1);

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("email", "new@example.com");
        params.addValue("id", 1L);
        int rowsAffected = queryExecutor.execute("UPDATE users SET email = :email WHERE id = :id", params);

        assertThat(rowsAffected).isEqualTo(1);
        verify(jdbc, times(1)).update(anyString(), any(MapSqlParameterSource.class));
    }

    @Test
    @DisplayName("Should return 0 when no rows affected")
    void testExecuteNoRowsAffected() {
        when(jdbc.update(anyString(), any(MapSqlParameterSource.class))).thenReturn(0);

        MapSqlParameterSource params = new MapSqlParameterSource();
        int rowsAffected = queryExecutor.execute("DELETE FROM users WHERE id = :id", params);

        assertThat(rowsAffected).isZero();
    }

    // ==================== searchAs Tests ====================

    @Test
    @DisplayName("Should return CursorPage with content and pagination info")
    void testSearchAsWithReflectiveMapper() {
        User user1 = new User(1L, "alice", "alice@example.com");
        User user2 = new User(2L, "bob", "bob@example.com");
        List<User> queryResult = List.of(user1, user2);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(queryResult);

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users ORDER BY id ASC LIMIT 21",
            new HashMap<>(),
            21,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result).isNotNull();
        assertThat(result.getContent()).containsExactly(user1, user2);
        assertThat(result.isHasNext()).isFalse();
    }

    @Test
    @DisplayName("Should detect hasNext when result exceeds page size")
    void testSearchAsDetectsHasNext() {
        User user1 = new User(1L, "alice", "alice@example.com");
        User user2 = new User(2L, "bob", "bob@example.com");
        User user3 = new User(3L, "charlie", "charlie@example.com");
        List<User> queryResult = List.of(user1, user2, user3);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(queryResult);

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 3",
            new HashMap<>(),
            3,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.isHasNext()).isTrue();
        assertThat(result.getContent()).containsExactly(user1, user2);
        assertThat(result.getContent()).doesNotContain(user3);
    }

    @Test
    @DisplayName("Should generate cursor for next page")
    void testSearchAsGeneratesCursor() {
        User user1 = new User(1L, "alice", "alice@example.com");
        User user2 = new User(2L, "bob", "bob@example.com");
        User user3 = new User(3L, "charlie", "charlie@example.com");
        List<User> queryResult = List.of(user1, user2, user3);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(queryResult);

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 3",
            new HashMap<>(),
            3,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getNextCursor()).isNotNull();
        assertThat(result.getNextCursor()).isNotEmpty();
    }

    @Test
    @DisplayName("Should use custom RowMapper in searchAs")
    void testSearchAsWithCustomMapper() {
        User user1 = new User(1L, "alice", "alice@example.com");
        List<User> queryResult = List.of(user1);

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(queryResult);

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 21",
            new HashMap<>(),
            21,
            List.of("id")
        );

        RowMapper<User> customMapper = (rs, rowNum) -> new User(10L, "custom", "custom@example.com");
        CursorPage<User> result = queryExecutor.searchAs(searchResult, customMapper);

        assertThat(result).isNotNull();
    }

    @Test
    @DisplayName("Should handle empty search result")
    void testSearchAsEmptyResult() {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(new ArrayList<>());

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 21",
            new HashMap<>(),
            21,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getContent()).isEmpty();
        assertThat(result.isHasNext()).isFalse();
        assertThat(result.getNextCursor()).isNull();
    }

    @Test
    @DisplayName("Should include search parameters in query execution")
    void testSearchAsWithParameters() {
        User user1 = new User(1L, "alice", "alice@example.com");
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user1));

        Map<String, Object> params = new HashMap<>();
        params.put("username", "alice");
        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users WHERE username = :username LIMIT 21",
            params,
            21,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        ArgumentCaptor<MapSqlParameterSource> paramsCaptor = ArgumentCaptor.forClass(MapSqlParameterSource.class);
        verify(jdbc).query(anyString(), paramsCaptor.capture(), any(RowMapper.class));

        assertThat(paramsCaptor.getValue().getValues()).containsEntry("username", "alice");
    }

    // ==================== buildCursorValues Tests ====================

    @Test
    @DisplayName("Should build cursor values from object properties")
    void testBuildCursorValuesFromProperties() throws Exception {
        User user = new User(42L, "john_doe", "john@example.com");

        CursorPage<User> result = executeSearchWithMockingToCaptureCursor(user);

        assertThat(result.getNextCursor()).isNotNull();
    }

    private CursorPage<User> executeSearchWithMockingToCaptureCursor(User user) {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user, new User(43L, "jane", "jane@example.com")));

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 2",
            new HashMap<>(),
            2,
            List.of("id", "username")
        );

        return queryExecutor.searchAs(searchResult, User.class);
    }

    @Test
    @DisplayName("Should build cursor values from getter methods")
    void testBuildCursorValuesFromGetters() {
        User user = new User(42L, "john_doe", "john@example.com");

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user, new User(43L, "jane", "jane@example.com")));

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 2",
            new HashMap<>(),
            2,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getNextCursor()).isNotNull();
    }

    @Test
    @DisplayName("Should handle null row in buildCursorValues")
    void testBuildCursorValuesNullRow() {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(new ArrayList<>());

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 21",
            new HashMap<>(),
            21,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getNextCursor()).isNull();
    }

    @Test
    @DisplayName("Should ignore null sort fields in cursor")
    void testBuildCursorValuesNullSortFields() {
        User user = new User(42L, "john_doe", "john@example.com");

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user));

        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users LIMIT 2",
            new HashMap<>(),
            2,
            new ArrayList<>()
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getNextCursor()).isNull();
    }

    // ==================== Integration-like Tests ====================

    @Test
    @DisplayName("Should work with complete workflow: build, execute, paginate")
    void testCompleteSearchWorkflow() {
        User user1 = new User(1L, "alice", "alice@example.com");
        User user2 = new User(2L, "bob", "bob@example.com");

        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(List.of(user1, user2));

        Map<String, Object> params = new HashMap<>();
        SearchResult searchResult = new SearchResult(
            "SELECT * FROM users WHERE active = :active ORDER BY id ASC LIMIT 21",
            params,
            21,
            List.of("id")
        );

        CursorPage<User> result = queryExecutor.searchAs(searchResult, User.class);

        assertThat(result.getContent()).containsExactly(user1, user2);
        assertThat(result.getSize()).isEqualTo(2);
        assertThat(result.isHasNext()).isFalse();
    }

    @Test
    @DisplayName("Should handle special characters in parameters")
    void testParametersWithSpecialCharacters() {
        when(jdbc.query(anyString(), any(MapSqlParameterSource.class), any(RowMapper.class)))
            .thenReturn(new ArrayList<>());

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("email", "test+tag@example.co.uk");
        queryExecutor.queryAs("SELECT * FROM users WHERE email = :email", params, User.class);

        ArgumentCaptor<MapSqlParameterSource> paramsCaptor = ArgumentCaptor.forClass(MapSqlParameterSource.class);
        verify(jdbc).query(anyString(), paramsCaptor.capture(), any(RowMapper.class));

        assertThat(paramsCaptor.getValue().getValues()).containsEntry("email", "test+tag@example.co.uk");
    }
}
