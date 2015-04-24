using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Service.Interface;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Models;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace DSHOrder.Web.Controllers
{
    public class PhoneFeeController : Controller
    {
        //
        // GET: /PhoneFee/

        public ActionResult Index(int? id, PhoneFeeInfoSearchModel searchModel)
        {
            searchModel.Result = Request.QueryString["selectedStatusList"];
            if (!string.IsNullOrEmpty(searchModel.Result) && searchModel.Result.Equals("全部"))
            {
                searchModel.Result = "";
            }
            if (searchModel.ResetIndex == -1)
            {
                id = 1;
            }
            searchModel.ResetIndex = 0;

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id.HasValue ? id.Value : 1;

            IPhoneFeeInfoService service = new PhoneFeeInfoService();
            IList<PhoneFeeInfo> feeList = service.GetPhoneFeeInfoByCondition(paging, searchModel.OrderID, searchModel.BuyerName,
                searchModel.GetGoodsAddr, searchModel.PhoneNumber, searchModel.Result, searchModel.OrderDateFrom,
                searchModel.OrderDateTo,searchModel.RechargeDateFrom,searchModel.RechargeDateTo);

            PagedList<PhoneFeeInfo> pagedPhoneFeeInofList = new PagedList<PhoneFeeInfo>(feeList, paging.CurrentPageIndex, paging.PageSize, paging.RowCount??0);
            searchModel.PhoneFeeInfoList = pagedPhoneFeeInofList;

            List<SelectListItem> selectedStatusList = new List<SelectListItem>();
            selectedStatusList.Add(new SelectListItem() { Text = "全部", Value = "全部" });

            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/StatusConfig.xml"));
            XmlNodeList xmlNodeList = doc.SelectNodes("InfoStatus/PhoneFee/Result");
            foreach (XmlNode item in xmlNodeList)
            {
                if (item.InnerText.Equals(searchModel.Result))
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
        // GET: /PhoneFee/Details/5

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportInfo(HttpPostedFileBase file)
        {
             string fileName = string.Empty;
             if (file.ContentLength == 0)           
             {                //文件大小大（以字节为单位）为0时，做一些操作　　　　　　
                 return View();　　　　
             }　　　　
             else　　　　{　　　　　　//文件大小不为0　　　　　　
                
                 //保存成自己的文件全路径,newfile就是你上传后保存的文件,　　　　　
                 //服务器上的UpLoadFile文件夹必须有读写权限　　
                 fileName = Server.MapPath(@"~\UploadFile\") + file.FileName.Substring(file.FileName.LastIndexOf('\\')+1);
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
                string strFirstSheetName = GetFirstSheetName(fileName);

                OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + strFirstSheetName + "$A:CV]", oConnection);
                oConnection.Open();
                oda.Fill(myDataSet);
                oConnection.Close();

                System.Data.DataTable dt = myDataSet.Tables[0].DefaultView.ToTable();

                string createBy = this.User.Identity.Name;
                IPhoneFeeInfoService service = new PhoneFeeInfoService();
                List<PhoneFeeInfo> infoList = new List<PhoneFeeInfo>();
                int columnCount = dt.Columns.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    PhoneFeeInfo info = new PhoneFeeInfo();
                    if (dr[0] == null || dr[0].ToString() == string.Empty)
                        break;
                    info.OrderID = dr[0].ToString();
                    info.BuyerName = dr[1].ToString();
                    if (dr[2] != null && dr[2].ToString() != string.Empty)
                    {
                        info.Payment = decimal.Parse(dr[2].ToString());
                    }
                    info.GetGoodsAddr = dr[3].ToString();
                    info.PhoneNumber = dr[4].ToString();

                    info.Result = dr[5].ToString();
                    if (columnCount >= 7)
                    {
                        info.Remark = dr[6].ToString();
                    }
                    info.CreateBy = createBy;
                    info.CreateTime = DateTime.Now;
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

        private static string GetFirstSheetName(string fileName)
        {
            Application obj = default(Application);
            Workbook objWB = default(Workbook);
            string strFirstSheetName = null;
            obj = (Application)Interaction.CreateObject("Excel.Application", string.Empty);
            objWB = obj.Workbooks.Open(fileName, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            strFirstSheetName = ((Worksheet)objWB.Worksheets[1]).Name;
            objWB.Close(Type.Missing, Type.Missing, Type.Missing);
            objWB = null;
            obj.Quit();
            obj = null;
            return strFirstSheetName;
        } 

        //
        // POST: /PhoneFee/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /PhoneFee/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PhoneFee/Edit/5

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
        // GET: /PhoneFee/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PhoneFee/Delete/5

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
    }
}
