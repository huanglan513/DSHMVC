using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class GroupByInfo
    {
        public GroupByGroup GroupByGroupEntity { get; set; }
        public GroupByItem GroupByItemEntity { get; set; }
        public Customer CustomerEntity { get; set; }

        public string SettleType { set; get; }
        //public GroupByPortal GroupByPortalEntity { get; set; }

        public DateTime? FirstPaymentDate { get; set; }
        public DateTime? SecondPaymentDate { get; set; }
    }
}