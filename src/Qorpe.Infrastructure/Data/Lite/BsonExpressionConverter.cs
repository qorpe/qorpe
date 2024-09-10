using LiteDB;
using System.Linq.Expressions;

namespace Qorpe.Infrastructure.Data.Lite;

/// <summary>
/// A utility class that provides functionality to convert LINQ expressions 
/// into LiteDB-compatible <see cref="BsonExpression"/> objects.
/// This allows for the dynamic creation of queries in LiteDB using LINQ-like syntax.
/// 
/// Supports conversion of various expression types such as binary comparisons, 
/// method calls (e.g., string methods like Contains, StartsWith), and logical operations.
/// </summary>
public static class BsonExpressionConverter
{
    /// <summary>
    /// Converts a given LINQ expression into a LiteDB-compatible BsonExpression.
    /// </summary>
    /// <typeparam name="T">The type of the document.</typeparam>
    /// <param name="expression">The LINQ expression to convert.</param>
    /// <returns>A BsonExpression that can be used in LiteDB queries.</returns>
    public static BsonExpression ConvertExpression<T>(Expression<Func<T, bool>> expression)
    {
        return ParseExpression(expression.Body);
    }

    /// <summary>
    /// Parses an expression and returns a BsonExpression equivalent.
    /// Handles different expression types such as BinaryExpression, MemberExpression, and others.
    /// </summary>
    /// <param name="expression">The expression to parse.</param>
    /// <returns>A BsonExpression representing the parsed expression.</returns>
    private static BsonExpression ParseExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                return ParseBinaryExpression(binaryExpression);

            case MemberExpression memberExpression:
                return ParseMemberExpression(memberExpression);

            case ConstantExpression constantExpression:
                return ParseConstantExpression(constantExpression);

            case MethodCallExpression methodCallExpression:
                return ParseMethodCallExpression(methodCallExpression);

            case UnaryExpression unaryExpression:
                return ParseUnaryExpression(unaryExpression);

            default:
                throw new NotSupportedException($"Expression type '{expression.GetType()}' is not supported.");
        }
    }

    /// <summary>
    /// Parses a BinaryExpression (e.g., AND, OR, ==, <, >) and converts it into a BsonExpression string.
    /// </summary>
    /// <param name="expression">The binary expression to parse.</param>
    /// <returns>A BsonExpression equivalent to the binary operation.</returns>
    private static BsonExpression ParseBinaryExpression(BinaryExpression expression)
    {
        // Parse left and right sides of the binary expression
        var left = ParseExpression(expression.Left);
        var right = ParseExpression(expression.Right);

        // Convert based on the type of binary operation
        switch (expression.NodeType)
        {
            case ExpressionType.AndAlso:
                return BsonExpression.Create($"{left} AND {right}");

            case ExpressionType.OrElse:
                return BsonExpression.Create($"{left} OR {right}");

            case ExpressionType.Equal:
                return BsonExpression.Create($"{left} = {right}");

            case ExpressionType.NotEqual:
                return BsonExpression.Create($"{left} != {right}");

            case ExpressionType.GreaterThan:
                return BsonExpression.Create($"{left} > {right}");

            case ExpressionType.GreaterThanOrEqual:
                return BsonExpression.Create($"{left} >= {right}");

            case ExpressionType.LessThan:
                return BsonExpression.Create($"{left} < {right}");

            case ExpressionType.LessThanOrEqual:
                return BsonExpression.Create($"{left} <= {right}");

            default:
                throw new NotSupportedException($"Binary operator '{expression.NodeType}' is not supported.");
        }
    }

    /// <summary>
    /// Parses a MemberExpression, typically representing a field or property.
    /// Converts it into a BsonExpression that references the field.
    /// </summary>
    /// <param name="expression">The member expression to parse.</param>
    /// <returns>A BsonExpression that points to the field or property.</returns>
    private static BsonExpression ParseMemberExpression(MemberExpression expression)
    {
        // MemberExpression typically refers to a property, e.g., x => x.Name
        return BsonExpression.Create($"$.{expression.Member.Name}");
    }

    /// <summary>
    /// Parses a ConstantExpression, which typically holds a constant value.
    /// Converts it into a BsonExpression with the constant value.
    /// </summary>
    /// <param name="expression">The constant expression to parse.</param>
    /// <returns>A BsonExpression containing the constant value.</returns>
    private static BsonExpression ParseConstantExpression(ConstantExpression expression)
    {
        // ConstantExpression holds a constant value like 30 in x => x.Age > 30
        return BsonExpression.Create(expression.Value?.ToString());
    }

    /// <summary>
    /// Parses a MethodCallExpression, used when a method is called in the expression.
    /// Supports string methods like Contains, StartsWith, and EndsWith.
    /// </summary>
    /// <param name="expression">The method call expression to parse.</param>
    /// <returns>A BsonExpression representing the method call operation.</returns>
    private static BsonExpression ParseMethodCallExpression(MethodCallExpression expression)
    {
        var methodName = expression.Method.Name;

        // Handle string.Contains method
        if (methodName == "Contains")
        {
            var member = ParseExpression(expression.Object); // e.g., x.Name in x.Name.Contains("test")
            var argument = ParseExpression(expression.Arguments[0]); // "test"
            return BsonExpression.Create($"{member} LIKE '%{argument}%'");
        }

        // Handle string.StartsWith method
        if (methodName == "StartsWith")
        {
            var member = ParseExpression(expression.Object);
            var argument = ParseExpression(expression.Arguments[0]);
            return BsonExpression.Create($"{member} LIKE '{argument}%'");
        }

        // Handle string.EndsWith method
        if (methodName == "EndsWith")
        {
            var member = ParseExpression(expression.Object);
            var argument = ParseExpression(expression.Arguments[0]);
            return BsonExpression.Create($"{member} LIKE '%{argument}'");
        }

        throw new NotSupportedException($"Method '{methodName}' is not supported.");
    }

    /// <summary>
    /// Parses a UnaryExpression, typically representing negation (NOT).
    /// Converts it into a BsonExpression that negates the expression.
    /// </summary>
    /// <param name="expression">The unary expression to parse.</param>
    /// <returns>A BsonExpression representing the negated condition.</returns>
    private static BsonExpression ParseUnaryExpression(UnaryExpression expression)
    {
        // Handle negation (!)
        if (expression.NodeType == ExpressionType.Not)
        {
            var operand = ParseExpression(expression.Operand);
            return BsonExpression.Create($"NOT({operand})");
        }

        throw new NotSupportedException($"Unary operator '{expression.NodeType}' is not supported.");
    }
}
