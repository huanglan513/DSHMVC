using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class SubIndustryService : ISubIndustryService
    {
        SubIndustryRepository repository = null;
        public SubIndustryService()
        {
            repository = new SubIndustryRepository();
        }

        #region ISubIndustryService 成员

        public IList<SubIndustry> GetAllSubIndustries()
        {
            return repository.GetAll<SubIndustry>();
        }

        public IList<SubIndustry> GetSubIndustriesByIndustryID(int industryID)
        {
            return repository.CreateQuery<SubIndustry>().Where(p => p.IndustryID == industryID).ToList();
        }

        public IList<SubIndustry> GetSubIndustriesBySubIndustryID(int subIndustryID)
        {
            return repository.GetSubIndustriesBySubIndustryID(subIndustryID);
        }
        #endregion

        #region ISubIndustryService 成员


        public SubIndustry GetByID(int id)
        {
            return repository.GetByID(id);
        }

        #endregion
    }
}
