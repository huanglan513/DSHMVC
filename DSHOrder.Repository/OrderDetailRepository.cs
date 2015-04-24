using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class OrderDetailRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityOrderDetail; }
        }

        public IList<OrderDetail> GetByOfflineContidion(Pagination paging, string groupByName, string groupByNumber, string taobaoOrderID, decimal? price, string buyerName, string addr, DateTime? dealTimeFrom, DateTime? dealTimeTo, int dealStatus)
        {
            var query = from q in this.CreateQuery<OrderDetail>()
                        where (!price.HasValue || q.TotalFee == price) && q.DeleteInd == 0
                        select q;

            switch (dealStatus)
            {
                case (int)Entity.OrderDetailSearchType.All://全部订单
                    query = query.Where(p => p.OfflineDealStatus.HasValue);
                    break;
                case (int)Entity.OrderDetailSearchType.Pending://未办理
                    query = query.Where(p => p.OfflineDealStatus.HasValue && p.OfflineDealStatus == 1);
                    break;
                case (int)Entity.OrderDetailSearchType.Done://已成功办理
                    query = query.Where(p => p.OfflineDealStatus.HasValue && p.OfflineDealStatus == 2);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(groupByName))
            {
                query = query.Where(p => p.GroupByItem.GroupByGroup.GroupByGroupName.Contains(groupByName));
            }

            if (!string.IsNullOrEmpty(groupByNumber))
            {
                query = query.Where(p => p.GroupByItem.GroupByNum.ToString().Contains(groupByNumber));
            }

            if (!string.IsNullOrEmpty(taobaoOrderID))
            {
                query = query.Where(p => p.Oid.Equals(taobaoOrderID));
            }

            if (!string.IsNullOrEmpty(buyerName))
            {
                query = query.Where(p => p.BuyerNick.Contains(buyerName));
            }
            if (!string.IsNullOrEmpty(addr))
            {
                query = query.Where(p => p.Address.Contains(addr));
            }
            if (dealTimeFrom.HasValue)
            {
                query = query.Where(p => p.DealTime >= dealTimeFrom.Value);
            }
            if (dealTimeTo.HasValue)
            {
                DateTime dateTime = dealTimeTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(p => p.DealTime <= dateTime);
            }
            query = query.OrderBy(p => p.DealTime);
            return paging.ParseQueryPaging<OrderDetail>(query).ToList();
        }

        public IList<OrderDetail> GetByContidion(Pagination paging, int groupByItemID, string buyerName, string addr, DateTime? onlineDate, DateTime? dealTimeFrom, DateTime? dealTimeTo, int dealStatus, string tradeRate)
        {
            var query = from q in this.CreateQuery<OrderDetail>()
                        where q.GroupByItemID==groupByItemID && q.DeleteInd==0
                        select q;
            switch (dealStatus)
            {
                case 1://未办理
                    query = query.Where(p => !p.HasCSDeal.HasValue || p.HasCSDeal.Value == 0);
                    break;
                case 2://已成功办理
                    query = query.Where(p => p.HasCSDeal.HasValue && p.HasCSDeal.Value == 1);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(tradeRate))
            {
                int rateId = Convert.ToInt32(tradeRate);
                query = query.Where(p => p.TradeRate == rateId);
            }
            if (!string.IsNullOrEmpty(buyerName))
            {
                query = query.Where(p=>p.BuyerNick.Contains(buyerName));
            }
            if(!string.IsNullOrEmpty(addr))
            {
                query = query.Where(p => p.Address.Contains(addr));
            }            
            if (dealTimeFrom.HasValue)
            {
                query=query.Where(p=>p.DealTime>=dealTimeFrom.Value);
            }
            if(dealTimeTo.HasValue)
            {
                DateTime dateTime=dealTimeTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query=query.Where(p=>p.DealTime<=dateTime);
            }
            if (onlineDate.HasValue)
            {
                query = query.Where(p => p.GroupByItem.StartDay.HasValue && p.GroupByItem.StartDay == onlineDate);
            }
            query = query.OrderBy(p => p.DealTime);
            return paging.ParseQueryPaging<OrderDetail>(query).ToList();
        }

        public GroupByGroup GetGroupByGroup(int orderDetailID)
        {
            var query = from q in this.CreateQuery<OrderDetail>()
                        join p in this.CreateQuery<GroupByItem>()
                        on q.GroupByItemID equals p.GroupByItemID
                        join s in this.CreateQuery<GroupByGroup>()
                        on p.GroupByGroupID equals s.GroupByGroupID
                        where q.OrderDetailID == orderDetailID && q.DeleteInd == 0 &&
                          p.DeleteInd == 0 && s.DeleteInd == 0
                        select s;
            List<GroupByGroup> groupByGroupList = query.ToList();
            if (groupByGroupList.Count > 0)
            {
                return groupByGroupList[0];
            }
            else
            {
                return null;
            }
        }
    }
}
