using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class PhoneFeeInfoSearchModel
    {
        public string OrderID { get; set; }

        public string BuyerName { get; set; }

        public string GetGoodsAddr { get; set; }

        public string PhoneNumber { get; set; }

        public string Result { get; set; }

        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }

        public DateTime? RechargeDateFrom { get; set; }
        public DateTime? RechargeDateTo { get; set; }

        public PagedList<PhoneFeeInfo> PhoneFeeInfoList { get; set; }

        public int ResetIndex { get; set; }
    }
}