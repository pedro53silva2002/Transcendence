package transcendence.backend.common.search;

import java.util.List;

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