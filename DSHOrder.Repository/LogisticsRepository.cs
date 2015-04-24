using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using System.Data;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class LogisticsRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityLogisticsInfo; }
        }

        public LogisticsInfo UpdateEntity(LogisticsInfo entity)
        {
            LogisticsInfo item = this.CreateQuery<LogisticsInfo>().FirstOrDefault(p => p.SerialNum == entity.SerialNum);
            if (item == null)
            {
                this.Add(entity,true);
            }
            else
            {
                item.SerialNum = entity.SerialNum;
                item.OrderID = entity.OrderID;
                item.Project = entity.Project;
                item.GetGoodsDate = entity.GetGoodsDate;
                item.SourceStop = entity.SourceStop;
                item.DestStop = entity.DestStop;
                item.OperateDate = entity.OperateDate;
                item.ArriveStopDate = entity.ArriveStopDate;
                item.Address = entity.Address;
                item.TelPhone = entity.TelPhone;
                item.CustomerName = entity.CustomerName;
                item.GoodsName = entity.GoodsName;
                item.SalesNum = entity.SalesNum;
                item.Carrier = entity.Carrier;
                item.Remark = entity.Remark;
                item.CreateBy = entity.CreateBy;
                item.CreateDate = entity.CreateDate;
                this.Update(item, true);
            }

            return entity;
        }

        public IList<LogisticsInfo> GetLogisticsByCondition(Pagination paging, string serialNum, string orderId, DateTime? getGoodsDateFrom,
             DateTime? getGoodsDateTo, DateTime? arrivaStopDateFrom, DateTime? arrivaStopDateTo, string status,
             string customerName, string telPhone, string carrier, string addr)
        {
            var query = from q in this.CreateQuery<LogisticsInfo>() select q;

            if (!string.IsNullOrEmpty(serialNum) && !string.IsNullOrEmpty(serialNum.Trim()))
            {
                query = query.Where(q => q.SerialNum.Contains(serialNum.Trim()));
            }
            if (!string.IsNullOrEmpty(orderId) && !string.IsNullOrEmpty(orderId.Trim()))
            {
                query = query.Where(q => q.OrderID.Contains(orderId.Trim()));
            }
            if (getGoodsDateFrom.HasValue)
            {
                query = query.Where(q => q.GetGoodsDate >= getGoodsDateFrom.Value);
            }
            if (getGoodsDateTo.HasValue)
            {
                getGoodsDateTo = getGoodsDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(q => q.GetGoodsDate <= getGoodsDateTo.Value);
            }
            if (arrivaStopDateFrom.HasValue)
            {
                query = query.Where(q => q.ArriveStopDate >= arrivaStopDateFrom.Value);
            }
            if (arrivaStopDateTo.HasValue)
            {
                arrivaStopDateTo = arrivaStopDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(q => q.ArriveStopDate <= arrivaStopDateTo.Value);
            }
            if (!string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(status.Trim()))
            {
                query = query.Where(q => q.Status.Contains(status.Trim()));
            }
            if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(customerName.Trim()))
            {
                query = query.Where(q => q.CustomerName.Contains(customerName.Trim()));
            }
            if (!string.IsNullOrEmpty(telPhone) && !string.IsNullOrEmpty(telPhone.Trim()))
            {
                query = query.Where(q => q.TelPhone.Contains(telPhone.Trim()));
            }
            if (!string.IsNullOrEmpty(carrier) && !string.IsNullOrEmpty(carrier.Trim()))
            {
                query = query.Where(q => q.Carrier.Contains(carrier.Trim()));
            }
            if (!string.IsNullOrEmpty(addr) && !string.IsNullOrEmpty(addr.Trim()))
            {
                query = query.Where(q => q.Address.Contains(addr.Trim()));
            }
            query = query.OrderByDescending(q => q.CreateDate);
            return paging.ParseQueryPaging<LogisticsInfo>(query).ToList();          
        }
    }
}
