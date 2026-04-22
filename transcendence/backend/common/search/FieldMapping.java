package transcendence.backend.common.search;

/**
 * Representa o mapeamento de um campo para construção de consultas dinâmicas.
 * Cada campo possui um nome de coluna, um tipo de dado, e indica se é filtrável e/ou ordenável.
 */
public record FieldMapping(
    String column,
    class<?> type,
    boolean filterable,
    boolean sortable
) {}