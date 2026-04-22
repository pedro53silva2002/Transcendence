# Guia Step-by-Step — Construir o SearchQueryBuilder em Java

**Objetivo:** Perceber a lógica por trás do SearchQueryBuilder em C# e reconstruí-lo em Java/Spring Boot, passo a passo, com os protótipos de todas as classes e métodos.

**Convenção:** Os protótipos mostram a assinatura completa e comentários sobre o que cada método deve fazer internamente. O corpo dos métodos fica para ti implementares.

---

## Passo 0 — Perceber o que o SearchQueryBuilder faz

Antes de escrever qualquer código, precisas de perceber o que esta classe resolve. Lê isto com atenção porque tudo o resto depende deste entendimento.

O SearchQueryBuilder recebe uma query SQL base (ex: `SELECT * FROM trips`) e vai adicionando partes dinâmicas — filtros WHERE, ORDER BY, paginação — com base no que o frontend envia. No final, produz uma query SQL completa com parâmetros seguros (nunca concatena valores directamente).

O fluxo é sempre este:

```
Query base ("SELECT * FROM trips")
    ↓
+ Filtros dinâmicos do frontend (status = 'PLANNING', name CONTAINS 'porto')
    ↓
+ Filtros fixos do backend (deleted_at IS NULL, user_id = :currentUser)
    ↓
+ ORDER BY dinâmico (start_date DESC)
    ↓
+ Paginação (LIMIT + cursor)
    ↓
= Query SQL final parametrizada + mapa de parâmetros
```

O ponto crítico é: o utilizador nunca controla o SQL directamente. Ele envia nomes lógicos de campos ("status", "startDate") e o builder mapeia para colunas SQL reais ("t.status", "t.start_date"). Se o campo não existir na whitelist, a query é rejeitada.

---

## Passo 1 — Definir os DTOs de entrada

Estes são os objectos que o frontend envia e que o controller deserializa. São o contrato entre frontend e backend. Vivem todos em `common/search/`.

### FilterOperator.java

```java
package com.tripcollab.common.search;

/**
 * Operadores suportados nos filtros de search.
 * Cada valor mapeia para um fragmento SQL específico.
 * 
 * Jackson deserializa case-insensitive se configurares:
 * @JsonProperty ou objectMapper.enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS)
 */
public enum FilterOperator {
    EQ,          // coluna = :param
    NEQ,         // coluna <> :param
    GT,          // coluna > :param
    GTE,         // coluna >= :param
    LT,          // coluna < :param
    LTE,         // coluna <= :param
    CONTAINS,    // coluna ILIKE :param  (com % wrapping)
    BEFORE,      // coluna < :param      (alias semântico de LT para datas)
    AFTER,       // coluna > :param      (alias semântico de GT para datas)
    IS_NULL,     // coluna IS NULL       (sem parâmetro)
    IS_NOT_NULL, // coluna IS NOT NULL   (sem parâmetro)
    IN           // coluna = ANY(:param) (para listas)
}
```

### SortDirection.java

```java
package com.tripcollab.common.search;

public enum SortDirection {
    ASC,
    DESC
}
```

### FilterCriteria.java

```java
package com.tripcollab.common.search;

/**
 * Um filtro individual enviado pelo frontend.
 * 
 * Exemplo JSON:
 * { "field": "status", "operator": "EQ", "value": "PLANNING" }
 * { "field": "startDate", "operator": "AFTER", "value": "2026-06-01" }
 * { "field": "deletedAt", "operator": "IS_NULL", "value": null }
 */
public class FilterCriteria {

    private String field;              // Nome lógico do campo (ex: "status", "startDate")
    private FilterOperator operator;   // Operador do filtro
    private Object value;              // Valor — pode ser String, Number, null

    // Construtor vazio (Jackson precisa)
    public FilterCriteria() {}

    public FilterCriteria(String field, FilterOperator operator, Object value) {
        this.field = field;
        this.operator = operator;
        this.value = value;
    }

    // Getters e setters
    public String getField() { return field; }
    public void setField(String field) { this.field = field; }

    public FilterOperator getOperator() { return operator; }
    public void setOperator(FilterOperator operator) { this.operator = operator; }

    public Object getValue() { return value; }
    public void setValue(Object value) { this.value = value; }
}
```

### SortCriteria.java

```java
package com.tripcollab.common.search;

/**
 * Um critério de ordenação enviado pelo frontend.
 * 
 * Exemplo JSON:
 * { "field": "startDate", "direction": "DESC" }
 */
public class SortCriteria {

    private String field;
    private SortDirection direction;

    public SortCriteria() {}

    public SortCriteria(String field, SortDirection direction) {
        this.field = field;
        this.direction = direction;
    }

    // Getters e setters
    public String getField() { return field; }
    public void setField(String field) { this.field = field; }

    public SortDirection getDirection() { return direction; }
    public void setDirection(SortDirection direction) { this.direction = direction; }
}
```

### CursorPageRequest.java

```java
package com.tripcollab.common.search;

/**
 * Pedido de paginação por cursor.
 * 
 * Exemplo JSON (primeira página):
 * { "size": 20, "cursor": null }
 * 
 * Exemplo JSON (páginas seguintes):
 * { "size": 20, "cursor": "eyJzdGFydERhdGUiOiIyMDI2LTA3LTAxIiwiaWQiOiJhYmMtMTIzIn0=" }
 */
public class CursorPageRequest {

    private int size;        // Número de resultados por página (max 100)
    private String cursor;   // Cursor opaco (Base64url) ou null para primeira página

    public CursorPageRequest() {
        this.size = 20;  // Default
    }

    public CursorPageRequest(int size, String cursor) {
        this.size = size;
        this.cursor = cursor;
    }

    // Getters e setters
    public int getSize() { return size; }
    public void setSize(int size) { this.size = size; }

    public String getCursor() { return cursor; }
    public void setCursor(String cursor) { this.cursor = cursor; }
}
```

