using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DSHOrder.Web.Ashx
{
    /// <summary>
    /// Summary description for FileHandler
    /// </summary>
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile FileData = context.Request.Files["Filedata"];

            string result = "";
            try
            {
               // result = Path.GetFileName(FileData.FileName);//获得文件名
                string ext = Path.GetExtension(FileData.FileName);//获得文件扩展名
                string newFileName=Guid.NewGuid().ToString();
                string saveName =newFileName + ext;//实际保存文件名
                saveFile(FileData, context.Request.MapPath("~" + context.Request["folder"] + "/"), saveName);//保存文件
                result = saveName;
            }
            catch (Exception ex)
            {
                result = "";
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }
        private void saveFile(HttpPostedFile postedFile, string phyPath, string saveName)
        {
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            try
            {
                postedFile.SaveAs(phyPath + saveName);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}