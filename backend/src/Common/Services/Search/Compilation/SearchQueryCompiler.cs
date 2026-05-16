using System.Collections;
using System.Text;
using Trippie.Common.Services.Search.Abstractions;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Compilation;

/// <summary>
/// Translates a <see cref="SearchPayload"/> + schema + dialect into an executable
/// <see cref="CompiledQuery"/>. Pure CPU — no I/O.
/// </summary>
internal sealed class SearchQueryCompiler<TEntity>(
    SearchSchema<TEntity> schema,
    ISqlDialect dialect,
    ValueConverterRegistry converters,
    SearchOptions options)
{
    private readonly SearchOptions _opts = options;

    public CompiledQuery Compile(SearchPayload? payload)
    {
        payload ??= new SearchPayload();
        var page = payload.Page ?? new CursorPageRequest();

        var parameters = new SqlParameterSet();
        var where = new List<string>(schema.FixedConditions.Count + (payload.Filters?.Count ?? 0));

        foreach (var fc in schema.FixedConditions)
        {
            where.Add(fc.Sql);
            if (fc.ParameterName is not null)
                parameters.AddNamed(fc.ParameterName, fc.Value);
        }

        if (payload.Filters is { Count: > 0 })
        {
            foreach (var filter in payload.Filters)
                where.Add(BuildFilter(filter, parameters));
        }

        var (orderBySql, sortFields, includesKey) = BuildOrderBy(payload.Sort);

        if (!string.IsNullOrWhiteSpace(page.EncodedCursor))
        {
            var cursor = CursorCodec.Decode(page.EncodedCursor);

            var sortsForKeyset = payload.Sort is { Count: > 0 }
                ? payload.Sort
                : (IReadOnlyList<SortCriterion>)[new SortCriterion(schema.KeyFieldName)];

            var keyset = BuildKeysetClause(cursor, sortsForKeyset, addKeyTiebreaker: !includesKey, parameters);
            if (keyset is not null) where.Add(keyset);
        }

        var requested = page.PageSize <= 0 ? _opts.DefaultPageSize : page.PageSize;
        var size = Math.Clamp(requested, 1, _opts.MaxPageSize);
        var limit = size + 1;

        var sb = new StringBuilder(schema.BaseQuery.Length + 256);
        sb.Append(schema.BaseQuery);
        if (where.Count > 0)
        {
            sb.Append(" WHERE ").AppendJoin(" AND ", where);
        }
        sb.Append(orderBySql).Append(dialect.FormatLimit(limit));

        return new CompiledQuery(sb.ToString(), parameters.Snapshot(), limit, sortFields);
    }

    private string BuildFilter(FilterCriterion f, SqlParameterSet parameters)
    {
        if (string.IsNullOrWhiteSpace(f.Column))
            throw new SearchValidationException("Filter column cannot be empty.");

        if (!schema.Fields.TryGetValue(f.Column, out var field))
            throw new SearchValidationException($"Unknown field: {f.Column}");

        if (!field.Filterable)
            throw new SearchValidationException($"Field not filterable: {f.Column}");

        var col = field.Column;
        var p = dialect.ParameterPrefix;

        string Bind(object? value, string? hint = null)
        {
            var converted = converters.Convert(value, field.ClrType);
            var name = parameters.Add(hint ?? col, converted);
            return $"{p}{name}";
        }

        return f.Operator switch
        {
            FilterOperator.Equal => $"{col} = {Bind(f.Value)}",
            FilterOperator.NotEqual => $"{col} <> {Bind(f.Value)}",
            FilterOperator.GreaterThan => $"{col} > {Bind(f.Value)}",
            FilterOperator.GreaterThanOrEqual => $"{col} >= {Bind(f.Value)}",
            FilterOperator.LessThan => $"{col} < {Bind(f.Value)}",
            FilterOperator.LessThanOrEqual => $"{col} <= {Bind(f.Value)}",

            FilterOperator.After => $"{col} > {Bind(f.Value)}",
            FilterOperator.Before => $"{col} < {Bind(f.Value)}",

            FilterOperator.Between => BuildBetween(col, f, parameters),

            FilterOperator.Contains => dialect.Like(col, parameters.Add(col, $"%{dialect.EscapeLike(f.Value?.ToString() ?? "")}%")),
            FilterOperator.StartsWith => dialect.Like(col, parameters.Add(col, $"{dialect.EscapeLike(f.Value?.ToString() ?? "")}%")),
            FilterOperator.EndsWith => dialect.Like(col, parameters.Add(col, $"%{dialect.EscapeLike(f.Value?.ToString() ?? "")}")),

            FilterOperator.IsNull => $"{col} IS NULL",
            FilterOperator.IsNotNull => $"{col} IS NOT NULL",

            FilterOperator.In => dialect.AnyOf(col, parameters.Add(col, NormalizeCollection(f.Value, field.ClrType, f.Operator, f.Column))),
            FilterOperator.NotIn => dialect.NoneOf(col, parameters.Add(col, NormalizeCollection(f.Value, field.ClrType, f.Operator, f.Column))),

            _ => throw new InvalidOperationException($"Unhandled operator {f.Operator}.")
        };
    }

    private string BuildBetween(string column, FilterCriterion f, SqlParameterSet parameters)
    {
        if (f.ValueTo is null)
            throw new SearchValidationException($"BETWEEN requires valueTo for column: {f.Column}");

        var field = schema.Fields[f.Column];
        var pFrom = parameters.Add(column, converters.Convert(f.Value, field.ClrType));
        var pTo = parameters.Add($"{column}_to", converters.Convert(f.ValueTo, field.ClrType));
        var p = dialect.ParameterPrefix;
        return $"{column} BETWEEN {p}{pFrom} AND {p}{pTo}";
    }

    private object NormalizeCollection(object? value, Type elementType, FilterOperator op, string column)
    {
        if (value is null)
            throw new SearchValidationException($"{op} requires a non-null collection for column: {column}");

        if (value is string)
            throw new SearchValidationException($"{op} requires a list/array for column: {column}");

        if (value is not IEnumerable enumerable)
            throw new SearchValidationException($"{op} requires a list/array for column: {column}");

        var list = new List<object?>();
        foreach (var item in enumerable)
            list.Add(converters.Convert(item, elementType));
        return list;
    }

    private (string Sql, IReadOnlyList<string> Fields, bool IncludesKey) BuildOrderBy(
        IReadOnlyList<SortCriterion>? sorts)
    {
        if (sorts is null || sorts.Count == 0)
        {
            var keyCol = schema.Fields[schema.KeyFieldName].Column;
            return ($" ORDER BY {keyCol} ASC", [schema.KeyFieldName], true);
        }

        var clauses = new List<string>(sorts.Count + 1);
        var fields = new List<string>(sorts.Count + 1);
        var includesKey = false;

        foreach (var s in sorts)
        {
            if (string.IsNullOrWhiteSpace(s.Column))
                throw new SearchValidationException("Sort column cannot be empty.");
            if (!schema.Fields.TryGetValue(s.Column, out var field))
                throw new SearchValidationException($"Unknown field for sort: {s.Column}");
            if (!field.Sortable)
                throw new SearchValidationException($"Field not sortable: {s.Column}");

            clauses.Add($"{field.Column} {(s.Direction == SortDirection.Asc ? "ASC" : "DESC")}");
            fields.Add(s.Column);

            if (string.Equals(s.Column, schema.KeyFieldName, StringComparison.Ordinal))
                includesKey = true;
        }

        if (!includesKey)
        {
            var keyCol = schema.Fields[schema.KeyFieldName].Column;
            clauses.Add($"{keyCol} ASC");
            fields.Add(schema.KeyFieldName);
        }
        return ($" ORDER BY {string.Join(", ", clauses)}", fields, includesKey);
    }

    private string? BuildKeysetClause(
        CursorData? cursor,
        IReadOnlyList<SortCriterion> sorts,
        bool addKeyTiebreaker,
        SqlParameterSet parameters)
    {
        if (cursor is null || cursor.Data.Count == 0) return null;

        var cols = new List<string>(sorts.Count + 1);
        var refs = new List<string>(sorts.Count + 1);
        var p = dialect.ParameterPrefix;

        foreach (var s in sorts)
        {
            if (!schema.Fields.TryGetValue(s.Column, out var field))
                throw new SearchValidationException($"Unknown field in cursor sorting: {s.Column}");

            if (!cursor.Data.TryGetValue(s.Column, out var rawValue)) return null;

            var paramName = $"cursor_{s.Column}";
            cols.Add(field.Column);
            refs.Add($"{p}{paramName}");
            parameters.AddNamed(paramName, converters.Convert(rawValue, field.ClrType));
        }

        if (addKeyTiebreaker)
        {
            var keyName = schema.KeyFieldName;
            if (cursor.Data.TryGetValue(keyName, out var idVal))
            {
                var keyField = schema.Fields[keyName];
                cols.Add(keyField.Column);
                refs.Add($"{p}cursor_{keyName}");
                parameters.AddNamed($"cursor_{keyName}", converters.Convert(idVal, keyField.ClrType));
            }
        }

        return $"({string.Join(", ", cols)}) > ({string.Join(", ", refs)})";
    }
}
