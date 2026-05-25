/**
 * Operators are UPPERCASE to match the backend's FilterOperator enum exactly.
 * The backend does a direct enum valueOf() on this string — casing matters.
 */

export type SearchOperator =
  | 'EQ'
  | 'NEQ'
  | 'GT'
  | 'GTE'
  | 'LT'
  | 'LTE'
  | 'BEFORE'
  | 'AFTER'
  | 'BETWEEN'
  | 'CONTAINS'
  | 'STARTSWITH'
  | 'ENDSWITH'
  | 'ISNULL'
  | 'ISNOTNULL'
  | 'IN'
  | 'NOTIN';

/**
 * A single filter value.
 *
 * Three forms are supported:
 *   - Plain value          → "john"         → becomes EQ filter
 *   - Operator + value     → { op: 'CONTAINS', value: 'john' }
 *   - BETWEEN (two values) → { op: 'BETWEEN', value: startDate, valueTo: endDate }
 */
export type SearchFieldValue =
  | string
  | number
  | boolean
  | Date
  | { op: Exclude<SearchOperator, 'BETWEEN'>; value: string | number | boolean | Date | null }
  | { op: 'BETWEEN'; value: string | number | Date; valueTo: string | number | Date };

/**
 * The exact JSON structure the backend's SearchPayload expects after decoding ?q=.
 *
 * filters → List<FilterCriteria> in Java
 *   column   = the logical field name registered in SearchQueryBuilder
 *   operator = must match the FilterOperator enum value exactly (uppercase)
 *   value    = primary value (omitted for IS_NULL / IS_NOT_NULL)
 *   valueTo  = secondary value (BETWEEN only)
 *
 * sort → List<SortCriteria> in Java
 *   column    = the logical field name registered in SearchQueryBuilder
 *   direction = "ASC" or "DESC" — must match the SortDirection enum exactly
 *
 * page → CursorPageRequest in Java
 *   pageSize      = number of items per page (backend caps at 100)
 *   encodedCursor = Base64url cursor from the previous response, null for the first page
 */
interface BackendSearchPayload {
  filters: {
    column: string;
    operator: SearchOperator;
    value?: string | number | boolean | null;
    valueTo?: string | number | boolean | null;
  }[];
  sort: {
    column: string;
    direction: 'ASC' | 'DESC';
  }[];
  page: {
    pageSize: number;
    encodedCursor: string | null;
  };
}

/**
 * The paginated response shape returned by the backend's CursorPage<T>.
 *
 * content    → the items for this page (not "results" — matches the Java field name)
 * nextCursor → send this back as encodedCursor to get the next page; null if no more pages
 * hasNext    → whether there are more results after this page
 * size       → how many items are actually in this page
 *
 * Note: the backend does NOT return previousCursor or totalItems.
 * Cursor pagination is forward-only by design. To go back, store the cursor
 * history yourself in a stack on the frontend (see component example below).
 */
export type CursorPage<T> = {
  content: T[];
  nextCursor: string | null;
  hasNext: boolean;
  size: number;
};

/**
 * The top-level parameters you pass when building a search query.
 *
 * TSearchDto    = shape of the search fields (must match field names registered on the backend)
 * TOrderByEnum  = string or numeric enum of valid sort fields
 *
 * Using generics ensures TypeScript catches typos in field names at compile time.
 */
export interface SearchParams<
  TSearchDto extends Record<string, unknown>,
  TOrderByEnum extends string | number,
> {
  search?: Partial<Record<keyof TSearchDto, SearchFieldValue>>;
  orderBy?: { field: TOrderByEnum; descending?: boolean }[];
  pageSize?: number;
  cursor?: string | null;
}

/**
 * Serialises a SearchParams object into a Base64-encoded string
 * suitable for use as a ?q= query parameter.
 *
 * The resulting payload sent to the server looks like:
 * {
 *   search:   [{ Field: "name", Operator: "contains", Value: "john" }],
 *   orderBy:  [{ Field: "createdAt", Descending: true }],
 *   page:     1,
 *   pageSize: 20
 * }
 *
 * Note: Field names are PascalCase to match the C# backend DTO conventions.
 *
 * The Base64 encoding uses TextEncoder to safely handle Unicode characters
 * before encoding, avoiding issues with btoa() on non-ASCII strings.
 */
export function searchToQuery<
  TSearchDto extends Record<string, unknown>,
  TOrderByEnum extends string | number,
>(params: SearchParams<TSearchDto, TOrderByEnum>): string {
  if (!params) return '';

  const payload: BackendSearchPayload = {
    filters: [],
    sort: [],
    page: {
      pageSize: params.pageSize ?? 20,
      encodedCursor: params.cursor ?? null,
    },
  };

  if (params.search) {
    for (const [key, raw] of Object.entries(params.search)) {
      if (raw == undefined || raw == null) continue;

      if (typeof raw === 'object' && !(raw instanceof Date)) {
        if (raw.op === 'BETWEEN' && 'valueTo' in raw) {
          payload.filters.push({
            column: key,
            operator: 'BETWEEN',
            value: raw.value instanceof Date ? raw.value.toISOString() : raw.value,
            valueTo: raw.valueTo instanceof Date ? raw.valueTo.toISOString() : raw.valueTo,
          });
        } else if ('op' in raw) {
          payload.filters.push({
            column: key,
            operator: raw.op,
            value: raw.value instanceof Date ? raw.value.toISOString() : raw.value,
          });
        } else {
          payload.filters.push({
            column: key,
            operator: 'EQ',
            value: raw instanceof Date ? (raw as Date).toISOString() : raw,
          });
        }
      }
    }
  }

  if (params.orderBy) {
    for (const o of params.orderBy) {
      payload.sort.push({
        column: String(o.field),
        direction: o.descending ? 'DESC' : 'ASC',
      });
    }
  }

  const json = JSON.stringify(payload);

  return btoa(
    new TextEncoder().encode(json).reduce((data, byte) => data + String.fromCharCode(byte), ''),
  );
}

/**
 * Appends the encoded search payload as ?q= to the given API path.
 * Returns the path unchanged if the payload encodes to an empty string.
 *
 * Usage in a feature service:
 *   this._get(buildSearchUrl('/invoices', params))
 */
export function buildSearchUrl<
  TSearchDto extends Record<string, unknown>,
  TOrderByEnum extends string | number,
>(path: string, params: SearchParams<TSearchDto, TOrderByEnum>): string {
  const q = searchToQuery(params);
  return q ? '${path}?q=${q}' : path;
}