### SearchPayload.java

```java
package com.tripcollab.common.search;

import java.util.List;

/**
 * Payload completo de search enviado pelo frontend.
 * Agrupa filtros + ordenação + paginação.
 * 
 * Exemplo JSON completo:
 * {
 *   "filters": [
 *     { "field": "status", "operator": "EQ", "value": "PLANNING" },
 *     { "field": "startDate", "operator": "AFTER", "value": "2026-06-01" }
 *   ],
 *   "sort": [
 *     { "field": "startDate", "direction": "DESC" }
 *   ],
 *   "pagination": {
 *     "size": 20,
 *     "cursor": null
 *   }
 * }
 */
public class SearchPayload {

    private List<FilterCriteria> filters;
    private List<SortCriteria> sort;
    private CursorPageRequest pagination;

    public SearchPayload() {}

    // Getters e setters
    public List<FilterCriteria> getFilters() { return filters; }
    public void setFilters(List<FilterCriteria> filters) { this.filters = filters; }

    public List<SortCriteria> getSort() { return sort; }
    public void setSort(List<SortCriteria> sort) { this.sort = sort; }

    public CursorPageRequest getPagination() { return pagination; }
    public void setPagination(CursorPageRequest pagination) { this.pagination = pagination; }
}
```

---

## Passo 2 — Definir o FieldMapping e o SearchResult

### FieldMapping.java

```java
package com.tripcollab.common.search;

/**
 * Mapeia um campo lógico do frontend para uma coluna SQL.
 * Cada repositório define os seus FieldMappings.
 * 
 * Exemplo de uso no repositório:
 *   new FieldMapping("t.name", String.class, true, true)
 *   → campo filtrável e ordenável, mapeia para coluna "t.name"
 * 
 * @param column     Coluna SQL real (ex: "t.start_date", "u.display_name")
 * @param type       Tipo Java esperado (ex: String.class, LocalDate.class, UUID.class)
 * @param filterable Se o frontend pode usar este campo em filtros
 * @param sortable   Se o frontend pode usar este campo em ORDER BY
 */
public record FieldMapping(
    String column,
    Class<?> type,
    boolean filterable,
    boolean sortable
) {}
```

### SearchResult.java

```java
package com.tripcollab.common.search;

import java.util.Map;

/**
 * Output do SearchQueryBuilder.build().
 * Contém o SQL final, os parâmetros, e o limit original.
 * 
 * O QueryExecutor usa este objecto para executar a query
 * e construir o CursorPage.
 * 
 * @param sql    Query SQL completa com named parameters
 * @param params Mapa de parâmetros (nome → valor)
 * @param limit  Número de resultados pedidos (sem o +1)
 */
public record SearchResult(
    String sql,
    Map<String, Object> params,
    int limit
) {}
```

### CursorData.java

```java
package com.tripcollab.common.search;

import java.util.Map;

/**
 * Dados internos do cursor.
 * Contém os valores da última linha da página anterior
 * para as colunas de ordenação.
 * 
 * Exemplo: { "startDate": "2026-07-01", "id": "abc-123" }
 * 
 * É serializado para JSON e codificado em Base64url para
 * formar o cursor opaco que o frontend recebe.
 */
public class CursorData {

    private Map<String, Object> values;

    public CursorData() {}

    public CursorData(Map<String, Object> values) {
        this.values = values;
    }

    public Map<String, Object> getValues() { return values; }
    public void setValues(Map<String, Object> values) { this.values = values; }
}
```

### CursorPage.java

```java
package com.tripcollab.common.search;

import java.util.List;

/**
 * Resposta paginada por cursor.
 * Devolvida ao frontend como parte da response JSON.
 * 
 * Exemplo de serialização:
 * {
 *   "data": { "items": [...], "nextCursor": "eyJ...", "hasMore": true, "size": 20 }
 * }
 * 
 * @param <T> Tipo dos items
 */
public class CursorPage<T> {

    private List<T> items;
    private String nextCursor;   // null se não há mais páginas
    private boolean hasMore;
    private int size;            // Número de items devolvidos

    public CursorPage() {}

    public CursorPage(List<T> items, String nextCursor, boolean hasMore, int size) {
        this.items = items;
        this.nextCursor = nextCursor;
        this.hasMore = hasMore;
        this.size = size;
    }

    // Getters e setters
    public List<T> getItems() { return items; }
    public void setItems(List<T> items) { this.items = items; }

    public String getNextCursor() { return nextCursor; }
    public void setNextCursor(String nextCursor) { this.nextCursor = nextCursor; }

    public boolean isHasMore() { return hasMore; }
    public void setHasMore(boolean hasMore) { this.hasMore = hasMore; }

    public int getSize() { return size; }
    public void setSize(int size) { this.size = size; }
}
```

---

## Passo 3 — Criar a classe SearchQueryBuilder

Esta é a classe central. Mantém estado interno enquanto vai sendo construída com chamadas fluent, e no final produz o `SearchResult`.

