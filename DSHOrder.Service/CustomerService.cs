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
    public class CustomerService : ICustomerService
    {
        CustomerRepository repository = null;
        public CustomerService()
        {
            repository = new CustomerRepository();
        }
        #region ICustomerService 成员

        public IList<Customer> GetCustomersByCity(int cityID = 0, int districtID = 0)
        {
            var query = repository.CreateQuery<Customer>().Where(p => p.DeleteInd == 0);
            if (cityID == 0 && districtID == 0)
            {
                return query.ToList();
            }
            else if (cityID == 0)
            {
                return query.Where(p => p.DistrictID == districtID).ToList();
            }
            else if (districtID == 0)
            {
                return query.Where(p => p.CityID == cityID).ToList();
            }
            else
            {
                return query.Where(p => p.CityID == cityID && p.DistrictID == districtID).ToList();
            }
        }

        public Customer Add(Customer entity)
        {
            return repository.Add<Customer>(entity);
        }

        public IList<CustomerExtend> GetCustomersByNameAndCity(Pagination paging, string customerName, int cityID = 0, int viewType = 0, int isCertifiedSearch = 100)
        {
            return repository.GetCustomersByNameAndCity(paging, customerName, cityID, viewType, isCertifiedSearch);
        }

        public int Update(IList<Customer> list)
        {
            foreach (Customer item in list)
            {
                repository.Update<Customer>(item, true);
            }
            return repository.SaveChanges();
        }

        public Customer GetCustomerByID(int customerID)
        {
            return repository.GetBy<Customer>(p => p.CustomerID == customerID);
        }

        public Customer Update(Customer customer)
        {
            return repository.Update<Customer>(customer);
        }

        public int MarkDeleted(int[] ids)
        {
            return repository.MarkDeleted(ids);
        }

        public List<CooperationRecord> GetCooperationHistory(int customerID, Pagination paging)
        {
            return repository.GetCooperationHistory(customerID, paging);
        }

        public bool ExistCustomer(string customerName,int customerId=0)
        {
            Customer entity=null;
            if (customerId==0)
            {
                entity = repository.GetBy<Customer>(p => p.CustomerName == customerName);
            }
            else
            {
                entity = repository.GetBy<Customer>(p => p.CustomerName == customerName && p.CustomerID!=customerId);
            }
            if (entity == null)
                return false;
            else
                return true;
        }
        #endregion


        #region ICustomerService 成员


        public List<Customer> SearchCustomer(string CustomerName, int CityID, int IsCertified, int subIndustryId)
        {
            return repository.SearchCustomer(CustomerName, CityID, IsCertified, subIndustryId);
        }

        #endregion
    }
}
