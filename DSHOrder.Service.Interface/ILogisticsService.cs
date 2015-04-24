using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface ILogisticsService
    {
        int Add(List<LogisticsInfo> logisticsList);

        IList<LogisticsInfo> GetLogisticsInfo();

        IList<LogisticsInfo> GetLogisticsByCondition(Pagination paging, string serialNum, string orderId, DateTime? getGoodsDateFrom,
             DateTime? getGoodsDateTo, DateTime? arrivaStopDateFrom, DateTime? arrivaStopDateTo, string status,
             string customerName, string telPhone, string carrier, string addr);
    }
}