```java
package com.tripcollab.common.search;

import java.math.BigDecimal;
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.*;

/**
 * Constrói queries SQL dinâmicas a partir de filtros, ordenação e cursor pagination.
 * 
 * Uso típico num repositório:
 * 
 *   SearchQueryBuilder builder = new SearchQueryBuilder("trips", "SELECT t.* FROM trips t")
 *       .field("name", "t.name", String.class, true, true)
 *       .field("status", "t.status", String.class, true, true)
 *       .field("startDate", "t.start_date", LocalDate.class, true, true)
 *       .field("createdAt", "t.created_at", Instant.class, true, true)
 *       .fixedCondition("t.deleted_at IS NULL", null, null)
 *       .fixedCondition("tm.user_id = :currentUserId", "currentUserId", currentUserId);
 * 
 *   SearchResult result = builder.build(payload);
 */
public class SearchQueryBuilder {

    // --- Estado interno ---

    private final String baseTable;      // Alias da tabela principal (para tiebreaker)
    private final String baseSelect;     // Query SQL base passada no construtor

    // Mapa: nome lógico do campo → FieldMapping
    // LinkedHashMap preserva ordem de inserção (útil para debug)
    private final Map<String, FieldMapping> allowedFields = new LinkedHashMap<>();

    // Condições WHERE fixas (impostas pelo backend, não pelo frontend)
    private final List<String> fixedConditions = new ArrayList<>();

    // Parâmetros das condições fixas
    private final Map<String, Object> fixedParams = new HashMap<>();

    // --- Construtor ---

    /**
     * @param baseTable  Alias da tabela principal (ex: "trips", "t")
     *                   Usado para adicionar tiebreaker: "{baseTable}.id"
     * @param baseSelect Query SQL base (ex: "SELECT t.* FROM trips t JOIN ...")
     */
    public SearchQueryBuilder(String baseTable, String baseSelect) {
        // TODO: guardar os dois campos
    }

    // --- Métodos de configuração (fluent) ---

    /**
     * Regista um campo que o frontend pode usar para filtrar e/ou ordenar.
     * 
     * @param name       Nome lógico (o que o frontend envia, ex: "startDate")
     * @param column     Coluna SQL real (ex: "t.start_date")
     * @param type       Tipo Java esperado (ex: LocalDate.class)
     * @param filterable Se pode ser usado em filtros
     * @param sortable   Se pode ser usado em ORDER BY
     * @return this (fluent)
     */
    public SearchQueryBuilder field(String name, String column, Class<?> type,
                                     boolean filterable, boolean sortable) {
        // TODO: criar FieldMapping e adicionar a allowedFields
        return this;
    }

    /**
     * Adiciona uma condição WHERE que está sempre presente,
     * independentemente dos filtros do frontend.
     * 
     * Exemplos:
     *   .fixedCondition("t.deleted_at IS NULL", null, null)
     *   .fixedCondition("tm.user_id = :currentUserId", "currentUserId", userId)
     * 
     * @param sql       Fragmento SQL da condição
     * @param paramName Nome do parâmetro (null se a condição não tem parâmetro)
     * @param value     Valor do parâmetro (null se a condição não tem parâmetro)
     * @return this (fluent)
     */
    public SearchQueryBuilder fixedCondition(String sql, String paramName, Object value) {
        // TODO: adicionar sql a fixedConditions
        // TODO: se paramName não é null, adicionar a fixedParams
        return this;
    }

    // --- Método principal: build ---

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
     * @throws InvalidFilterException se um campo não existir em allowedFields
     * @throws InvalidSortException se um campo não for sortable
     */
    public SearchResult build(SearchPayload payload) {
        // TODO: implementar as 6 etapas abaixo

        // --- Variáveis de trabalho ---
        // StringBuilder where = new StringBuilder();
        // Map<String, Object> params = new HashMap<>(fixedParams);
        // int paramIndex = 0;

        // --- Etapa 1+2: WHERE ---
        // buildWhereClause(payload.getFilters(), where, params, paramIndex)

        // --- Etapa 3: ORDER BY ---
        // String orderBy = buildOrderByClause(payload.getSort())

        // --- Etapa 4: Keyset pagination ---
        // if (cursor != null) appendKeysetCondition(cursor, sorts, where, params)

        // --- Etapa 5: LIMIT ---
        // int limit = Math.min(payload.getPagination().getSize(), 100)
        // SQL final = baseSelect + where + orderBy + " LIMIT " + (limit + 1)

        // --- Etapa 6: Return ---
        // return new SearchResult(sql, params, limit)

        return null; // placeholder
    }

    // --- Métodos privados de construção ---

    /**
     * Gera o fragmento SQL para um filtro individual.
     * 
     * Recebe o operador e devolve o SQL correspondente:
     *   EQ       → "coluna = :paramName"
     *   NEQ      → "coluna <> :paramName"
     *   GT/AFTER → "coluna > :paramName"
     *   GTE      → "coluna >= :paramName"
     *   LT/BEFORE→ "coluna < :paramName"
     *   LTE      → "coluna <= :paramName"
     *   CONTAINS → "coluna ILIKE :paramName"
     *   IS_NULL  → "coluna IS NULL"
     *   IS_NOT_NULL → "coluna IS NOT NULL"
     *   IN       → "coluna = ANY(:paramName)"
     * 
     * @param column    Coluna SQL (do FieldMapping)
     * @param operator  Operador do filtro
     * @param paramName Nome do parâmetro gerado (ex: "p0", "p1")
     * @return Fragmento SQL
     */
    private String buildFilterClause(String column, FilterOperator operator, String paramName) {
        // TODO: switch sobre o operador, retornar fragmento SQL
        return null; // placeholder
    }

    /**
     * Constrói a cláusula ORDER BY a partir dos SortCriteria.
     * 
     * Se o payload não tem sort, aplica default: createdAt DESC.
     * Depois de todos os critérios do utilizador, adiciona SEMPRE
     * o tiebreaker: "{baseTable}.id {direcçãoDoPrimeiroSort}"
     * 
     * Exemplo output: "ORDER BY t.start_date DESC, t.id DESC"
     * 
     * @param sorts Lista de SortCriteria do payload (pode ser null/vazia)
     * @return String com a cláusula ORDER BY completa
     * @throws InvalidSortException se um campo não for sortable
     */
    private String buildOrderByClause(List<SortCriteria> sorts) {
        // TODO: validar campos, construir string, adicionar tiebreaker
        return null; // placeholder
    }

    /**
     * Gera a condição WHERE de keyset pagination.
     * 
     * Caso simples (todas as colunas na mesma direcção DESC):
     *   "(start_date, id) < (:cursor_start_date, :cursor_id)"
     * 
     * Caso complexo (direcções mistas, ex: start_date DESC, name ASC):
     *   "start_date < :c_start_date
     *    OR (start_date = :c_start_date AND name > :c_name)
     *    OR (start_date = :c_start_date AND name = :c_name AND id < :c_id)"
     * 
     * @param cursor Dados do cursor descodificados
     * @param sorts  Critérios de sort (incluindo tiebreaker)
     * @param params Mapa de parâmetros (para adicionar os valores do cursor)
     * @return Fragmento SQL da condição keyset
     */
    private String buildKeysetClause(CursorData cursor, List<SortCriteria> sorts,
                                      Map<String, Object> params) {
        // TODO: ver explicação detalhada no Passo 7
        return null; // placeholder
    }

    /**
     * Converte o valor do filtro (que vem como Object do JSON)
     * para o tipo Java correcto definido no FieldMapping.
     * 
     * Conversões suportadas:
     *   String      → sem conversão
     *   Integer     → Integer.parseInt(value.toString())
     *   Long        → Long.parseLong(value.toString())
     *   BigDecimal  → new BigDecimal(value.toString())
     *   Boolean     → Boolean.parseBoolean(value.toString())
     *   UUID        → UUID.fromString(value.toString())
     *   LocalDate   → LocalDate.parse(value.toString())
     *   LocalTime   → LocalTime.parse(value.toString())
     *   Instant     → Instant.parse(value.toString())
     *   Enum        → Enum.valueOf(enumClass, value.toString().toUpperCase())
     * 
     * @param value      Valor bruto do JSON
     * @param targetType Tipo Java esperado (do FieldMapping)
     * @return Valor convertido
     * @throws IllegalArgumentException se a conversão falhar
     */
    private Object convertValue(Object value, Class<?> targetType) {
        // TODO: switch/if sobre targetType, converter, lançar excepção se inválido
        return null; // placeholder
    }

    /**
     * Escapa caracteres especiais de LIKE para o operador CONTAINS.
     * 
     * Os caracteres %, _ e \ no valor do utilizador devem ser escapados
     * para evitar pattern matching arbitrário.
     * 
     * Exemplo: "100%" → "100\%"
     * 
     * @param value Valor original do utilizador
     * @return Valor escapado
     */
    private String escapeForLike(String value) {
        // TODO: replace "\" → "\\", "%" → "\%", "_" → "\_"
        return null; // placeholder
    }

    /**
     * Descodifica um cursor opaco (Base64url → JSON → CursorData).
     * 
     * @param cursor String Base64url recebida do frontend
     * @return CursorData com os valores das colunas de ordenação
     * @throws IllegalArgumentException se o cursor for inválido
     */
    private CursorData decodeCursor(String cursor) {
        // TODO: Base64.getUrlDecoder().decode(cursor)
        // TODO: objectMapper.readValue(bytes, CursorData.class)
        return null; // placeholder
    }

    /**
     * Codifica os valores de cursor para uma string opaca (CursorData → JSON → Base64url).
     * 
     * @param values Mapa com os valores da última linha da página
     * @return String Base64url para enviar ao frontend como nextCursor
     */
    public static String encodeCursor(Map<String, Object> values) {
        // TODO: objectMapper.writeValueAsBytes(new CursorData(values))
        // TODO: Base64.getUrlEncoder().encodeToString(bytes)
        return null; // placeholder
    }
}
```

