using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using DSHOrder.Entity;

namespace DSHOrder.Web.Models
{
    public class ProcessPaymentModel
    {
        public int GroupByItemID { get; set; }

        public GroupByBaseInfo GroupBaseInfo { get; set; }

        public GroupByItemForShelf GroupItemInfo { get; set; }

        public Payment Payment { get; set; }

        public int PaymentID { get; set; }

        public string Payer { get; set; }
    }
}