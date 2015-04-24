using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Web.Models;
using DSHOrder.Common;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;
using System.Text;

namespace DSHOrder.Web.Controllers
{
    public class FinancialPaymentController : Controller
    {
        private readonly IGroupByGroupService groupService;
        private readonly IIndustryService industryService;
        private readonly IPaymentService paymentService;
        private readonly IGroupByItemService itemService;
        public FinancialPaymentController(IGroupByGroupService groupService, IIndustryService industryService, IPaymentService paymentService, IGroupByItemService itemService)
        {
            this.groupService = groupService;
            this.industryService = industryService;
            this.paymentService = paymentService;
            this.itemService = itemService;
        }

        // GET: /FinancialPayment/NotApplyPaymentList

        public ActionResult NotApplyPaymentList(int? id, string GroupByName, string Seller, string CustomerName, int CustomerCityID = 0, int GroupByPortalID = 0, int ViewType = 0)
        {
            ApplyPaymentSearchModel model = new ApplyPaymentSearchModel();
            model.CustomerCityID = CustomerCityID;
            model.GroupByPortalID = GroupByPortalID;
            model.GroupByName = GroupByName == null ? "" : GroupByName.Trim();
            model.Seller = Seller == null ? "" : Seller.Trim();
            model.CustomerName = CustomerName == null ? "" : CustomerName.Trim();
            model.CityList = ComUtil.GetCityList();
            model.PortalList = ComUtil.GetPortalList();
            model.ViewType = ViewType;

            ViewData["PhoneLogisticSubIndustryIDList"] = GetPhoneLogisticSubIndustryIDList();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id ?? 1;

            List<GroupByPaymentInfo> groupList = groupService.GetPaymentList(paging, model.GroupByName, model.Seller, model.CustomerName, model.ViewType,0, model.CustomerCityID, model.GroupByPortalID);
            model.GroupByPaymentList = new PagedList<GroupByPaymentInfo>(groupList, id ?? 1, 10, paging.RowCount.Value);
            return View(model);
        }

        private IEnumerable<int> GetPhoneLogisticSubIndustryIDList()
        {
            IList<SubIndustry> subIndustryList=industryService.GetPhoneLogisticSubIndustryIDList();
            IEnumerable<int> subIndustryIDList = subIndustryList.Select(p => p.SubIndustryID);
            return subIndustryIDList;
        }

        //
        // GET: /FinancialPayment/ApplyPayment/5

