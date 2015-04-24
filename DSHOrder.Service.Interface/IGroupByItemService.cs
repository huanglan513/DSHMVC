using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IGroupByItemService
    {
        GroupByItem Add(GroupByItem entity);

        GroupByItem GetById(int groupByItemID);

        GroupByItem Update(GroupByItem entity);

        int Add(List<GroupByItem> list);

        IList<GroupByItem> GetItemsById(int id);

        int Update(List<GroupByItem> list);

        IList<GroupByItem> GetAll();

        IList<GroupByItem> Search(int customerCityID, int portalID, int userID, string groupByNumber, string groupByName, string customerName, string filterType);
        GroupByItem GetByToaobaoProductId(string productID);

        List<GroupByItem> GetByCondition(Common.Pagination paging, int? citySelected, int? portalSelected, string groupByNumber, string groupByName, string groupBySale, string customerName, int? groupByStatus);

        List<GroupByItem> GetItemListByItemIDs(int[] groupByItemIDs);
    }
}
