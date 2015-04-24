using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using DSHOrder.Entity;
using System.Web.Mvc.Html;
using System.Text;


namespace DSHOrder.Web.Common
{
    public static partial class ListExtensions
    {
        public static MvcHtmlString CodeTableRadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IList<CodeTable> listCodeTable)
        {
            StringBuilder sbReturn = new StringBuilder();

            foreach (var item in listCodeTable)
            {
                sbReturn.Append(htmlHelper.RadioButtonFor(expression, item.CodeValue).ToHtmlString());
                sbReturn.Append(item.CodeDesc);
                sbReturn.Append("&nbsp;&nbsp;");
            }

            return new MvcHtmlString(sbReturn.ToString());
        }

        public static MvcHtmlString CodeTableCheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            IList<CodeTable> listCodeTable)
        {
            StringBuilder sbReturn = new StringBuilder();

            foreach (var item in listCodeTable)
            {
                sbReturn.Append(htmlHelper.CheckBoxFor(expression, new { value = item.CodeValue }).ToHtmlString());
                sbReturn.Append(item.CodeDesc);
                sbReturn.Append("&nbsp;&nbsp;");
            }

            return new MvcHtmlString(sbReturn.ToString());
        }


    }
}