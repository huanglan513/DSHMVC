using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Common
{
    public static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, Pagination paging)
        {
            if (paging != null)
                return paging.ParseQuery(query) as IQueryable<T>;
            return query;
        }
    }
}