        public ActionResult ApplyPayment(int groupByItemID)
        {
            ApplyPaymentModel model = LoadNotApplyModel(groupByItemID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyPayment(ApplyPaymentModel model, FormCollection collection)
        {
            Payment payment = paymentService.GetPaymentById(model.PaymentID);

            payment.ApprovalStatus = (int)Utils.ApprovalPaymentStatus.PendingApproval;
            payment.ApplyDate = DateTime.Now;
            payment.Applicant = this.User.Identity.Name;

            FinApplyInstance applyInstance = new FinApplyInstance();
            applyInstance.PaymentID = payment.PaymentID;
            applyInstance.Applicant = this.User.Identity.Name;
            applyInstance.ApplyDate = DateTime.Now;
            payment.FinApplyInstance.Add(applyInstance);

            paymentService.UpdatePayment(payment);

            return RedirectToAction("NotApplyPaymentList");
        }

        private ApplyPaymentModel LoadNotApplyModel(int groupByItemID)
        {
            ApplyPaymentModel model = new ApplyPaymentModel();

            GroupByItem groupItem = itemService.GetById(groupByItemID);

            GroupByGroup group = groupItem.GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);
            model.GroupItemInfo = GetItemInfo(groupItem, group);
            model.GroupByItemID = groupByItemID;
            
            Payment payment = groupItem.Payment.Where(p => p.DeleteInd == 0 && (!p.ApprovalStatus.HasValue || p.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Disagree)).OrderBy(p => p.PaymentType).FirstOrDefault();

            model.Payment = payment;
            if (payment != null)
            {
                model.PaymentID = payment.PaymentID;
                model.GroupItemInfo.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";
            }
            return model;
        }

        public ActionResult ApplyPaymentMerger(string itemIds)
        {
            int[] groupByItemIDs = (from q in itemIds.Split(',') select int.Parse(q)).ToArray();


            ApplyPaymentMergerModel model = new ApplyPaymentMergerModel();

            IList<GroupByItem> itemList = itemService.GetItemListByItemIDs(groupByItemIDs);

            GroupByGroup group = itemList[0].GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);

            List<GroupByItemForShelf> groupItemInfoList = new List<GroupByItemForShelf>();
            List<Payment> paymentList = new List<Payment>();
            StringBuilder paymentIDs = new StringBuilder();
            foreach (GroupByItem item in itemList)
            {
                GroupByItemForShelf itemForShelf = GetItemInfo(item, item.GroupByGroup);
                Payment payment = item.Payment.Where(p => p.DeleteInd == 0 && (!p.ApprovalStatus.HasValue || p.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Disagree)).OrderBy(p => p.PaymentType).FirstOrDefault();
                itemForShelf.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";

                groupItemInfoList.Add(itemForShelf);
                paymentList.Add(payment);
                paymentIDs.Append(payment.PaymentID + ",");
            }

            model.GroupItemInfoList = groupItemInfoList;
            model.PaymentList = paymentList;
            model.PaymentIDs = paymentIDs.ToString().Substring(0,paymentIDs.Length-1);
            model.Applicant = this.User.Identity.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyPaymentMerger(ApplyPaymentMergerModel model, FormCollection collection)
        {
            int[] paymentIDs = (from q in model.PaymentIDs.Split(',')
                                select int.Parse(q)).ToArray();
           
            IList<Payment> paymentList = paymentService.GetPaymentsByIds(paymentIDs);
            foreach (Payment payment in paymentList)
            {
                payment.ApprovalStatus = (int)Utils.ApprovalPaymentStatus.PendingApproval;
                payment.ApplyDate = DateTime.Now;
                payment.Applicant = this.User.Identity.Name;

                FinApplyInstance applyInstance = new FinApplyInstance();
                applyInstance.PaymentID = payment.PaymentID;
                applyInstance.Applicant = this.User.Identity.Name;
                applyInstance.ApplyDate = DateTime.Now;
                payment.FinApplyInstance.Add(applyInstance);
            }

            paymentService.UpdatePayments(paymentList);

            return RedirectToAction("NotApplyPaymentList");
        }

        //
        // GET: /FinancialPayment/Create

        public ActionResult ApprovalPaymentList(int? id, string GroupByName, string Seller, string CustomerName, int CustomerCityID = 0, int GroupByPortalID = 0, int ViewType = 0)
        {
            ApplyPaymentSearchModel model = new ApplyPaymentSearchModel();
            model.CustomerCityID = CustomerCityID;
            model.GroupByPortalID = GroupByPortalID;
            model.GroupByName = GroupByName == null ? "" : GroupByName.Trim();
            model.Seller = Seller == null ? "" : Seller.Trim();
            model.CustomerName = CustomerName == null ? "" : CustomerName.Trim();
            model.CityList = ComUtil.GetCityList();
            model.PortalList = ComUtil.GetPortalList();
            model.ViewType = ViewType;


            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id ?? 1;

            List<GroupByPaymentInfo> groupList = groupService.GetPaymentList(paging, model.GroupByName, model.Seller, model.CustomerName, model.ViewType, (int)ComUtil.ApplyPaymentType.Applied, model.CustomerCityID, model.GroupByPortalID);
            model.GroupByPaymentList = new PagedList<GroupByPaymentInfo>(groupList, id ?? 1, 10, paging.RowCount.Value);
            return View(model);
        }

        public ActionResult ApprovalPayment(int groupByItemID)
        {
            ApprovalPaymentModel model = LoadApprovalModel(groupByItemID);
            return View(model);
        }

        private ApprovalPaymentModel LoadApprovalModel(int groupByItemID)
        {
            ApprovalPaymentModel model = new ApprovalPaymentModel();

            GroupByItem groupItem = itemService.GetById(groupByItemID);

            GroupByGroup group = groupItem.GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);
            model.GroupItemInfo = GetItemInfo(groupItem, group);
            model.GroupByItemID = groupByItemID;

            List<Payment> appliedPaymentList = new List<Payment>();
            Payment payment = groupItem.Payment.Where(p => p.DeleteInd == 0 && (!p.ApprovalStatus.HasValue || p.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.PendingApproval)).OrderBy(p => p.PaymentType).FirstOrDefault();

            model.Payment = payment;
            if (payment != null)
            {
                model.PaymentID = payment.PaymentID;
                model.GroupItemInfo.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";
            }
            model.Approver = this.User.Identity.Name;
            return model;
        }

