using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Drawing;
using System.Data.Linq;
using System.Text;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class ExpertDoc
    {
        public virtual void CreateDataObject()
        {

        }
      

        public virtual string Expert()
        {
            return null;
        }


        public static void DownloadDoc(string strUrl)
        {
            string strDownloadFileName = "";
            int iTemp = strUrl.LastIndexOf(",");
            if (iTemp >= 0)
            {
                strDownloadFileName = strUrl.Substring(iTemp + 1);
                strUrl = strUrl.Substring(0, iTemp);
            }

            string strFile = strUrl.Substring(strUrl.IndexOf(@"/Report")).Replace(@"/", @"\");
            strFile = HttpUtility.UrlDecode(strFile);
            strFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath.Trim('\\'), strFile.Trim('\\'));

            FileInfo file = new FileInfo(strFile);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名    
            if (string.IsNullOrEmpty(strDownloadFileName))
            {
                strDownloadFileName = file.Name;
            }
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", FileNameHelper.GetDownloadFileName(strDownloadFileName)));

            // 添加头信息，指定文件大小，让浏览器能够显示下载进度    
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            // 指定返回的是一个不能被客户端读取的流，必须被下载    
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            // 把文件流发送到客户端    
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.End();
        }



        public string GetExpertTimeString(DateTime dt)
        {
            var weekdays = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            var weekday = weekdays[(int)dt.DayOfWeek];

            string strReturn = string.Format("本報表生成時間： {0:dd/MM/yyyy HH:mm:ss}({1})", dt, weekday);
            strReturn = strReturn.Replace("-", "/");

            return strReturn;
        }


    }


    public static class FileNameHelper
    {
        private static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        ///指定一个字符是否应该被编码 Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";
            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;
            return true;
        }
        /// <summary>
        /// 为非英文字符串编码Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }


        public static string GetDownloadFileName(string fileName)
        {
            string strReturn = "";

            string strFileName = fileName;
            int iLeft = fileName.LastIndexOf("(");
            int iRight = fileName.LastIndexOf(")");
            if (iLeft > -1 && iRight > -1)
            {
                string strTemp = fileName.Substring(iLeft, iRight - iLeft + 1);
                strFileName = fileName.Replace(strTemp, "");
            }

            var Request = HttpContext.Current.Request;

            //使用自定义的
            string encodefileName = ToHexString(strFileName);
            if (Request.Browser.Browser.Contains("IE"))
            {
                //得到扩展名
                string ext = encodefileName.Substring(encodefileName.LastIndexOf('.'));
                //得到文件名称
                string name = encodefileName.Remove(encodefileName.Length - ext.Length);
                //关键代码
                name = name.Replace(".", "%2e");

                strReturn = name + ext;
            }
            else
            {
                strReturn = encodefileName;
            }

            return strReturn;
        }


    }
}