using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Webinex.Clippo
{
    internal class ExpressionUtil
    {
        public static PropertyInfo Property<TModel, TValue>(
            [NotNull] Expression<Func<TModel, TValue>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            
            var memberExpression = (MemberExpression)navigator.Body;
            var memberInfo = memberExpression.Member;
            if ((memberInfo.MemberType & MemberTypes.Property) == 0)
                throw new InvalidOperationException("Might be property");

            return (PropertyInfo)memberInfo;
        }
    }
}