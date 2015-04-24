using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{

    public interface ISubIndustryService
    {
        IList<SubIndustry> GetAllSubIndustries();

        IList<SubIndustry> GetSubIndustriesByIndustryID(int industryID);

        IList<SubIndustry> GetSubIndustriesBySubIndustryID(int subIndustryID);

        SubIndustry GetByID(int id);

    }
}
