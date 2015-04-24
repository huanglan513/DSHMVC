using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class GroupBySinglePlatForm
    {
        public int? GroupByItemID { get; set; }

        public int? GroupByGroupID { get; set; }

        public int? GroupByPortalID { get; set; }

        public string GroupByName { get; set; }

        public string GroupByContent { get; set; }

        public int? DepartmentID { get; set; }

        public int? IndustryID { get; set; }

        public int? SubIndustryID { get; set; }

        public Customer CustomerEntity { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? SellingPrice { get; set; }

        public string Url { get; set; }

        public int? SettlementType { get; set; }

        public int? RefundType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string GroupByContactNo { get; set; }

        public IList<int> Sellers { get; set; }

        public IList<int> Cities { get; set; }

        public int? CustomerCityID { get; set; }

        public int? CustomerDistrictID { get; set; }

        public int? CustomerID { get; set; }

        public int? LowestNum { get; set; }

        public int? HighestNum { get; set; }

    }
}