        private List<GroupByItemForPayment> GetGroupByItemList(GroupByGroup group)
        {
            List<GroupByItemForPayment> itemPaymentList = new List<GroupByItemForPayment>();
            IList<GroupByItem> itemList = group.GroupByItem.Where(p => p.DeleteInd == 0).ToList();
            foreach (GroupByItem item in itemList)
            {
                GroupByItemForPayment itemPayment = new GroupByItemForPayment();
                itemPayment.GroupByItemID = item.GroupByItemID;
                itemPayment.PortalName = item.GroupByPortal.PortalName;
                itemPayment.SellNum = item.SellNum;
                itemPayment.SellingPrice = item.SellingPrice;
                itemPayment.ProfitOne = (item.SellingPrice - group.OriginalPrice).HasValue ? (item.SellingPrice - group.OriginalPrice).Value : 0;
                itemPayment.RefundPrice = item.RefundPrice;
                itemPayment.GroupByGroupName = group.GroupByGroupName;
                itemPayment.OriginalPrice = group.OriginalPrice;
                itemPaymentList.Add(itemPayment);
            }
            return itemPaymentList;
        }

        private GroupByItemForShelf GetItemInfo(GroupByItem item, GroupByGroup group)
        {
            GroupByItemForShelf itemShelf = new GroupByItemForShelf();
            itemShelf.GroupByGroupName = group.GroupByGroupName;
            itemShelf.OriginalPrice = group.OriginalPrice;
            itemShelf.SellingPrice = item.SellingPrice;
            itemShelf.LogisticCharge = item.LogisticCharge;
            itemShelf.OtherCharge = item.OtherCharge;
            itemShelf.PortalName = item.GroupByPortal.PortalName;
            itemShelf.OnlineRefundNum = item.OnlineRefundNum;
            itemShelf.ActualOnlineRefundNum = item.ActualOnlineRefundNum;
            itemShelf.OfflineRefundNum = item.OfflineRefundNum;
            itemShelf.ActualOfflineRefundNum = item.ActualOfflineRefundNum;
            itemShelf.SellNum = item.SellNum;
            itemShelf.ActualSellNum = item.ActualSellNum;
            itemShelf.RefundPrice = item.RefundPrice;
            itemShelf.ActualRefundPrice = item.ActualRefundPrice;
            itemShelf.TotalProfit = item.TotalProfit;
            itemShelf.ActualTotalProfit = item.ActualTotalProfit;
            itemShelf.TotalPayment = item.TotalPayment;
            itemShelf.ActualTotalPayment = item.ActualTotalPayment;
            itemShelf.Status = item.Status;

            return itemShelf;
        }


