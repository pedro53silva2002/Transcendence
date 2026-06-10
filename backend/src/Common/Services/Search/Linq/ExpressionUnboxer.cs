using System.Linq.Expressions;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Helpers for working with <c>Expression&lt;Func&lt;TEntity, object?&gt;&gt;</c>
/// resolvers supplied by callers. Returning <c>object?</c> from the lambda is the
/// most ergonomic public API (one signature for every value type), but it forces
/// the compiler to insert a <c>Convert(..., object)</c> node around value-type
/// access (e.g. <c>x =&gt; (object)x.Id</c>). EF Core can translate that, but we
/// also need the <i>original</i> value type so we can coerce the JSON value the
/// client sent into a strongly-typed constant. Stripping the box gives us both.
/// </summary>
internal static class ExpressionUnboxer
{
    /// <summary>
    /// Peels off a leading <see cref="UnaryExpression"/> with NodeType Convert /
    /// ConvertChecked whose target is <see cref="object"/>. Returns the original
    /// inner expression and its CLR type. Idempotent for already-unboxed bodies.
    /// </summary>
    public static (Expression Inner, Type ValueType) Unbox(Expression body)
    {
        if (body is UnaryExpression u
            && (u.NodeType == ExpressionType.Convert || u.NodeType == ExpressionType.ConvertChecked)
            && u.Type == typeof(object))
        {
            return (u.Operand, u.Operand.Type);
        }
        return (body, body.Type);
    }

    /// <summary>
    /// Rewrites <paramref name="body"/> so every reference to <paramref name="from"/>
    /// becomes a reference to <paramref name="to"/>. Used to splice a caller-supplied
    /// lambda body into a fresh lambda built on our own parameter — necessary when
    /// combining multiple resolvers under a single <c>x</c> in the final tree.
    /// </summary>
    public static Expression Rebind(Expression body, ParameterExpression from, ParameterExpression to)
        => new ParameterRewriter(from, to).Visit(body)!;

    private sealed class ParameterRewriter(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
            => node == from ? to : node;
    }
}
