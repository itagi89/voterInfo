using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VoterDeatils.DataAccess
{
    public static class SortUtility
    {
        public static IQueryable<TEntity> OrderByExtension<TEntity>(this IQueryable<TEntity> source,
                                                                    string orderByProperty, bool asc = true)
        {
            string command = asc ? "OrderBy" : "OrderByDescending";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(IQueryable), command, new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

    } 

    public static class StringUtilities
    {
        /// <summary>
        /// Lists to delimter seprated.
        /// </summary>
        /// <param name="listTobeConvert">The list tobe convert.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ListToDelimterSeprated(this ICollection<string> listTobeConvert, string delimiter = ",")
        {
            if (listTobeConvert?.Count != 0)
                return string.Join(delimiter, listTobeConvert);
            else
                return null;
        }

        /// <summary>
        /// Delimiters the seprated to list.
        /// </summary>
        /// <param name="valueToBeConvert">The value to be convert.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static List<string> DelimiterSepratedToList(this string valueToBeConvert, char delimiter = ',')
        {
            return new List<string>(valueToBeConvert?.Split(delimiter));
        }
    }
}
