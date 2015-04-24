using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IGroupByCityService
    {
        GroupByCity Add(GroupByCity entity);

        int Add(IList<GroupByCity> entities);

        IList<GroupByCity> GetCitiesByPortalId(int GroupByItemId);
    }
}
