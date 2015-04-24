using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Entity;
using System.ComponentModel;
using DSHOrder.Web.Extensions;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class OrderSearchModel
    {
        public int GroupByItemId { get; set; }
        
        /// <summary>
        /// 团购编号/名称
        /// </summary>
        [DisplayName("团购名称")]
        public string GroupByName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        //[DataType(DataType.Currency)]
        [DisplayName("价格")]        
        [DisplayFormat(DataFormatString = "{0:C}")]        
        public decimal? Price { get; set; }

        /// <summary>
        /// 业务人员名称
        /// </summary>
        [DisplayName("业务员")]
        public string GroupBySale { get; set; }

        /// <summary>
        /// 上线日期
        /// </summary>
        [DisplayName("上线日期")]
        public DateTime? OnlineDate { get; set; }

        /// <summary>
        /// 订单评价
        /// </summary>
        [DisplayName("订单评价")]
        [DropdownList("TradeRate")]
        public string TradeRate { get; set; }

        /// <summary>
        /// 买家昵称
        /// </summary>
        [DisplayName("买家昵称")]
        public string BuyerNick { get; set; }

        /// <summary>
        /// 地址/电话
        /// </summary>
        [DisplayName("地址/电话")]
        public string Address { get; set; }

        /// <summary>
        /// 办理日期(由)
        /// </summary>
        [DisplayName("办理日期(由)")]
        public DateTime? DealDateFrom { get; set; }

        /// <summary>
        /// 办理日期(至)
        /// </summary>
        [DisplayName("办理日期(至)")]
        public DateTime? DealDateTo { get; set; }

        /// <summary>
        /// 订单处理状态
        /// </summary>
        public string OrderDealStatus { get; set; }

        /// <summary>
        /// 订单清单列表
        /// </summary>
        public PagedList<OrderDetail> OrderDetailList { get; set; }

        /// <summary>
        /// 列表的页数，注意:此项为弱赋值引用：
        /// PageIndexParameterName = "PageIndex"
        /// 在cshtml上并没有强引用
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 仅仅用于页面数据格式转换,订单状态显示信息
        /// </summary>
        public IDictionary<int, string> OrderStatusTypes { get; set; }

        /// <summary>
        /// 仅仅用于页面数据格式转换,退款状态显示信息
        /// </summary>
        public IDictionary<int, string> RefundStatusTypes { get; set; }
    }

    public class OfflineRefundSearchModel : OrderSearchModel
    {
        [DisplayName("淘宝子订单号")]
        public string Oid { get; set; }

        [DisplayName("买家应付金额")]
        public decimal? TotalFee { get; set; }
              
        public string OfflineSearchStatus { get; set; }

        /// <summary>
        /// 仅仅用于页面数据格式转换,线下退款处理状态显示
        /// </summary>
        public IDictionary<int, string> OfflineDealStatusTypes { get; set; }
    }
}