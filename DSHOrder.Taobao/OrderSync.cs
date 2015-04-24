using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Domain;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Service;

namespace DSHOrder.Taobao
{
    public class OrderSync : BaseSync
    {
        private const string TOP_REQ_TRADE_FIELDS = "buyer_nick,shipping_type,title,tid,buyer_alipay_no,total_fee,receiver_state, receiver_city, receiver_district, receiver_address, num_iid, buyer_message, trade_from,orders";
        private const string TOP_REQ_REFUND_FIELDS = "created, modified, has_good_return,refund_id, refund_fee, payment, reason, desc, good_return_time, refund_remind_timeout.timeout";
        protected const string SYS_LAST_SYNC_TIME_KEY = "lastOrderSyncTime";

        protected IOrderDetailService odService;
        protected List<OrderDetail> orderDetails;
        protected ISystemParamService spService;
        protected string userName;
        protected DateTime? lastSyncTime;

        public OrderSync()
        {
            odService = new OrderDetailService();
            spService = new SystemParamService();
            orderDetails = new List<OrderDetail>();
            userName = string.Empty;
        }

        private string GetFullAddress(Trade trade)
        {
            return new StringBuilder().AppendFormat("{0} {1} {2} {3}",
                trade.ReceiverState, trade.ReceiverCity,
                trade.ReceiverDistrict, trade.ReceiverAddress).ToString();
        }

        protected TradeRate GetTradeRate(long oid)
        {
            TraderatesGetRequest request = new TraderatesGetRequest();
            request.Fields = "tid,oid,role,rated_nick,nick,result,created,item_title,item_price,content,reply";
            request.RateType = "get";
            request.Role = "buyer";
            TraderatesGetResponse response = client.Execute<TraderatesGetResponse>(request, TaobaoConfig.TestSesionKey);
            if (response.IsError)
            {
                Log.Error(response.ErrMsg + "." + response.SubErrMsg);
                //TODO throw new NotImplementedException();
                return null;
            }

            if (response.TotalResults > 0)
            {
                return response.TradeRates[0];
            }
            return null;
        }

        protected Refund GetRefund(long refundId)
        {
            RefundGetRequest request = new RefundGetRequest();
            request.Fields = TOP_REQ_REFUND_FIELDS;
            request.RefundId = refundId;

            RefundGetResponse response = client.Execute<RefundGetResponse>(request, TaobaoConfig.TestSesionKey);
            if (response.IsError)
            {
                Log.Error(response.ErrMsg + "." + response.SubErrMsg);
                //TODO throw new NotImplementedException();
                return null;
            }
            return response.Refund;
        }

        protected Trade GetTradeFullInfo(long tid)
        {
            TradeFullinfoGetRequest request = new TradeFullinfoGetRequest();

            request.Fields = TOP_REQ_TRADE_FIELDS;
            request.Tid = tid;
            TradeFullinfoGetResponse response = client.Execute<TradeFullinfoGetResponse>(request, TaobaoConfig.TestSesionKey);
            if (response.IsError)
            {
                Log.Error(response.ErrMsg + "." + response.SubErrMsg);
                //TODO throw new NotImplementedException();
                return null;
            }
            return response.Trade;
        }

        protected void MapDSHOrderDetailForNew(Trade trade, Order order, Refund refund, TradeRate tradeRate)
        {
            OrderDetail od = new OrderDetail();

            MapDSHOrderDetailForUpdate(od, trade, order, refund, tradeRate);

            od.CreateBy = userName;
            od.CreateTime = DateTime.Now;          
            od.DeleteInd = default(int);

            orderDetails.Add(od);
        }

