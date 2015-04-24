using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;
using System.Web.Mvc;
using System.Web.Mail;
using System.Text;
using System.Configuration;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public static class GBGHelper
    {
        public static string GetSales(Entity.GroupByGroup gbg)
        {
            string strReturn = "";
            if (gbg.GroupBySales != null)
            {
                gbg.GroupBySales.ToList().ForEach(r => strReturn += string.Concat(r.User.UserName, ","));
            }

            return strReturn.Trim(',');
        }

        public static string GetAllCities(Entity.GroupByGroup gbg)
        {
            string strReturn = "";

            gbg.GroupByItem.ToList().ForEach(m =>
            {
                strReturn += string.Concat(m.GroupByPortal.PortalName, ":", GetCities(m.GroupByItemID), "\n");
            }
                );

            return strReturn.Trim();
        }
        public static string GetAllCities(Entity.GroupByItem item)
        {
            string strReturn = "";

            strReturn += string.Concat(item.GroupByPortal.PortalName, ":", GetCities(item.GroupByItemID), "\n");

            return strReturn.Trim();
        }

        public static string GetCities(int GroupByItemId)
        {
            string strReturn = "";

            IGroupByCityService serivce = new GroupByCityService();
            IList<GroupByCity> rows = serivce.GetCitiesByPortalId(GroupByItemId);

            rows.ToList().ForEach(r => strReturn += r.City.CityName + ",");
            strReturn = strReturn.Trim(',');

            return strReturn;
        }


        public static string Truncat(string input, int length)
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


        public static string UploadFileUrl(this UrlHelper url, UploadFile uf)
        {
            if (uf == null) return "";

            string strReturn = string.Concat(uf.FilePath, uf.FileName);

            strReturn = strReturn.Replace(@"\", @"/");

            if (!string.IsNullOrEmpty(strReturn))
            {
                strReturn = url.Content(strReturn);
            }

            return strReturn;
        }


        // GBGHelper.SendMail("dingj@hpsidc.com", "test content", "test subject");
        public static void SendMail(string MailTo, StringBuilder Content, string Subject)
        {
            SendMail(MailTo, Content, Subject);
        }

        public static void SendMail(string MailTo, string Content, string Subject)
        {
            try
            {
                MailMessage mailObj = new MailMessage();  //asp.net自封装的类
                mailObj.From = ConfigurationManager.AppSettings["WebAccountMail"];  // <!--邮件服务器--> <add key="WebAccountMail" value="admin@mspil.edu.cn"/>及发件人的邮箱地址
                mailObj.To = MailTo;     //收件人邮箱地址
                mailObj.Subject = Subject;  //邮件主题
                mailObj.Body = Content;  //邮件内容
                // 可选: 使用html格式的Email
                mailObj.BodyFormat = MailFormat.Html;
                // 可选: 设置邮件的优先级别为高
                mailObj.Priority = MailPriority.High;
                SmtpMail.SmtpServer = ConfigurationManager.AppSettings["MailServer"]; //<add key="MailServer" value="172.16.5.132"/>及获取发送电子邮件SMTP 中继邮件服务器的名称
                SmtpMail.Send(mailObj);
            }
            catch (Exception ex)
            {
            }
        }

    }
}