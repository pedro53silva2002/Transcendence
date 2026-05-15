package com.transcendence.common.search;

/**
 * A Java record is a concise way to define an immutable data carrier.
 * Instead of creating a full class with fields, constructor, getters,
 * equals, hashCode, and toString, a record generates all of that automatically.
 *
 * This record represents the mapping of a field in a data model,
 * including its column name, type, and whether it supports filtering
 * and sorting operations.
 * 
 * Metadata definition for a searchable or sortable field.
 *
 * @param column     the database column or path
 * @param type       the Java class used for type conversion
 * @param filterable whether filtering is permitted on this field
 * @param sortable   whether sorting is permitted on this field
 */
public record FieldMapping(
    String column,
    Class<?> type,
    boolean filterable,
    boolean sortable
) {}