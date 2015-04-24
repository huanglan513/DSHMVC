using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Service.Interface;
using DSHOrder.Web.Common;
using DSHOrder.Web.Models;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Extensions;
using System.Web.Routing;

namespace DSHOrder.Web.Controllers
{
    public class OfflineReturnController : BaseController
    {
        private CustomerServiceListDataProvider csdProvider;
        private IGroupByItemService gbiService;
        private IGroupByGroupService gbgService;
        private IOrderDetailService odService;
        private ICodeTableService ctService;

        protected override void Initialize(RequestContext requestContext)
        {
            if (csdProvider == null) { csdProvider = CustomerServiceListDataProvider.New(); }
            if (gbiService == null) { gbiService = new GroupByItemService(); }
            if (gbgService == null) { gbgService = new GroupByGroupService(); }
            if (odService == null) { odService = new OrderDetailService(); }
            if (ctService == null) { ctService = new CodeTableService(); }

            base.Initialize(requestContext);
        }

        #region Order

        #region OrderIndex
        public ActionResult Index(OfflineRefundSearchModel model)
        {  
            if (model == null)
            {
                model = new OfflineRefundSearchModel();
            }

            if (string.IsNullOrEmpty(model.OfflineSearchStatus))
            {
                model.OfflineSearchStatus = ((int)OfflineReturnStatus.All).ToString();
            }

            model.OrderStatusTypes = BuildOrderStatusTypes();
            model.RefundStatusTypes = BuildRefundStatusTypes();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = model.PageIndex ?? 1;
            int dealStatus = (int)DSHOrder.Entity.OfflineReturnStatus.All;
            if (!string.IsNullOrEmpty(model.OfflineSearchStatus))
            {
                dealStatus = Convert.ToInt32(model.OfflineSearchStatus);
            }

            IList<OrderDetail> orderDetailList = odService.GetByOfflineContidion(paging, model.GroupByName, null, model.Oid, model.TotalFee, model.BuyerNick, model.Address, model.DealDateFrom, model.DealDateTo, dealStatus);
            PagedList<OrderDetail> pagedOrderDetailList = new PagedList<OrderDetail>(orderDetailList, paging.CurrentPageIndex, paging.PageSize, paging.RowCount.HasValue ? paging.RowCount.Value : 0);
            model.OrderDetailList = pagedOrderDetailList;

            model.OfflineDealStatusTypes = GetOfflineDealStatusTypes();

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "edit", Argument = "id")]
        public ActionResult EditOfOrderIndex(int id)
        {
            return RedirectToAction("Edit", "OfflineReturn", new { @id = id });
        }

        private IDictionary<int, string> GetOfflineDealStatusTypes()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add((int)OfflineReturnStatus.All, OfflineReturnStatus.All.ToString());
            dict.Add((int)OfflineReturnStatus.Done, OfflineReturnStatus.Done.ToString());
            dict.Add((int)OfflineReturnStatus.Pending, OfflineReturnStatus.Pending.ToString());

