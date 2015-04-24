using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface IGroupByGroupService
    {
        GroupByGroup Add(GroupByGroup entity);

        GroupByGroup GetByName(string groupByName);

        GroupByGroup Update(GroupByGroup entity);

        GroupByGroup GetById(int groupByGroupId);

        List<GroupByPaymentInfo> GetPaymentList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID = 0, int groupByPortalID = 0);


        List<GroupByPaymentInfo> GetShelfList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID = 0, int groupByPortalID = 0);


        List<GroupByGroup> GetAll();


        List<GroupByGroup> SearchGroupBy(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string SearchType);

        List<GroupByItem> SearchGroupByItem(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string CurrentNode);
    }
}
