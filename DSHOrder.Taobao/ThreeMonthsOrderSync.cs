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
using Quartz;

namespace DSHOrder.Taobao
{
    public class ThreeMonthsOrderSync : OrderSync,IJob
    {
        // 切隔时间（公式为：24*每页记录数[推荐50]/日均订单量），如日均订单量为100的店铺，可按每24*50/100=12小时切割一段
        private const int HOUR_SECTION = 24;
        private const long PAGE_SIZE = 50;    

        public ThreeMonthsOrderSync()
        {
         
        }

        public void Execute(IJobExecutionContext context)
        {
            InnerExecute(context.JobDetail.JobDataMap.GetString("userName"));
        }

        /// <summary>
        /// The entry of the job.
        /// </summary>
        /// <returns></returns>
        public bool InnerExecute(string name)
        {
            try
            {
                bool isSuccess = true;
                Log.Info("ThreeMonthsOrderSync is Beginning.");

                this.userName = name;

                DateTime end = GetTaobaoDateTime();
                DateTime start = end.AddMonths(-1);

                IList<DateTime[]> timeList = SplitTimeByHours(start, end, HOUR_SECTION);

                foreach (DateTime[] dts in timeList)
                {
                    isSuccess = isSuccess && GetSoldTradesByPeriod(dts[0], dts[1]);
                    if (!isSuccess) return false;
                }

                odService.FillOrder(orderDetails);
                SetLastSyncTime(SYS_LAST_SYNC_TIME_KEY, end);

                Log.Info("ThreeMonthsOrderSync is successful.");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ":" + ex.StackTrace);
                return false;
                //throw;
            }
        }

        private bool GetSoldTradesByPeriod(DateTime start, DateTime end)
        {
            TradesSoldGetRequest request = new TradesSoldGetRequest();
            request.Fields = "tid";
            request.StartCreated = start;
            request.EndCreated = end;
            request.PageNo = 1;
            request.PageSize = PAGE_SIZE;

            long pageCount = 0;
            TradesSoldGetResponse response = null;

            do
            {
                response = client.Execute<TradesSoldGetResponse>(request, TaobaoConfig.TestSesionKey);
                if (response.IsError)
                {
                    //TODO throw new NotImplementedException();
                    return false;
                }

                //Get the details about the trades by tid.
                foreach (Trade t in response.Trades)
                {
                    Trade trade = GetTradeFullInfo(t.Tid);
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

                            MapDSHOrderDetailForNew(trade, order, refund, rate);
	                    }
                    }
                }

                request.PageNo++;
                pageCount = GetPageCount(response.TotalResults, request.PageSize.GetValueOrDefault());
            }
            while (request.PageNo <= pageCount);

            return true;
        }
    }
}
