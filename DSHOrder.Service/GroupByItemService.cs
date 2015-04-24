using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class GroupByItemService : IGroupByItemService
    {
        GroupByItemRepository repository = null;
        public GroupByItemService()
        {
            repository = new GroupByItemRepository();
        }
        #region IGroupByItemService 成员

        public GroupByItem Add(GroupByItem entity)
        {
            return repository.Add<GroupByItem>(entity);
        }

        public GroupByItem GetById(int groupByItemID)
        {
            return repository.GetBy<GroupByItem>(p => p.GroupByItemID == groupByItemID);
        }

        public GroupByItem GetByToaobaoProductId(string productID)
        {
            return repository.GetBy<GroupByItem>(p => p.TaobaoProductID == productID);
        }

        public GroupByItem Update(GroupByItem entity)
        {
            return repository.Update<GroupByItem>(entity);
        }

        public int Add(List<GroupByItem> list)
        {
            foreach (GroupByItem item in list)
            {
                repository.Add<GroupByItem>(item, true);
            }
            return repository.SaveChanges();
        }

        public IList<GroupByItem> GetItemsById(int id)
        {
            return repository.GetItemsById(id);
        }

        public int Update(List<GroupByItem> list)
        {
            foreach (GroupByItem item in list)
            {
                repository.Update<GroupByItem>(item, true);
            }
            return repository.SaveChanges();
        }


        public IList<GroupByItem> GetAll()
        {
            return repository.GetAll<GroupByItem>();
        }

        public IList<GroupByItem> Search(int customerCityID, int portalID, int userID, string groupByNumber, string groupByName, string customerName, string filterType)
        {
            var query = from q in repository.CreateQuery<GroupByItem>()
                        //            where q.DeleteInd == 0 && (string.IsNullOrEmpty(groupByName) || q.GroupByName.Contains(groupByName)) 
                        //                  && (portalID == 0 || q.GroupByPortalID == portalID)
                        select q;

            //if (customerCityID > 0 || !string.IsNullOrEmpty(customerName))
            //{
            //    query = from q in query
            //            join p in repository.CreateQuery<Customer>() on q.CustomerID equals p.CustomerID
            //            where (customerCityID == 0 || p.CityID == customerCityID) && (string.IsNullOrEmpty(customerName) || p.CustomerName.Contains(customerName)) && p.DeleteInd == 0
            //            select q;
            //}

            //if (userID > 0)
            //{
            //    query = from q in query
            //            join p in repository.CreateQuery<GroupBySales>() on q.GroupByItemID equals p.GroupByItemID
            //            where p.UserID == userID && p.DeleteInd == 0
            //            select q;
            //}

            if (!string.IsNullOrEmpty(filterType))
            {
                if (filterType.Equals("groupbying"))
                {
                    query = from q in query
                            where q.StartDay <= DateTime.Now && q.EndDay > DateTime.Now
                            select q;
                }
                else if (filterType.Equals("groupbyed"))
                {
                    query = from q in query
                            where q.EndDay <= DateTime.Now
                            select q;
                }
                else if (filterType.Equals("groupbypay"))
                {
                    //TODO to be implemented
                }
                else if (filterType.Equals("groupbypre"))
                {
                    query = from q in query
                            where q.StartDay > DateTime.Now
                            select q;
                }
            }

            return query.ToList<GroupByItem>();
        }

        public List<GroupByItem> GetItemListByItemIDs(int[] groupByItemIDs)
        {
            return repository.GetAllBy<GroupByItem>(p => groupByItemIDs.Contains(p.GroupByItemID) && p.DeleteInd == 0).ToList();
        }

        #endregion


        public List<GroupByItem> GetByCondition(Common.Pagination paging, int? citySelected, int? portalSelected, string groupByNumber, string groupByName, string groupBySale, string customerName, int? groupByStatus)
        {

            var groupQuery = from q in repository.CreateQuery<GroupByGroup>()
                             where q.DeleteInd == 0
                                   && (string.IsNullOrEmpty(groupByName) || q.GroupByGroupName.Contains(groupByName) || q.GroupByCodeNo.Contains(groupByName))
                                   //&& (string.IsNullOrEmpty(groupByNumber) || q.GroupByCodeNo.Contains(groupByNumber))
                                   && (string.IsNullOrEmpty(customerName) || q.Customer.CustomerName.Contains(customerName))
                             select q;
            
            if (!string.IsNullOrEmpty(groupBySale))
            {
                groupQuery = from q in repository.CreateQuery<GroupByGroup>()
                                 join p in repository.CreateQuery<GroupBySales>() on q.GroupByGroupID equals p.GroupByGroupID
                                 where p.DeleteInd == 0 && p.User.UserName.Contains(groupBySale)
                                 select q;
            }

            var itemQuery = from q in repository.CreateQuery<GroupByItem>()
                            join p in groupQuery on q.GroupByGroupID equals p.GroupByGroupID
                            where q.DeleteInd == 0 && (!portalSelected.HasValue || q.GroupByPortalID == portalSelected)                            
                            select q;

            if (citySelected.HasValue)
            {
                itemQuery = from p in repository.CreateQuery<GroupByCity>()
                            join q in itemQuery on p.GroupByItemID equals q.GroupByItemID
                            where p.DeleteInd == 0 && p.CityID == citySelected                            
                            select q;
            }

            if (groupByStatus.HasValue)
            {
                if (groupByStatus == (int)DSHOrder.Entity.GroupBySearchStatus.GroupBying)
                {
                    itemQuery = from q in itemQuery
                                where q.StartDay <= DateTime.Now && q.EndDay > DateTime.Now
                                select q;
                }

                if (groupByStatus == (int)DSHOrder.Entity.GroupBySearchStatus.GroupByed)
                {
                    itemQuery = from q in itemQuery
                                where q.EndDay <= DateTime.Now
                                select q;
                }

                if (groupByStatus == (int)DSHOrder.Entity.GroupBySearchStatus.GroupBypay)
                {
                    DateTime deadlineTime = DateTime.Now.AddDays(7);
                    itemQuery = from q in itemQuery
                                join p in repository.CreateQuery<Payment>() on q.GroupByItemID equals p.GroupByItemID
                                where p.DeleteInd == 0 && p.PaymentDeadline.Value < deadlineTime && !p.PaymentTime.HasValue
                                select q;
                }

                if (groupByStatus == (int)DSHOrder.Entity.GroupBySearchStatus.GroupBypending)
                {
                    itemQuery = from q in itemQuery
                                where q.StartDay > DateTime.Now
                                select q;
                }
            }

            itemQuery = from q in itemQuery
                        orderby q.GroupByGroupID, q.CreateTime descending
                        select q;

            return paging.ParseQueryPaging<GroupByItem>(itemQuery).ToList();
        }
    }
}