---

## Passo 4 — Criar as excepções de domínio

Estas excepções são lançadas pelo builder quando os filtros ou sorts são inválidos. O `GlobalExceptionHandler` apanha-as e devolve HTTP 400. Vivem em `common/error/`.

```java
package com.tripcollab.common.error;

/**
 * Base para todas as excepções de domínio.
 * O GlobalExceptionHandler trata subclasses desta excepção.
 */
public abstract class DomainException extends RuntimeException {

    private final String code;

    protected DomainException(String code, String message) {
        super(message);
        this.code = code;
    }

    public String getCode() { return code; }
}
```

```java
package com.tripcollab.common.error;

/**
 * Lançada quando o frontend envia um campo de filtro inválido.
 * Resulta em HTTP 400.
 */
public class InvalidFilterException extends DomainException {

    public InvalidFilterException(String message) {
        super("INVALID_FILTER", message);
    }
}
```

```java
package com.tripcollab.common.error;

/**
 * Lançada quando o frontend envia um campo de sort inválido.
 * Resulta em HTTP 400.
 */
public class InvalidSortException extends DomainException {

    public InvalidSortException(String message) {
        super("INVALID_SORT", message);
    }
}
```

```java
package com.tripcollab.common.error;

/**
 * Lançada pelo queryAsSingle quando há mais de 1 resultado.
 * Resulta em HTTP 500 (erro interno — nunca devia acontecer).
 */
public class NonUniqueResultException extends DomainException {

    public NonUniqueResultException(String message) {
        super("NON_UNIQUE_RESULT", message);
    }
}
```

---

## Passo 5 — Criar o ReflectiveRowMapper

Vive em `common/query/`. É a peça que elimina RowMappers manuais.

