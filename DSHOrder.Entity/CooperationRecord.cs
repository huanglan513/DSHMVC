using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Entity
{
    public class CooperationRecord
    {
        public string PortalName { get; set; }

        public string GroupByGroupName { get; set; }

        public DateTime? StartDay { get; set; }

        public DateTime? EndDay { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public decimal TotalRevenue { get; set; }

        public int SellNum { get; set; }

        public decimal RankValue { get; set; }

        public string Url { get; set; }

        public int? ActualSellNum { get; set; }

        public decimal? ActualTotalProfit { get; set; }

        public decimal? OtherCharge { get; set; }
    }
}
