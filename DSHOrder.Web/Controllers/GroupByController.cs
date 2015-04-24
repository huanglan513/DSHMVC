using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;
using DSHOrder.Web.Models;
using System.Data;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Common;
using System.Text;

namespace DSHOrder.Web.Controllers
{
    public class GroupByController : BaseController
    {
        // GET: /GroupBy/
        public ActionResult Index(int? pageIndex, int? CitySelected, int? PortalSelected, string GroupByName, string GroupBySale, string CustomerName, int? FilterType)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            GroupByItemSearchModel model = new GroupByItemSearchModel();
            ICityService cityService = new CityService();
            IGroupByPortalService gbpService = new GroupByPortalService();
            IGroupByGroupService gbgService = new GroupByGroupService();
            IGroupByItemService gbiService = new GroupByItemService();

            model.CityList = cityService.GetAllCities().Select<City, SelectListItem>(o => new SelectListItem { Text = o.CityName, Value = o.CityID.ToString() });
            model.PortalList = gbpService.GetAllPortals().Select<GroupByPortal, SelectListItem>(o => new SelectListItem { Text = o.PortalName, Value = o.GroupByPortalID.ToString() });
            model.GroupByStatus = FilterType;
            model.CitySelected = CitySelected;
            model.PortalSelected = PortalSelected;
            model.GroupByName = GroupByName;
            model.GroupBySale = GroupBySale;
            model.CustomerName = CustomerName;

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = pageIndex ?? 1;
            List<GroupByItem> itemList = gbiService.GetByCondition(paging, model.CitySelected, model.PortalSelected, model.GroupByNumber, model.GroupByName, model.GroupBySale, model.CustomerName, model.GroupByStatus);

            model.GroupByItemList = new PagedList<GroupByItem>(itemList, pageIndex ?? 1, 10, paging.RowCount.HasValue ? paging.RowCount.Value : 0);

            return View(model);
        }

        public ActionResult AccessDenyView()
        {
            return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
        }

