namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// A single condition the client wants applied to the WHERE clause.
/// </summary>
/// <param name="Column">
/// The <b>logical</b> name of the field (the key registered in the schema, e.g. "username"),
/// NOT the database column. The compiler resolves it to the real column via the schema.
/// </param>
/// <param name="Operator">Comparison operator (see <see cref="FilterOperator"/>).</param>
/// <param name="Value">
/// Primary value. Type is <c>object?</c> because JSON deserialization doesn't know
/// the target type yet — it's the schema that tells us "this field is a long" and the
/// <c>ValueConverterRegistry</c> coerces the boxed JSON value into the right CLR type.
/// </param>
/// <param name="ValueTo">
/// Secondary value, only used by <see cref="FilterOperator.Between"/>. Null otherwise.
/// </param>
/// <remarks>
/// This is a <c>record</c>, not a class. A record gives us for free:
///   - immutability (init-only setters),
///   - structural equality (two FilterCriterion with the same field values are equal),
///   - a tidy ToString,
///   - deconstruction.
/// All of those are wins for testing and for safely passing the value across threads.
/// </remarks>
public sealed record FilterCriterion(
    string Column,
    FilterOperator Operator,
    object? Value = null,
    object? ValueTo = null);