```java
package com.tripcollab.common.query;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.jdbc.core.RowMapper;

import java.beans.PropertyDescriptor;
import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

/**
 * RowMapper genérico que mapeia ResultSet → instância de T
 * usando convenção snake_case → camelCase.
 * 
 * Suporta: String, int, long, double, boolean, BigDecimal,
 *          UUID, Instant, LocalDate, LocalTime, Enum, JSONB.
 * 
 * Colunas do ResultSet sem propriedade correspondente em T são ignoradas.
 * Propriedades de T sem coluna correspondente ficam com valor default (null/0/false).
 * 
 * O mapa de PropertyDescriptor é construído uma vez no construtor
 * e reutilizado em cada chamada a mapRow — não faz reflection por linha.
 */
public class ReflectiveRowMapper<T> implements RowMapper<T> {

    private final Class<T> type;
    private final ObjectMapper objectMapper;
    private final Map<String, PropertyDescriptor> propertyMap;

    /**
     * @param type         Classe alvo (ex: Trip.class)
     * @param objectMapper Para deserialização de JSONB
     */
    public ReflectiveRowMapper(Class<T> type, ObjectMapper objectMapper) {
        this.type = type;
        this.objectMapper = objectMapper;
        this.propertyMap = buildPropertyMap(type);
    }

    @Override
    public T mapRow(ResultSet rs, int rowNum) throws SQLException {
        // TODO: 
        // 1. Instanciar T via BeanUtils.instantiateClass(type)
        // 2. Iterar sobre colunas do ResultSet (rs.getMetaData())
        // 3. Para cada coluna:
        //    a. Obter nome via getColumnLabel (respeita aliases)
        //    b. Converter snake_case → camelCase
        //    c. Procurar no propertyMap
        //    d. Se encontrar, chamar extractValue() para obter o valor convertido
        //    e. Atribuir via pd.getWriteMethod().invoke(instance, value)
        // 4. Retornar instance

        return null; // placeholder
    }

    /**
     * Extrai e converte o valor de uma coluna do ResultSet
     * para o tipo Java da propriedade correspondente.
     * 
     * Conversões:
     *   UUID           ← rs.getObject() → UUID.fromString()
     *   Enum           ← rs.getString() → Enum.valueOf()
     *   Instant        ← rs.getTimestamp() → Timestamp.toInstant()
     *   LocalDate      ← rs.getDate() → java.sql.Date.toLocalDate()
     *   LocalTime      ← rs.getTime() → java.sql.Time.toLocalTime()
     *   BigDecimal     ← rs.getBigDecimal()
     *   JSONB (PGobject) ← objectMapper.readValue(pgo.getValue(), targetType)
     *   Resto          ← rs.getObject() (cast directo)
     * 
     * @param rs         ResultSet posicionado na linha actual
     * @param index      Índice da coluna (1-based)
     * @param targetType Tipo Java esperado
     * @return Valor convertido, ou null
     */
    private Object extractValue(ResultSet rs, int index, Class<?> targetType) 
            throws SQLException {
        // TODO: obter raw com rs.getObject(index)
        // TODO: se raw == null, retornar null
        // TODO: switch/if sobre targetType, converter e retornar
        return null; // placeholder
    }

    /**
     * Converte nome de coluna de snake_case para camelCase.
     * 
     * Exemplos:
     *   "budget_currency" → "budgetCurrency"
     *   "id"              → "id"
     *   "start_date"      → "startDate"
     *   "created_at"      → "createdAt"
     * 
     * @param snake Nome em snake_case
     * @return Nome em camelCase
     */
    private String snakeToCamel(String snake) {
        // TODO: iterar caracteres, quando encontrar '_' marcar próximo como uppercase
        return null; // placeholder
    }

    /**
     * Constrói o mapa de PropertyDescriptor para o tipo T.
     * Indexado pelo nome da propriedade em camelCase (lowercase).
     * 
     * Usa java.beans.Introspector.getBeanInfo(type).getPropertyDescriptors()
     * Filtra propriedades que têm write method (setter).
     * 
     * @param type Classe a introspeccionar
     * @return Mapa propertyName → PropertyDescriptor
     */
    private Map<String, PropertyDescriptor> buildPropertyMap(Class<T> type) {
        // TODO: Introspector.getBeanInfo(type)
        // TODO: filtrar por writeMethod != null
        // TODO: indexar por pd.getName().toLowerCase()
        return new HashMap<>(); // placeholder
    }
}
```

---

## Passo 6 — Criar o QueryExecutor

Vive em `common/query/`. É o componente Spring que executa queries e mapeia resultados.

