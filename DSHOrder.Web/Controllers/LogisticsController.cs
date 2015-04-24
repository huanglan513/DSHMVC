using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;
using DSHOrder.Web.Models;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace DSHOrder.Web.Controllers
{
    public class LogisticsController : Controller
    {
        //
        // GET: /Logistics/

        public ActionResult Index(int? id, LogisticsSearchModel searchModel)
        {
            searchModel.Status = Request.QueryString["selectedStatusList"];
            if (!string.IsNullOrEmpty(searchModel.Status) && searchModel.Status.Equals("全部"))
            {
                searchModel.Status = "";
            }
            if (searchModel.ResetIndex == -1)
            {
                id = 1;
            }
            searchModel.ResetIndex = 0;

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id.HasValue ? id.Value : 1;

           

            ILogisticsService service = new LogisticsService();
            IList<LogisticsInfo> logisticsList = service.GetLogisticsByCondition(paging, searchModel.SerialNum, searchModel.OrderID, searchModel.GetGoodsDateFrom,
                searchModel.GetGoodsDateTo, searchModel.ArriveStopFrom, searchModel.ArriveStopTo, searchModel.Status, searchModel.CustomerName,
                searchModel.TelPhone, searchModel.Carrier, searchModel.Address);

            PagedList<LogisticsInfo> pagedLogisticsList = new PagedList<LogisticsInfo>(logisticsList, paging.CurrentPageIndex, paging.PageSize,paging.RowCount??0);
            searchModel.LogisticsList = pagedLogisticsList;

           List<SelectListItem> selectedStatusList = new List<SelectListItem>();
           selectedStatusList.Add(new SelectListItem() { Text = "全部", Value = "全部" });

            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/StatusConfig.xml"));
            XmlNodeList xmlNodeList = doc.SelectNodes("InfoStatus/Logisitics/Status");
            foreach (XmlNode item in xmlNodeList)
            {
                if (item.InnerText.Equals(searchModel.Status))
                {
                    selectedStatusList.Add(new SelectListItem() { Text = item.InnerText, Value = item.InnerText, Selected = true });
                }
                else
                {
                    selectedStatusList.Add(new SelectListItem() { Text = item.InnerText, Value = item.InnerText });
                }
            }
            ViewData["selectedStatusList"] = selectedStatusList;
            return View(searchModel);
        }

        //
        // GET: /Logistics/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PhoneFee/Create
        public ActionResult ImportInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportInfo(HttpPostedFileBase file)
        {
            string fileName = string.Empty;
            if (file.ContentLength == 0)
            {                //文件大小大（以字节为单位）为0时，做一些操作　　　　　　
                return View();
            }
            else
            {　　　　　　//文件大小不为0　　　　　　

                //保存成自己的文件全路径,newfile就是你上传后保存的文件,　　　　　
                //服务器上的UpLoadFile文件夹必须有读写权限　　
                fileName = Server.MapPath(@"~\UploadFile\") + file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1);
                file.SaveAs(fileName);
            }
           
            string result = string.Empty;
            string strConn = string.Empty;
            int index = fileName.LastIndexOf(".");
            string suffix = fileName.Substring(index + 1);
            if (suffix.Equals("xls"))
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + "; " + "Extended Properties='Excel 8.0;IMEX=0';";
            }
            else if (suffix.Equals("xlsx"))
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 12.0;";
            }
            DataSet myDataSet = new DataSet();
            OleDbConnection oConnection = new System.Data.OleDb.OleDbConnection(strConn);
            try
            {
                string strSecondSheetName = GetSecondSheetName(fileName);

                OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + strSecondSheetName + "$A:CV]", oConnection);
                oConnection.Open();
                oda.Fill(myDataSet);
                oConnection.Close();

                System.Data.DataTable dt = myDataSet.Tables[0].DefaultView.ToTable();
                
                string createBy = this.User.Identity.Name;
                ILogisticsService service = new LogisticsService();
                List<LogisticsInfo> infoList = new List<LogisticsInfo>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    LogisticsInfo info = new LogisticsInfo();
                    if (dr[0] == null || dr[0].ToString() == string.Empty)
                        break;
                    info.SerialNum = dr[0].ToString();
                    info.OrderID = dr[1].ToString();
                    info.Project = dr[2].ToString();
                    if (dr[3] != null && dr[3].ToString() != string.Empty)
                    {
                        info.GetGoodsDate = DateTime.Parse(dr[3].ToString());
                    }
                    info.SourceStop = dr[4].ToString();
                    info.DestStop = dr[5].ToString();
                    info.Status = dr[6].ToString();
                    if (dr[7] != null && dr[7].ToString() != string.Empty)
                    {
                        info.OperateDate = DateTime.Parse(dr[7].ToString());
                    }
                   
                    if (dr[8] != null && dr[8].ToString() != string.Empty)
                    {
                        info.ArriveStopDate = DateTime.Parse(dr[8].ToString());
                    }
                    info.Address = dr[9].ToString();
                    info.TelPhone = dr[10].ToString();
                    info.CustomerName = dr[11].ToString();
                    info.GoodsName = dr[12].ToString();
                    if (dr[13] != null && dr[13].ToString() != string.Empty)
                    {
                        info.SalesNum = int.Parse(dr[13].ToString());
                    }
                    info.Carrier = dr[14].ToString();
                    
                  //  info.Remark = dr[15].ToString();
                    info.CreateBy = createBy;
                    info.CreateDate = DateTime.Now;
                    infoList.Add(info);
                }

                service.Add(infoList);
                result = "导入成功！";
            }
            catch (Exception ex)
            {
                if (oConnection.State == ConnectionState.Open)
                    oConnection.Close();
                result = "导入失败!" + ex.Message;
            }

            finally
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }

            //JsonResult json = new JsonResult();
            //json.Data = result;
            //return json;
            ViewData["Msg"] = result;
            return View();
        } 

        
        //
        // GET: /Logistics/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Logistics/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Logistics/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Logistics/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static string GetSecondSheetName(string fileName)
        {
            Application obj = default(Application);
            Workbook objWB = default(Workbook);
            string strSecondSheetName = null;
            obj = (Application)Interaction.CreateObject("Excel.Application", string.Empty);
            objWB = obj.Workbooks.Open(fileName, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            strSecondSheetName = ((Worksheet)objWB.Worksheets[2]).Name;
            objWB.Close(Type.Missing, Type.Missing, Type.Missing);
            objWB = null;
            obj.Quit();
            obj = null;
            return strSecondSheetName;
        } 
    }
}
