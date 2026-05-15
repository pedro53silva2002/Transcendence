package com.transcendence.common.search;

import java.util.List;

/**
 * Data Transfer Object (DTO) representing a complete search request from the client.
 * <p>
 * This class encapsulates all criteria required to perform a filtered, sorted, 
 * and paginated query. It acts as the primary input for the {@code SearchQueryBuilder} 
 * to generate dynamic SQL.
 * </p>
 *
 * @see FilterCriteria
 * @see SortCriteria
 * @see CursorPageRequest
 */
public class SearchPayload {
    private List<FilterCriteria> filters;
    private List<SortCriteria> sort;
    private CursorPageRequest page;

    public SearchPayload() {}
    public List<FilterCriteria> getFilters() { return filters; }
    public void setFilters(List<FilterCriteria> filters) { this.filters = filters; }
    public List<SortCriteria> getSort() { return sort; }
    public void setSort(List<SortCriteria> sort) { this.sort = sort; }
    public CursorPageRequest getPage() { return page; }
    public void setPage(CursorPageRequest page) { this.page = page; }
}