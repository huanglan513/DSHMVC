using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Configuration;

namespace DSHOrder.Taobao
{
    public class TaobaoTradeModel:BaseTaobaoModel
    {
        /// <summary>
        /// Get the trade by trade ID.
        /// </summary>
        /// <param name="tradeID"></param>
        /// <returns></returns>
        public Trade GetTradeByID(long tradeID)
        {
            TradeGetRequest request = new TradeGetRequest();
            request.Fields = "seller_nick, buyer_nick, title, type, created, tid, seller_rate, buyer_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, commission_fee, buyer_memo, seller_memo, alipay_no, buyer_message, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, orders";
            request.Tid = tradeID;
            TradeGetResponse response = client.Execute(request, taobaoConfig.TestSesionKey);
            return response.Trade;
        }

        /// <summary>
        /// Get the list of Trads include the follow information:
        /// seller_nick, buyer_nick, title, type, created, tid, seller_rate, buyer_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, commission_fee, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, receiver_zip, receiver_mobile, receiver_phone，seller_flag, orders
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="tradeStatus"></param>
        /// <param name="buyerNick"></param>
        /// <param name="rateStatus"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Trade> GetAllTrades(DateTime? startTime, DateTime? endTime, Constants.TradeStatus? tradeStatus, string buyerNick, Constants.TradeStatus? rateStatus, long? pageNo, long? pageSize)
        {
            TradesSoldGetRequest request = new TradesSoldGetRequest();
            request.Fields = "seller_nick, buyer_nick, title, type, created, tid, seller_rate, buyer_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, commission_fee, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, receiver_zip, receiver_mobile, receiver_phone,orders";
            request.StartCreated = startTime;
            request.EndCreated = endTime;
            request.Status = tradeStatus.HasValue? tradeStatus.ToString() : string.Empty;
            request.BuyerNick = buyerNick;
            request.RateStatus = rateStatus.HasValue ? rateStatus.ToString() : string.Empty;
            request.PageNo = pageNo;
            request.PageSize = pageSize;
            TradesSoldGetResponse response = client.Execute(request, taobaoConfig.TestSesionKey);

            return response.TotalResults > 0 ? response.Trades : null;
        }

