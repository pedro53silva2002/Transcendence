package transcendence.backend.common.search;

/**
 * Classe que representa um critério de ordenação para construção de consultas dinâmicas.
 * Cada instância desta classe encapsula uma coluna e uma direção de ordenação
 * que serão usados para construir uma cláusula ORDER BY em uma consulta.
 */
public class SortCriteria {
    private String column;
    private SortDirection direction;

    public SortCriteria() {}

    public SortCriteria(String column, SortDirection direction) {
        this.column = column;
        this.direction = direction;
    }

    public String getColumn() { return column; }
    public void setColumn(String column) { this.column = column; }
    public SortDirection getDirection() { return direction; }
    public void setDirection(SortDirection direction) { this.direction = direction; }
}