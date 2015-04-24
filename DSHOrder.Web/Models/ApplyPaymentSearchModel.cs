using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;
using System.ComponentModel;
using System.Web.Mvc;

namespace DSHOrder.Web.Models
{
    public class ApplyPaymentSearchModel
    {
        [DisplayName("团购名称/编号")]
        public string GroupByName { get; set; }

        [DisplayName("业务员")]
        public string Seller { get; set; }

        [DisplayName("商家名称")]
        public string CustomerName { get; set; }

        [DisplayName("商家所在城市")]
        public int CustomerCityID { get; set; }

        [DisplayName("平台")]
        public int GroupByPortalID { get; set; }

        [DisplayName("付款状态")]
        public int GroupPaymentStatus { get; set; }

        public int ViewType { get; set; }

        public IEnumerable<SelectListItem> CityList { get; set; }

        public IEnumerable<SelectListItem> PortalList { get; set; }

        public IEnumerable<SelectListItem> GroupPaymentStatusList { get; set; }

        public PagedList<GroupByPaymentInfo> GroupByPaymentList { get; set; }
    }
}