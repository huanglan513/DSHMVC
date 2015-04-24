using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace DSHOrder.Common
{
    public class Pagination:ICloneable
    {
        private bool _paging = true;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Pagination()
        {
            CurrentPageIndex = 1;
            PageSize = 10;
            SortExpress = new List<string>();
        }

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool Paging
        {
            get { return _paging && PageSize > 0; }
            set { _paging = value; }
        }

        /// <summary>
        /// 系统ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 数据数量，涉及总行数变更的查询时,赋值为null。备注：为NULL是查询总行数
        /// </summary>
        public int? RowCount { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (RowCount.HasValue)
                    return (int)(RowCount.Value / PageSize) + ((RowCount.Value % PageSize) == 0 ? 0 : 1);
                else
                    return 0;
            }
        }

        public List<string> SortExpress { get; set; }

        /// <summary>
        /// 按照指定字段排序，如果字段已经存在排序表达式中，则逆反排序方向
        /// </summary>
        /// <param name="expression">排序字段</param>
        /// <returns></returns>
        public Pagination OrderBy(string expression)
        {
            var item = SortExpress.Select(c => c.Split(' ')).Where(c => c[0] == expression).FirstOrDefault();

            bool desc = false;
            if (item != null)
            {
                desc = item[1] != "desc";
            }
            SortExpress = new List<string>() { expression + (desc ? " desc" : " asc") };
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public Pagination OrderBy(string expression, bool desc)
        {
            SortExpress = new List<string>() { expression + (desc ? " desc" : " asc") };
            return this;
        }

        /// <summary>
        /// 增加一个排序项
        /// </summary>
        /// <typeparam name="T">排序类型</typeparam>
        /// <param name="expression">排序表达式</param>
        /// <returns><see cref="Pagination"/></returns>
        /// <example>
        /// <code><![CDATA[
        ///    var paging = new Pagination();
        ///    paging.Paging = true;
        ///    paging.PageSize = 25;
        ///    paging.OrderBy<Customer>(c => c.ContactName);
        /// ]]>
        /// </code>
        /// </example>
        public Pagination OrderBy<T>(params Expression<Func<T, object>>[] expression)
        {
            return OrderBy<T>(expression, false);
        }

        /// <summary>
        /// 增加一个倒排序项
        /// </summary>
        /// <typeparam name="T">排序类型</typeparam>
        /// <param name="expression">排序表达式</param>
        /// <returns><see cref="Pagination"/></returns>
        /// <example>
        /// <code><![CDATA[
        ///    var paging = new Pagination();
        ///    paging.Paging = true;
        ///    paging.PageSize = 25;
        ///    paging.OrderByDesc<Customer>(c => c.ContactName);
        /// ]]>
        /// </code>
        /// </example>
        public Pagination OrderByDesc<T>(params Expression<Func<T, object>>[] expression)
        {
            return OrderBy<T>(expression, true);
        }

        private Pagination OrderBy<T>(Expression<Func<T, object>>[] expression, bool desc)
        {
            foreach (var item in expression)
            {
                MemberExpression mexp = null;
                if (item.Body.NodeType == ExpressionType.Convert)
                {
                    var temp = (UnaryExpression)item.Body;
                    if (temp.Operand.NodeType == ExpressionType.MemberAccess)
                        mexp = (MemberExpression)temp.Operand;
                }
                else
                    mexp = item.Body as MemberExpression;

                if (mexp != null && mexp.Member != null)
                {
                    var s = string.Format("{0} {1}", mexp.Member.Name, desc ? "desc" : "asc");
                    SortExpress.Add(s);
                }
                else
                {
                    throw new ArgumentException("没有找到合适的属性");
                }
            }
            return this;
        }

        /// <summary>
        /// order 扩展
        /// </summary>
        /// <param name="source"><see cref="IQueryable"/></param>
        /// <param name="ordering">排序语句</param>
        /// <param name="values">参数</param>
        /// <returns><see cref="IQueryable"/></returns>
        //public static IQueryable OrderBy(this IQueryable source, string ordering, params object[] values)
        //{
        //    if (source == null) throw new ArgumentNullException("source");
        //    if (ordering == null) throw new ArgumentNullException("ordering");
        //    ParameterExpression[] parameters = new ParameterExpression[] {
        //        Expression.Parameter(source.ElementType, "") };
        //    ExpressionParser parser = new ExpressionParser(parameters, ordering, values);
        //    IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering();
        //    Expression queryExpr = source.Expression;
        //    string methodAsc = "OrderBy";
        //    string methodDesc = "OrderByDescending";
        //    foreach (DynamicOrdering o in orderings)
        //    {
        //        queryExpr = Expression.Call(
        //            typeof(Queryable), o.Ascending ? methodAsc : methodDesc,
        //            new Type[] { source.ElementType, o.Selector.Type },
        //            queryExpr, Expression.Quote(Expression.Lambda(o.Selector, parameters)));
        //        methodAsc = "ThenBy";
        //        methodDesc = "ThenByDescending";
        //    }
        //    return source.Provider.CreateQuery(queryExpr);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        /// 用此方法前必须要有OrderBy的语句
        public IQueryable<T> ParseQueryPaging<T>(IQueryable<T> query)
        {
            return ParseQuery(query) as IQueryable<T>; 
        }

        public IQueryable ParseQuery(IQueryable query)
        {
            if (Paging)
            {
                if (!RowCount.HasValue)
                    RowCount = (int)query.Provider.Execute( Expression.Call( typeof(Queryable), "Count", new Type[] { query.ElementType }, query.Expression));

               // if (SortExpress != null && SortExpress.Count > 0)
                   // query = query.OrderBy(string.Join(" ", this.SortExpress.ToArray()));
                     
                if (CurrentPageIndex < 1)
                    throw new ArgumentOutOfRangeException("当前页不能小于1");

                var qSkip = query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Skip", new Type[] { query.ElementType }, query.Expression, Expression.Constant((CurrentPageIndex - 1) * PageSize)));
                var qTake = qSkip.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Take", new Type[] { qSkip.ElementType }, qSkip.Expression, Expression.Constant(PageSize)));
                return qTake;
            }
            else
                return query;
        }


        /// <summary>
        /// 将当前分页信息复制到另一个分页实例
        /// </summary>
        /// <param name="paging">目标实例</param>
        public void ApplyTo(Pagination paging)
        {
            paging.ApplicationId = this.ApplicationId;
            paging.CurrentPageIndex = this.CurrentPageIndex;
            paging.PageSize = this.PageSize;
            paging.RowCount = this.RowCount;
            paging.SortExpress = this.SortExpress;
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            string exp = this.SortExpress == null ? string.Empty : string.Join(" ", this.SortExpress);
            string val = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                            this.ApplicationId, this.CurrentPageIndex,
                            this.PageCount, this.PageSize, this.Paging, this.RowCount, exp);
            return val.GetHashCode();

        }
    }
}

