using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using System.Data;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class PhoneFeeInfoRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityPhoneFeeInfo; }
        }

        public PhoneFeeInfo UpdateEntity(PhoneFeeInfo entity)
        {
            PhoneFeeInfo item = this.CreateQuery<PhoneFeeInfo>().FirstOrDefault(p => p.OrderID == entity.OrderID);
            if (item == null)
            {
                this.Add(entity,true);
            }
            else
            {
                item.OrderID = entity.OrderID;
                item.BuyerName = entity.BuyerName;
                item.Payment = entity.Payment;
                item.GetGoodsAddr = entity.GetGoodsAddr;
                item.PhoneNumber = entity.PhoneNumber;
                item.SalesNum = entity.SalesNum;
                item.Result = entity.Result;
                item.OrderDate = entity.OrderDate;
                item.RechargeDate = entity.RechargeDate;
                item.Remark = entity.Remark;
                item.CreateBy = entity.CreateBy;
                item.CreateTime = entity.CreateTime;
                this.Update(item, true);
            }

            return entity;
        }

        public IList<PhoneFeeInfo> GetPhoneFeeInfoByCondition(Pagination paging, string orderId, string buyerName,
            string addr, string telPhone, string result, DateTime? orderDateFrom, DateTime? orderDateTo,
            DateTime? rechargeDateFrom, DateTime? rechargeDateTo)
        {
            var query = from q in this.CreateQuery<PhoneFeeInfo>() select q;

            if (!string.IsNullOrEmpty(orderId) && !string.IsNullOrEmpty(orderId.Trim()))
            {
                query = query.Where(q => q.OrderID.Contains(orderId.Trim()));
            }
            if (!string.IsNullOrEmpty(buyerName) && !string.IsNullOrEmpty(buyerName.Trim()))
            {
                query = query.Where(q => q.BuyerName.Contains(buyerName.Trim()));
            }
            if (!string.IsNullOrEmpty(addr) && !string.IsNullOrEmpty(addr.Trim()))
            {
                query = query.Where(q => q.GetGoodsAddr.Contains(addr.Trim()));
            }
            if (!string.IsNullOrEmpty(telPhone) && !string.IsNullOrEmpty(telPhone.Trim()))
            {
                query = query.Where(q => q.PhoneNumber.Contains(telPhone.Trim()));
            }
            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(result.Trim()))
            {
                query = query.Where(q => q.Result.Contains(result.Trim()));
            }
            if (orderDateFrom.HasValue)
            {
                query = query.Where(q => q.OrderDate >= orderDateFrom.Value);
            }
            if (orderDateTo.HasValue)
            {
                orderDateTo = orderDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(q => q.OrderDate <= orderDateTo.Value);
            }
            if (rechargeDateFrom.HasValue)
            {
                query = query.Where(q => q.RechargeDate >= rechargeDateFrom.Value);
            }
            if (rechargeDateTo.HasValue)
            {
                rechargeDateTo = rechargeDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(q => q.RechargeDate <= rechargeDateTo.Value);
            }
            query = query.OrderByDescending(q => q.CreateTime);
            return paging.ParseQueryPaging<PhoneFeeInfo>(query).ToList();          
        }
    }
}
