using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class ApproveRecordGroupByService : IApproveRecordGroupByService
    {
        IRepository repository = null;
        public ApproveRecordGroupByService()
        {
            repository = new ApproveRecordGroupByRepository();
        }

        #region IApproveRecordGroupByService 成员

        public ApproveRecordGroupBy Add(ApproveRecordGroupBy entity)
        {
            return repository.Add(entity);
        }

        #endregion
    }
}
