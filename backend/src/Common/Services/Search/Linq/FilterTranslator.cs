using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Converts a single <see cref="FilterCriterion"/> plus its caller-supplied
/// member selector into a typed <c>Expression&lt;Func&lt;TEntity, bool&gt;&gt;</c>
/// that EF Core's query translator can turn into a SQL WHERE fragment.
/// </summary>
internal static class FilterTranslator
{
    public static Expression<Func<TEntity, bool>> Build<TEntity>(
        FilterCriterion criterion,
        Expression<Func<TEntity, object?>> selector)
    {
        var param = selector.Parameters[0];
        var (member, valueType) = ExpressionUnboxer.Unbox(selector.Body);
        var col = criterion.Column;

        Expression body = criterion.Operator switch
        {
            FilterOperator.Equal
                => Expression.Equal(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.NotEqual
                => Expression.NotEqual(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.GreaterThan or FilterOperator.After
                => Expression.GreaterThan(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.GreaterThanOrEqual
                => Expression.GreaterThanOrEqual(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.LessThan or FilterOperator.Before
                => Expression.LessThan(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.LessThanOrEqual
                => Expression.LessThanOrEqual(member, ValueCoercer.ToParameterExpression(criterion.Value, valueType)),
            FilterOperator.Between => BuildBetween(member, valueType, criterion),
            FilterOperator.Contains => BuildIlike(member, criterion.Value, col, "%{0}%"),
            FilterOperator.StartsWith => BuildIlike(member, criterion.Value, col, "{0}%"),
            FilterOperator.EndsWith => BuildIlike(member, criterion.Value, col, "%{0}"),
            FilterOperator.IsNull => Expression.Equal(member, Expression.Constant(null, member.Type)),
            FilterOperator.IsNotNull => Expression.NotEqual(member, Expression.Constant(null, member.Type)),
            FilterOperator.In => BuildContains(member, valueType, criterion.Value, col, negate: false),
            FilterOperator.NotIn => BuildContains(member, valueType, criterion.Value, col, negate: true),
            _ => throw new SearchValidationException($"Unsupported operator '{criterion.Operator}' for column '{col}'."),
        };

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }

    private static Expression BuildBetween(Expression member, Type valueType, FilterCriterion c)
    {
        if (c.ValueTo is null)
            throw new SearchValidationException($"Between requires valueTo for column: {c.Column}");
        var lo = ValueCoercer.ToParameterExpression(c.Value, valueType);
        var hi = ValueCoercer.ToParameterExpression(c.ValueTo, valueType);
        return Expression.AndAlso(
            Expression.GreaterThanOrEqual(member, lo),
            Expression.LessThanOrEqual(member, hi));
    }

    /// <summary>
    /// Builds <c>EF.Functions.ILike(member, pattern, "\")</c>. ILIKE is
    /// PostgreSQL's case-insensitive LIKE. The user value is escaped before
    /// the wildcards are inserted so a value of <c>"50%"</c> matches a literal
    /// percent sign rather than acting as a wildcard.
    /// </summary>
    private static Expression BuildIlike(Expression member, object? value, string col, string template)
    {
        if (member.Type != typeof(string))
            throw new SearchValidationException(
                $"Contains/StartsWith/EndsWith require a string column. '{col}' is {member.Type.Name}.");

        var raw = value?.ToString() ?? string.Empty;
        var escaped = raw
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
        var pattern = string.Format(template, escaped);

        // ILike is a static extension method, so we call it with a null instance
        // and pass EF.Functions in as the first argument.
        return Expression.Call(
            IlikeWithEscapeMI,
            Expression.Constant(EF.Functions),
            member,
            ValueCoercer.ToParameterExpression(pattern, typeof(string)),
            Expression.Constant("\\"));
    }

    private static Expression BuildContains(Expression member, Type elementType, object? value, string col, bool negate)
    {
        var list = ValueCoercer.CoerceList(value, elementType, col);
        var listType = typeof(List<>).MakeGenericType(elementType);
        var containsMI = listType.GetMethod(nameof(List<int>.Contains), [elementType])!;
        var call = Expression.Call(Expression.Constant(list, listType), containsMI, member);
        return negate ? Expression.Not(call) : call;
    }

    private static readonly MethodInfo IlikeWithEscapeMI =
        typeof(NpgsqlDbFunctionsExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == nameof(NpgsqlDbFunctionsExtensions.ILike)
                && m.GetParameters().Length == 4);
}
