using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;

namespace DSHOrder.Web.Models
{
    public class CustomerDetailModel
    {
        public Customer CustomerInfo { get; set; }

        public PagedList<CooperationRecord> CooperationHistory { get; set; }
    }
}