using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Repository;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;

namespace DSHOrder.Service
{
    public class IndustryService : IIndustryService
    {
        IndustryRepository repository = null;
        public IndustryService()
        {
            repository = new IndustryRepository();
        }

        #region IIndustryService 成员

        public IList<Industry> GetAllIndustries()
        {
            return repository.GetAll<Industry>();
        }

        public IList<SubIndustry> GetPhoneLogisticSubIndustryIDList()
        {
            return repository.GetPhoneLogisticSubIndustryIDList();
        }
        #endregion

        #region IIndustryService 成员


        public Industry GetByID(int id)
        {
            var rows = repository.GetAllBy<Industry>(m => m.IndustryID == id);
            return rows.Single();
        }

        #endregion

        #region IIndustryService 成员


        public bool CheckIndustryForSale(int industryID, int UserID)
        {
            return repository.CheckIndustryForSale(industryID, UserID);
        }

        #endregion
    }
}
