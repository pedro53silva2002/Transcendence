package backend.common.search;

import java.util.Map;

/**
 * Wrapper for the raw data used to generate and parse pagination cursors.
 */
public class CursorData {
	private Map<String, Object> data;
	
	public CursorData() {}
	
	/**
     * Creates a new cursor data container.
     *
     * @param data map of field names and values representing the cursor position
     */
	public CursorData(Map<String, Object> data) {
		this.data = data;
	}

	public Map<String, Object> getData() { return data; }
	public void setData(Map<String, Object> data) { this.data = data; }
}