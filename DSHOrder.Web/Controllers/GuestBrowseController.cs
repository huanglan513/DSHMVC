﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Web.Models;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Web.Extensions;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Common;
using DSHOrder.Web.Common;
using System.Web.Routing;


namespace DSHOrder.Web.Controllers
{
    public class GuestBrowseController : Controller
    {
        private GuestBrowseListDataProvider csdProvider;
        private IGroupByItemService gbiService;
        private IGroupByGroupService gbgService;
        private IOrderDetailService odService;
        private ICodeTableService ctService;
        private IUserService usrService;

        protected override void Initialize(RequestContext requestContext)
        {
            if (csdProvider == null) { csdProvider = GuestBrowseListDataProvider.New(); }
            if (gbiService == null) { gbiService = new GroupByItemService(); }
            if (gbgService == null) { gbgService = new GroupByGroupService(); }
            if (odService == null) { odService = new OrderDetailService(); }
            if (ctService == null) { ctService = new CodeTableService(); }
            if (usrService == null) { usrService = new UserService(); }

            base.Initialize(requestContext);
        }

        #region Groupon
        public ActionResult GrouponIndex(GrouponSearchModel model)
        {
            if (model == null)
            {
                model = new GrouponSearchModel();
            }
               
            User guest = usrService.GetUserByName(User.Identity.Name);
            Customer c = guest.Customer;
            if (c != null)
            {
                model.CustomerName = c.CustomerName;
            }
        
            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = model.PageIndex ?? 1;
            int? citySelected = null;
            int? portalSelected = null;
            int? grouponStatus = null;
            if (!string.IsNullOrEmpty(model.CitySelected))
            {
                citySelected = Convert.ToInt32(model.CitySelected);
            }
            if (!string.IsNullOrEmpty(model.PortalSelected))
            {
                portalSelected = Convert.ToInt32(model.PortalSelected);
            }
            if (!string.IsNullOrEmpty(model.GroupByStatus))
            {
                grouponStatus = Convert.ToInt32(model.GroupByStatus);
            }

            List<GroupByItem> itemList = gbiService.GetByCondition(paging, citySelected, portalSelected, model.GroupByName, model.GroupByName, model.GroupBySale, model.CustomerName, grouponStatus);
            model.GroupByItemList = new PagedList<GroupByItem>(itemList, paging.CurrentPageIndex, paging.PageSize, paging.RowCount.HasValue ? paging.RowCount.Value : 0);

            return View(model);
        }

        [HttpPost]
        [ActionName("GrouponIndex")]
        [MultiButton(Name = "order", Argument = "id")]
        public ActionResult OrderOfGrouponIndex(int id)
        {
            return RedirectToAction("OrderIndex", "GuestBrowse", new { @id = id });
        }
        #endregion

        #region Order

        #region OrderIndex
        public ActionResult OrderIndex(int? id, OrderSearchModel model)
        {
            if (!id.HasValue)
            {
                id = model.GroupByItemId;
            }
            GroupByItem gbi = gbiService.GetById(id.Value);
            if (model == null)
            {
                model = new OrderSearchModel();
            }

            model.OrderStatusTypes = BuildOrderStatusTypes();
            model.RefundStatusTypes = BuildRefundStatusTypes();
            if (gbi == null) return View(model);

            if (model.OrderDealStatus == null)
            {
                model.OrderDealStatus = ((int)OrderDetailSearchType.All).ToString();
            }
            model.GroupByItemId = id.Value;
            model.GroupByName = gbi.GroupByGroup.GroupByGroupName;
            model.OnlineDate = gbi.StartDay;
            IEnumerable<GroupBySales> sales = gbi.GroupByGroup.GroupBySales;
            string strSales = string.Empty;
            foreach (var sale in sales)
            {
                strSales += sale.User.UserName + " ";
            }
            model.GroupBySale = strSales;

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = model.PageIndex ?? 1;
            int dealStatus = (int)DSHOrder.Entity.OrderDetailSearchType.Pending;
            if (!string.IsNullOrEmpty(model.OrderDealStatus))
            {
                dealStatus = Convert.ToInt32(model.OrderDealStatus);
            }

            IList<OrderDetail> orderDetailList = odService.GetByContidion(paging, id.Value, model.BuyerNick, model.Address, model.OnlineDate, model.DealDateFrom, model.DealDateTo, dealStatus, model.TradeRate);
            PagedList<OrderDetail> pagedOrderDetailList = new PagedList<OrderDetail>(orderDetailList, paging.CurrentPageIndex, paging.PageSize, paging.RowCount.HasValue ? paging.RowCount.Value : 0);
            model.OrderDetailList = pagedOrderDetailList;

            return View(model);
        }

