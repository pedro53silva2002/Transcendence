namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// A WHERE clause that is ALWAYS applied to the query, regardless of what the
/// client sends. Typical uses: tenant isolation (<c>tenant_id = @tenantId</c>) or
/// soft-delete filter (<c>deleted_at IS NULL</c>).
/// </summary>
/// <param name="Sql">SQL fragment, e.g. <c>"u.deleted_at IS NULL"</c>.</param>
/// <param name="ParameterName">
/// Name of a parameter referenced in <see cref="Sql"/>, or null if the clause has none.
/// </param>
/// <param name="Value">Value to bind to <see cref="ParameterName"/>.</param>
public sealed record FixedCondition(string Sql, string? ParameterName, object? Value);
