using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace DSHOrder.Web.Models
{
    public class JobInfoModel
    {
        public IJobDetail Job { get; set; }

        public IList<ITrigger> Triggers { get; set; }
    }
}