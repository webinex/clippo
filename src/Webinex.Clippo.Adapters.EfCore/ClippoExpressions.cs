using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Webinex.Clippo.Adapters.EfCore
{
    internal static class ClippoExpressions
    {
        public static Expression<Func<TModel, bool>> Contains<TModel, TValue>(
            Expression<Func<TModel, TValue>> valueAccessor,
            IEnumerable<TValue> values)
        {
            valueAccessor = valueAccessor ?? throw new ArgumentNullException(nameof(valueAccessor));
            values = values ?? throw new ArgumentNullException(nameof(values));
            
            var containsExp = (MethodCallExpression)_____ContainsExpression<TValue>().Body;
            var parameterExpression = valueAccessor.Parameters[0];
            var constantValuesExpression = Expression.Constant(values, typeof(IEnumerable<object>));
            var containsExpression = Expression.Call(null, containsExp.Method, constantValuesExpression, valueAccessor.Body);
            return Expression.Lambda<Func<TModel, bool>>(containsExpression, new[] { parameterExpression });
        }
        
        public static Expression<Func<TModel, bool>> Equals<TModel, TValue>(
            Expression<Func<TModel, TValue>> valueAccessor,
            TValue value)
        {
            valueAccessor = valueAccessor ?? throw new ArgumentNullException(nameof(valueAccessor));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var equalsExpressionBody = (MethodCallExpression)_____EqualsExpression<TValue>().Body;
            var parameterExpression = valueAccessor.Parameters[0];
            var constantValueExpression = Expression.Constant(value, typeof(TValue));
            var castExpression = Expression.Convert(valueAccessor.Body, typeof(object));
            var containsExpression = Expression.Call(constantValueExpression, equalsExpressionBody.Method, castExpression);
            return Expression.Lambda<Func<TModel, bool>>(containsExpression, new[] { parameterExpression });
        }

        private static Expression<Func<T, bool>> _____EqualsExpression<T>()
        {
            return (v) => v.Equals(default(T));
        }

        private static Expression<Func<T[], bool>> _____ContainsExpression<T>()
        {
            return (values) => values.Contains(default);
        }
    }
}