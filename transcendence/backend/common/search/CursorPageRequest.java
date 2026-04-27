package transcendence.backend.common.search;

/**
 * 
 */
public class CursorPageRequest {
    private int pageSize; //number of items per page
    private String encodedCursor; //opaque string representing the position in the dataset, can be null for the first page

    public CursorPageRequest() {
        this.pageSize = 20;
        this.encodedCursor = null;
    }

    public CursorPageRequest(int pageSize, String encodedCursor) {
        this.pageSize = pageSize;
        this.encodedCursor = encodedCursor;
    }

    public int getPageSize() { return pageSize; }
    void setPageSize(int pageSize) { this.pageSize = pageSize; }
    public String getEncodedCursor() { return encodedCursor; }
    public void setEncodedCursor(String encodedCursor) { this.encodedCursor = encodedCursor; }
}