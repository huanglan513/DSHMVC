using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class DistrictService:IDistrictService
    {
         DistrictRepository repository = null;
         public DistrictService()
        {
            repository = new DistrictRepository();
        }

        #region IDistrictService 成员

        public IList<District> GetDistrictByCity(int cityID)
        {
            return repository.CreateQuery<District>().Where(p => p.CityID == cityID).ToList();
        }

        #endregion
    }
}
