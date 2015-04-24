using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class ApplyPaymentModel
    {
        public int GroupByItemID { get; set; }

        public GroupByBaseInfo GroupBaseInfo { get; set; }

        public GroupByItemForShelf GroupItemInfo { get; set; }

        public Payment Payment { get; set; }

        public int PaymentID { get; set; }
    }

    public class GroupByItemForPayment
    {
        public int GroupByItemID { get; set; }

        [DisplayName("团购平台")]
        public string PortalName { get; set; }

        [DisplayName("团购价")]
        public decimal? SellingPrice { get; set; }

        [DisplayName("单份利润")]
        public decimal ProfitOne { get; set; }

        [DisplayName("总销售量")]
        public int? SellNum { get; set; }

        [DisplayName("退款总计")]
        public decimal? RefundPrice { get; set; }

        [DisplayName("团购名称")]
        public string GroupByGroupName { get; set; }

        [DisplayName("底价")]
        public decimal? OriginalPrice { get; set; }
    }

    public class GroupByBaseInfo
    {
       

        [DisplayName("商家公司全称")]
        public string CustomerName { get; set; }

        [DisplayName("结算方式")]
        public string SettlementType { get; set; }

        [DisplayName("月结日")]
        public string DefaultPaymentDate { get; set; }

        [DisplayName("商家联系电话")]
        public string CustomerPhone { get; set; }

        [DisplayName("收款人")]
        public string BankReceiver { get; set; }

        [DisplayName("收款账号")]
        public string BankAccount { get; set; }

        [DisplayName("收款银行")]
        public string BankName { get; set; }

       
    }
}