package transcendence.backend.common.search;

/**
 * Classe que representa um critério de filtro para construção de consultas dinâmicas.
 * Cada instância desta classe encapsula uma coluna, um operador de comparação e um valor
 * que serão usados para construir uma cláusula WHERE em uma consulta.
 */
public class FilterCriteria {
    private String column;
    private FilterOperator operator;
    private Object value;
    private Object valueTo; // usado para operadores que precisam de dois valores, como BETWEEN

    public FilterCriteria() {}

    public FilterCriteria(String column, FilterOperator operator, Object value) {
        this.column = column;
        this.operator = operator;
        this.value = value;
    }

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