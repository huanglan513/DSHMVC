using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Entity
{
    public class CustomerExtend:Customer
    {
        public string CityName { get; set; }

        public double AvgRankValue { get; set; }

        public int? GroupCount { get; set; }
    }
}
