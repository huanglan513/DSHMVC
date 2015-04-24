using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class ApprovalPaymentMergerModel
    {
        public GroupByBaseInfo GroupBaseInfo { get; set; }

        public IList<GroupByItemForShelf> GroupItemInfoList { get; set; }

        public IList<Payment> PaymentList { get; set; }

        public string PaymentIDs { get; set; }

        public string Approver { get; set; }
    }
}