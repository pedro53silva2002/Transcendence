package transcendence.backend.common.search;

import java.util.Map;

/**
 * Stores cursor-related key-value data used in operations such as
 * pagination or tracking the current position in a dataset.
 */
public class CursorData {
	private Map<String, Object> data;
	
	public CursorData() {}
	
	public CursorData(Map<String, Object> data) {
		this.data = data;
	}

	public Map<String, Object> getData() { return data; }
	public void setData(Map<String, Object> data) { this.data = data; }
}