```java
package com.tripcollab.common.query;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.tripcollab.common.error.NonUniqueResultException;
import com.tripcollab.common.search.CursorPage;
import com.tripcollab.common.search.SearchQueryBuilder;
import com.tripcollab.common.search.SearchResult;
import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.function.Function;

/**
 * Executa queries SQL e mapeia resultados para objectos Java tipados.
 * 
 * Três modos de uso:
 * - queryAs:       Lista de objectos tipados
 * - queryAsSingle: Objecto único opcional
 * - searchAs:      Paginação por cursor (integrado com SearchQueryBuilder)
 * 
 * Usa NamedParameterJdbcTemplate para parâmetros nomeados (:param).
 * Usa ReflectiveRowMapper para mapeamento automático snake_case → camelCase.
 */
@Component
public class QueryExecutor {

    private final NamedParameterJdbcTemplate jdbc;
    private final ObjectMapper objectMapper;

    public QueryExecutor(NamedParameterJdbcTemplate jdbc, ObjectMapper objectMapper) {
        this.jdbc = jdbc;
        this.objectMapper = objectMapper;
    }

    /**
     * Executa query e mapeia cada linha para uma instância de T.
     * 
     * Exemplo:
     *   List<Trip> trips = queryExecutor.queryAs(
     *       "SELECT * FROM trips WHERE status = :status",
     *       Map.of("status", "PLANNING"),
     *       Trip.class
     *   );
     * 
     * @param sql    Query SQL com named parameters
     * @param params Mapa de parâmetros
     * @param type   Classe alvo
     * @return Lista de instâncias de T
     */
    public <T> List<T> queryAs(String sql, Map<String, Object> params, Class<T> type) {
        // TODO: criar ReflectiveRowMapper<T>(type, objectMapper)
        // TODO: chamar jdbc.query(sql, params, rowMapper)
        // TODO: retornar resultado
        return null; // placeholder
    }

    /**
     * Executa query e mapeia a primeira (e única esperada) linha para T.
     * 
     * Exemplo:
     *   Optional<Trip> trip = queryExecutor.queryAsSingle(
     *       "SELECT * FROM trips WHERE id = :id AND deleted_at IS NULL",
     *       Map.of("id", tripId),
     *       Trip.class
     *   );
     * 
     * @param sql    Query SQL
     * @param params Parâmetros
     * @param type   Classe alvo
     * @return Optional vazio se 0 resultados, Optional com valor se 1
     * @throws NonUniqueResultException se mais de 1 resultado
     */
    public <T> Optional<T> queryAsSingle(String sql, Map<String, Object> params, 
                                          Class<T> type) {
        // TODO: chamar queryAs(sql, params, type)
        // TODO: se results.size() > 1 → lançar NonUniqueResultException
        // TODO: se vazio → Optional.empty()
        // TODO: senão → Optional.of(results.get(0))
        return Optional.empty(); // placeholder
    }

    /**
     * Executa query sem mapeamento tipado.
     * Devolve lista de mapas coluna → valor.
     * 
     * Útil para queries ad-hoc, relatórios, ou quando
     * o tipo de retorno é determinado em runtime.
     * 
     * @param sql    Query SQL
     * @param params Parâmetros
     * @return Lista de Map<String, Object>
     */
    public List<Map<String, Object>> queryAsUnchecked(String sql, Map<String, Object> params) {
        // TODO: chamar jdbc.queryForList(sql, params)
        return null; // placeholder
    }

    /**
     * Executa search com keyset pagination.
     * Integrado com o output do SearchQueryBuilder.
     * 
     * Este método:
     * 1. Executa a query (que pede limit + 1 resultados)
     * 2. Verifica se tem mais páginas (resultado.size > limit?)
     * 3. Se sim, remove a linha extra e constrói o cursor da próxima página
     * 4. Devolve CursorPage<T>
     * 
     * Exemplo:
     *   CursorPage<Trip> page = queryExecutor.searchAs(
     *       searchResult,
     *       Trip.class,
     *       trip -> Map.of("startDate", trip.getStartDate(), "id", trip.getId())
     *   );
     * 
     * @param searchResult Output do SearchQueryBuilder.build()
     * @param type         Classe alvo
     * @param cursorExtractor Função que extrai valores de cursor do último item.
     *                        As chaves do mapa devem corresponder aos campos de sort.
     * @return CursorPage com items, nextCursor, hasMore, size
     */
    public <T> CursorPage<T> searchAs(SearchResult searchResult, Class<T> type,
                                       Function<T, Map<String, Object>> cursorExtractor) {
        // TODO:
        // 1. List<T> results = queryAs(searchResult.sql(), searchResult.params(), type)
        // 2. boolean hasMore = results.size() > searchResult.limit()
        // 3. List<T> items = hasMore ? results.subList(0, limit) : results
        // 4. String nextCursor = null
        // 5. Se hasMore && !items.isEmpty():
        //    a. T lastItem = items.get(items.size() - 1)
        //    b. Map<String, Object> cursorValues = cursorExtractor.apply(lastItem)
        //    c. nextCursor = SearchQueryBuilder.encodeCursor(cursorValues)
        // 6. return new CursorPage<>(items, nextCursor, hasMore, items.size())

        return null; // placeholder
    }

    /**
     * Sobrecarga que aceita um RowMapper customizado para casos
     * onde o ReflectiveRowMapper não é suficiente (ex: JOINs complexos
     * com projecções que não mapeiam 1:1).
     */
    public <T> List<T> queryAs(String sql, Map<String, Object> params, RowMapper<T> mapper) {
        // TODO: jdbc.query(sql, params, mapper)
        return null; // placeholder
    }
}
```

---

## Passo 7 — Keyset Pagination — Detalhe da lógica

Esta é a parte mais complexa. Vamos dissecar a lógica do `buildKeysetClause`.

### Caso simples — Todas as colunas na mesma direcção

Se o ORDER BY é `start_date DESC, id DESC` (ambos DESC), podes usar tuple comparison do PostgreSQL:

```sql
WHERE (t.start_date, t.id) < (:c_start_date, :c_id)
```

O `<` em tuplos compara lexicograficamente: primeiro compara start_date, se igual compara id. É exactamente o que queremos e é optimizável pelo PostgreSQL com um índice composto `(start_date DESC, id DESC)`.

Para ASC, usas `>`:

```sql
WHERE (t.start_date, t.id) > (:c_start_date, :c_id)
```

### Caso complexo — Direcções mistas

Se o ORDER BY é `start_date DESC, name ASC, id DESC`, o operador de tuplo não funciona porque assume que todas as colunas têm a mesma direcção. Tens de expandir para uma disjunção:

