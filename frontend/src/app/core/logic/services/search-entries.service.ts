import type { SearchOperator, SearchParams } from './search.service';

/**
 * An Entry is a single unit of search or sort intent.
 *
 * WHY THIS ABSTRACTION?
 * Components often build search queries conditionally:
 *   - "If the user typed something, add a CONTAINS filter"
 *   - "If a date range is set, add a BETWEEN filter"
 *   - "Always sort by createdAt DESC"
 *
 * Rather than building these with if/else trees, you declare an array
 * of entries — some of which may be false/null when the condition isn't met —
 * and pass the whole array here. Falsy entries are silently ignored.
 *
 * This is similar to how the `clsx` library handles conditional classNames.
 *
 * Example:
 *   searchEntriesToParams([
 *     { type: 'search', value: { field: 'name', operator: 'CONTAINS', value: nameFilter } },
 *     { type: 'search', value: { field: 'createdAt', operator: 'BETWEEN', value: fromDate, valueTo: toDate } },
 *     { type: 'order', value: { field: 'createdAt', direction: 'desc' } },
 *   ], 20, cursor)
 */
type Entry =
  | {
      type: 'search';
      value: {
        field: string;
        operator?: SearchOperator;
        value: string | number | boolean | Date | null;
        valueTo?: string | number | boolean | Date;
      };
    }
  | {
      type: 'order';
      value: {
        field: string;
        direction: 'asc' | 'desc';
      };
    };

/**
 * Converts an array of loosely-defined Entry values into a typed SearchParams object
 * ready to be passed to buildSearchUrl().
 *
 * Falsy values (false, null, undefined) in the array are silently ignored,
 * allowing clean conditional inclusion (see example above).
 *
 * @param entries  - Mixed array of Entry | false | null | undefined
 * @param pageSize - Items per page (default 20, backend caps at 100)
 * @param cursor   - Cursor from previous response, null for first page
 */
export function searchEntriesToParams<
  TSearchDto extends Record<string, unknown>,
  TOrderByEnum extends string | number,
>(
  entries: (Entry | undefined | null | false)[],
  pageSize = 20,
  cursor: string | null = null,
): SearchParams<TSearchDto, TOrderByEnum> {
  const search: Partial<Record<keyof TSearchDto, unknown>> = {};
  const orderBy: { field: TOrderByEnum; descending?: boolean }[] = [];

  const normalise = (v: string | number | boolean | Date | null) =>
    v === null ? null : v instanceof Date ? v.toISOString() : String(v);

  for (const e of entries) {
    if (!e) continue;

    if (e.type === 'search') {
      const { field, operator, value, valueTo } = e.value;

      if (value === undefined) continue;

      if (operator === 'BETWEEN' && valueTo !== undefined) {
        search[field as keyof TSearchDto] = {
          op: 'BETWEEN' as const,
          value: normalise(value),
          valueTo: normalise(valueTo),
        };
      } else if (operator) {
        search[field as keyof TSearchDto] = {
          op: operator,
          value: normalise(value),
        };
      } else {
        search[field as keyof TSearchDto] = normalise(value);
      }
    } else if (e.type === 'order') {
      orderBy.push({
        field: e.value.field as TOrderByEnum,
        descending: e.value.direction === 'desc',
      });
    }
  }

  return {
    search: Object.keys(search).length
      ? (search as SearchParams<TSearchDto, TOrderByEnum>['search'])
      : undefined,
    orderBy: orderBy.length ? orderBy : undefined,
    pageSize,
    cursor,
  };
}
