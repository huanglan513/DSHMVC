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
    public class OrderDetailService:IOrderDetailService
    {
         OrderDetailRepository repository = null;
         public OrderDetailService()
        {
            repository = new OrderDetailRepository();
        }
        #region IOrderDetailService 成员

         public IList<OrderDetail> GetByContidion(Pagination paging, int groupByItemID, string buyerName, string addr, DateTime? onlineDate, DateTime? dealTimeFrom, DateTime? dealTimeTo, int dealStatus, string tradeRate)
        {
            return repository.GetByContidion(paging, groupByItemID, buyerName, addr, onlineDate, dealTimeFrom, dealTimeTo, dealStatus, tradeRate);
        }

         public IList<OrderDetail> GetByOfflineContidion(Pagination paging, string GroupByName, string GroupByNumber, string TaobaoOrderID, decimal? Price, string BuyerName, string Addr, DateTime? DealDateFrom, DateTime? DealDateTo, int dealType)
         {
             return repository.GetByOfflineContidion(paging, GroupByName, GroupByNumber, TaobaoOrderID, Price, BuyerName, Addr, DealDateFrom, DealDateTo, dealType);
         }

        public int FillOrder(List<OrderDetail> orders)
        {
            foreach (OrderDetail item in orders)
            {
                repository.Add<OrderDetail>(item, true);
            }
            return repository.SaveChanges();
        }

        public OrderDetail GetByTid(string tid)
        {
            return repository.GetBy<OrderDetail>(p => p.Tid == tid);
        }

        public IList<OrderDetail> GetByGroupByItemId(int id)
        {
            return repository.GetAllBy<OrderDetail>(p => p.GroupByItemID == id);
        }
        public OrderDetail GetById(int orderDetailID)
        {
            return repository.GetBy<OrderDetail>(p => p.OrderDetailID == orderDetailID);
        }

        public GroupByGroup GetGroupByGroup(int orderDetailID)
        {
            return repository.GetGroupByGroup(orderDetailID);
        }

        public OrderDetail Update(OrderDetail orderDetail)
        {
            return repository.Update(orderDetail);
        }

        public int Update(IList<OrderDetail> orderDetails)
        { 
            foreach(OrderDetail od in orderDetails)
            {
                repository.Update<OrderDetail>(od, true);        
            }
            return repository.SaveChanges();            
        }

        public IList<OrderDetail> GetEmptyGroupByItem()
        {
            return repository.GetAllBy<OrderDetail>(p => !p.GroupByItemID.HasValue);
        }
        #endregion
    }
}
