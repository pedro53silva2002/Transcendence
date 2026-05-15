package com.transcendence.common.search;

/**
 * Request object defining pagination parameters for a search.
 */
public class CursorPageRequest {
    private int pageSize; //number of items per page
    private String encodedCursor; //opaque string representing the position in the dataset, can be null for the first page

    /**
     * Creates a default request, typically starting at the first page with 20 items.
     */
    public CursorPageRequest() {
        this.pageSize = 20;
        this.encodedCursor = null;
    }

    /**
     * Creates a request for a specific page position.
     *
     * @param pageSize      number of items to retrieve
     * @param encodedCursor opaque string representing the dataset position (null for first page)
     */
    public CursorPageRequest(int pageSize, String encodedCursor) {
        this.pageSize = pageSize;
        this.encodedCursor = encodedCursor;
    }

    public int getPageSize() { return pageSize; }
    void setPageSize(int pageSize) { this.pageSize = pageSize; }
    public String getEncodedCursor() { return encodedCursor; }
    public void setEncodedCursor(String encodedCursor) { this.encodedCursor = encodedCursor; }
}