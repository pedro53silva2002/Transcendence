package transcendence.backend.common.search;

/**
 * A Java record is a concise way to define an immutable data carrier.
 * Instead of creating a full class with fields, constructor, getters,
 * equals, hashCode, and toString, a record generates all of that automatically.
 *
 * This record represents the mapping of a field in a data model,
 * including its column name, type, and whether it supports filtering
 * and sorting operations.
 */
public record FieldMapping(
    String column,
    Class<?> type,
    boolean filterable,
    boolean sortable
) {}