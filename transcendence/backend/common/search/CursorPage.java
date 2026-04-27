package transcendence.backend.common.search;

import java.util.List;

/**
 * Cursor-based paginated response (like a bookmark that represents a position in the dataset).
 * Returned to the frontend as part of a JSON response.
 *
 * Structure:
 * - items: list of elements in the current page
 * - nextCursor: cursor used to fetch the next page (the position in database) encoded as a string, so we are not dependent on the internal structure
 * - hasNext: indicates if more results are available
 * - size: number of items in the current page
 *
 * Example:
 * {
 *   "items": [...],
 *   "nextCursor": "eyJ...",
 *   "hasNext": true,
 *   "size": 20
 * }
 *
 * @param <T> type of the items in the list
 */
public class CursorPage<T> {

	private List<T> content;
	private String nextCursor;
	private boolean hasNext;
	private int size;

	public CursorPage() {}

	public CursorPage(List<T> content, String nextCursor, boolean hasNext, int size) {
		this.content = content;
		this.nextCursor = nextCursor;
		this.hasNext = hasNext;
		this.size = size;
	}

	public List<T> getContent() { return content; }
	public void setContent(List<T> content) { this.content = content; }
	public String getNextCursor() { return nextCursor; }
	public void setNextCursor(String nextCursor) { this.nextCursor = nextCursor; }
	public boolean isHasNext() { return hasNext; }
	public void setHasNext(boolean hasNext) { this.hasNext = hasNext; }
	public int getSize() { return size; }
	public void setSize(int size) { this.size = size; }
}
