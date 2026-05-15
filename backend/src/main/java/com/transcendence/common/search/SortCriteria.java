package com.transcendence.common.search;

/**
 * Class that represents a sorting criterion for building dynamic queries.
 * Each instance of this class encapsulates a column and a sort direction
 * that will be used to construct an ORDER BY clause in a query.
 * basically it defines the sorting order for a specific field.
 */
public class SortCriteria {
    private String column;
    private SortDirection direction;

    public SortCriteria() {}

    /**
     * Creates a sort instruction.
     *
     * @param column    the logical field name to sort by
     * @param direction the direction (ASC or DESC)
     */
    public SortCriteria(String column, SortDirection direction) {
        this.column = column;
        this.direction = direction;
    }

    public String getColumn() { return column; }
    public void setColumn(String column) { this.column = column; }
    public SortDirection getDirection() { return direction; }
    public void setDirection(SortDirection direction) { this.direction = direction; }
}