```sql
WHERE t.start_date < :c_start_date                                          -- start_date "avançou" (DESC = menor)
   OR (t.start_date = :c_start_date AND t.name > :c_name)                   -- start_date igual, name "avançou" (ASC = maior)
   OR (t.start_date = :c_start_date AND t.name = :c_name AND t.id < :c_id)  -- ambos iguais, id "avançou" (DESC = menor)
```

A regra geral: para N colunas de sort, geras N condições OR. Cada condição fixa as primeiras K-1 colunas com `=` e aplica `<` ou `>` na coluna K, conforme a direcção:
- DESC → `<` (queremos valores menores = mais antigos)
- ASC → `>` (queremos valores maiores = mais recentes)

### Protótipo detalhado

```java
/**
 * Gera a condição keyset para N colunas de sort.
 * 
 * Algoritmo:
 * Para cada posição i (0..N-1):
 *   - As colunas 0..i-1 são fixas com "= :c_{campo}"
 *   - A coluna i usa "<" (se DESC) ou ">" (se ASC) com ":c_{campo}"
 *   - Gera uma condição AND com estas partes
 * Junta todas as condições com OR
 * Envolve tudo em parêntesis
 * 
 * Exemplo para [start_date DESC, name ASC, id DESC]:
 * Posição 0: start_date < :c_start_date
 * Posição 1: start_date = :c_start_date AND name > :c_name
 * Posição 2: start_date = :c_start_date AND name = :c_name AND id < :c_id
 * Final: (pos0 OR pos1 OR pos2)
 * 
 * @param cursor    Dados descodificados do cursor
 * @param sortColumns Lista ordenada de (coluna SQL, direcção, nome no cursor)
 * @param params    Mapa de parâmetros para adicionar valores do cursor
 * @return Fragmento SQL entre parêntesis
 */
private String buildKeysetClause(
        CursorData cursor,
        List<KeysetColumn> sortColumns,
        Map<String, Object> params) {

    // KeysetColumn é um record auxiliar:
    // record KeysetColumn(String sqlColumn, SortDirection direction, String cursorKey) {}

    // TODO:
    // 1. Para cada sortColumn, adicionar valor do cursor aos params
    //    params.put("c_" + cursorKey, cursor.getValues().get(cursorKey))
    //
    // 2. Lista de condições OR
    //    List<String> orConditions = new ArrayList<>()
    //
    // 3. Para i = 0 até sortColumns.size()-1:
    //    a. List<String> andParts = new ArrayList<>()
    //    b. Para j = 0 até i-1:
    //       andParts.add(sortColumns[j].sqlColumn + " = :c_" + sortColumns[j].cursorKey)
    //    c. String op = sortColumns[i].direction == DESC ? "<" : ">"
    //       andParts.add(sortColumns[i].sqlColumn + " " + op + " :c_" + sortColumns[i].cursorKey)
    //    d. orConditions.add("(" + String.join(" AND ", andParts) + ")")
    //
    // 4. return "(" + String.join(" OR ", orConditions) + ")"

    return null; // placeholder
}
```

---

## Passo 8 — Integrar num repositório (exemplo TripRepository)

```java
package com.tripcollab.trips.model;

import com.tripcollab.common.query.QueryExecutor;
import com.tripcollab.common.search.*;
import org.springframework.stereotype.Repository;

import java.time.Instant;
import java.time.LocalDate;
import java.util.Map;
import java.util.Optional;
import java.util.UUID;

/**
 * Repositório de trips.
 * Usa SearchQueryBuilder para search dinâmico
 * e QueryExecutor para execução e mapeamento.
 */
@Repository
public class TripRepository {

    private final QueryExecutor queryExecutor;

    public TripRepository(QueryExecutor queryExecutor) {
        this.queryExecutor = queryExecutor;
    }

    /**
     * Pesquisa trips do utilizador actual com filtros dinâmicos.
     * 
     * Filtros fixos (sempre aplicados):
     * - t.deleted_at IS NULL (soft delete)
     * - tm.user_id = :currentUserId (só trips do utilizador)
     * - tm.left_at IS NULL (só membros activos)
     * 
     * Campos filtráveis pelo frontend:
     * - name (CONTAINS, EQ)
     * - status (EQ, NEQ, IN)
     * - startDate (AFTER, BEFORE, EQ)
     * - endDate (AFTER, BEFORE, EQ)
     * - destinationName (CONTAINS, EQ)
     * - budgetCurrency (EQ)
     * - createdAt (AFTER, BEFORE)
     * 
     * Campos ordenáveis pelo frontend:
     * - name, status, startDate, endDate, destinationName, createdAt
     */
    public CursorPage<Trip> search(UUID currentUserId, SearchPayload payload) {
        // TODO:
        // 1. Criar SearchQueryBuilder com query base (SELECT + JOIN trip_members)
        // 2. Adicionar campos com .field(...)
        // 3. Adicionar condições fixas com .fixedCondition(...)
        // 4. Chamar .build(payload)
        // 5. Chamar queryExecutor.searchAs(result, Trip.class, cursorExtractor)
        // 6. Retornar CursorPage<Trip>

        // Esboço:
        // SearchQueryBuilder builder = new SearchQueryBuilder("t",
        //     "SELECT t.* FROM trips t " +
        //     "JOIN trip_members tm ON tm.trip_id = t.id")
        //     .field("name", "t.name", String.class, true, true)
        //     .field("status", "t.status", String.class, true, true)
        //     .field("startDate", "t.start_date", LocalDate.class, true, true)
        //     .field("endDate", "t.end_date", LocalDate.class, true, true)
        //     .field("destinationName", "t.destination_name", String.class, true, true)
        //     .field("budgetCurrency", "t.budget_currency", String.class, true, false)
        //     .field("createdAt", "t.created_at", Instant.class, true, true)
        //     .fixedCondition("t.deleted_at IS NULL", null, null)
        //     .fixedCondition("tm.user_id = :currentUserId", "currentUserId", currentUserId)
        //     .fixedCondition("tm.left_at IS NULL", null, null);
        //
        // SearchResult result = builder.build(payload);
        //
        // return queryExecutor.searchAs(result, Trip.class, trip -> Map.of(
        //     "startDate", trip.getStartDate(),
        //     "id", trip.getId()
        // ));

        return null; // placeholder
    }

    /**
     * Busca trip por ID.
     */
    public Optional<Trip> findById(UUID id) {
        // TODO:
        // return queryExecutor.queryAsSingle(
        //     "SELECT * FROM trips WHERE id = :id AND deleted_at IS NULL",
        //     Map.of("id", id),
        //     Trip.class
        // );
        return Optional.empty(); // placeholder
    }

    /**
     * Persiste uma trip (INSERT ou UPDATE).
     */
    public Trip save(Trip trip) {
        // TODO:
        // if (trip.getId() == null) → INSERT com UUID v7
        // else → UPDATE
        // Usar jdbc.update() com mapa de parâmetros
        return trip; // placeholder
    }

    /**
     * Soft delete.
     */
    public void softDelete(UUID id) {
        // TODO:
        // jdbc.update("UPDATE trips SET deleted_at = NOW() WHERE id = :id", Map.of("id", id))
    }
}
```

