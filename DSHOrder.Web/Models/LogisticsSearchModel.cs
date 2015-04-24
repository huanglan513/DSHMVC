using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;

namespace DSHOrder.Web.Models
{
    public class LogisticsSearchModel
    {
        public string SerialNum { get; set; }

        public string OrderID { get; set; }

        public DateTime? GetGoodsDateFrom { get; set; }

        public DateTime? GetGoodsDateTo { get; set; }

        public DateTime? ArriveStopFrom { get; set; }

        public DateTime? ArriveStopTo { get; set; }

        public string Status { get; set; }

        public string TelPhone { get; set; }

        public string CustomerName { get; set; }

        public string Carrier { get; set; }

        public string Address { get; set; }

        public PagedList<LogisticsInfo> LogisticsList { get; set; }

        public int ResetIndex { get; set; }
    }
}