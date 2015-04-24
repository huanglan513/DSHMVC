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
    public class PhoneFeeInfoService:IPhoneFeeInfoService
    {
        PhoneFeeInfoRepository repository = null;
         public PhoneFeeInfoService()
        {
            repository = new PhoneFeeInfoRepository();
        }

         public int Add(List<PhoneFeeInfo> phoneList)
         {
             foreach (PhoneFeeInfo item in phoneList)
             {
                 repository.UpdateEntity(item);
             }
             return repository.SaveChanges();
         }

         public IList<PhoneFeeInfo> GetPhoneFeeInfo()
         {
             return repository.GetAll<PhoneFeeInfo>();
         }

         public IList<PhoneFeeInfo> GetPhoneFeeInfoByCondition(Pagination paging, string orderId, string buyerName,
             string addr, string telPhone, string result, DateTime? orderDateFrom, DateTime? orderDateTo, 
             DateTime? rechargeDateFrom, DateTime? rechargeDateTo)
         {
             return repository.GetPhoneFeeInfoByCondition(paging, orderId, buyerName, addr, telPhone, result, orderDateFrom, orderDateTo,
                 rechargeDateFrom, rechargeDateTo);
         }
    }
}
