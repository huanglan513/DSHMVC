using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;

namespace DSHOrder.Web.Models
{
    public class CustomerSearchModel
    {
        public int ViewType { get; set; }

        public string CustomerNameSearch { get; set; }

        public int CityIDSearch { get; set; }

        public int IsCertifiedSearch { get; set; }

        public PagedList<CustomerExtend> CustomerList { get; set; }
    }
}