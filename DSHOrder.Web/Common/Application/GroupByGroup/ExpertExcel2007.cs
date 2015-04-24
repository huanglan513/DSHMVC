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
using OfficeOpenXml;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class ExpertExcel2007 : ExpertDoc
    {
        #region var

        private ExcelPackage xlPackage = null;
        private ExcelWorksheet worksheet = null;

        private string strCompany = "";
        private string strSubject = "";
        private int indexRowExpertTime = 0;
        private int indexColumnExpertTime = 0;

        private int currentRowsIndex = 0;

        private DateTime dtNow = DateTime.Now;

        private string strFileName = "";

        private int iDataDefaultStyleID = -1;
        private int iRedStyleID = -1;
        private int iGreenStyleID = -1;
        private int iYellowStyleID = -1;

        #endregion

        public ExpertExcel2007(string strTemplateFile, string strCompany, string strSubject, int indexRowExpertTime, int indexColumnExpertTime)
        {
            dtNow = DateTime.Now;

            InitializeWorkbook(strTemplateFile, strCompany, strSubject);

            this.strCompany = strCompany;
            this.strSubject = strSubject;
            this.indexRowExpertTime = indexRowExpertTime;
            this.indexColumnExpertTime = indexColumnExpertTime;

            iDataDefaultStyleID = this.worksheet.Cell(1, 104).StyleID;
            iRedStyleID = this.worksheet.Cell(2, 104).StyleID;
            iGreenStyleID = this.worksheet.Cell(3, 104).StyleID;
            iYellowStyleID = this.worksheet.Cell(4, 104).StyleID;
        }

        #region private method

        private void InitializeWorkbook(string strTemplateFile, string strCompany, string strSubject)
        {
            strFileName = string.Concat(Guid.NewGuid(), ".xlsx");
            string strFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, string.Format(@"ExpertFile\Excel\{0}", strFileName));

            strTemplateFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, strTemplateFile);

            FileInfo fi = new FileInfo(strFilePath);
            if (!Directory.Exists(fi.Directory.Name))
            {
                fi.Directory.Create();
            }

            FileInfo newFile = new FileInfo(strFilePath);
            FileInfo template = new FileInfo(strTemplateFile);
            this.xlPackage = new ExcelPackage(newFile, template);
            this.worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
        }

        private void SetCellValue(int iColumnIndex, object o)
        {
            SetCellValue(currentRowsIndex, iColumnIndex, o);
        }
        private void SetCellValue(int iRowIndex, int iColumnIndex, object o)
        {
            SetCellValue(iRowIndex, iColumnIndex, iDataDefaultStyleID, o);
        }
        private void SetCellValue(int iRowIndex, int iColumnIndex, int iStyleID, object o)
        {
            ExcelCell cell = this.worksheet.Cell(iRowIndex, iColumnIndex);
            cell.StyleID = iStyleID;
            cell.Value = o.ToString();
        }

        #endregion

        #region public method

        public override string Expert()
        {
            xlPackage.Save();

            xlPackage.Dispose();

            string strReturn = @"\ExpertFile\Excel\" + strFileName;
            strReturn = string.Concat(strReturn, ",", strSubject, ".xlsx");

            return strReturn;
        }

        public override void CreateDataObject()
        {

        }

        #endregion
    }
}
