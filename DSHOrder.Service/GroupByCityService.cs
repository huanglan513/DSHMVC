using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class GroupByCityService : IGroupByCityService
    {
        IRepository repository = null;
        public GroupByCityService()
        {
            repository = new GroupByCityRepository();
        }
        #region IGroupByCityService 成员

        public GroupByCity Add(GroupByCity entity)
        {
            return repository.Add<GroupByCity>(entity);
        }

        public int Add(IList<GroupByCity> entities)
        {
            foreach (GroupByCity item in entities)
            {
                repository.Add<GroupByCity>(item, true);
            }
            return repository.SaveChanges();
        }

        #endregion

        #region IGroupByCityService 成员


        public IList<GroupByCity> GetCitiesByPortalId(int GroupByItemId)
        {
            var qGroupByCity = repository.CreateQuery<GroupByCity>();
            var qGroupByItem = repository.CreateQuery<GroupByItem>();

            var liReturn = (from gbc in qGroupByCity
                            from gbi in qGroupByItem
                            where gbi.GroupByItemID == gbc.GroupByItemID
                                    && gbi.GroupByItemID == GroupByItemId
                                    && gbc.DeleteInd == 0
                                    && gbi.DeleteInd == 0
                            select gbc).ToList();

            return liReturn;
        }

        #endregion
    }
}
