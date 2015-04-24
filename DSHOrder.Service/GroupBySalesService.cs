using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class GroupBySalesService : IGroupBySalesService
    {
        IRepository repository = null;
        public GroupBySalesService()
        {
            repository = new GroupBySalesRepository();
        }
        #region IGroupBySalesService 成员

        public GroupBySales Add(GroupBySales entity)
        {
            return repository.Add<GroupBySales>(entity);
        }

        public int Add(IList<GroupBySales> entities)
        {
            foreach (GroupBySales item in entities)
            {
                repository.Add<GroupBySales>(item, true);
            }
            return repository.SaveChanges();
        }

        #endregion
    }
}
