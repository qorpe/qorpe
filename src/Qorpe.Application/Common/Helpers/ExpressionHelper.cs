using System.Linq.Expressions;
using System.Reflection;

namespace Qorpe.Application.Common.Helpers;

/// <summary>
/// A static helper class that provides utility methods for building and manipulating expressions.
/// This class contains generic methods to dynamically create and modify LINQ expressions, 
/// such as building filter expressions based on the properties of an object.
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    /// Builds a filter expression dynamically based on the non-null and non-empty properties of the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object containing filter parameters.</typeparam>
    /// <typeparam name="TDocument">The type of the document in the collection.</typeparam>
    /// <param name="parameters">An object containing the filter parameters.</param>
    /// <returns>An expression that represents the filtering criteria based on the object's non-null properties.</returns>
    public static Expression<Func<TDocument, bool>> BuildFilterExpression<T, TDocument>(T parameters)
    {
        var parameterExpression = Expression.Parameter(typeof(TDocument), "x");
        Expression? combinedExpression = null;

        // Iterate over each property in the provided parameters object
        foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Get the property value
            var propertyValue = propertyInfo.GetValue(parameters);

            // Skip null or empty values (this can be customized for other "ignore" rules)
            if (propertyValue == null || (propertyValue is string str && string.IsNullOrEmpty(str)))
                continue;

            // Create the property access expression (x.Property)
            var documentProperty = Expression.Property(parameterExpression, propertyInfo.Name);

            // Create the constant expression for comparison
            var constantValue = Expression.Constant(propertyValue);

            // Create the equality comparison (x.Property == propertyValue)
            var comparisonExpression = Expression.Equal(documentProperty, constantValue);

            // Combine the expressions with 'AndAlso' for multiple conditions
            combinedExpression = combinedExpression == null
                ? comparisonExpression
                : Expression.AndAlso(combinedExpression, comparisonExpression);
        }

        // If there are no conditions, return a default "always true" expression
        return combinedExpression != null
            ? Expression.Lambda<Func<TDocument, bool>>(combinedExpression, parameterExpression)
            : x => true;
    }
}
