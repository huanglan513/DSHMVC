using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Entity;

namespace DSHOrder.Web.Common
{
    public static class HtmlHelpers
    {

        /// <summary>
        /// Truncat the string to specified length.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Truncat(this HtmlHelper helper, string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length) + "...";
            }
        }

        /// <summary>
        /// Transform the GroupByGroup Status to String
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ConvertGBPStsToString(this HtmlHelper helper, int? status)
        {
            string rst = string.Empty;

            if (!status.HasValue || status.Value == 0){return rst;}

            switch (status.Value)
            {
                case (int)GroupByGroupStatus.Returning:
                    rst = "未停止退款";
                    break;
                case (int)GroupByGroupStatus.Returned:
                    rst = "停止退款";
                    break;
                case (int)GroupByGroupStatus.FirstPay:
                    rst = "第一次打款";
                    break;
                case (int)GroupByGroupStatus.SecondPay:
                    rst = "第二次打款";
                    break;
                default:
                    break;
            }
            return rst;
        }

        /// <summary>
        /// 转换聚划算的团购平台显示
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="tradeFrom"></param>
        /// <returns></returns>
        public static string ConvertTradeFromToString(this HtmlHelper helper, string tradeFrom)
        {
            string rst = string.Empty;
            if (!string.IsNullOrEmpty(tradeFrom))
            {
                string[] strs = tradeFrom.Split(',');
                foreach (string tempStr in strs)
                { 
                    switch(tempStr)
                    {
                        case "WAP":
                            rst += "手机" + "<br/>";
                            break;
                        case "HITAO":
                            rst += "嗨淘" + "<br/>";
                            break;
                        case "TOP":
                            rst += "TOP平台" + "<br/>";
                            break;
                        case "TAOBAO":
                            rst += "普通淘宝" + "<br/>";
                            break;
                        case "JHS":
                            rst += "聚划算" + "<br/>";
                            break;    
                    }
                }
            }

            return rst;
        }
    }
}