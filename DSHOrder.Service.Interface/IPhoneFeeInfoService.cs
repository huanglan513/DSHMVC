using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface IPhoneFeeInfoService
    {
        int Add(List<PhoneFeeInfo> phoneList);

        IList<PhoneFeeInfo> GetPhoneFeeInfo();

        IList<PhoneFeeInfo> GetPhoneFeeInfoByCondition(Pagination paging, string orderId, string buyerName,
             string addr, string telPhone, string result, DateTime? orderDateFrom, DateTime? orderDateTo,
             DateTime? rechargeDateFrom, DateTime? rechargeDateTo);
    }
}