        private GroupByBaseInfo GetGroupBaseInfo(Customer customer)
        {
            GroupByBaseInfo baseInfo = new GroupByBaseInfo();
            if (customer != null)
            {
                baseInfo.CustomerName = customer.CustomerName;
                baseInfo.CustomerPhone = customer.ContactPhoneNo;
                baseInfo.BankAccount = customer.BankAccount;
                baseInfo.BankName = customer.BankName;
                baseInfo.BankReceiver = customer.BankReceiver;

                ICodeTableService codeTableService = new CodeTableService();
                CodeTable ct = codeTableService.GetCodeTableByID(customer.DefaultPaymentType.Value);
                if (ct != null)
                {
                    baseInfo.SettlementType = ct.CodeDesc;
                    if (ct.CodeDesc.Trim() == "月结")
                    {
                        baseInfo.DefaultPaymentDate = customer.DefaultPaymentDate == -1 ? "月末最后一天" : customer.DefaultPaymentDate.ToString();
                    }
                }


            }
            return baseInfo;
        }

        //
        // POST: /FinancialPayment/Create

        [HttpPost]
        public ActionResult ApprovalPayment(ApprovalPaymentModel model,FormCollection collection)
        {
            try
            {
                Payment payment = paymentService.GetPaymentById(model.PaymentID);
                payment.ActualPaymentDeadline = DateTime.Parse(collection["PaymentDeadline"]);
                payment.ActualPaymentProportion = decimal.Parse(collection["PaymentProportion"].ToString().Replace("%", "")) / 100;
                payment.ActualPaymentPrice = decimal.Parse(collection["PaymentPrice"]);
                payment.ApprovalStatus = int.Parse(collection["result"]);

                FinApprovalRecord record = new FinApprovalRecord();
                record.Approver = this.User.Identity.Name;
                if (int.Parse(collection["result"]) == 3)
                {
                    record.ApprovalResult = 0; //拒绝
                }
                else
                {
                    record.ApprovalResult = 1; //同意
                }
                record.ActualPaymentDeadline = payment.ActualPaymentDeadline;
                record.ActualPaymentProportion = payment.ActualPaymentProportion;
                record.ActualPaymentPrice = payment.ActualPaymentPrice;
                record.ApprovalComment = collection["Comment"];
                record.ApprovalDate = DateTime.Now;

                FinApplyInstance applyInstance = payment.FinApplyInstance.OrderByDescending(p => p.ApplyDate).FirstOrDefault();
                applyInstance.FinApprovalRecord.Add(record);

                paymentService.UpdatePayment(payment);

                return RedirectToAction("ApprovalPaymentList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ApprovalPaymentMerger(string itemIds)
        {
            int[] groupByItemIDs = (from q in itemIds.Split(',') select int.Parse(q)).ToArray();


            ApprovalPaymentMergerModel model = new ApprovalPaymentMergerModel();

            IList<GroupByItem> itemList = itemService.GetItemListByItemIDs(groupByItemIDs);

            GroupByGroup group = itemList[0].GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);

            List<GroupByItemForShelf> groupItemInfoList = new List<GroupByItemForShelf>();
            List<Payment> paymentList = new List<Payment>();
            StringBuilder paymentIDs = new StringBuilder();
            foreach (GroupByItem item in itemList)
            {
                GroupByItemForShelf itemForShelf = GetItemInfo(item, item.GroupByGroup);
                Payment payment = item.Payment.Where(p => p.DeleteInd == 0 && (p.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.PendingApproval)).OrderBy(p => p.PaymentType).FirstOrDefault();
                itemForShelf.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";

                groupItemInfoList.Add(itemForShelf);
                paymentList.Add(payment);
                paymentIDs.Append(payment.PaymentID + ",");
            }

            model.GroupItemInfoList = groupItemInfoList;
            model.PaymentList = paymentList;
            model.PaymentIDs = paymentIDs.ToString().Substring(0, paymentIDs.Length - 1);
            model.Approver = this.User.Identity.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult ApprovalPaymentMerger(ApprovalPaymentMergerModel model, FormCollection collection)
        {
            int[] paymentIDs = (from q in model.PaymentIDs.Split(',')
                                select int.Parse(q)).ToArray();

            IList<Payment> paymentList = paymentService.GetPaymentsByIds(paymentIDs);
            foreach (Payment payment in paymentList)
            {
                payment.ActualPaymentDeadline = DateTime.Parse(collection["PaymentDeadline"+payment.PaymentID]);
                payment.ActualPaymentProportion = decimal.Parse(collection["PaymentProportion" + payment.PaymentID].ToString().Replace("%", "")) / 100;
                payment.ActualPaymentPrice = decimal.Parse(collection["PaymentPrice" + payment.PaymentID]);
                payment.ApprovalStatus = int.Parse(collection["result"]);

                FinApprovalRecord record = new FinApprovalRecord();
                record.Approver = this.User.Identity.Name;
                if (int.Parse(collection["result"]) == 3)
                {
                    record.ApprovalResult = 0; //拒绝
                }
                else
                {
                    record.ApprovalResult = 1; //同意
                }
                record.ActualPaymentDeadline = payment.ActualPaymentDeadline;
                record.ActualPaymentProportion = payment.ActualPaymentProportion;
                record.ActualPaymentPrice = payment.ActualPaymentPrice;
                record.ApprovalComment = collection["Comment"];
                record.ApprovalDate = DateTime.Now;

                FinApplyInstance applyInstance = payment.FinApplyInstance.OrderByDescending(p => p.ApplyDate).FirstOrDefault();
                applyInstance.FinApprovalRecord.Add(record);
               
            }
            paymentService.UpdatePayments(paymentList);

            return RedirectToAction("ApprovalPaymentList");
        }
        
        //
        // GET: /FinancialPayment/Edit/5
 
        public ActionResult PaymentList(int? id, string GroupByName, string Seller, string CustomerName,  int CustomerCityID=0, int GroupByPortalID=0,int ViewType=1)
        {
            ApplyPaymentSearchModel model = new ApplyPaymentSearchModel();
            model.CustomerCityID = CustomerCityID;
            model.GroupByPortalID = GroupByPortalID;
            model.GroupByName = GroupByName == null ? "" : GroupByName.Trim();
            model.Seller = Seller == null ? "" : Seller.Trim();
            model.CustomerName = CustomerName == null ? "" : CustomerName.Trim();
            model.CityList = ComUtil.GetCityList();
            model.PortalList = ComUtil.GetPortalList();
            model.ViewType = ViewType;
          //  model.GroupPaymentStatus = GroupPaymentStatus;
          
         //   model.GroupPaymentStatusList = GetGroupPaymentStatusList();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id ?? 1;

            List<GroupByPaymentInfo> groupList = groupService.GetPaymentList(paging, model.GroupByName, model.Seller, model.CustomerName, model.ViewType, (int)ComUtil.ApplyPaymentType.UnPaid, model.CustomerCityID, model.GroupByPortalID);
            model.GroupByPaymentList = new PagedList<GroupByPaymentInfo>(groupList, id ?? 1, 10, paging.RowCount.Value);
            return View(model);
        }


        public ActionResult ProcessPayment(int groupByItemID)
        {
            ProcessPaymentModel model = LoadNotPaidModel(groupByItemID);
            return View(model);
        }

        private ProcessPaymentModel LoadNotPaidModel(int groupByItemID)
        {
            ProcessPaymentModel model = new ProcessPaymentModel();

            GroupByItem groupItem = itemService.GetById(groupByItemID);

            GroupByGroup group = groupItem.GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);
            model.GroupItemInfo = GetItemInfo(groupItem, group);
            model.GroupByItemID = groupByItemID;

            List<Payment> appliedPaymentList = new List<Payment>();
            Payment payment = groupItem.Payment.Where(p => p.DeleteInd == 0 && (p.ApprovalStatus==(int)Utils.ApprovalPaymentStatus.Agree) &&(!p.PaymentStatus.HasValue || p.PaymentStatus == (int)Utils.PaymentStatus.NotPayment)).OrderBy(p => p.PaymentType).FirstOrDefault();

            model.Payment = payment;
            if (payment != null)
            {
                model.PaymentID = payment.PaymentID;
                model.GroupItemInfo.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";
            }
            model.Payer = this.User.Identity.Name;
            return model;
        }

        //
        // POST: /FinancialPayment/Edit/5

        [HttpPost]
        public ActionResult ProcessPayment(ProcessPaymentModel model, FormCollection collection)
        {
            try
            {
                Payment payment=paymentService.GetPaymentById(model.PaymentID);

                payment.PaymentStatus = int.Parse(collection["paymentStatus"]);
                payment.PaymentTime = DateTime.Parse(collection["PaymentDate"]);
                payment.CertificateImg = collection["txtImgCertificateImg"];
                payment.Payer = this.User.Identity.Name;
                payment.PaymentComment = collection["Comment"];

                if (payment.IsLastPayment.HasValue && payment.IsLastPayment.Value)
                {
                    payment.GroupByItem.Status = (int)Utils.GroupByItemStatus.Paied;
                }

                paymentService.UpdatePayment(payment);

               

                return RedirectToAction("PaymentList");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ProcessPaymentMerger(string itemIds)
        {
            int[] groupByItemIDs = (from q in itemIds.Split(',') select int.Parse(q)).ToArray();


            ProcessPaymentMergerModel model = new ProcessPaymentMergerModel();

            IList<GroupByItem> itemList = itemService.GetItemListByItemIDs(groupByItemIDs);

            GroupByGroup group = itemList[0].GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);

            List<GroupByItemForShelf> groupItemInfoList = new List<GroupByItemForShelf>();
            List<Payment> paymentList = new List<Payment>();
            StringBuilder paymentIDs = new StringBuilder();
            foreach (GroupByItem item in itemList)
            {
                GroupByItemForShelf itemForShelf = GetItemInfo(item, item.GroupByGroup);
                Payment payment = item.Payment.Where(p => p.DeleteInd == 0 && (p.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Agree) && (!p.PaymentStatus.HasValue || p.PaymentStatus == (int)Utils.PaymentStatus.NotPayment)).OrderBy(p => p.PaymentType).FirstOrDefault();
                if (payment != null)
                {
                    itemForShelf.IsFinalPayment = payment.IsLastPayment == true ? "是" : "否";
                    paymentIDs.Append(payment.PaymentID + ",");
                }

                groupItemInfoList.Add(itemForShelf);
                paymentList.Add(payment);             
            }

            model.GroupItemInfoList = groupItemInfoList;
            model.PaymentList = paymentList;
            model.PaymentIDs = paymentIDs.Length > 0 ? paymentIDs.ToString().Substring(0, paymentIDs.Length - 1) : "0";
            model.Payer = this.User.Identity.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult ProcessPaymentMerger(ProcessPaymentMergerModel model,FormCollection collection)
        {
            int[] paymentIDs = (from q in model.PaymentIDs.Split(',')
                                select int.Parse(q)).ToArray();
            List<int> lastPaymentItemList = new List<int>();

            IList<Payment> paymentList = paymentService.GetPaymentsByIds(paymentIDs);
            foreach (Payment payment in paymentList)
            {
                payment.PaymentStatus = int.Parse(collection["paymentStatus"]);
                payment.PaymentTime = DateTime.Parse(collection["PaymentDate"]);
                payment.CertificateImg = collection["txtImgCertificateImg"];
                payment.Payer = this.User.Identity.Name;
                payment.PaymentComment = collection["Comment"];
                if (payment.IsLastPayment.HasValue && payment.IsLastPayment.Value)
                {
                    lastPaymentItemList.Add(payment.GroupByItemID.Value);
                    payment.GroupByItem.Status = (int)Utils.GroupByItemStatus.Paied;
                }

                
            }

            paymentService.UpdatePayments(paymentList);
            return RedirectToAction("PaymentList");
        }


        public ActionResult PaymentHistory(int groupByItemID)
        {
            IList<Payment> paymentList = paymentService.GetPaymentsByItemID(groupByItemID);

            List<PaymentHistoryModel> modelList = new List<PaymentHistoryModel>();
            foreach (Payment payment in paymentList)
            {
                if (payment.ApprovalStatus.HasValue && payment.ApprovalStatus >= (int)Utils.ApprovalPaymentStatus.PendingApproval)
                {
                    foreach (FinApplyInstance applyInstance in payment.FinApplyInstance)
                    {
                        PaymentHistoryModel model = new PaymentHistoryModel();
                        model.PaymentType = payment.PaymentType.Value;
                        model.PaymentDeadline = payment.PaymentDeadline.Value;
                        model.PaymentProportion = payment.PaymentProportion;
                        model.PaymentPrice = payment.PaymentPrice;
                        model.Action = "申请";
                        model.User = applyInstance.Applicant;
                        model.ActionDate = applyInstance.ApplyDate.Value;
                        modelList.Add(model);

                        foreach (FinApprovalRecord record in applyInstance.FinApprovalRecord)
                        {
                            PaymentHistoryModel modelRecord = new PaymentHistoryModel();
                            modelRecord.PaymentType = payment.PaymentType.Value;
                            modelRecord.PaymentDeadline = payment.PaymentDeadline.Value;
                            modelRecord.PaymentProportion = payment.PaymentProportion;
                            modelRecord.PaymentPrice = payment.PaymentPrice;
                            modelRecord.ActualPaymentDeadline = record.ActualPaymentDeadline;
                            modelRecord.ActualPaymentProportion = record.ActualPaymentProportion;
                            modelRecord.ActualPaymentPrice = record.ActualPaymentPrice;
                            modelRecord.Action = "审批";
                            modelRecord.Result = record.ApprovalResult == 1 ? "同意" : "拒绝";
                            modelRecord.User = record.Approver;
                            modelRecord.ActionDate = record.ApprovalDate.Value;
                            modelRecord.Comment = record.ApprovalComment;
                            modelList.Add(modelRecord);
                        }
                    }

                    if (payment.PaymentStatus.HasValue)
                    {
                        PaymentHistoryModel modelPayment = new PaymentHistoryModel();
                        modelPayment.PaymentType = payment.PaymentType.Value;
                        modelPayment.PaymentDeadline = payment.PaymentDeadline.Value;
                        modelPayment.PaymentProportion = payment.PaymentProportion;
                        modelPayment.PaymentPrice = payment.PaymentPrice;
                        modelPayment.ActualPaymentDeadline = payment.ActualPaymentDeadline;
                        modelPayment.ActualPaymentProportion = payment.ActualPaymentProportion;
                        modelPayment.ActualPaymentPrice = payment.ActualPaymentPrice;
                        modelPayment.Action = "付款";
                        modelPayment.Result = payment.PaymentStatus == 1 ? "已付" : "未付";
                        modelPayment.CertificateImage = payment.CertificateImg;
                        modelPayment.User = payment.Payer;
                        modelPayment.ActionDate = payment.PaymentTime.Value;
                        modelPayment.Comment = payment.PaymentComment;
                        modelList.Add(modelPayment);
                    }
                }
                else
                {
                    break;
                }
            }
            return View(modelList);
        }
      

        public IEnumerable<SelectListItem> GetGroupPaymentStatusList()
        {
            List<SelectListItem> GroupPaymentStatusList=new List<SelectListItem>();
            GroupPaymentStatusList.Add(new SelectListItem() { Text = "全部", Value = "0" });
            GroupPaymentStatusList.Add(new SelectListItem() { Text = "等待付款", Value = "1"});
            GroupPaymentStatusList.Add(new SelectListItem() { Text = "已部分付款", Value = "2" });
            GroupPaymentStatusList.Add(new SelectListItem() { Text = "付款全部完毕", Value = "3" });
            return GroupPaymentStatusList;
        }
    }
}
