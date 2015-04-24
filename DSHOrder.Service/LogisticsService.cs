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
    public class LogisticsService:ILogisticsService
    {
        LogisticsRepository repository = null;
        public LogisticsService()
        {
            repository = new LogisticsRepository();
        }

         public int Add(List<LogisticsInfo> logisticsList)
         {
             foreach (LogisticsInfo item in logisticsList)
             {
                 repository.UpdateEntity(item);
             }
             return repository.SaveChanges();
         }

         public IList<LogisticsInfo> GetLogisticsInfo()
         {
             return repository.GetAll<LogisticsInfo>();
         }

         public IList<LogisticsInfo> GetLogisticsByCondition(Pagination paging, string serialNum, string orderId, DateTime? getGoodsDateFrom,
             DateTime? getGoodsDateTo, DateTime? arrivaStopDateFrom, DateTime? arrivaStopDateTo, string status,
             string customerName, string telPhone, string carrier, string addr)
         {
             return repository.GetLogisticsByCondition(paging,serialNum, orderId, getGoodsDateFrom, getGoodsDateTo,
                 arrivaStopDateFrom,arrivaStopDateTo,status,customerName, telPhone, carrier, addr);
         }
    }
}