        #region Index actions : new, edit, pay, order or refund

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "new")]
        public ActionResult IndexForNew()
        {
            return RedirectToAction("Create", "GroupBy");
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "edit", Argument = "id")]
        public ActionResult IndexForEdit(int id)
        {
            return RedirectToAction("Create", "GroupBy", new { @id = id });
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "pay", Argument = "id")]
        public ActionResult IndexForPay(int id)
        {
            return RedirectToAction("Payment", "GroupBy", new { @id = id });
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "order", Argument = "id")]
        public ActionResult IndexForOrder(int id)
        {
            return RedirectToAction("Index", "OrderDetail", new { @id = id });
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "refund", Argument = "id")]
        public ActionResult IndexForRefund(int id)
        {

            IGroupByGroupService gbgService = new GroupByGroupService();
            GroupByGroup gbg = gbgService.GetById(id);
            //Set the refund status to stop refund.
            gbg.Status = (int)GroupByGroupStatus.Returned;
            gbgService.Update(gbg);
            return RedirectToAction("Index", "GroupBy", new { @id = id });
        }

        #endregion

        public ActionResult OrderDetailFilter(int type, FormCollection collection)
        {
            return View();
        }

        public ActionResult OrderDetail(int itemId, int? id, OrderDetailSearchModel searchModel)
        {
            //searchModel.Id = itemId;
            IGroupByItemService groupByItemService = new GroupByItemService();
            GroupByItem item = groupByItemService.GetById(itemId);
            searchModel.GroupByName = item.GroupByGroup.GroupByGroupName;            
            //searchModel.Price = item.SellingPrice.HasValue ? item.SellingPrice.Value.ToString("N2") : "0";

            StringBuilder sb = new StringBuilder();
            foreach (GroupBySales sales in item.GroupByGroup.GroupBySales)
            {
                sb.Append(sales.User.UserName);
                sb.Append(",");
            }
            searchModel.Sellers = sb.ToString().Length > 0 ? sb.ToString().Remove(sb.ToString().Length - 1) : "";

            ICodeTableService codeTableService = new CodeTableService();
            IList<CodeTable> ctList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.RefundType);
            ViewData["CodeTableList"] = ctList;

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id.HasValue ? id.Value : 1;

            IOrderDetailService service = new OrderDetailService();
            IList<OrderDetail> orderDetailList = service.GetByContidion(paging, itemId, searchModel.BuyerName, searchModel.Addr, searchModel.OnlineDate, searchModel.DealDateFrom, searchModel.DealDateTo, searchModel.StatusType, null);
            PagedList<OrderDetail> pagedOrderDetailList = new PagedList<OrderDetail>(orderDetailList, paging.CurrentPageIndex, paging.PageSize, paging.PageCount);
            searchModel.OrderDetailList = pagedOrderDetailList;

            return View(searchModel);
        }

        //[HttpPost]
        //public ActionResult OrderDetail(OrderDetailSearchModel model,FormCollection collection)
        //{
        //    ViewData["Index"] = collection["hdIndex"];
        //    Pagination paging = new Pagination();
        //    paging.PageSize = 10;
        //    paging.CurrentPageIndex = 1;

        //    IOrderDetailService service = new OrderDetailService();
        //    IList<OrderDetail> orderDetailList = service.GetByContidion(paging, model.Id, model.BuyerName, model.Addr, model.DealDateFrom, model.DealDateTo, int.Parse(ViewData["Index"].ToString()));
        //    PagedList<OrderDetail> pagedOrderDetailList = new PagedList<OrderDetail>(orderDetailList, paging.CurrentPageIndex, paging.PageSize, paging.PageCount);
        //    model.OrderDetailList = pagedOrderDetailList;

        //    return View(model);

        //}

        
        // GET: /GroupBy/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(int? id)
        {
            GroupByAllInfo allInfo = new GroupByAllInfo();

            IList<GroupByPortal> portalList = InitPortal();

            IList<GroupByItem> groupByItemList = null;

            if (id.HasValue)
            {
                groupByItemList = GetGroupDataByID(id, allInfo);
            }

            IList<GroupByItem> groupByItemAlllist = new List<GroupByItem>();
            int j = 0;
            for (int i = 0; i < portalList.Count; i++)
            {
                GroupByPortal portal = portalList[i];
                if (groupByItemList != null && groupByItemList.Count > 0 && j < groupByItemList.Count && groupByItemList[j].GroupByPortalID == portal.GroupByPortalID)
                {
                    groupByItemAlllist.Add(groupByItemList[j]);
                    j++;
                }
                else
                {
                    groupByItemAlllist.Add(new GroupByItem());
                }
            }
            allInfo.GroupByItemList = groupByItemAlllist;

            InitGroupData(allInfo);

            return View(allInfo);

        }

        #region for Create action

        [HttpPost]
        public ActionResult Create(GroupByAllInfo model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.GroupByGroupEntity.GroupByGroupID != 0)
                    {
                        UpdateGroupByAllInfo(model, collection);
                    }
                    else
                    {
                        CreateGroupByAllInfo(model, collection);
                    }

                    return RedirectToAction("Index");
                }
                InitPortal();
                InitGroupData(model);

                InitOther(model, collection);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Program Error", ex);

                InitPortal();
                InitGroupData(model);
                InitOther(model, collection);
                return View(model);
            }
        }

        private static void InitOther(GroupByAllInfo model, FormCollection collection)
        {
            string[] users = collection["cbxUser"].Split(',');
            if (users != null && users.Length > 0)
            {
                List<int> userList = new List<int>();
                foreach (string userId in users)
                {
                    userList.Add(int.Parse(userId));
                }
                model.Sellers = userList;
            }

            //string[] keys = collection.AllKeys.Where(p => p.ToString().Contains("cbxCity")).ToArray();

            //int startIdx = "cbxCity".Length;
            //foreach (string key in keys)
            //{
            //    int index = int.Parse(key.Substring(startIdx));
            //    // model.GroupByItemList[index].
            //}

        }

        private IList<GroupByPortal> InitPortal()
        {
            IGroupByPortalService service = new GroupByPortalService();
            IList<GroupByPortal> portalList = service.GetAllPortals();
            portalList = portalList.OrderBy(p => p.GroupByPortalID).ToList();
            ViewData["portal"] = portalList;
            return portalList;
        }

        private void InitGroupData(GroupByAllInfo allInfo)
        {
            InitIndustry(allInfo);

            InitCustomer(allInfo);

            InitUser(allInfo);

            InitPaymentType();
        }

        private void InitPaymentType()
        {
            ICodeTableService codeTableService = new CodeTableService();
            IList<CodeTable> refundTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.RefundType);
            ViewData["refundTypeSelectList"] = new SelectList(refundTypeList, "CodeValue", "CodeDesc");

            IList<CodeTable> paymentTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.PaymentType);

            ViewData["paymentTypeSelectList"] = new SelectList(paymentTypeList, "CodeValue", "CodeDesc");
        }

        private void InitUser(GroupByAllInfo allInfo)
        {
            IUserService userService = new UserService();
            ViewData["userList"] = userService.GetAllUsers();
        }

        private void InitCustomer(GroupByAllInfo allInfo)
        {
            ICityService cityService = new CityService();
            IList<City> cityList = cityService.GetAllCities();
            ViewData["city"] = cityList;

            List<SelectListItem> selectCityList = new List<SelectListItem>();
            //  selectCityList.Add(new SelectListItem { Text = "城市", Value = "0" });
            foreach (City item in cityList)
            {
                selectCityList.Add(new SelectListItem { Text = item.CityName, Value = item.CityID.ToString() });
            }
            ViewData["citySelectList"] = new SelectList(cityList, "CityID", "CityName");

            List<SelectListItem> selectDistrictList = new List<SelectListItem>();

            if (allInfo.GroupByGroupEntity != null && allInfo.GroupByGroupEntity.CustomerID.HasValue)
            {
                selectDistrictList = GetDistrictList(allInfo.CityID.Value);
            }
            else
            {
                selectDistrictList.Add(new SelectListItem { Text = "地区", Value = "0" });
                foreach (District item in cityList[0].District)
                {
                    selectDistrictList.Add(new SelectListItem { Text = item.DistrictName, Value = item.DistrictID.ToString() });
                }
            }
            ViewData["districtSelectList"] = selectDistrictList; //new SelectList(cityList[0].District, "DistrictID", "DistrictName");
            IList<Customer> customerList = null;
            if (allInfo.GroupByGroupEntity != null && allInfo.GroupByGroupEntity.CustomerID.HasValue)
            {
                customerList = GetCustomerList(allInfo.CityID.Value, allInfo.DistrictID.Value);

                List<SelectListItem> customerSelectList = new List<SelectListItem>();
                foreach (Customer customer in customerList)
                {
                    if (customer.CustomerID == allInfo.GroupByGroupEntity.CustomerID)
                    {
                        customerSelectList.Add(new SelectListItem() { Text = customer.CustomerName, Value = customer.CustomerID.ToString(), Selected = true });
                    }
                    else
                    {
                        customerSelectList.Add(new SelectListItem() { Text = customer.CustomerName, Value = customer.CustomerID.ToString() });
                    }
                }
                ViewData["customerSelectList"] = customerSelectList;
            }
            else
            {
                customerList = cityList[0].Customer.ToList();
                ViewData["customerSelectList"] = new SelectList(customerList, "CustomerID", "CustomerName");
            }
        }

        private void InitIndustry(GroupByAllInfo allInfo)
        {
            IIndustryService industryService = new IndustryService();
            IList<Industry> industryList = industryService.GetAllIndustries();
            ViewData["industrySelectList"] = new SelectList(industryList, "IndustryID", "IndustryName");

            if (allInfo.GroupByGroupEntity != null && allInfo.GroupByGroupEntity.SubIndustryID != null)
            {
                IList<SubIndustry> subIndustryList = GetSubIndustryListBySubIndustryId(allInfo.GroupByGroupEntity.SubIndustryID.Value);
                if (subIndustryList.Count > 0)
                {
                    allInfo.IndustryID = subIndustryList[0].IndustryID;
                }
                ViewData["subIndustrySelectList"] = new SelectList(subIndustryList, "SubIndustryID", "SubIndustryName");
            }
            else if (industryList.Count > 0)
            {
                ViewData["subIndustrySelectList"] = new SelectList(industryList[0].SubIndustry, "SubIndustryID", "SubIndustryName");
            }
        }

        private IList<GroupByItem> GetGroupDataByID(int? id, GroupByAllInfo allInfo)
        {
            allInfo.KeyId = id.Value;

            IGroupByGroupService groupByGroupService = new GroupByGroupService();
            GroupByGroup groupByGroup = groupByGroupService.GetById(id.Value);
            if (groupByGroup != null)
            {
                allInfo.CityID = groupByGroup.Customer != null ? groupByGroup.Customer.CityID : null;
                allInfo.DistrictID = groupByGroup.Customer != null ? groupByGroup.Customer.DistrictID : null;

                IList<Payment> paymentList = null;// groupByGroup.Payment.Where(p => p.DeleteInd == 0).ToList();
                for (int i = 0; i < paymentList.Count; i++)
                {
                    switch (i + 1)
                    {
                        case 1:
                            allInfo.FirstPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 2:
                            allInfo.SecondPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 3:
                            allInfo.ThirdPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 4:
                            allInfo.ForthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 5:
                            allInfo.FifthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 6:
                            allInfo.SixthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 7:
                            allInfo.SeventhPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 8:
                            allInfo.EighthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 9:
                            allInfo.NinthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                        case 10:
                            allInfo.TenthPaymentDate = paymentList[i].PaymentDeadline;
                            break;
                    }
                }

                List<int> sellers = new List<int>();
                foreach (GroupBySales sales in groupByGroup.GroupBySales)
                {
                    sellers.Add(sales.UserID.Value);
                }
                allInfo.Sellers = sellers;
                allInfo.GroupByGroupEntity = groupByGroup;

                return groupByGroup.GroupByItem.ToList();
            }
            else
            {
                return null;
            }
        }

        private void CreateGroupByAllInfo(GroupByAllInfo model, FormCollection collection)
        {
            string[] portals = collection["cbxPortal"].Split(',');
            string[] users = collection["cbxUser"].Split(',');

            GroupByGroup groupByGroup = model.GroupByGroupEntity;
            groupByGroup.CreateBy = this.User.Identity.Name;
            groupByGroup.CreateTime = DateTime.Now;
            groupByGroup.Status = 0;


            foreach (string userID in users)
            {
                GroupBySales sales = new GroupBySales();
                sales.GroupByGroupID = groupByGroup.GroupByGroupID;
                sales.UserID = int.Parse(userID);
                groupByGroup.GroupBySales.Add(sales);
            }

            CreatePaymentList(model, groupByGroup);

            foreach (string portal in portals)
            {
                int portalID = int.Parse(portal);
                GroupByItem groupByItem = model.GroupByItemList.FirstOrDefault(p => p.GroupByPortalID == portalID);
                if (groupByItem == null)
                {
                    continue;
                }
                string[] citys = collection["cbxCity" + portal].Split(',');
                foreach (string cityID in citys)
                {
                    GroupByCity groupByCity = new GroupByCity();
                    groupByCity.GroupByItemID = groupByItem.GroupByItemID;
                    groupByCity.CityID = int.Parse(cityID);
                    groupByItem.GroupByCity.Add(groupByCity);
                }

                groupByItem.CreateBy = this.User.Identity.Name;
                groupByItem.CreateTime = DateTime.Now;
                groupByGroup.GroupByItem.Add(groupByItem);
            }

            IGroupByGroupService groupByGroupService = new GroupByGroupService();
            groupByGroupService.Add(groupByGroup);

        }

        private void CreatePaymentList(GroupByAllInfo model, GroupByGroup groupByGroup)
        {
            CreatePaymentEntity(model.FirstPaymentDate, groupByGroup, 1);
            CreatePaymentEntity(model.SecondPaymentDate, groupByGroup, 2);
            int paymentCount = 2;
            if (model.ThirdPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.ThirdPaymentDate, groupByGroup, paymentCount);
            }
            if (model.ForthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.ForthPaymentDate, groupByGroup, paymentCount);
            }
            if (model.FifthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.FifthPaymentDate, groupByGroup, paymentCount);
            }
            if (model.SixthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.SixthPaymentDate, groupByGroup, paymentCount);
            }
            if (model.SeventhPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.SeventhPaymentDate, groupByGroup, paymentCount);
            }
            if (model.EighthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.EighthPaymentDate, groupByGroup, paymentCount);
            }
            if (model.NinthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.NinthPaymentDate, groupByGroup, paymentCount);
            }
            if (model.TenthPaymentDate.HasValue)
            {
                paymentCount++;
                CreatePaymentEntity(model.TenthPaymentDate, groupByGroup, paymentCount);
            }
        }

        private void CreatePaymentEntity(DateTime? paymentDate, GroupByGroup groupByGroup, int paymentCount)
        {
            Payment payment = new Payment();
          //  payment.GroupByGroupID = groupByGroup.GroupByGroupID;
            payment.PaymentType = paymentCount;
            payment.PaymentDeadline = paymentDate;
            payment.CreateBy = this.User.Identity.Name;
            payment.CreateTime = DateTime.Now;
         //   groupByGroup.Payment.Add(payment);
        }

        private void UpdateGroupByAllInfo(GroupByAllInfo model, FormCollection collection)
        {
            string[] portals = collection["cbxPortal"].Split(',');
            string[] users = collection["cbxUser"].Split(',');

            //  GroupByGroupExtend groupByGroupExtend = model.GroupByGroupEntity;
            GroupByGroup groupByGroupExtend = model.GroupByGroupEntity;

            IGroupByGroupService groupByGroupService = new GroupByGroupService();
            GroupByGroup groupByGroup = groupByGroupService.GetById(model.GroupByGroupEntity.GroupByGroupID);
            groupByGroup.GroupByGroupName = groupByGroupExtend.GroupByGroupName;
            groupByGroup.GroupByContractNo = groupByGroupExtend.GroupByContractNo;
            groupByGroup.GroupByContent = groupByGroupExtend.GroupByContent;
            groupByGroup.OriginalPrice = groupByGroupExtend.OriginalPrice;
            groupByGroup.CustomerID = groupByGroupExtend.CustomerID;
            groupByGroup.SubIndustryID = groupByGroupExtend.SubIndustryID;
            groupByGroup.SettlementType = groupByGroupExtend.SettlementType;
            groupByGroup.LastModifyBy = this.User.Identity.Name;
            groupByGroup.LastModifyTime = DateTime.Now;

            ReBuildUpGroupBySalesList(groupByGroup, users);
            ReBuildUpPaymentList(groupByGroup, model);

            // IGroupByItemService groupByItemService = new GroupByItemService();
            IList<GroupByItem> groupByItemDbList = groupByGroup.GroupByItem.ToList();//groupByItemService.GetItemsById(groupByGroupExtend.GroupByGroupID);  //由于页面上的groupByItem中的有些值丢失，所以需要再去数据库取一次，重新赋值

            List<GroupByItem> groupByItemList = new List<GroupByItem>();

            foreach (GroupByItem item in groupByItemDbList)
            {
                if (!portals.Contains(item.GroupByPortalID.ToString())) //删除某个平台的团购 ??是否需要删除子表
                {
                    item.DeleteInd = 1;
                    foreach (GroupByCity groupByCity in item.GroupByCity)
                    {
                        groupByCity.DeleteInd = 1;
                    }
                    //  groupByItemList.Add(item);
                }
            }

            for (int i = 0; i < portals.Length; i++)
            {
                string portal = portals[i];
                int portalID = int.Parse(portal);
                GroupByItem groupByItem = model.GroupByItemList.FirstOrDefault(p => p.GroupByPortalID == portalID);
                GroupByItem groupByItemDb = groupByItemDbList.FirstOrDefault(p => p.GroupByPortalID == portalID);

                if (groupByItem != null && groupByItemDb == null)   //新增某个平台的团购
                {
                    string[] citys = collection["cbxCity" + portal].Split(',');
                    foreach (string cityID in citys)
                    {
                        GroupByCity groupByCity = new GroupByCity();
                        groupByCity.GroupByItemID = groupByItem.GroupByItemID;
                        groupByCity.CityID = int.Parse(cityID);
                        groupByItem.GroupByCity.Add(groupByCity);
                    }
                    //  groupByItemList.Add(groupByItem);
                    groupByGroup.GroupByItem.Add(groupByItem);
                }
                else if (groupByItem != null && groupByItemDb != null)  //修改某个平台的团购
                {
                    groupByItemDb.StartDay = groupByItem.StartDay;
                    groupByItemDb.EndDay = groupByItem.EndDay;
                    groupByItemDb.ExpireDay = groupByItem.ExpireDay;
                    groupByItemDb.LowestNum = groupByItem.LowestNum;
                    groupByItemDb.HighestNum = groupByItem.HighestNum;
                    groupByItemDb.SellingPrice = groupByItem.SellingPrice;
                    groupByItemDb.GroupByItemCodeNo = groupByItem.GroupByItemCodeNo;
                    groupByItemDb.URL = groupByItem.URL;
                    groupByItemDb.LastModifyBy = this.User.Identity.Name;
                    groupByItemDb.LastModifyTime = DateTime.Now;
                    string[] citys = collection["cbxCity" + portal].Split(',');
                    ReBuildUpGroupByCityList(groupByItemDb, citys);
                    // groupByItemList.Add(groupByItemDb);
                }
            }


            groupByGroupService.Update(groupByGroup);
            //     groupByItemService.Update(groupByItemList);
        }

        private void ReBuildUpGroupBySalesList(GroupByGroup groupByGroup, string[] userIDs)
        {
            List<GroupBySales> groupBySalesList = groupByGroup.GroupBySales.ToList();
            int salesCount = groupBySalesList.Count;
            int userCount = userIDs.Length;
            if (salesCount >= userCount)
            {
                int i = 0;
                for (; i < userCount; i++)
                {
                    GroupBySales sales = groupBySalesList[i];
                    sales.UserID = int.Parse(userIDs[i]);
                }
                for (int j = i; j <= salesCount - userCount; j++)
                {
                    GroupBySales sales = groupBySalesList[j];
                    sales.DeleteInd = 1;
                }
            }
            else
            {
                int i = 0;
                for (; i < salesCount; i++)
                {
                    GroupBySales sales = groupBySalesList[i];
                    sales.UserID = int.Parse(userIDs[i]);
                }
                for (int j = i; j < userCount - salesCount; j++)
                {
                    GroupBySales sales = new GroupBySales();
                    sales.GroupByGroupID = groupByGroup.GroupByGroupID;
                    sales.UserID = int.Parse(userIDs[j - i]);
                    groupByGroup.GroupBySales.Add(sales);
                }
            }
        }

        private void ReBuildUpGroupByCityList(GroupByItem item, string[] cityIDs)
        {
            List<GroupByCity> groupByCityList = item.GroupByCity.ToList();
            int groupByCityCount = groupByCityList.Count;
            int newCityCount = cityIDs.Length;
            if (groupByCityCount >= newCityCount)
            {
                int i = 0;
                for (; i < newCityCount; i++)
                {
                    GroupByCity groupByCity = groupByCityList[i];
                    groupByCity.CityID = int.Parse(cityIDs[i]);
                }
                for (int j = i; j <= groupByCityCount - newCityCount; j++)
                {
                    GroupByCity groupByCity = groupByCityList[j];
                    groupByCity.DeleteInd = 1;
                }
            }
            else
            {
                int i = 0;
                for (; i < groupByCityCount; i++)
                {
                    GroupByCity groupByCity = groupByCityList[i];
                    groupByCity.CityID = int.Parse(cityIDs[i]);
                }
                for (int j = i; j < newCityCount - groupByCityCount; j++)
                {
                    GroupByCity groupByCity = new GroupByCity();
                    groupByCity.GroupByItemID = item.GroupByItemID;
                    groupByCity.CityID = int.Parse(cityIDs[j - i]);
                    item.GroupByCity.Add(groupByCity);
                }
            }
        }

        private void ReBuildUpPaymentList(GroupByGroup groupByGroup, GroupByAllInfo model)
        {
            if (1==1)//groupByGroup.Payment.Count(p => p.DeleteInd == 0) == 0)
            {
                CreatePaymentList(model, groupByGroup);
            }
            else
            {
                List<DateTime?> paymentDateList = new List<DateTime?>();
                if (model.FirstPaymentDate.HasValue)
                    paymentDateList.Add(model.FirstPaymentDate);
                if (model.SecondPaymentDate.HasValue)
                    paymentDateList.Add(model.SecondPaymentDate);
                if (model.ThirdPaymentDate.HasValue)
                    paymentDateList.Add(model.ThirdPaymentDate);
                if (model.ForthPaymentDate.HasValue)
                    paymentDateList.Add(model.ForthPaymentDate);
                if (model.FifthPaymentDate.HasValue)
                    paymentDateList.Add(model.FifthPaymentDate);
                if (model.SixthPaymentDate.HasValue)
                    paymentDateList.Add(model.SixthPaymentDate);
                if (model.SeventhPaymentDate.HasValue)
                    paymentDateList.Add(model.SeventhPaymentDate);
                if (model.EighthPaymentDate.HasValue)
                    paymentDateList.Add(model.EighthPaymentDate);
                if (model.NinthPaymentDate.HasValue)
                    paymentDateList.Add(model.NinthPaymentDate);
                if (model.TenthPaymentDate.HasValue)
                    paymentDateList.Add(model.TenthPaymentDate);

                IList<Payment> paymentList = null;// groupByGroup.Payment.Where(p => p.DeleteInd == 0).ToList();
                int minCount = Math.Min(paymentList.Count, paymentDateList.Count);
                for (int i = 0; i < minCount; i++)
                {
                    Payment item = paymentList[i];
                    item.PaymentDeadline = paymentDateList[i];
                }
                if (paymentList.Count > paymentDateList.Count)
                {
                    for (int j = minCount; j < paymentList.Count; j++)
                    {
                        paymentList[j].DeleteInd = 1;
                    }
                }
                else
                {
                    for (int k = minCount; k < paymentDateList.Count; k++)
                    {
                        CreatePaymentEntity(paymentDateList[k], groupByGroup, k + 1);
                    }
                }
            }

        }

        #endregion

        // GET: /GroupBy/Create

        //public ActionResult Create2(int? id)
        //{
        //    GroupBySinglePlatForm model = new GroupBySinglePlatForm();
        //    if (id.HasValue)
        //    {
        //        IGroupByItemService itemService = new GroupByItemService();
        //        GroupByItem item = itemService.GetById(id.Value);

        //        IList<int> groupByCityList = new List<int>();
        //        foreach (GroupByCity groupByCity in item.GroupByCity)
        //        {
        //            groupByCityList.Add(groupByCity.CityID.Value);
        //        }

        //        model.Cities = groupByCityList;


        //        IList<int> sellersList = new List<int>();
        //        foreach (GroupBySales sales in item.GroupBySales)
        //        {
        //            sellersList.Add(sales.UserID.Value);
        //        }

        //        model.Sellers = sellersList;

        //        model.GroupByItemID = item.GroupByItemID;
        //        model.CustomerEntity = item.Customer;
        //        if(item.Customer!=null)
        //        {
        //            model.CustomerCityID = item.Customer.CityID;
        //            model.CustomerDistrictID = item.Customer.DistrictID;
        //            model.CustomerID = item.CustomerID;
        //        }
        //        model.DepartmentID = item.DepartmentID;
        //        model.EndDate = item.EndDay;
        //        model.ExpireDate = item.ExpireDay;
        //        model.GroupByContactNo = item.GroupByContactNo;
        //        model.GroupByContent = item.GroupByContent;
        //        model.GroupByName = item.GroupByName;
        //        model.GroupByPortalID = item.GroupByPortalID;
        //        if (item.SubIndustry != null)
        //        {
        //            model.IndustryID = item.SubIndustry.Industry.IndustryID;
        //        }
        //        model.OriginalPrice = item.OriginalPrice;
        //        model.RefundType = item.RefundType;
        //        model.SellingPrice = item.SellingPrice;
        //        model.SettlementType = item.SettlementType;
        //        model.StartDate = item.StartDay;
        //        model.SubIndustryID = item.SubIndustryID;
        //        model.Url = item.URL;
        //        model.LowestNum = item.LowestNum;
        //        model.HighestNum = item.HighestNum;
        //    }

        //    IGroupByPortalService service = new GroupByPortalService();

        //    Dictionary<int, string> portals = new Dictionary<int, string>();
        //    portals.Add(1, "QQ");
        //    portals.Add(2, "聚划算");
        //    ViewData["portal"] = service.GetAllPortals();

        //    IDepartmentService departmentService = new DepartmentService();
        //    IList<Department> departmentList = departmentService.GetAllDepartments();
        //    //List<SelectListItem> list = new List<SelectListItem>();
        //    //foreach (Department item in departmentList)
        //    //{
        //    //    list.Add(new SelectListItem { Text = item.DepartmentName, Value = item.DepartmentID.ToString() });
        //    //}
        //    ViewData["departmentSelectList"] = new SelectList(departmentList, "DepartmentID", "DepartmentName");
        //    if (model.DepartmentID.HasValue)
        //    {
        //        IUserService userService = new UserService();
        //        ViewData["userList"] = userService.GetUsersByDepartmentID(model.DepartmentID.Value);
        //    }
        //    else
        //    {
        //        if (departmentList.Count == 0 || departmentList[0].UserDepartment.Count == 0)
        //        {
        //            ViewData["userList"] = null;
        //        }
        //        else
        //        {
        //            IUserService userService = new UserService();
        //            ViewData["userList"] = userService.GetUsersByDepartmentID(departmentList[0].DepartmentID);
        //        }
        //    }

        //    IIndustryService industryService = new IndustryService();
        //    industryList = industryService.GetAllIndustries();
        //    ViewData["industrySelectList"] = new SelectList(industryList, "IndustryID", "IndustryName");
        //    if (model.IndustryID.HasValue)
        //    {
        //        ViewData["subIndustrySelectList"] = new SelectList(GetSubIndustryList(model.IndustryID.Value), "SubIndustryID", "SubIndustryName");
        //    }
        //    else
        //    {
        //        ViewData["subIndustrySelectList"] = new SelectList(industryList[0].SubIndustry, "SubIndustryID", "SubIndustryName");
        //    }

        //    ICityService cityService = new CityService();
        //    IList<City> cityList = cityService.GetAllCities();
        //    ViewData["city"] = cityList;

        //    List<SelectListItem> selectCityList = new List<SelectListItem>();
        //    //  selectCityList.Add(new SelectListItem { Text = "城市", Value = "0" });
        //    foreach (City item in cityList)
        //    {
        //        selectCityList.Add(new SelectListItem { Text = item.CityName, Value = item.CityID.ToString() });
        //    }
        //    ViewData["citySelectList"] = new SelectList(cityList, "CityID", "CityName");

        //    List<SelectListItem> selectDistrictList = new List<SelectListItem>();

        //    if (model.CustomerCityID.HasValue)
        //    {
        //        selectDistrictList = GetDistrictList(model.CustomerCityID.Value);
        //    }
        //    else
        //    {
        //        selectDistrictList.Add(new SelectListItem { Text = "地区", Value = "0" });
        //        foreach (District item in cityList[0].District)
        //        {
        //            selectDistrictList.Add(new SelectListItem { Text = item.DistrictName, Value = item.DistrictID.ToString() });
        //        }
        //    }
        //    ViewData["districtSelectList"] = selectDistrictList; //new SelectList(cityList[0].District, "DistrictID", "DistrictName");
        //    IList<Customer> customerList = null;
        //    if (model.CustomerCityID.HasValue && model.CustomerDistrictID.HasValue)
        //    {
        //        customerList = GetCustomerList(model.CustomerCityID.Value, model.CustomerDistrictID.Value);

        //        List<SelectListItem> customerSelectList = new List<SelectListItem>();
        //        foreach (Customer customer in customerList)
        //        {
        //            if (customer.CustomerID == model.CustomerID)
        //            {
        //                customerSelectList.Add(new SelectListItem() { Text = customer.CustomerName, Value = customer.CustomerID.ToString(), Selected = true });
        //            }
        //            else
        //            {
        //                customerSelectList.Add(new SelectListItem() { Text = customer.CustomerName, Value = customer.CustomerID.ToString() });
        //            }
        //        }
        //        ViewData["customerSelectList"] = customerSelectList;
        //    }
        //    else
        //    {
        //        customerList = cityList[0].Customer.ToList();
        //        ViewData["customerSelectList"] = new SelectList(customerList, "CustomerID", "CustomerName");
        //    }


        //    ICodeTableService codeTableService = new CodeTableService();
        //    IList<CodeTable> refundTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.RefundType);
        //    ViewData["refundTypeSelectList"] = new SelectList(refundTypeList, "CodeValue", "CodeDesc");

        //    IList<CodeTable> paymentTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.PaymentType);
        //    if (customerList.Count > 0)
        //    {
        //        var customer=customerList.First();
        //        ViewData["paymentTypeSelectList"] = new SelectList(paymentTypeList, "CodeValue", "CodeDesc", customer.DefaultPaymentType);
        //        ViewData["customerName"] = customer.CustomerName;
        //        ViewData["bank"] = customer.BankName;
        //        ViewData["reciever"] = customer.BankReceiver;
        //        ViewData["bankAccountNo"] = customer.BankAccount;
        //        ViewData["contactPhoneNo"] = customer.ContactPhoneNo;
        //    }
        //    {
        //        ViewData["paymentTypeSelectList"] = new SelectList(paymentTypeList, "CodeValue", "CodeDesc");
        //    }


        //    return View(model);
        //}

        public JsonResult GetSubIndustry(int industryID)
        {
            IList<SubIndustry> list = GetSubIndustryList(industryID);
            return Json(new SelectList(list, "SubIndustryID", "SubIndustryName"), JsonRequestBehavior.AllowGet);

        }

        public IList<SubIndustry> GetSubIndustryList(int industryID)
        {
            ISubIndustryService service = new SubIndustryService();
            IList<SubIndustry> list = service.GetSubIndustriesByIndustryID(industryID);
            return list;
        }

        public IList<SubIndustry> GetSubIndustryListBySubIndustryId(int subIndustryID)
        {
            ISubIndustryService service = new SubIndustryService();
            return service.GetSubIndustriesBySubIndustryID(subIndustryID);
        }

        public JsonResult GetUsersByDepartment(int departmentID)
        {
            IList<User> list = GetUsersListByDepartment(departmentID);

            return Json(new SelectList(list, "UserID", "UserName"), JsonRequestBehavior.AllowGet);
        }

        public IList<User> GetUsersListByDepartment(int departmentID)
        {
            IUserService service = new UserService();
            IList<User> list = service.GetUsersByDepartmentID(departmentID);
            return list;
        }

        public JsonResult GetDistrict(int cityID)
        {
            List<SelectListItem> selectDistrictList = GetDistrictList(cityID);
            return Json(selectDistrictList, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetDistrictList(int cityID)
        {
            IDistrictService service = new DistrictService();
            IList<District> list = service.GetDistrictByCity(cityID);

            List<SelectListItem> selectDistrictList = new List<SelectListItem>();
            selectDistrictList.Add(new SelectListItem { Text = "地区", Value = "0" });
            foreach (District item in list)
            {
                selectDistrictList.Add(new SelectListItem { Text = item.DistrictName, Value = item.DistrictID.ToString() });
            }
            return selectDistrictList;
        }

        public JsonResult GetCustomer(int cityID, int districtID)
        {
            IList<Customer> customerList = GetCustomerList(cityID, districtID);
            return Json(new SelectList(customerList, "CustomerID", "CustomerName"), JsonRequestBehavior.AllowGet);
        }

        public IList<Customer> GetCustomerList(int cityID, int districtID)
        {
            ICustomerService customerService = new CustomerService();
            IList<Customer> customerList = customerService.GetCustomersByCity(cityID, districtID);
            return customerList;
        }

        #region Create2 deleted
        // POST: /GroupBy/Create
        [HttpPost]
        //public ActionResult Create2(GroupBySinglePlatForm model, FormCollection collection)
        //{
        //    try
        //    {
        //        if (!model.GroupByItemID.HasValue)
        //        {
        //            IGroupByGroupService groupService = new GroupByGroupService();
        //            GroupByGroup group = new GroupByGroup();
        //            group.GroupByGroupName = model.GroupByName;
        //            group = groupService.Add(group);



        //            IGroupByItemService itemService = new GroupByItemService();
        //            GroupByItem item = new GroupByItem();
        //            item.GroupByPortalID = int.Parse(collection["cbxPortal"].Split(',')[0]);
        //            item.GroupByName = model.GroupByName;
        //            item.StartDay = model.StartDate;
        //            item.EndDay = model.EndDate;
        //            item.DepartmentID = int.Parse(collection["DepartmentID"]);
        //            item.SubIndustryID = int.Parse(collection["SubIndustryID"]);
        //            item.CustomerID = int.Parse(collection["CustomerID"]);
        //            item.URL = model.Url;
        //            item.HighestNum = model.HighestNum;
        //            item.LowestNum = model.LowestNum;
        //            item.GroupByContent = model.GroupByContent;
        //            item.CreateTime = DateTime.Now;
        //            item.CreateBy = this.User.Identity.Name;
        //            item.GroupByContactNo = model.GroupByContactNo;
        //            item.ExpireDay = model.ExpireDate;
        //            item.OriginalPrice = model.OriginalPrice;
        //            item.SellingPrice = model.SellingPrice;
        //            item.SettlementType = int.Parse(collection["SettlementType"]);
        //            item.RefundType = int.Parse(collection["RefundType"]);

        //            foreach (string salesID in collection["cbxUser"].Split(','))
        //            {
        //                GroupBySales sales = new GroupBySales();
        //                sales.GroupByItemID = item.GroupByItemID;
        //                sales.UserID = int.Parse(salesID);
        //                item.GroupBySales.Add(sales);
        //            }
        //            foreach (string cityID in collection["cbxCity"].Split(','))
        //            {
        //                GroupByCity city = new GroupByCity();
        //                city.GroupByItemID = item.GroupByItemID;
        //                city.CityID = int.Parse(cityID);
        //                item.GroupByCity.Add(city);
        //            }
        //            itemService.Add(item);
        //        }
        //        else
        //        {
        //            IGroupByItemService itemService = new GroupByItemService();
        //            GroupByItem item = itemService.GetById(model.GroupByItemID.Value);
        //            item.GroupByPortalID = int.Parse(collection["cbxPortal"].Split(',')[0]);
        //            item.GroupByName = model.GroupByName;
        //            item.StartDay = model.StartDate;
        //            item.EndDay = model.EndDate;
        //            item.DepartmentID = int.Parse(collection["DepartmentID"]);
        //            item.SubIndustryID = int.Parse(collection["SubIndustryID"]);
        //            item.CustomerID = int.Parse(collection["CustomerID"]);
        //            item.URL = model.Url;
        //            item.HighestNum = model.HighestNum;
        //            item.LowestNum = model.LowestNum;
        //            item.GroupByContent = model.GroupByContent;
        //            item.LastModifyTime = DateTime.Now;
        //            item.LastModifyBy = this.User.Identity.Name;
        //            item.GroupByContactNo = model.GroupByContactNo;
        //            item.ExpireDay = model.ExpireDate;
        //            item.OriginalPrice = model.OriginalPrice;
        //            item.SellingPrice = model.SellingPrice;
        //            item.SettlementType = int.Parse(collection["SettlementType"]);
        //            item.RefundType = int.Parse(collection["RefundType"]);

        //            List<GroupBySales> groupBySalesList = item.GroupBySales.ToList();
        //            string[] userIDs=collection["cbxUser"].Split(',');
        //            int salesCount=groupBySalesList.Count;
        //            int userCount=userIDs.Length;
        //            if (salesCount >= userCount)
        //            {
        //                int i = 0;
        //                for (; i < userCount; i++)
        //                {
        //                    GroupBySales sales = groupBySalesList[i];
        //                    sales.UserID = int.Parse(userIDs[i]);
        //                }
        //                for (int j = i; j <= salesCount - userCount; j++)
        //                {
        //                    GroupBySales sales = groupBySalesList[j];
        //                    sales.DeleteInd = 1;
        //                }
        //            }
        //            else
        //            {
        //                int i = 0;
        //                for (; i < salesCount; i++)
        //                {
        //                    GroupBySales sales = groupBySalesList[i];
        //                    sales.UserID = int.Parse(userIDs[i]);
        //                }
        //                for (int j = i; j <= userCount - salesCount; j++)
        //                {
        //                    GroupBySales sales = new GroupBySales();
        //                    sales.GroupByItemID = model.GroupByItemID;
        //                    sales.UserID = int.Parse(userIDs[j]);
        //                    item.GroupBySales.Add(sales);
        //                }

        //            }

        //            List<GroupByCity> groupByCityList = item.GroupByCity.ToList();
        //            string[] cityIDs = collection["cbxCity"].Split(',');
        //            int groupByCityCount = groupByCityList.Count;
        //            int newCityCount = cityIDs.Length;
        //            if (groupByCityCount >= newCityCount)
        //            {
        //                int i = 0;
        //                for (; i < newCityCount; i++)
        //                {
        //                    GroupByCity groupByCity = groupByCityList[i];
        //                    groupByCity.CityID = int.Parse(cityIDs[i]);
        //                }
        //                for (int j = i; j <= groupByCityCount - newCityCount; j++)
        //                {
        //                    GroupByCity groupByCity = groupByCityList[j];
        //                    groupByCity.DeleteInd = 1;
        //                }
        //            }
        //            else
        //            {
        //                int i = 0;
        //                for (; i < groupByCityCount; i++)
        //                {
        //                    GroupByCity groupByCity = groupByCityList[i];
        //                    groupByCity.CityID = int.Parse(cityIDs[i]);
        //                }
        //                for (int j = i; j <= newCityCount - groupByCityCount; j++)
        //                {
        //                    GroupByCity groupByCity = new GroupByCity();
        //                    groupByCity.GroupByItemID = model.GroupByItemID;
        //                    groupByCity.CityID = int.Parse(cityIDs[j]);
        //                    groupByCityList.Add(groupByCity);
        //                }
        //            }

        //            itemService.Update(item);
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //}

        #endregion

        // GET: /GroupBy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: /GroupBy/Edit/5
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


        // GET: /GroupBy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: /GroupBy/Delete/5
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

        public ActionResult Payment(int id)
        {
            GroupByPayment groupByPayment = new GroupByPayment();

            IGroupByGroupService service = new GroupByGroupService();
            GroupByGroup groupByGroup = service.GetById(id);

            if (!groupByGroup.CustomerID.HasValue)
            {
                return RedirectToAction("Index");
            }

            groupByPayment.GroupByGroupID = groupByGroup.GroupByGroupID;
            groupByPayment.GroupByName = groupByGroup.GroupByGroupName;
            groupByPayment.OriginalPrice = groupByGroup.OriginalPrice;
            groupByPayment.FinalProfit = groupByGroup.FinalProfit;

            groupByPayment.GroupByItemList = groupByGroup.GroupByItem.ToList();
            groupByPayment.CustomerEntity = groupByGroup.Customer;
          //  groupByPayment.PaymentList = groupByGroup.Payment.Where(p => p.DeleteInd == 0).ToList();

            if (groupByGroup.SettlementType.HasValue)
            {
                ICodeTableService codeTableService = new CodeTableService();
                CodeTable ct = codeTableService.GetCodeTableByID(groupByGroup.SettlementType.Value);
                groupByPayment.SettleTypeName = ct.CodeDesc;
            }
            return View(groupByPayment);
        }

        [HttpPost]
        public ActionResult Payment(FormCollection collection)
        {
            IGroupByGroupService groupByGroupService = new GroupByGroupService();
            GroupByGroup groupByGroup = groupByGroupService.GetById(int.Parse(collection["hdGroupByID"]));
            if (!string.IsNullOrEmpty(collection["txtFinalCharge"]))
            {
                groupByGroup.FinalProfit = decimal.Parse(collection["txtFinalCharge"]);
            }

            List<GroupByItem> groupByItemList = groupByGroup.GroupByItem.ToList();

            foreach (GroupByItem item in groupByItemList)
            {
                int id = item.GroupByItemID;

                if (!string.IsNullOrEmpty(collection["txtSellNum" + id]))
                {
                    item.SellNum = (int)decimal.Parse(collection["txtSellNum" + id]);
                }
                if (!string.IsNullOrEmpty(collection["txtRefundPrice" + id]))
                {
                    item.RefundPrice = decimal.Parse(collection["txtRefundPrice" + id]);
                }
                if (!string.IsNullOrEmpty(collection["txtRankValue" + id]))
                {
                    item.RankingValue = (int)decimal.Parse(collection["txtRankValue" + id]);
                }
                if (!string.IsNullOrEmpty(collection["txtRankNum" + id]))
                {
                    item.RankingNum = (int)decimal.Parse(collection["txtRankNum" + id]);
                }
            }

            List<Payment> paymentList = null;// groupByGroup.Payment.Where(p => p.DeleteInd == 0).ToList();
            string index = string.Empty;
            for (int i = 0; i < paymentList.Count; i++)
            {
                Payment payment = paymentList[i];
                index = (i + 1).ToString();
                if (!string.IsNullOrEmpty(collection["txtPaymentNum" + index]))
                {
                    payment.PaymentPrice = decimal.Parse(collection["txtPaymentNum" + index]);
                }
                if (!string.IsNullOrEmpty(collection["txtPaymentRate" + index]) && collection["txtPaymentRate" + index].Trim() != "%")
                {
                    payment.PaymentProportion = decimal.Parse(collection["txtPaymentRate" + index].Replace("%", "")) / (decimal)100;
                }
                if (!string.IsNullOrEmpty(collection["txtPaymentDate" + index]))
                {
                    payment.PaymentTime = DateTime.Parse(collection["txtPaymentDate" + index]);
                }

                payment.LastModifyBy = this.User.Identity.Name;
                payment.LastModifyTime = DateTime.Now;
            }

            groupByGroupService.Update(groupByGroup);

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult GroupByItemPayment(GroupByItem model, decimal? orginalPrice)
        {
            if (orginalPrice.HasValue)
            {
                ViewData["profitOne"] = model.SellingPrice - orginalPrice;
            }
            else
            {
                ViewData["profitOne"] = model.SellingPrice;
            }

            ViewData["ItemModel"] = model;
            return PartialView(model);
        }
    }
}
