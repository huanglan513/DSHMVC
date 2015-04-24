using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class SubIndustryRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntitySubIndustry; }
        }

        public IList<SubIndustry> GetSubIndustriesBySubIndustryID(int subIndustryID)
        {
            var query = from q in this.CreateQuery<SubIndustry>()
                        join p in this.CreateQuery<SubIndustry>()
                        on q.IndustryID equals p.IndustryID
                        where q.SubIndustryID == subIndustryID
                        select p;
            return query.ToList();
        }

        public SubIndustry GetByID(int id)
        {
            var rows = from SubIndustry row in this.CreateQuery<SubIndustry>()
                       where row.SubIndustryID == id
                       select row;

            return rows.Single();
        }
    }
}