        /// <summary>
        /// Get the latest list of the trades including the follow information:
        /// seller_nick, buyer_nick, title, type, created, tid, seller_rate, buyer_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, commission_fee, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, receiver_zip, receiver_mobile, receiver_phone,orders
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Trade> GetLatestTrads(DateTime startTime, DateTime endTime, long? pageNo, long? pageSize)
        {
            TradesSoldIncrementGetRequest request = new TradesSoldIncrementGetRequest();
            request.Fields = "seller_nick, buyer_nick, title, type, created, tid, seller_rate, buyer_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, commission_fee, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, receiver_zip, receiver_mobile, receiver_phone,orders";
            request.StartModified = startTime;
            request.EndModified = endTime;
            request.PageNo = pageNo;
            request.PageSize = pageSize;
            TradesSoldIncrementGetResponse response = client.Execute(request, taobaoConfig.TestSesionKey);

            List<Trade> list = null;
            if (response.TotalResults > 0)
            {
                IOrderDetailService odService = new OrderDetailService();
                foreach (Trade t in response.Trades)
                {
                    OrderDetail od = odService.GetByTid(t.Tid.ToString());
                    if (od == null)
                    {
                        if (list == null)
                        {
                            list = new List<Trade>();
                        }
                        list.Add(t);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Migrate the trades information and refund information.
        /// </summary>
        /// <param name="tradeList"></param>
        /// <returns></returns>
        public List<OrderDetail> MigrateTradesToOrders(List<Trade> tradeList)
        {
            Taobao.TaobaoRefundModel refundModel = new TaobaoRefundModel();

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (Top.Api.Domain.Trade trade in tradeList)
            {
                foreach (Top.Api.Domain.Order order in trade.Orders)
                {
                    OrderDetail od = new OrderDetail();
                    od.BuyerNick = trade.BuyerNick;
                    od.AlipayNo = trade.BuyerAlipayNo;
                    od.TaobaoProductID = trade.NumIid.ToString();
                    od.TotalFee = Convert.ToDecimal(trade.TotalFee);
                    if (!string.IsNullOrEmpty(trade.PayTime))
                    {
                        od.PayTime = Convert.ToDateTime(trade.PayTime);
                    }

                    if (order.RefundStatus.Equals(Constants.RefundStatus.NO_REFUND.ToString()))
                    {                
                        od.ShippingType = trade.ShippingType;
                        od.Tid = trade.Tid.ToString();
                        od.Oid = order.Oid.ToString();                        
                        od.Address = trade.ReceiverState + " " + trade.ReceiverCity + " " + trade.ReceiverDistrict + " " + trade.ReceiverAddress;
                    }
                    else 
                    {
                        Refund refund = refundModel.GetRefundByRefundID(order.RefundId);
                        if (refund != null)
                        {
                            od.ShippingType = refund.ShippingType;
                            od.Tid = refund.Tid.ToString();
                            od.Oid = refund.Oid.ToString();
                            od.TotalFee = Convert.ToDecimal(refund.TotalFee);
                            if (!string.IsNullOrEmpty(refund.Created))
                            {
                                od.ApplyRefundTime = Convert.ToDateTime(refund.Created);
                            }
                            if (!string.IsNullOrEmpty(refund.Modified))
                            {
                                od.UpdateRefundTime = Convert.ToDateTime(refund.Modified);
                            }
                            od.OrderStatus = ParseRefundOrderStatus(refund.OrderStatus);
                            od.RefundStatus = ParsRefundStatus(refund.Status);
                            od.HasGoodReturn = refund.HasGoodReturn.ToString();
                            if (!string.IsNullOrEmpty(refund.GoodReturnTime))
                            {
                                od.GoodReturnTime = Convert.ToDateTime(refund.GoodReturnTime);
                            }
                            od.RefundFee = Convert.ToDecimal(refund.RefundFee);
                            od.Payment = Convert.ToDecimal(refund.Payment);
                            od.Reason = refund.Reason;
                            od.Desc = refund.Desc;
                        }
                    }
                  
                    orderDetails.Add(od);
                }
            }

            return orderDetails;
        }

        public List<OrderDetail> StoreOrdersToDb(List<OrderDetail> srcList, string name)
        {   
            IOrderDetailService odService = new OrderDetailService();
            IGroupByItemService gbiService = new GroupByItemService();
            IGroupByGroupService gbgService = new GroupByGroupService();
            TaobaoItemModel itemModel = new TaobaoItemModel();
            List<GroupByItem> gbiList = new List<GroupByItem>();

            foreach (OrderDetail od in srcList)
            {
                od.CreateBy = name;
                od.CreateTime = DateTime.Now;
                od.LastModifyBy = name;
                od.LastModifyTime = DateTime.Now;
                GroupByItem gbi = gbiService.GetByToaobaoProductId(od.TaobaoProductID);
                //if (gbi == null) {
                //    gbi = gbiList.SingleOrDefault<GroupByItem>(p => p.TaobaoProductID.Equals(od.TaobaoProductID));
                //}
                if (gbi != null)
                {
                    od.GroupByItemID = gbi.GroupByItemID;
                }
                else {                    
                    //gbi = TaobaoItemModel.BuildGroupByItem(gbgService, TaobaoItemModel.GetItemByID(Convert.ToInt64(od.TaobaoProductID)), name);
                    //gbiList.Add(gbi);
                    //od.GroupByItem = gbi;
                }
            }
            int count = odService.FillOrder(srcList);
            return count > 0 ? srcList : null;
        }

        private int ParseRefundOrderStatus(string status)
        {
            int rst = 0;
            switch (status)
            {
                case "TRADE_NO_CREATE_PAY":
                    rst = 1;
                    break;
                case "WAIT_BUYER_PAY":
                    rst = 2;
                    break;
                case "WAIT_SELLER_SEND_GOODS":
                    rst = 3;
                    break;
                case "WAIT_BUYER_CONFIRM_GOODS":
                    rst = 4;
                    break;
                case "TRADE_BUYER_SIGNED":
                    rst = 5;
                    break;
                case "TRADE_FINISHED":
                    rst = 6;
                    break;
                case "TRADE_CLOSED":
                    rst = 7;
                    break;
                case "TRADE_CLOSED_BY_TAOBAO":
                    rst = 8;
                    break;
                case "ALL_WAIT_PAY":
                    rst = 9;
                    break;
                case "ALL_CLOSED":
                    rst = 10;
                    break;               
                default:
                    break;
            }
            return rst;
        }

        private int ParsRefundStatus(string status)
        {
            int rst = 0;
            switch (status)
            {
                case "WAIT_SELLER_AGREE":
                    rst = 1;
                    break;
                case "WAIT_BUYER_RETURN_GOODS":
                    rst = 2;
                    break;
                case "WAIT_SELLER_CONFIRM_GOODS":
                    rst = 3;
                    break;
                case "SELLER_REFUSE_BUYER":
                    rst = 4;
                    break;
                case "CLOSED":
                    rst = 5;
                    break;
                case "SUCCESS":
                    rst = 6;
                    break;
                default:
                    break;
            }
            return rst;
        }

    }
}
