using System.Linq.Expressions;

namespace Domain.Utils
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            // Substitui os parâmetros nas expressões originais pelos parâmetros compartilhados
            var leftVisitor = new ReplaceExpressionVisitor();
            leftVisitor.Add(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor();
            rightVisitor.Add(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            // Combina as expressões usando AndAlso
            var combined = Expression.AndAlso(left, right);

            // Retorna uma nova expressão lambda combinada
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Dictionary<Expression, Expression> _replaceMap;

            public ReplaceExpressionVisitor()
            {
                _replaceMap = new Dictionary<Expression, Expression>();
            }

            public void Add(Expression oldValue, Expression newValue)
            {
                _replaceMap.Add(oldValue, newValue);
            }

            public override Expression Visit(Expression node)
            {
                if (node != null && _replaceMap.TryGetValue(node, out var newValue))
                {
                    return newValue;
                }
                return base.Visit(node);
            }
        }
    }
}