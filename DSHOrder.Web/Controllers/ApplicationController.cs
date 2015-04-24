using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;
using DSHOrder.Web.Models;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.IO;

using DSHOrder.Common;
using System.Threading;
using System.Xml.Serialization;
using DSHOrder.Web.Common;
using DSHOrder.Web.Common.Application.GroupByGroup;
using System.Linq.Expressions;
using Webdiyer.WebControls.Mvc;
using System.Collections;

namespace DSHOrder.Web.Controllers
{
    public class ApplicationController : Controller
    {

        public ApplicationController()
        {

            IFunctionService service = new FunctionService();
            ViewData["functionList"] = service.GetAllFunctions();

        }

        #region StartForm

        [HttpGet]
        public ActionResult StartForm()
        {
            StartFormModel model = new StartFormModel(this.User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult StartForm(StartFormModel model)
        {
            // StartFormModel model = new StartFormModel(this.User.Identity.Name);
            var m1 = GetIndustryBySubId(model.SubIndustryID);
            model.IndustryName = m1.IndustryName;

            var m2 = GetSubIndustryById(model.SubIndustryID);
            model.SubIndustryName = m2.SubIndustryName;

            this.TempData["StartFormModel"] = model;

            return RedirectToAction("Form");
        }

        #endregion

        #region List

        public ActionResult List(AppSearchModel model, GBGFlowEnumFLowNode id)
        {
            int pageIndex = 1;

            if (this.Request.HttpMethod == "GET")
            {
                if (this.Session["model"] != null)
                {
                    model = this.Session["model"] as AppSearchModel;
                }
                if (this.Request.QueryString["pageIndex"] != null)
                {
                    pageIndex = int.Parse(this.Request.QueryString["pageIndex"]);
                }
            }

            model.CurrentNode = id;

            model = SearchGroupBy(model, pageIndex);

            this.Session["model"] = model;

            return View(model);
        }

        // todo: GetStatusBySearchType
        private List<string> GetStatusBySearchType(GBGFlowEnumSearchType searchType)
        {
            List<string> liReturn = new List<string>();

            switch (searchType)
            {
                // 全部团购
                case GBGFlowEnumSearchType.Node2Type1:
                    break;
                // 进行中的团购   
                case GBGFlowEnumSearchType.Node2Type2:
                    break;
                // 往期团购
                case GBGFlowEnumSearchType.Node2Type3:
                    break;
                // 未开始的团购
                case GBGFlowEnumSearchType.Node2Type4:
                    break;

                // 未审批的团购
                case GBGFlowEnumSearchType.Node3Type1:
                    break;
                // 已审批的团购
                case GBGFlowEnumSearchType.Node3Type2:
                    break;

                // 未审批的团购
                case GBGFlowEnumSearchType.Node4Type1:
                    break;
                // 已审批的团购
                case GBGFlowEnumSearchType.Node4Type2:
                    break;

                // 未审批的团购
                case GBGFlowEnumSearchType.Node5Type1:
                    break;
                // 已审批的团购   
                case GBGFlowEnumSearchType.Node5Type2:
                    break;

                // 未提交的团购
                case GBGFlowEnumSearchType.Node6Type1:
                    break;
                // 已提交的团购
                case GBGFlowEnumSearchType.Node6Type2:
                    break;

                // 未审批的团购
                case GBGFlowEnumSearchType.Node7Type1:
                    break;
                // 已审批的团购
                case GBGFlowEnumSearchType.Node7Type2:
                    break;

                // 未设计的团购   
                case GBGFlowEnumSearchType.Node8Type1:
                    break;
                // 已设计的团购   
                case GBGFlowEnumSearchType.Node8Type2:
                    break;

                // 未处理的团购   
                case GBGFlowEnumSearchType.Node9Type1:
                    break;
                // 已处理的团购
                case GBGFlowEnumSearchType.Node9Type2:
                    break;


                // 未处理的团购
                case GBGFlowEnumSearchType.Node10Type1:
                    break;
                // 已处理的团购
                case GBGFlowEnumSearchType.Node10Type2:
                    break;
                default:
                    break;
            }

            return liReturn;
        }

        private AppSearchModel SearchGroupBy(AppSearchModel model, int pageIndex)
        {
            Pagination paging = new Pagination();
            paging.PageSize = 20;
            paging.CurrentPageIndex = pageIndex;

            //todo: 加入 GetStatusBySearchType 条件

            var ris = new List<ResultItem>();
            IGroupByGroupService service = new GroupByGroupService();

            // 按团购
            if (model.CurrentNode <= GBGFlowEnumFLowNode.XSZLTJTZSP || model.CurrentNode == GBGFlowEnumFLowNode.END)
            {
                var items = service.SearchGroupBy(paging, model.CustomerCityId, model.PortalId, model.GroupByCodeOrName, model.Sale, model.CustomerName, model.SearchType.ToString());

                foreach (var item in items)
                {
                    ResultItem ri = new ResultItem();

                    ri.GroupByGroupId = item.GroupByGroupID;
                    ri.GroupByItemId = -1;
                    ri.CustomerCity = (item.Customer != null && item.Customer.City != null ? item.Customer.City.CityName : "");
                    ri.CustomerName = item.Customer != null ? item.Customer.CustomerName : "";
                    ri.GroupByGroupName = item.GroupByGroupName;
                    ri.Sales = GBGHelper.GetSales(item);
                    ri.AllCities = GBGHelper.GetAllCities(item);

                    ris.Add(ri);
                }
            }
            else
            {
                // 按平台
                List<GroupByItem> items = service.SearchGroupByItem(paging, model.CustomerCityId, model.PortalId, model.GroupByCodeOrName, model.Sale, model.CustomerName, model.CurrentNode.ToString());

                foreach (var item in items)
                {
                    ResultItem ri = new ResultItem();

                    ri.GroupByGroupId = item.GroupByGroup.GroupByGroupID;
                    ri.GroupByItemId = item.GroupByItemID;
                    ri.CustomerCity = (item.GroupByGroup.Customer != null && item.GroupByGroup.Customer.City != null ? item.GroupByGroup.Customer.City.CityName : "");
                    ri.CustomerName = item.GroupByGroup.Customer != null ? item.GroupByGroup.Customer.CustomerName : "";
                    ri.GroupByGroupName = item.GroupByGroup.GroupByGroupName;
                    ri.Sales = GBGHelper.GetSales(item.GroupByGroup);
                    ri.AllCities = GBGHelper.GetAllCities(item);

                    ris.Add(ri);
                }
            }

            model.ResultItems = ris;

            var Pager = new PagedList<ResultItem>(ris, paging.CurrentPageIndex, paging.PageSize, paging.RowCount.Value);

            model.Pager = Pager;

            return model;
        }


        [HttpPost]
        [ActionName("List")]
        [MultiButton(Name = "Open", Argument = "id")]
        public ActionResult ListForOpen(int id)
        {
            return RedirectToAction("Form", "Application", new { @id = id });
        }

        [HttpPost]
        [ActionName("List")]
        [MultiButton(Name = "NewApp", Argument = "id")]
        public ActionResult NewApp()
        {
            return RedirectToAction("StartForm", "Application");
        }

        //[HttpPost]
        //[ActionName("List")]
        //[MultiButton(Name = "Search", Argument = "id")]
        //public ActionResult Search(AppSearchModel model, FormCollection fc)
        //{
        //    int pageIndex = 1;

        //    if (fc["pageIndex"] != null)
        //    {
        //        pageIndex = int.Parse(fc["pageIndex"]);
        //    }

        //    Pagination paging = new Pagination();
        //    paging.PageSize = 10;
        //    paging.CurrentPageIndex = pageIndex;

        //    IGroupByGroupService service = new GroupByGroupService();
        //    var items = service.SearchGroupBy(paging, model.CustomerCityId, model.PortalId, model.GroupByCodeOrName, model.Sale, model.CustomerName);
        //    var ris = new List<ResultItem>();

        //    foreach (var item in items)
        //    {
        //        ResultItem ri = new ResultItem();

        //        ri.CustomerCity = (item.Customer != null && item.Customer.City != null ? item.Customer.City.CityName : "");
        //        ri.CustomerName = item.Customer != null ? item.Customer.CustomerName : "";
        //        ri.GroupByGroupName = item.GroupByGroupName;

        //        ris.Add(ri);
        //    }

        //    model.ResultItems = ris;

        //    // List<ResultItem> list = new List<ResultItem>();
        //    var Pager = new PagedList<ResultItem>(ris, paging.CurrentPageIndex, paging.PageSize, paging.RowCount.Value);
        //    model.Pager = Pager;

        //    return View(model);
        //}

        #endregion

        #region Form


        //[HttpGet]
        //public ActionResult Form(string id, string itemId)
        //{
        //    GroupByFlowInfo model = new GroupByFlowInfo(int.Parse(id), int.Parse(itemId));

        //    var ni = GBGFlowInfo.Instance.GetNodeInfo(model.GroupByGroup.CurrentNode);

        //    model.ParseToModel();

        //    return View(model);
        //}

        [HttpGet]
        public ActionResult Form(string id)
        {
            //GroupByFlowInfo model = new GroupByFlowInfo();
            //model.StartForm = this.TempData["StartFormModel"] as StartFormModel;

            //model.IndustryID = model.StartForm.IndustryID;
            //model.GroupByGroup.SubIndustryID = model.StartForm.SubIndustryID;

            // GBGHelper.SendMail("dingj@hpsidc.com", "test content", "test subject");

            GroupByFlowInfo model = null;

            if (!string.IsNullOrEmpty(id))
            {
                int Id = -1;
                int ItemId = -1;

                if (int.TryParse(id, out Id))
                {
                    model = new GroupByFlowInfo(Id);
                }
                else
                {
                    var ids = id.Split(',');
                    Id = int.Parse(ids[0]);
                    ItemId = int.Parse(ids[1]);

                    model = new GroupByFlowInfo(Id, ItemId);
                }
            }
            else
            {
                model = new GroupByFlowInfo();
                model.StartForm = this.TempData["StartFormModel"] as StartFormModel;
                model.GroupByGroup.SubIndustryID = model.StartForm.SubIndustryID;
                // model.GroupByGroup.SubIndustryID = 4;
            }

            var ni = GBGFlowInfo.Instance.GetNodeInfo(model.CurrentNode);
            TempData["ni"] = ni;

            model.ParseToModel();


            // GBGFlowInfoValidation v = new GBGFlowInfoValidation();


            return View(model);
        }


        [HttpPost]
        [ActionName("Form")]
        [MultiButton(Name = "Submit", Argument = "FLowAction")]
        public ActionResult FormSubmit(GroupByFlowInfo model, FormCollection form, string id, GBGFlowEnumFLowAction FLowAction)
        {
            model.ParseFromModel(form);

            var ni = GBGFlowInfo.Instance.GetNodeInfo(model.CurrentNode);
            ni.ExecAction(FLowAction, model);

            if (string.IsNullOrEmpty(id))
            {
                id = model.GroupByGroup.GroupByGroupID.ToString();
            }

            return RedirectToAction("Form", new { id = id });
        }

        #endregion

        #region JsonResult

        public JsonResult GetUsersByCityId(int cityId)
        {
            IUserService service = new UserService();
            IList<User> list = service.GetUsersByCityId(cityId);

            return Json(new SelectList(list, "UserID", "UserName"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubIndustryNameById(int Id)
        {
            var si = GetSubIndustryById(Id);
            return Json(new { si.SubIndustryID, si.SubIndustryName }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndustryNameBySubId(int subId)
        {
            var i = GetIndustryBySubId(subId);
            return Json(new { i.IndustryID, i.IndustryName }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUploadFile(string Id)
        {
            string Url = "";
            string OriginalFileName = "";

            int id = -1;
            if (int.TryParse(Id, out id))
            {
                IUploadFileService service = new UploadFileService();
                UploadFile uf = service.GetById(id);

                Url = GetUploadFileUrl(uf);
                OriginalFileName = uf.OriginalFileName;
            }

            return Json(new { Id, Url, OriginalFileName }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomer(int customerID)
        {
            ICustomerService customerService = new CustomerService();
            var customer = customerService.GetCustomerByID(customerID);

            object oReturn = new
            {
                customer.CustomerID,
                customer.CustomerName,

                customer.PageDesignContact,
                customer.PageDesignPhone,
                customer.PageDesignEmail,
                customer.PageDesignQQ,
                customer.PageDesignMobile,

                customer.ComplaintHandlingContact,
                customer.ComplaintHandlingPhone,
                customer.ComplaintHandlingEmail,
                customer.ComplaintHandlingQQ,
                customer.ComplaintHandlingMobile,

                customer.FinancialContact,
                customer.FinancialPhone,
                customer.FinancialEmail,
                customer.FinancialQQ,
                customer.FinancialMobile
            };

            return Json(oReturn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckIndustryForSale(int industryID, int UserID)
        {
            IIndustryService service = new IndustryService();
            bool IsOK = service.CheckIndustryForSale(industryID, UserID);

            return Json(IsOK, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCustomer(string CustomerName, int CityID, int IsCertified, int subIndustryId)
        {
            ICustomerService service = new CustomerService();
            List<Customer> customers = service.SearchCustomer(CustomerName, CityID, IsCertified, subIndustryId);

            var list = new ArrayList();
            customers.ForEach(m => list.Add(new
                        {
                            m.CustomerName,
                            m.City.CityName,
                            IsCertified = (m.IsCertified.HasValue ? (m.IsCertified.Value ? "是" : "否") : "否"),
                            m.ContactName,
                            m.ContactPhoneNo,
                            m.CustomerID
                        }
                    )
                );

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult _XSZLTJTZSP_SendEmail(int GroupByID, string cc)
        {
            string strReturn = "发送成功";

            try
            {
                IGroupByGroupService service = new GroupByGroupService();
                GroupByGroup gbg = service.GetById(GroupByID);
                string strEmail = gbg.Customer.ContactEmail;
                if (!string.IsNullOrEmpty(strEmail))
                {
                    GBGHelper.SendMail(strEmail, "Content: 销售助理提交团长审批", "Subject: 销售助理提交团长审批");
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    GBGHelper.SendMail(cc, "Content: 销售助理提交团长审批", "Subject: 销售助理提交团长审批");
                }
            }
            catch
            {
                strReturn = "发送失败";
            }

            return Json(strReturn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _CHB_SendEmail(int GroupByItemID, string cc)
        {
            string strReturn = "发送成功";

            try
            {
                IGroupByItemService service = new GroupByItemService();
                var gbg = service.GetById(GroupByItemID);
                string strEmail = gbg.GroupByGroup.Customer.ContactEmail;
                if (!string.IsNullOrEmpty(strEmail))
                {
                    GBGHelper.SendMail(strEmail, "Content: 策划部", "Subject: 策划部");
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    GBGHelper.SendMail(cc, "Content: 策划部", "Subject: 策划部");
                }
            }
            catch
            {
                strReturn = "发送失败";
            }

            return Json(strReturn, JsonRequestBehavior.AllowGet);
        }


        public JsonResult _XSZLTJTZSP_Expert(int GroupByGroupId)
        {
            ExpertDoc ed = ed = new ExpertExcel2007(@"template\申请表.xlsx", "DSH", "产品申请单", 2, 1);

            ed.CreateDataObject();
            string strFileName = ed.Expert();

            strFileName = ParamCode.Encrypt(strFileName);

            object o = new { IsOK = true, Url = HttpUtility.UrlEncode(strFileName), FileName = strFileName };

            return Json(o, JsonRequestBehavior.AllowGet);
        }


        public FileResult DownloadDoc(string id)
        {
            string strFileName = id;

            strFileName = ParamCode.Decrypt(strFileName);

            var file = strFileName.Split(',');

            return File(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file[0], "xls application/vnd.ms-excel", file[1]);
        }




        #endregion

        #region private method

        private SubIndustry GetSubIndustryById(int Id)
        {
            ISubIndustryService service = new SubIndustryService();
            var si = service.GetByID(Id);
            return si;
        }
        private Industry GetIndustryBySubId(int subId)
        {
            ISubIndustryService service = new SubIndustryService();
            var si = service.GetByID(subId);
            return si.Industry;
        }

        private string GetUploadFileUrl(UploadFile uf)
        {
            string strReturn = string.Concat(uf.FilePath, uf.FileName);
            strReturn = string.Concat(@"~/", strReturn.Replace(@"\", "/").Trim('/'));
            strReturn = this.Url.Content(strReturn);

            return strReturn;
        }

        //// 提交表单
        //private void YWYTXTGSQB_Submit(GroupByFlowInfo model, GBGFlowEnumFLowAction FLowAction)
        //{
        //    ActionInfo ai = GBGFlowInfo.Instance.GetActionInfo(FLowAction);

        //    UpdateBaseField(model, ai, true);

        //    IGroupByGroupService service = new GroupByGroupService();
        //    service.Add(model.GroupByGroup);

        //    InsertAR(model.GroupByGroup.GroupByGroupID, model.GroupByGroup.GroupByGroupName,
        //        null, null,
        //        ai.CurrentNode.ToString(), ai.CurrentNodeName,
        //        ai.Action.ToString(), "", "");
        //}

        //// 提交表单
        //private void BMJLJZJYS_Pass(GroupByFlowInfo model, GBGFlowEnumFLowAction FLowAction)
        //{
        //    DateTime dtNow = DateTime.Now;
        //    ActionInfo ai = GBGFlowInfo.Instance.GetActionInfo(FLowAction);

        //    UpdateBaseField(model, ai);

        //    IGroupByGroupService service = new GroupByGroupService();
        //    var gbg = service.GetById(model.GroupByGroup.GroupByGroupID);
        //    gbg.ApproveBMJLActionTime = dtNow;
        //    gbg.ApproveBMJLMemo = model.GroupByGroup.ApproveBMJLMemo;
        //    gbg.ApproveBMJLResult = ai.Action.ToString();
        //    gbg.ApproveBMJLUserID = model.GroupByGroup.ApproveBMJLUserID;

        //    gbg.CurrentNode = model.GroupByGroup.CurrentNode;
        //    gbg.CurrentStatus = model.GroupByGroup.CurrentStatus;

        //    service.Update(gbg);

        //    InsertAR(model.GroupByGroup.GroupByGroupID, model.GroupByGroup.GroupByGroupName,
        //        null, null,
        //        ai.CurrentNode.ToString(), ai.CurrentNodeName,
        //        ai.Action.ToString(), model.GroupByGroup.ApproveBMJLMemo, "");
        //}



        //private void UpdateBaseField(GroupByFlowInfo model, ActionInfo ai)
        //{
        //    UpdateBaseField(model, ai, false);
        //}
        //private void UpdateBaseField(GroupByFlowInfo model, ActionInfo ai, bool isCreate)
        //{
        //    DateTime dtNow = DateTime.Now;
        //    string strUser = User.Identity.Name;

        //    if (isCreate)
        //    {
        //        model.GroupByGroup.CreateBy = strUser;
        //        model.GroupByGroup.CreateTime = dtNow;

        //        model.GroupByGroup.GroupByItem.ToList().ForEach(m => { m.CreateBy = strUser; m.CreateTime = dtNow; });
        //    }
        //    else
        //    {
        //        model.GroupByGroup.LastModifyBy = strUser;
        //        model.GroupByGroup.LastModifyTime = dtNow;

        //        model.GroupByGroup.GroupByItem.ToList().ForEach(m => { m.LastModifyBy = strUser; m.LastModifyTime = dtNow; });
        //    }

        //    model.GroupByGroup.CurrentNode = ai.NextNode.ToString();
        //    model.GroupByGroup.CurrentStatus = ai.NextStatus.ToString();
        //}

        //private void InsertAR(
        //    int GroupByGroupID, string GroupByGroupName,
        //    int? GroupByPortalID, string PortalName,
        //    string NodeCode, string NodeName,
        //    string ApproveResult, string ApproveMemo, string ApproveMemoMore
        //    )
        //{

        //    DateTime dtNow = DateTime.Now;
        //    string strUser = User.Identity.Name;

        //    ApproveRecordGroupBy ar = new ApproveRecordGroupBy();
        //    ar.CreateBy = strUser;
        //    ar.CreateTime = dtNow;

        //    ar.LastModifyBy = strUser;
        //    ar.LastModifyTime = dtNow;

        //    ar.DeleteInd = false;
        //    ar.HandleUserName = strUser;
        //    ar.HandleTime = dtNow;
        //    // ar.HandleUserID = 

        //    ar.GroupByGroupID = GroupByGroupID;
        //    ar.GroupByGroupName = GroupByGroupName;
        //    ar.GroupByPortalID = GroupByPortalID;
        //    ar.PortalName = PortalName;
        //    ar.NodeCode = NodeCode;
        //    ar.NodeName = NodeName;

        //    ar.ApproveResult = ApproveResult;
        //    ar.ApproveMemo = ApproveMemo;
        //    ar.ApproveMemoMore = ApproveMemoMore;

        //    IApproveRecordGroupByService service = new ApproveRecordGroupByService();

        //    service.Add(ar);
        //}




        #endregion
    }
}
