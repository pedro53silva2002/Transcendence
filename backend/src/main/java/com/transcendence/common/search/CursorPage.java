package com.transcendence.common.search;

import java.util.List;

/**
 * Represents a paginated response using cursor-based navigation.
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

	/**
     * Creates a populated page of results.
     *
     * @param content    the list of items for the current page
     * @param nextCursor the encoded string used to fetch the next page
     * @param hasNext    indicator if more results are available
     * @param size       the number of items in the current page
     */
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
