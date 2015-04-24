using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class ShelfModel
    {
        public int GroupByItemID { get; set; }

        public GroupByBaseInfo GroupBaseInfo { get; set; }

        public GroupByItemForShelf GroupItemInfo { get; set; }

        //public Dictionary<int, KeyValuePair<DateTime?, decimal?>> PaymentList { get; set; }

        public IList<Payment> PaymentList { get; set; }

        [Required(ErrorMessage = "第一次打款日期不能为空")]
        [Display(Name = "第一次打款日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "第一次打款日期格式错误，应为:yyyy-MM-dd")]
        public DateTime? FirstPaymentDate { get; set; }

        public decimal? FirstPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SecondPaymentDate { get; set; }

        public decimal? SecondPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ThirdPaymentDate { get; set; }

        public decimal? ThirdPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ForthPaymentDate { get; set; }

        public decimal? ForthPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FifthPaymentDate { get; set; }

        public decimal? FifthPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SixthPaymentDate { get; set; }

        public decimal? SixthPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SeventhPaymentDate { get; set; }

        public decimal? SeventhPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EighthPaymentDate { get; set; }

        public decimal? EighthPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NinthPaymentDate { get; set; }

        public decimal? NinthPaymentPercent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TenthPaymentDate { get; set; }

        public decimal? TenthPaymentPercent { get; set; }
    }

    public class GroupByItemForShelf
    {
        public int GroupByItemID { get; set; }

        public int? Status { get; set; }

        [DisplayName("团购平台")]
        public string PortalName { get; set; }

        [DisplayName("售价")]
        public decimal? SellingPrice { get; set; }

        [DisplayName("单份利润")]
        public decimal ProfitOne { get; set; }

        [DisplayName("团购名称")]
        public string GroupByGroupName { get; set; }

        [DisplayName("成本价")]
        public decimal? OriginalPrice { get; set; }

        [DisplayName("物流费")]
        public decimal? LogisticCharge { get; set; }

        [DisplayName("其他费用")]
        public decimal? OtherCharge { get; set; }

        [DisplayName("后台数量")]
        public int? OnlineRefundNum { get; set; }

        [DisplayName("实际退款数量")]
        public int? ActualOnlineRefundNum { get; set; }

        [DisplayName("后台数量")]
        public int? OfflineRefundNum { get; set; }

        [DisplayName("实际退款数量")]
        public int? ActualOfflineRefundNum { get; set; }

        [DisplayName("后台数量")]
        public int? SellNum { get; set; }

        [DisplayName("实际销售数量")]
        public int? ActualSellNum { get; set; }

        [DisplayName("后台退款额")]
        public decimal? RefundPrice { get; set; }

        [DisplayName("实际退款额")]
        public decimal? ActualRefundPrice { get; set; }

        [DisplayName("后台利润")]
        public decimal? TotalProfit { get; set; }

        [DisplayName("实际利润")]
        public decimal? ActualTotalProfit { get; set; }

        [DisplayName("后台总付款金额")]
        public decimal? TotalPayment { get; set; }

        [DisplayName("实际总付款金额")]
        public decimal? ActualTotalPayment { get; set; }

        [DisplayName("是否尾款")]
        public string IsFinalPayment { get; set; }
    }
}