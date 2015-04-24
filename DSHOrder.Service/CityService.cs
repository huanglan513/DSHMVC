using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class CityService:ICityService
    {
        CityRepository repository = null;
        public CityService()
        {
            repository = new CityRepository();
        }
        #region ICityService 成员

        public IList<City> GetAllCities()
        {
            return repository.GetAll<City>();
        }

        #endregion
    }
}
