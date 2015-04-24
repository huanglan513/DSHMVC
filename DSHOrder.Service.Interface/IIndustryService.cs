using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IIndustryService
    {
        IList<Industry> GetAllIndustries();

        Industry GetByID(int id);

        IList<SubIndustry> GetPhoneLogisticSubIndustryIDList();

        bool CheckIndustryForSale(int industryID, int UserID);
    }
}
