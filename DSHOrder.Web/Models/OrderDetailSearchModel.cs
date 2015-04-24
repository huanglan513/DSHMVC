using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;

namespace DSHOrder.Web.Models
{
    public class OrderDetailSearchModel
    {
        public int GroupByItemId { get; set; }
        public string GroupByNumber { get; set; }
        public string TaobaoOrderID { get; set; }
        public string GroupByName { get; set; }
        public string Sellers { get; set; }
        public decimal? Price { get; set; }

        public string BuyerName { get; set; }

        public string Addr { get; set; }

        public DateTime? OnlineDate { get; set; }

        public DateTime? DealDateFrom { get; set; }

        public DateTime? DealDateTo { get; set; }

        public int StatusType { get; set; }

        public PagedList<OrderDetail> OrderDetailList { get; set; }
            
    } 
}