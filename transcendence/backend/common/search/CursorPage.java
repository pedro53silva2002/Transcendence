package transcendence.backend.common.search;

import java.util.List;

public class CursorPage<T> {

	private List<T> items;
	private String nextCursor;
	private boolean hasNext;
	private int size;

	public CursorPage() {}

	public CursorPage(List<T> items, String nextCursor, boolean hasNext, int size) {
		this.items = items;
		this.nextCursor = nextCursor;
		this.hasNext = hasNext;
		this.size = size;
	}

	public List<T> getItems() { return items; }
	public void setItems(List<T> items) { this.items = items; }
	public String getNextCursor() { return nextCursor; }
	public void setNextCursor(String nextCursor) { this.nextCursor = nextCursor; }
	public boolean isHasNext() { return hasNext; }
	public void setHasNext(boolean hasNext) { this.hasNext = hasNext; }
	public int getSize() { return size; }
	public void setSize(int size) { this.size = size; }
}