            return dict;
        }


        
        #endregion

        #region OrderDetail of Editing
        public ActionResult Edit(int id)
        {
            OfflineReturnDetailModel model = new OfflineReturnDetailModel();
            OrderDetail orderDetail = odService.GetById(id);

            if (orderDetail == null) { return View(model); }
            model.GroupByItemID = orderDetail.GroupByItemID.Value;
            model.OrderDetailID = orderDetail.OrderDetailID;
            model.Address = orderDetail.Address;
            model.AlipayNo = orderDetail.AlipayNo;
            model.ApplyRefundTime = orderDetail.ApplyRefundTime;
            model.BuyerMessage = orderDetail.BuyerMessage;
            model.BuyerNick = orderDetail.BuyerNick;
            model.DealTime = orderDetail.DealTime;
            model.Desc = orderDetail.Desc;
            model.GoodReturnTime = orderDetail.GoodReturnTime;
            model.GroupByName = orderDetail.ItemTitle;
            if (string.IsNullOrEmpty(orderDetail.HasGoodReturn))
            {
                model.HasGoodReturn = false;
            }
            else
            {
                model.HasGoodReturn = Convert.ToBoolean(orderDetail.HasGoodReturn);
            }

            //TODO to do IsDealed
            model.IsDealed = orderDetail.HasCSDeal.HasValue && orderDetail.HasCSDeal.Value == 1 ? true : false ;
            model.IsOfflineRefund = orderDetail.OfflineDealStatus.HasValue;
            model.IssueType = orderDetail.IssueType.ToString();
            model.Oid = orderDetail.Oid;
            model.RefundID = orderDetail.RefundID;
            model.RefundStatus = ConvertRefundStsToString(orderDetail.RefundStatus);
            model.OrderStatus = ConvertOrderStsToString(orderDetail.OrderStatus);
            model.Payment = orderDetail.Payment;
            model.Reason = orderDetail.Reason;
            model.RefundFee = orderDetail.RefundFee;
            model.Remark = orderDetail.Remark;
            model.TotalFee = orderDetail.TotalFee;
            model.TradeRate = orderDetail.TradeRate.ToString();
            model.UpdateRefundTime = orderDetail.UpdateRefundTime;
            model.OfflineDealTime = orderDetail.OfflineDealDate;
            model.OfflineRefundFee = orderDetail.OfflineReturnFee;
            model.OfflineRefundStatus = orderDetail.OfflineDealStatus.ToString();
            model.CredentialPath = orderDetail.CredentialPath;

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [MultiButton(Name = "Back", Argument = "id")]
        public ActionResult BackOfOrderDetailEdit(int id)
        {
            return RedirectToAction("Index", "OfflineReturn");
        }

        [HttpPost]
        [ActionName("Edit")]
        [MultiButton(Name = "Save", Argument = "id")]
        public ActionResult BackOfOrderDetailEdit(int id, OfflineReturnDetailModel model)
        {
            OrderDetail orderDetail = odService.GetById(id);

            orderDetail.OfflineDealBy = User.Identity.Name;
            orderDetail.OfflineDealDate = model.OfflineDealTime;
            orderDetail.OfflineReturnFee = model.OfflineRefundFee;
            orderDetail.CredentialPath = model.CredentialPath;

            if (!string.IsNullOrEmpty(model.OfflineRefundStatus))
            {
                orderDetail.OfflineDealStatus = Convert.ToInt32(model.OfflineRefundStatus);
            }
            else
            {
                orderDetail.OfflineDealStatus = null;
            }
           
            odService.Update(orderDetail);
            return RedirectToAction("Edit", "OfflineReturn", new { @id = id });
        }

        #endregion
       
        private string ConvertOrderStsToString(int? status)
        {
            string rst = string.Empty;
            if (!status.HasValue) return rst;

            IList<CodeTable> codeList = ctService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.OrderType });
            CodeTable ct = codeList.FirstOrDefault<CodeTable>(p => p.CodeValue == status.ToString());
            if (ct != null)
            {
                rst = ct.CodeDesc;
            }
            return rst;
        }

        private string ConvertRefundStsToString(int? status)
        {
            string rst = string.Empty;
            if (!status.HasValue) return rst;

            IList<CodeTable> codeList = ctService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.RefundStatus });
            CodeTable ct = codeList.FirstOrDefault<CodeTable>(p => p.CodeValue == status.ToString());
            if (ct != null)
            {
                rst = ct.CodeDesc;
            }
            return rst;
        }


        private IDictionary<int, string> BuildOrderStatusTypes()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            IList<CodeTable> codeList = ctService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.OrderType });
            foreach (CodeTable ct in codeList)
            {
                int id = Convert.ToInt32(ct.CodeValue);
                dict.Add(id, ct.CodeDesc);
            }
            return dict;
        }

        private IDictionary<int, string> BuildRefundStatusTypes()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            IList<CodeTable> codeList = ctService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.RefundStatus });
            foreach (CodeTable ct in codeList)
            {
                int id = Convert.ToInt32(ct.CodeValue);
                dict.Add(id, ct.CodeDesc);
            }

            return dict;
        }
        #endregion

        #region List Data Provider
        private class CustomerServiceListDataProvider : IListProvider
        { 
            private ICodeTableService codeService;

            public static CustomerServiceListDataProvider New()
            {
                CustomerServiceListDataProvider provider = new CustomerServiceListDataProvider();
                DSHOrder.Web.Extensions.ListProviders.SetListProvider(delegate { return provider; });

                return provider;
            }

            private CustomerServiceListDataProvider()
            { 
                codeService = new CodeTableService();
            }

            public IEnumerable<Extensions.ListItem> GetListItems(string listName)
            {
                IEnumerable<ListItem> items = new List<ListItem>();
                List<ListItem> temp;
                switch (listName)
                { 
                    case "IssueType":
                        items = codeService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.IssueType }).Select<CodeTable, ListItem>(o => new ListItem { Text = o.CodeDesc, Value = o.CodeValue });
                        temp = items.ToList<ListItem>();
                        temp.Insert(0, new ListItem { Text = "--请选择--", Value = string.Empty });
                        items = temp;
                        break;
                    case "OfflineReturnStatus":
                        items = new List<ListItem> { new ListItem{Text=OfflineReturnStatus.Pending.ToString(), Value=((int)OfflineReturnStatus.Pending).ToString()},
                                                     new ListItem{Text=OfflineReturnStatus.Done.ToString(), Value=((int)OfflineReturnStatus.Done).ToString()}};
                        break;
                    default:
                        break;
                }

                return items;
            }
        }
        #endregion

        //private IGroupByItemService gbiService { get; set; }
        //private IOrderDetailService odService { get; set; }
        //private ICodeTableService ctService { get; set; }

        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    gbiService = new GroupByItemService();
        //    odService = new OrderDetailService();
        //    ctService = new CodeTableService();
        //    base.Initialize(requestContext);
        //}

        ////
        //// GET: /OfflineReturn/

        //public ActionResult Index(int pageIndex = 1, string GroupByNumber = null, string TaobaoOrderID= null, string GroupByName =null, decimal? Price=null,  string BuyerName = null, string Addr = null, DateTime? DealDateFrom = null, DateTime? DealDateTo = null, int dealType = 0)
        //{
        //    //if (ValidateRole(null) == false) return AccessDenyView();

        //    OrderDetailSearchModel model = new OrderDetailSearchModel();

        //    model.GroupByName = GroupByName;
        //    model.GroupByNumber = GroupByNumber;
        //    model.TaobaoOrderID = TaobaoOrderID;
        //    model.Addr = Addr;
        //    model.BuyerName = BuyerName;
        //    model.Price = Price;
        //    model.DealDateFrom = DealDateFrom;
        //    model.DealDateTo = DealDateTo;
        //    model.StatusType = dealType;

        //    Pagination paging = new Pagination();
        //    paging.PageSize = 10;
        //    paging.CurrentPageIndex = pageIndex; //.HasValue ? pageIndex.Value : 1;

        //    IList<OrderDetail> orderDetailList = odService.GetByOfflineContidion(paging, GroupByName, GroupByNumber, TaobaoOrderID, Price, BuyerName, Addr, DealDateFrom, DealDateTo, dealType);
        //    PagedList<OrderDetail> pagedOrderDetailList = new PagedList<OrderDetail>(orderDetailList, paging.CurrentPageIndex, paging.PageSize, paging.PageCount);
        //    model.OrderDetailList = pagedOrderDetailList;

        //    return View(model);   
        //}

        //[HttpPost]
        //[ActionName("Index")]
        //[MultiButton(Name = "edit", Argument = "id")]
        //public ActionResult IndexForEdit(int id)
        //{
        //    return RedirectToAction("Edit", "OfflineReturn", new { @id = id });
        //}

        //public ActionResult Edit(int id)
        //{
        //    OrderDetail orderDetail = GetOrderValues(id);
        //    return View(orderDetail);
        //}

        //[HttpPost]
        //public ActionResult Edit(OrderDetail model, FormCollection collection)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IOrderDetailService orderService = new OrderDetailService();
        //        OrderDetail orderDetail = orderService.GetById(model.OrderDetailID);
        //        //orderDetail.RefundStatus = model.RefundStatus;
        //        //orderDetail.DealTime = model.DealTime;
        //        //orderDetail.DealBy = User.Identity.Name;
        //        //orderDetail.RefundFee = model.RefundFee;
        //        //orderDetail.Remark = model.Remark;
        //        orderDetail.OfflineDealStatus = model.OfflineDealStatus;
        //        orderDetail.OfflineReturnFee = model.OfflineReturnFee;
        //        orderDetail.LastModifyBy = User.Identity.Name;
        //        orderDetail.LastModifyTime = DateTime.Now;
        //        orderDetail.OfflineDealBy = User.Identity.Name;
        //        orderDetail.OfflineDealDate = model.OfflineDealDate;
        //        //orderDetail.LockBy = null;
        //        //orderDetail.LockTime = null;

        //        orderService.Update(orderDetail);

        //        return RedirectToAction("Index", "OfflineReturn");
        //    }
        //    else
        //    {
        //        OrderDetail orderDetail = GetOrderValues(model.OrderDetailID);
        //        return View(orderDetail);
        //    }
        //}

        //public ActionResult AccessDenyView()
        //{
        //    return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
        //}

        //private OrderDetail GetOrderValues(int id)
        //{
        //    int offlineReturnType = 8;
        //    IOrderDetailService orderService = new OrderDetailService();
        //    OrderDetail orderDetail = orderService.GetById(id);

        //    GroupByGroup groupByGroup = orderService.GetGroupByGroup(id);
        //    ViewData["GroupByName"] = groupByGroup.GroupByGroupName;
        //    ViewData["GroupByGroupID"] = groupByGroup.GroupByGroupID;

        //    ICodeTableService codeTableService = new CodeTableService();
        //    IList<CodeTable> offlineReturnStatusTypeList = codeTableService.GetCodeTablesByType(offlineReturnType);

        //    ViewData["OfflineReturnStatusType"] = new SelectList(offlineReturnStatusTypeList, "CodeValue", "CodeDesc");

        //    return orderDetail;
        //}

        //[HttpPost]
        //public ActionResult Back()
        //{
        //    return RedirectToAction("Index", "OfflineReturn");
        //}
    }
}