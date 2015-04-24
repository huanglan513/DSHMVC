using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface IOrderDetailService
    {
        IList<OrderDetail> GetByContidion(Pagination paging, int groupByItemID, string buyerName, string addr, DateTime? onlineDate, DateTime? dealTimeFrom, DateTime? dealTimeTo,int dealStatus, string tradeRate);

        IList<OrderDetail> GetByOfflineContidion(Pagination paging, string GroupByName, string GroupByNumber, string TaobaoOrderID, decimal? Price, string BuyerName, string Addr, DateTime? DealDateFrom, DateTime? DealDateTo, int dealType);
        
		int FillOrder(List<OrderDetail> orders);
        OrderDetail GetByTid(string tid);
        IList<OrderDetail> GetByGroupByItemId(int id);

        OrderDetail GetById(int orderDetailID);

        GroupByGroup GetGroupByGroup(int orderDetailID);

        OrderDetail Update(OrderDetail orderDetail);

        int Update(IList<OrderDetail> orderDetails);

        IList<OrderDetail> GetEmptyGroupByItem();
    }
}
