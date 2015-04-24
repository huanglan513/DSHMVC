using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Entity
{
    public class GroupByPaymentInfo
    {
        public int GroupByGroupID { get; set; }

        public string GroupByGroupName { get; set; }

        public string GroupByCodeNo { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string DeptName { get; set; }

        public string Seller { get; set; }

        public string GroupByPortal { get; set; }

        public string CustomerCityName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int PortalCount { get; set; }

        public int? GroupStatus { get; set; }

        public int? ItemStatus { get; set; }

        public int GroupByItemID { get; set; }

        public string ApplyDateInfo { get; set; }

        public int? SubIndustryID { get; set; }
    }
}