---

## Passo 9 — Diagrama de dependências entre componentes

```
Controller (router)
    │
    │  recebe SearchPayload via @RequestBody
    │  chama service
    ▼
Service
    │
    │  aplica regras de negócio
    │  chama repositório com (userId, payload)
    ▼
Repository
    │
    │  cria SearchQueryBuilder
    │  configura campos e condições fixas
    │  chama builder.build(payload) → SearchResult
    │  chama queryExecutor.searchAs(result, type, cursorExtractor)
    ▼
SearchQueryBuilder                    QueryExecutor
    │                                      │
    │  valida filtros                      │  executa SQL via JdbcTemplate
    │  gera WHERE                          │  mapeia via ReflectiveRowMapper
    │  gera ORDER BY                       │  trata limit+1 → CursorPage
    │  gera keyset condition               │
    │  retorna SearchResult                │
    ▼                                      ▼
SearchResult ─────────────────────> NamedParameterJdbcTemplate
(sql + params + limit)                     │
                                           ▼
                                    ReflectiveRowMapper<T>
                                           │
                                           │  snake_case → camelCase
                                           │  tipo conversion
                                           ▼
                                    CursorPage<T> → Controller → JSON response
```

---

## Passo 10 — Ordem de implementação recomendada

| Dia | O que implementar | Ficheiros | Teste |
|-----|-------------------|-----------|-------|
| 1 | DTOs e enums | FilterCriteria, FilterOperator, SortCriteria, SortDirection, CursorPageRequest, SearchPayload, CursorPage, CursorData, SearchResult, FieldMapping | Deserializar JSON de exemplo com Jackson |
| 2 | Excepções | DomainException, InvalidFilterException, InvalidSortException, NonUniqueResultException | — |
| 3 | SearchQueryBuilder — esqueleto + WHERE | Construtor, field(), fixedCondition(), buildFilterClause(), convertValue(), escapeForLike() | Teste unitário: builder com filtros → verificar SQL gerado |
| 4 | SearchQueryBuilder — ORDER BY | buildOrderByClause() com validação e tiebreaker | Teste: verificar ORDER BY com e sem sort no payload |
| 5 | ReflectiveRowMapper | Construtor, buildPropertyMap(), mapRow(), extractValue(), snakeToCamel() | Teste com ResultSet mockado e classe Trip |
| 6 | QueryExecutor | queryAs(), queryAsSingle(), queryAsUnchecked(), searchAs() | Teste de integração com PostgreSQL real |
| 7 | Keyset pagination | decodeCursor(), encodeCursor(), buildKeysetClause() | Teste: gerar cursor, descodificar, verificar condição SQL |
| 8 | TripRepository | search(), findById(), save(), softDelete() | Teste end-to-end: HTTP → controller → service → repo → DB → response |
| 9 | Refinamento | Direcções mistas no keyset, JSONB, edge cases | Testes: filtros vazios, sort vazio, cursor inválido, campo inexistente |

---

## Diferenças-chave entre o C# original e a versão Java

| Aspecto | C# Original | Versão Java |
|---------|------------|-------------|
| Validação de campos | Reflection sobre `TSearchFields` | Mapa explícito `Map<String, FieldMapping>` |
| Parâmetros SQL | Posicionais `@p0` via NpgsqlParameter | Nomeados `:p0` via NamedParameterJdbcTemplate |
| Paginação | OFFSET (`LIMIT x OFFSET y`) | Keyset/cursor (WHERE condition) |
| Mapeamento | Dapper (automático) | ReflectiveRowMapper (manual mas equivalente) |
| Execução | `dbConnection.QueryAsync<T>` | `jdbc.query(sql, params, rowMapper)` |
| Enums SQL | Mapping manual → PostgreSQL enum types | Enum como VARCHAR (sem custom types) |
| Tenant/user fixo | Flags `withTenant`/`withUsername` | `fixedCondition()` explícito |
| Resposta | `Result<T>` com Page/PageSize/TotalItems | `CursorPage<T>` com nextCursor/hasMore |

---

*Os protótipos mostram a estrutura completa — assinaturas, tipos, comentários com lógica interna. O corpo dos métodos é para implementares. Quando chegares a um TODO, sabes exactamente o que o método deve fazer e porquê.*
