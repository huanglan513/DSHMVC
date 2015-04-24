using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class GroupByPortalService : IGroupByPortalService
    {
        GroupByPortalRepository repository = null;
        public GroupByPortalService()
        {
            repository = new GroupByPortalRepository();
        }
        #region IGroupByPortalService 成员

        public IList<GroupByPortal> GetAllPortals()
        {
            return repository.GetAll<GroupByPortal>();
        }

        #endregion
    }
}
