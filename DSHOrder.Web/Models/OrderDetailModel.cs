using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DSHOrder.Web.Extensions;

namespace DSHOrder.Web.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }
        public int GroupByItemID { get; set; }
       
        [DisplayName("团购名称")]
        public string GroupByName { get; set; }

        [DisplayName("买家昵称")]
        public string BuyerNick { get; set; }

        [DisplayName("支付宝账号")]
        public string AlipayNo { get; set; }

        [DisplayName("淘宝订单编号")]
        public string Oid { get; set; }

        [DisplayName("淘宝退款编号")]
        public string RefundID { get; set; }

        [DisplayName("买家应付金额")]
        public decimal? TotalFee { get; set; }

        [DisplayName("买家地址")]
        public string Address { get; set; }

        [DisplayName("买家留言")]
        public string BuyerMessage { get; set; }

        [DisplayName("订单评价")]
        public string TradeRate { get; set; }

        [DisplayName("订单状态")]
        public string OrderStatus { get; set; }

        [DisplayName("退款状态")]
        public string RefundStatus { get; set; }

        [DisplayName("是否需要退货")]
        public bool HasGoodReturn { get; set; }

        [DisplayName("退货时间")]
        public DateTime? GoodReturnTime { get; set; }

        [DisplayName("问题类型")]
        [DropdownList("IssueType")]
        public string IssueType { get; set; }

        [DisplayName("退款原因")]
        public string Reason { get; set; }

        [DisplayName("退款说明")]
        public string Desc { get; set; }

        [DisplayName("退还给买家金额")]
        public decimal? RefundFee { get; set; }

        [DisplayName("支付给卖家金额")]
        public decimal? Payment { get; set; }

        [DisplayName("申请退款时间")]
        public DateTime? ApplyRefundTime { get; set; }

        [DisplayName("退款更新时间")]
        public DateTime? UpdateRefundTime { get; set; }

        [DisplayName("是否线下转款")]
        public bool IsOfflineRefund { get; set; }

        [DisplayName("是否处理完成")]
        public bool IsDealed { get; set; }

        [DisplayName("处理日期")]
        public DateTime? DealTime { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
    }

    public class OfflineReturnDetailModel : OrderDetailModel
    {
        [DisplayName("线下转款状态")]
        [DropdownList("OfflineReturnStatus")]
        public string OfflineRefundStatus { get; set; }

        [DisplayName("线下转款处理日期")]
        public DateTime? OfflineDealTime { get; set; }

        [DisplayName("线下转款金额")]
        public decimal? OfflineRefundFee { get; set; }

        [DisplayName("上传凭证")]
        public string CredentialPath { get; set; }
    }
}