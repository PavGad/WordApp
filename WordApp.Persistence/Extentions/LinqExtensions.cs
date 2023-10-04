using System.Linq.Expressions;

namespace WordApp.Persistence.Extentions
{
    internal static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

    }
}
