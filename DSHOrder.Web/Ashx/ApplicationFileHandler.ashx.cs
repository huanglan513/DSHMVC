using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;

using DSHOrder.Common;


namespace DSHOrder.Web.Ashx
{
    /// <summary>
    /// ApplicatoinFileHandler 的摘要说明
    /// </summary>
    public class ApplicationFileHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile FileData = context.Request.Files["Filedata"];

            try
            {
                string strOrgFileName = FileData.FileName;

                //获得文件扩展名
                string strExt = Path.GetExtension(FileData.FileName);
                string strNewFileName = Guid.NewGuid().ToString();
                //实际保存文件名
                string strSaveName = strNewFileName + strExt;

                UploadFile result = SaveFile(FileData, strOrgFileName, strSaveName);//保存文件

                string strFilePos = "-1";
                string strData = HttpContext.Current.Request.Params["Data"];
                var datas = strData.Split('_');
                if (datas.Length > 1)
                {
                    strFilePos = datas[1];
                }

                string strPath = result.FilePath.Replace("\\", "/") + strSaveName;

                context.Response.ContentType = "text/plain";
                object o = new { UploadFileID = result.UploadFileID, Path = strPath, FileName = result.OriginalFileName, FilePos = int.Parse(strFilePos) };

                context.Response.Write(o.ToJson());
            }
            catch (Exception ex)
            {
                // result = "";
            }

        }
        private UploadFile SaveFile(HttpPostedFile postedFile, string OrgFileName, string saveName)
        {
            string strType = HttpContext.Current.Request.Params["Data"];
            strType = strType.Split('_')[0];

            ICodeTableService service = new CodeTableService();
            CodeTable codetable = service.GetCodeTableByTypeAndValue((int)ComUtil.CodeType.UploadFileType, strType);
            string strPath = codetable.CodeDesc;

            string strFileFullPath = HttpContext.Current.Request.MapPath("~" + "/" + strPath.Trim('/', '\\') + "/");
            // string strReturn = HttpContext.Current.Request.MapPath("~" + "/" + strPath.Trim('/', '\\') + "/") + saveName;


            if (!Directory.Exists(strFileFullPath))
            {
                Directory.CreateDirectory(strFileFullPath);
            }
            try
            {
                postedFile.SaveAs(strFileFullPath + saveName);

                // 存入数据库
                UploadFile uf = new UploadFile();
                uf.TypeCode = strType;
                uf.OriginalFileName = OrgFileName;
                uf.FileName = saveName;
                uf.FilePath = strPath;
                uf.DeleteInd = false;
                uf.CreateBy = HttpContext.Current.User.Identity.Name;
                uf.CreateTime = DateTime.Now;

                IUploadFileService ufService = new UploadFileService();
                ufService.Add(uf);

                return uf;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }

            // return strPath.Replace("\\", "/") + saveName;

            return null;
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