package com.transcendence.common.search;

/**
 * Class that represents a filter criterion for building dynamic queries.
 * Each instance of this class encapsulates a column, a comparison operator, and a value
 * that will be used to construct a WHERE clause in a query.
 */
public class FilterCriteria {
    private String column;
    private FilterOperator operator;
    private Object value;
    private Object valueTo; // usado para operadores que precisam de dois valores, como BETWEEN

    public FilterCriteria() {}

    /**
     * Basic constructor for standard operators (EQUALS, LIKE, etc.).
     *
     * @param column   the logical field name to filter
     * @param operator the comparison operator
     * @param value    the value to compare against
     */
    public FilterCriteria(String column, FilterOperator operator, Object value) {
        this.column = column;
        this.operator = operator;
        this.value = value;
    }

    /**
     * Constructor for range-based operators.
     *
     * @param column   the logical field name to filter
     * @param operator the comparison operator (e.g., BETWEEN)
     * @param value    the start value or primary value
     * @param valueTo  the end value for range comparisons
     */
    public FilterCriteria(String column, FilterOperator operator, Object value, Object valueTo) {
        this.column = column;
        this.operator = operator;
        this.value = value;
        this.valueTo = valueTo;
    }

    public String getColumn() { return column; }
    public void setColumn(String column) { this.column = column; }
    public FilterOperator getOperator() { return operator; }
    public void setOperator(FilterOperator operator) { this.operator = operator; }
    public Object getValue() { return value; }
    public void setValue(Object value) { this.value = value; }
    public Object getValueTo() { return valueTo; }
    public void setValueTo(Object valueTo) { this.valueTo = valueTo; }
}