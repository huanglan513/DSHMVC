using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class IndustryRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityIndustry; }
        }

        public IList<SubIndustry> GetPhoneLogisticSubIndustryIDList()
        {
            var query = from q in this.CreateQuery<Industry>()
                        join p in this.CreateQuery<SubIndustry>() on q.IndustryID equals p.IndustryID
                        where q.IndustryName.Contains("物流") || q.IndustryName.Contains("话费")
                        select p;
            return query.ToList();
        }

        public bool CheckIndustryForSale(int industryID, int UserID)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dtThisMonthStart = new DateTime(dtNow.Year, dtNow.Month, 1);
            DateTime dtNextMonthStart = new DateTime(dtNow.Year, dtNow.Month, 1);
            var query = from gbg in this.CreateQuery<GroupByGroup>()
                        join gbs in this.CreateQuery<GroupBySales>() on gbg.GroupByGroupID equals gbs.GroupByGroupID
                        where gbg.SubIndustry.IndustryID == industryID && gbs.UserID == UserID
                          && gbg.CreateTime >= dtThisMonthStart && gbg.CreateTime < dtNextMonthStart 
                        select 1;

            if (query.Count() >= 2) return false;

            return true;
        }
    }
}