        protected void MapDSHOrderDetailForUpdate(OrderDetail od, Trade trade, Order order, Refund refund, TradeRate rate)
        {
            //od.OrderDetailID = generator;      //this is auto-generate
            //od.GroupByItemID = pending;     //this is mapped by another job.
            //od.LockBy = null;                 //here is no use.
            //od.PayTime = null;                 //here is no use.
            //od.DealTime = null;                 //here is no use.
            //od.Remark = null;                 //here is no use.
            //od.LockTime = null;                 //here is no use.
            //od.DealBy = null;                 //here is no use.
            //od.OfflineDealStatus = null;        //here is no use.
            //od.IssueType = null;               //here is no use.
            //od.BuyerID = null;                //it is abandoned field.

            od.BuyerNick = trade.BuyerNick;
            od.AlipayNo = trade.BuyerAlipayNo;
            od.TaobaoProductID = trade.NumIid.ToString();

            //TODO to check the really stand for?
            od.TotalFee = Convert.ToDecimal(order.TotalFee);    //Watch Out!!应付金额（商品价格 * 商品数量 + 手工调整金额 - 订单优惠金额）
            od.ShippingType = trade.ShippingType;
            od.Tid = trade.Tid.ToString();
            od.Oid = order.Oid.ToString();
            od.TradeFrom = trade.TradeFrom;
            od.AlipayNo = trade.BuyerAlipayNo;
            od.Address = GetFullAddress(trade);
            od.BuyerMessage = trade.BuyerMessage;
            od.OrderStatus = ParseOrderStatus(order.Status);
            od.RefundStatus = ParseRefundStatus(order.RefundStatus);
            od.ItemTitle = order.Title;

            if (refund != null)
            {
                od.RefundID = refund.RefundId.ToString();
                od.ApplyRefundTime = Convert.ToDateTime(refund.Created);
                if (!string.IsNullOrEmpty(refund.Modified))
                {
                    od.UpdateRefundTime = Convert.ToDateTime(refund.Modified);
                }
                od.HasGoodReturn = refund.HasGoodReturn.ToString();
                od.RefundFee = Convert.ToDecimal(refund.RefundFee);
                od.Payment = Convert.ToDecimal(refund.Payment);
                od.Reason = refund.Reason;
                od.Desc = refund.Desc;
                if (refund.HasGoodReturn && !string.IsNullOrEmpty(refund.GoodReturnTime))
                {
                    od.GoodReturnTime = Convert.ToDateTime(refund.GoodReturnTime);
                }
                if (refund.RefundRemindTimeout != null && !string.IsNullOrEmpty(refund.RefundRemindTimeout.Timeout))
                {   
                    od.RefundTimeout = Convert.ToDateTime(refund.RefundRemindTimeout.Timeout);
                }
            }

            if (rate != null)
            {
                od.TradeRate = ParseTradeRateStatus(rate.Result);
            }

            od.LastModifyBy = userName;
            od.LastModifyTime = DateTime.Now;
        }

        private int ParseTradeRateStatus(string rate)
        {
            int rst = 0;
            switch (rate)
            {
                case "good":
                    rst = 1;
                    break;
                case "neutral":
                    rst = 0;
                    break;
                case "bad":
                    rst = -1;
                    break;
            }
            return rst;
        }

        private int ParseOrderStatus(string status)
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

        private int ParseRefundStatus(string status)
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
                case "NO_REFUND":
                    rst = 7;
                    break;
                default:
                    break;
            }
            return rst;
        }

        protected DateTime? GetLastSyncTime(string sysName)
        {
            SystemParam sysParam = spService.GetSystemRecordByName(sysName);
            if (sysParam != null && !string.IsNullOrEmpty(sysParam.SystemValue))
            {
                return Convert.ToDateTime(sysParam.SystemValue);
            }
            return null;
        }

        protected void SetLastSyncTime(string sysName, DateTime end)
        {
            IList<SystemParam> sysParams = spService.GetAllSystemData();
            SystemParam sp = sysParams.First<SystemParam>(p => p.SystemName.Equals(sysName));
            if (sp == null)
            {
                sp = new SystemParam();
                sp.SystemID = sysParams.Count + 1;
                sp.SystemName = sysName;
            }
            sp.SystemValue = end.ToString();
            spService.UpdateSystemValue(sp);
        }
    }
}
