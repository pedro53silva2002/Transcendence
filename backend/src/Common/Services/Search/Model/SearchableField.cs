namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// Metadata about a single field that the application allows clients to filter
/// or sort on. This is the .NET equivalent of Java's <c>FieldMapping</c> record,
/// with extra info (LogicalName) so error messages can refer to either name.
/// </summary>
/// <param name="LogicalName">
/// What the client uses in a payload — e.g. <c>"createdAt"</c>. Usually camelCase
/// because that's the wire format.
/// </param>
/// <param name="Column">
/// The fully-qualified SQL column — e.g. <c>"u.created_at"</c>. Provided by the
/// developer when configuring the schema, so it never comes from user input.
/// </param>
/// <param name="ClrType">
/// The CLR type to coerce the client-supplied value into before binding it as a
/// parameter — e.g. <c>typeof(DateTimeOffset)</c>. JSON deserialization can give
/// us a string or a number; the converter registry uses this to pick the right
/// converter.
/// </param>
/// <param name="Filterable">If false, attempts to filter on this field throw 400.</param>
/// <param name="Sortable">If false, attempts to sort on this field throw 400.</param>
public sealed record SearchableField(
    string LogicalName,
    string Column,
    Type ClrType,
    bool Filterable,
    bool Sortable);
