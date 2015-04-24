using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service
{
    public class GroupByGroupService : IGroupByGroupService
    {
        GroupByGroupRepository repository = null;
        public GroupByGroupService()
        {
            repository = new GroupByGroupRepository();
        }
        #region IGroupByGroupService 成员

        public GroupByGroup Add(GroupByGroup entity)
        {
            return repository.Add<GroupByGroup>(entity);
        }

        public GroupByGroup GetByName(string groupByName)
        {
            return repository.GetBy<GroupByGroup>(p => p.GroupByGroupName == groupByName);
        }

        public GroupByGroup Update(GroupByGroup entity)
        {
            return repository.Update<GroupByGroup>(entity);
        }

        public GroupByGroup GetById(int groupByGroupId)
        {
            return repository.GetBy<GroupByGroup>(p => p.GroupByGroupID == groupByGroupId);
        }

        public List<GroupByPaymentInfo> GetPaymentList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID = 0, int groupByPortalID = 0)
        {
            return repository.GetPaymentList(paging, groupByName, seller, customerName, groupPaymentStatus, status, customerCityID, groupByPortalID);
        }

        public List<GroupByPaymentInfo> GetShelfList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID = 0, int groupByPortalID = 0)
        {
            return repository.GetShelfList(paging, groupByName, seller, customerName, groupPaymentStatus, status, customerCityID, groupByPortalID);
        }
        #endregion

        #region IGroupByGroupService 成员


        public List<GroupByGroup> GetAll()
        {
            return repository.GetAll<GroupByGroup>().ToList();
        }

        #endregion

        #region IGroupByGroupService 成员


        public List<GroupByGroup> SearchGroupBy(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string SearchType)
        {
            return repository.SearchGroupBy(paging, CustomerCityId, PortalId, GroupByCodeOrName, Sale, CustomerName, SearchType);
        }

        #endregion

        #region IGroupByGroupService 成员


        public List<GroupByItem> SearchGroupByItem(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string CurrentNode)
        {
            return repository.SearchGroupByItem(paging, CustomerCityId, PortalId, GroupByCodeOrName, Sale, CustomerName, CurrentNode);
        }

        #endregion
    }
}
