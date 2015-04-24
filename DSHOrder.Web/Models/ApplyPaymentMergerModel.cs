using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class ApplyPaymentMergerModel
    {

        public GroupByBaseInfo GroupBaseInfo { get; set; }

        public List<GroupByItemForShelf> GroupItemInfoList { get; set; }

        public List<Payment> PaymentList { get; set; }

        public string PaymentIDs { get; set; }

        public string Applicant { get; set; }
    }
}