        [HttpPost]
        [ActionName("OrderIndex")]
        [MultiButton(Name = "view", Argument = "id")]
        public ActionResult ViewOfOrderIndex(int id)
        {
            return RedirectToAction("OrderDetailView", "GuestBrowse", new { @id = id });
        }

        [HttpPost]
        [ActionName("OrderIndex")]
        [MultiButton(Name = "Back")]
        public ActionResult BackOfOrderIndex()
        {
            return RedirectToAction("GrouponIndex", "GuestBrowse");
        }
        #endregion

        #region OrderDetail of View
        public ActionResult OrderDetailView(int id)
        {
            OrderDetailModel model = new OrderDetailModel();
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
            model.IsDealed = orderDetail.HasCSDeal.HasValue && orderDetail.HasCSDeal.Value == 1 ? true : false;
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

            return View(model);
        }

        [HttpPost]
        [ActionName("OrderDetailView")]
        [MultiButton(Name = "Back", Argument = "id")]
        public ActionResult BackOfOrderDetailView(int id)
        {
            return RedirectToAction("OrderIndex", "GuestBrowse", new { @id = id });
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
        private class GuestBrowseListDataProvider : IListProvider
        {
            private ICityService cityService;
            private IGroupByPortalService gbpService;
            private ICodeTableService codeService;

            public static GuestBrowseListDataProvider New()
            {
                GuestBrowseListDataProvider provider = new GuestBrowseListDataProvider();
                DSHOrder.Web.Extensions.ListProviders.SetListProvider(delegate { return provider; });

                return provider;
            }

            private GuestBrowseListDataProvider()
            {
                cityService = new CityService();
                gbpService = new GroupByPortalService();
                codeService = new CodeTableService();
            }

            public IEnumerable<Extensions.ListItem> GetListItems(string listName)
            {
                IEnumerable<ListItem> items = new List<ListItem>();
                List<ListItem> temp;
                switch (listName)
                {
                    case "City":
                        items = cityService.GetAllCities().Select<City, ListItem>(o => new ListItem { Text = o.CityName, Value = o.CityID.ToString() });
                        temp = items.ToList<ListItem>();
                        temp.Insert(0, new ListItem { Text = "--请选择--", Value = string.Empty });
                        items = temp;
                        break;
                    case "Portal":
                        items = gbpService.GetAllPortals().Select<GroupByPortal, ListItem>(o => new ListItem { Text = o.PortalName, Value = o.GroupByPortalID.ToString() });
                        temp = items.ToList<ListItem>();
                        temp.Insert(0, new ListItem { Text = "--所有平台--", Value = string.Empty });
                        items = temp;
                        break;
                    case "TradeRate":
                        items = new List<ListItem>{
                            new ListItem{Text="--请选择---", Value=string.Empty},
                            new ListItem{Text="好评", Value="1"},
                            new ListItem{Text="中评", Value="0"},
                            new ListItem{Text="差评", Value="-1"}
                        };
                        break;
                    case "IssueType":
                        items = codeService.GetCodeTablesByTypes(new int[] { (int)ComUtil.CodeType.IssueType }).Select<CodeTable, ListItem>(o => new ListItem { Text = o.CodeDesc, Value = o.CodeValue });
                        temp = items.ToList<ListItem>();
                        temp.Insert(0, new ListItem { Text = "--请选择--", Value = string.Empty });
                        items = temp;
                        break;
                    default:
                        break;
                }

                return items;
            }
        }
        #endregion

    }
}
