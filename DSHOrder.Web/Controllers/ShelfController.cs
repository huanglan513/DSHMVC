using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Web.Models;
using DSHOrder.Common;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Service;
using Webdiyer.WebControls.Mvc;

namespace DSHOrder.Web.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IGroupByGroupService groupService;
        private readonly IGroupByItemService itemService;
        public ShelfController(IGroupByGroupService groupService, IGroupByItemService itemService)
        {
            this.groupService = groupService;
            this.itemService = itemService;
        }
        //
        // GET: /Shelf/

        public ActionResult Index(int? id, string GroupByName, string Seller, string CustomerName, int CustomerCityID = 0, int GroupByPortalID = 0, int ViewType = 0)
        {
            ApplyPaymentSearchModel model = new ApplyPaymentSearchModel();
            model.CustomerCityID = CustomerCityID;
            model.GroupByPortalID = GroupByPortalID;
            model.GroupByName = GroupByName == null ? "" : GroupByName.Trim();
            model.Seller = Seller == null ? "" : Seller.Trim();
            model.CustomerName = CustomerName == null ? "" : CustomerName.Trim();
            model.ViewType = ViewType;
            model.CityList = ComUtil.GetCityList();
            model.PortalList = ComUtil.GetPortalList();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id ?? 1;

            List<GroupByPaymentInfo> itemList = groupService.GetShelfList(paging, model.GroupByName, model.Seller, model.CustomerName, 0, model.ViewType, model.CustomerCityID, model.GroupByPortalID);
            model.GroupByPaymentList = new PagedList<GroupByPaymentInfo>(itemList, id ?? 1, 10, paging.RowCount.Value);
            return View(model);
        }

        //
        // GET: /Shelf/Details/5

        public ActionResult DealShelf(int groupByItemID)
        {
            ShelfModel model = new ShelfModel();

            GroupByItem item= itemService.GetById(groupByItemID);

            GroupByGroup group = item.GroupByGroup;
            model.GroupBaseInfo = GetGroupBaseInfo(group.Customer);
            model.GroupItemInfo = GetItemInfo(item, group);
            BindPaymentDate(model, item);

            return View(model);
        }

        private void BindPaymentDate(ShelfModel model, GroupByItem item)
        {
            Dictionary<int, KeyValuePair<DateTime?, decimal?>> paymentModelList = new Dictionary<int, KeyValuePair<DateTime?, decimal?>>();
            IList<Payment> paymentList = item.Payment.Where(p => p.DeleteInd == 0).ToList();
            for (int i = 0; i < paymentList.Count; i++)
            {
                paymentModelList.Add(i + 1, new KeyValuePair<DateTime?, decimal?>(paymentList[i].PaymentDeadline, paymentList[i].PaymentProportion));
            }

            model.PaymentList = paymentList;
        }

        //
        // POST: /Shelf/DealShelf

        [HttpPost]
        public ActionResult DealShelf(ShelfModel model,FormCollection collection)
        {
            try
            {
                GroupByItem item = itemService.GetById(model.GroupByItemID);

                GetLatelyItemInfo(model, item);
                if ((!item.Status.HasValue || item.Status < (int)Utils.GroupByItemStatus.Shelf) && collection["GroupItemInfo.cbxShelf"].IndexOf("true") > -1)
                {
                    item.Status = (int)Utils.GroupByItemStatus.Shelf;
                }

                IList<Payment> paymentList = item.Payment.Where(p=>p.DeleteInd==0).ToList();
                int i = 0;
                for (; i < paymentList.Count; i++)
                {
                    Payment payment = paymentList[i];
                    if (collection["PaymentDate" + (i + 1).ToString()]!=null && !string.IsNullOrEmpty(collection["PaymentDate" + (i + 1).ToString()].ToString()))
                    {
                        payment.PaymentDeadline = DateTime.Parse(collection["PaymentDate" + (i + 1).ToString()].ToString());
                        string percent = collection["PaymentPercent" + (i + 1).ToString()].ToString();
                        if (!string.IsNullOrEmpty(percent) && percent.Replace("%", "").Trim() != string.Empty)
                        {
                            payment.PaymentProportion = decimal.Parse(percent.Replace("%", "")) / 100;
                        }
                        payment.IsLastPayment = collection["cbxFinal" + (i + 1).ToString()].IndexOf("true") > -1 ? true : false;
                        string price = collection["PaymentPrice" + (i + 1).ToString()].ToString();
                        if (!string.IsNullOrEmpty(price))
                        {
                            payment.PaymentPrice = decimal.Parse(price);
                        }
                        else
                        {
                            payment.PaymentPrice = null;
                        }
                        payment.LastModifyBy = this.User.Identity.Name;
                        payment.LastModifyTime = DateTime.Now;
                    }
                    else if(collection["PaymentDate" + (i + 1).ToString()]!=null )
                    {
                        payment.DeleteInd = 1;
                        payment.LastModifyBy = this.User.Identity.Name;
                        payment.LastModifyTime = DateTime.Now;
                    } 
                }
                for (; i < 10; i++)
                {
                    if (collection["PaymentDate" + (i + 1).ToString()] != null && !string.IsNullOrEmpty(collection["PaymentDate" + (i + 1).ToString()].ToString()))
                    {
                        Payment payment = new Payment();
                        payment.GroupByItemID = item.GroupByItemID;
                        payment.PaymentDeadline = DateTime.Parse(collection["PaymentDate" + (i + 1).ToString()].ToString());
                     
                        string percent = collection["PaymentPercent" + (i + 1).ToString()].ToString();
                        if (!string.IsNullOrEmpty(percent) && percent.Replace("%", "").Trim() != string.Empty)
                        {
                            payment.PaymentProportion = decimal.Parse(percent.Replace("%", "")) / 100;
                        }
                        string price = collection["PaymentPrice" + (i + 1).ToString()].ToString();
                        if (!string.IsNullOrEmpty(price))
                        {
                            payment.PaymentPrice = decimal.Parse(price);
                        }
                        else
                        {
                            payment.PaymentPrice = null;
                        }
                        payment.IsLastPayment = collection["cbxFinal" + (i + 1).ToString()].IndexOf("true")>-1?true:false;
                        payment.PaymentType = i + 1;
                        payment.CreateBy = this.User.Identity.Name;
                        payment.CreateTime = DateTime.Now;
                        item.Payment.Add(payment);
                    }
                    else
                    {
                        break;
                    }
                }

                itemService.Update(item);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void GetLatelyItemInfo(ShelfModel model, GroupByItem item)
        {
            GroupByItemForShelf itemShelf = model.GroupItemInfo;
            item.LogisticCharge = itemShelf.LogisticCharge;
            item.OtherCharge = itemShelf.OtherCharge;  
            item.ActualOnlineRefundNum = itemShelf.ActualOnlineRefundNum;
            item.ActualOfflineRefundNum = itemShelf.ActualOfflineRefundNum;
            item.ActualSellNum = itemShelf.ActualSellNum;
            item.ActualRefundPrice = itemShelf.ActualRefundPrice;
            item.TotalProfit = itemShelf.TotalProfit;
            item.ActualTotalProfit = itemShelf.ActualTotalProfit;
            item.TotalPayment = itemShelf.TotalPayment;
            item.ActualTotalPayment = itemShelf.ActualTotalPayment;
            
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

        private GroupByItemForShelf GetItemInfo(GroupByItem item,GroupByGroup group)
        {
            GroupByItemForShelf itemShelf = new GroupByItemForShelf();
            itemShelf.GroupByGroupName = group.GroupByGroupName;
            itemShelf.OriginalPrice = group.OriginalPrice??0;
            itemShelf.SellingPrice = item.SellingPrice??0;
            itemShelf.LogisticCharge = item.LogisticCharge;
            itemShelf.OtherCharge = item.OtherCharge;
            itemShelf.PortalName = item.GroupByPortal.PortalName;
            itemShelf.OnlineRefundNum = item.OnlineRefundNum??0;
            itemShelf.ActualOnlineRefundNum = item.ActualOnlineRefundNum;
            itemShelf.OfflineRefundNum = item.OfflineRefundNum??0;
            itemShelf.ActualOfflineRefundNum = item.ActualOfflineRefundNum;
            itemShelf.SellNum = item.SellNum??0;
            itemShelf.ActualSellNum = item.ActualSellNum;
            itemShelf.RefundPrice = item.RefundPrice??0;
            itemShelf.ActualRefundPrice = item.ActualRefundPrice;
            itemShelf.TotalProfit = item.TotalProfit??0;
            itemShelf.ActualTotalProfit = item.ActualTotalProfit;
            itemShelf.TotalPayment = item.TotalPayment??0;
            itemShelf.ActualTotalPayment = item.ActualTotalPayment;
            itemShelf.Status = item.Status;

            return itemShelf;
        }
      
    }
}
