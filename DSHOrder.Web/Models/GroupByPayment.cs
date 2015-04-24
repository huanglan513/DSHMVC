using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class GroupByPayment
    {
        public IList<GroupByItem> GroupByItemList { get; set; }

        public Customer CustomerEntity { get; set; }

        public string SettleType { set; get; }

        public int GroupByGroupID { get; set; }

        public string GroupByName { get; set; }

        public decimal? OriginalPrice { get; set; }

     
        public string SettleTypeName { get; set; }

        public List<Payment> PaymentList { get; set; }

        public decimal? FinalProfit { get; set; }
    }
}