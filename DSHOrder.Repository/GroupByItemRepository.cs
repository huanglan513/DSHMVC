using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class GroupByItemRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityGroupByItem; }
        }

        public IList<GroupByItem> GetItemsById(int id)
        {
            var query = from q in this.CreateQuery<GroupByGroup>()
                        join p in this.CreateQuery<GroupByItem>()
                        on q.GroupByGroupID equals p.GroupByGroupID
                        where q.GroupByGroupID == id && q.DeleteInd==0 && p.DeleteInd==0
                        orderby p.GroupByPortalID
                        select p;
            return query.ToList();
        }

    }
}
