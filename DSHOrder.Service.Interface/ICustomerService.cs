using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface ICustomerService
    {
        IList<Customer> GetCustomersByCity(int cityID = 0, int districtID = 0);

        Customer Add(Customer entity);

        IList<CustomerExtend> GetCustomersByNameAndCity(Pagination paging, string customerName, int cityID = 0, int viewType = 0, int isCertifiedSearch=100);

        int Update(IList<Customer> list);

        Customer GetCustomerByID(int customerID);

        Customer Update(Customer customer);

        int MarkDeleted(int[] ids);

        List<CooperationRecord> GetCooperationHistory(int customerID,Pagination paging);

        bool ExistCustomer(string customerName, int customerId = 0);

        List<Customer> SearchCustomer(string CustomerName, int CityID, int IsCertified, int subIndustryId);
    }
}
