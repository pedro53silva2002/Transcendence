package transcendence.backend.common.search;

/**
 * 
 */
public class CursorPageRequest {
    private int pageSize; //number of items per page
    private String cursor; //opaque string representing the position in the dataset, can be null for the first page

    public CursorPageRequest() {
        this.pageSize = 20;
        this.cursor = null;
    }

    public CursorPageRequest(int pageSize, String cursor) {
        this.pageSize = pageSize;
        this.cursor = cursor;
    }

    public int getPageSize() { return pageSize; }
    void setPageSize(int pageSize) { this.pageSize = pageSize; }
    public String getCursor() { return cursor; }
    public void setCursor(String cursor) { this.cursor = cursor; }
}