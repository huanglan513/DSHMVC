using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using DSHOrder.Entity;

namespace DSHOrder.Taobao
{
    public class OneOrderSync : OrderSync
    {
        private long tid;
        private int orderDetailID;
        private OrderDetail od;

        public OneOrderSync(string name, long tid, int orderDetailID)
        {
            this.userName = name;
            this.tid = tid;
            this.orderDetailID = orderDetailID;
        }

        /// <summary>
        /// The entry of the job.
        /// </summary>
        /// <returns></returns>
        public bool InnerExecute()
        {
            try
            {
                bool isSuccess = GetOneSoldTradesByOrderID(tid);
                if (!isSuccess) return false;

                if (od != null)
                {
                    odService.Update(od);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ":" + ex.StackTrace);
                //throw;
            }
         
            return true;
        }

        private bool GetOneSoldTradesByOrderID(long tid)
        {   
            Trade trade = GetTradeFullInfo(tid);
            if (trade != null)
            {
                foreach (Order order in trade.Orders)
                {
                    Refund refund = null;
                    if (!order.RefundStatus.Equals(Constants.RefundStatus.NO_REFUND.ToString()))
                    {
                        refund = GetRefund(order.RefundId);
                    }
                    TradeRate rate = GetTradeRate(order.Oid);

                    od = odService.GetById(orderDetailID);
                    MapDSHOrderDetailForUpdate(od, trade, order, refund, rate);
                }
            }
            else {
                //TODO throw new NotImplementedException();
                return false;
            }
            return true;
        }
    }
}
