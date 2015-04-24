using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Web.Models;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGFlowHelper
    {
        // 判断两个 Expression 语义是否一致， 出于性能和方便考虑， 该函数只做简单的判断， 只保证对本流程有效
        public static bool ExpressionIsSame(Expression<Func<GroupByFlowInfo, object>> e1, Expression<Func<GroupByFlowInfo, object>> e2)
        {
            string str1 = GetFieldString(e1);
            string str2 = GetFieldString(e2);

            return str1 == str2;
        }

        public static bool UserInRole(string strUser, string strRole)
        {
            IUserRoleService service = new UserRoleService();
            bool bReturn = service.UserInRole(strUser, strRole);

            return bReturn;
        }

        public static string GetFieldString<TValue>(Expression<Func<GroupByFlowInfo, TValue>> e)
        {
            string strReturn = e.Body.ToString();
            string strParamName = e.Parameters[0].Name;

            if (strReturn.StartsWith("Convert("))
            {
                strReturn = strReturn.Replace("Convert(", "");
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            // 数组， 索引名必须为 i 
            int iStart = strReturn.IndexOf(".get_Item(value(");
            if (iStart >= 0)
            {
                int iEnd = strReturn.IndexOf(".i)");
                strReturn = string.Concat(strReturn.Substring(0, iStart), ".get_Item(", strReturn.Substring(iEnd + 1));
            }

            strReturn = strReturn.Substring(strParamName.Length);

            return strReturn;
        }
    }



}