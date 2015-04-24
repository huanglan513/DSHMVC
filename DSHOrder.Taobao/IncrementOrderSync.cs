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
    public class IncrementOrderSync : OrderSync,IJob
    {
        private const long PAGE_SIZE = 50;   

        public IncrementOrderSync()
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
            this.userName = name;
           
            try
            {
                Log.Info("IncrementOrderSync is Beginning.");
                lastSyncTime = GetLastSyncTime(SYS_LAST_SYNC_TIME_KEY);
                if (!lastSyncTime.HasValue)
                {
                    //TODO to throw a the lastsynctime not inited exception.
                    Log.Error("IncrementOrderSync Error: There is no last sync time.");
                    return false;
                }

                DateTime end = GetTaobaoDateTime();
                DateTime start = lastSyncTime.Value;

                bool isSuccess = GetIncrementSoldTradesByPeriod(start, end);
                if (!isSuccess)
                {
                    Log.Error("IncrementOrderSync Error: Sync fail.");
                    return false;
                }

                odService.FillOrder(orderDetails);
                SetLastSyncTime(SYS_LAST_SYNC_TIME_KEY, end);

                Log.Info("IncrementOrderSync Success.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ":" + ex.StackTrace);
                //throw;
                return false;
            }
           
            return true;
        }

        private bool GetIncrementSoldTradesByPeriod(DateTime start, DateTime end)
        {
            TradesSoldIncrementGetRequest  request = new TradesSoldIncrementGetRequest ();
            request.Fields = "tid";
            request.StartModified = start;
            request.EndModified = end;
            request.PageNo = 1;
            request.PageSize = PAGE_SIZE;

            TradesSoldIncrementGetResponse response = client.Execute<TradesSoldIncrementGetResponse>(request, TaobaoConfig.TestSesionKey);
            if (response.IsError)
            {
                Log.Error(response.ErrMsg + "." + response.SubErrMsg);
                //TODO throw new NotImplementedException();
                return false;
            }

            Log.Info("IncrementOrder total number:" + response.TotalResults.ToString());
            long pageCount = GetPageCount(response.TotalResults, request.PageSize.GetValueOrDefault());
            for (; pageCount > 0; pageCount--)
            {
                Log.Debug("IncrementOrder page count number:" + pageCount.ToString());
                request.PageNo = pageCount;
                request.UseHasNext = true;
                response = client.Execute<TradesSoldIncrementGetResponse>(request, TaobaoConfig.TestSesionKey);
                if (response.IsError)
                {
                    Log.Error(response.ErrMsg + "." + response.SubErrMsg);
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
            }
            return true;
        }
    }
}
