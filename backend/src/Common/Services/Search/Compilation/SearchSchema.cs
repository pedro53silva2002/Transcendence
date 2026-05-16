using System.Collections.Frozen;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Compilation;

/// <summary>
/// Ephemeral, immutable per-query schema consumed by <see cref="SearchQueryCompiler{TEntity}"/>.
/// Built once per call by <c>SearchQueryBuilder</c>; the compiler reads it but never mutates it.
/// </summary>
/// <typeparam name="TEntity">The row type returned when queries from this schema run.</typeparam>
internal sealed class SearchSchema<TEntity>
{
    internal SearchSchema(
        string baseQuery,
        IReadOnlyDictionary<string, SearchableField> fields,
        IReadOnlyList<FixedCondition> fixedConditions,
        string keyFieldName)
    {
        BaseQuery = baseQuery;
        Fields = fields is FrozenDictionary<string, SearchableField> frozen
            ? frozen
            : fields.ToFrozenDictionary(StringComparer.Ordinal);
        FixedConditions = fixedConditions;
        KeyFieldName = keyFieldName;
    }

    public string BaseQuery { get; }
    public IReadOnlyDictionary<string, SearchableField> Fields { get; }
    public IReadOnlyList<FixedCondition> FixedConditions { get; }
    public string KeyFieldName { get; }
}
