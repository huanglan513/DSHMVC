using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;

namespace DSHOrder.Service
{
    public class DepartmentService:IDepartmentService
    {
        DepartmentRepository repository = null;
        public DepartmentService()
        {
            repository = new DepartmentRepository();
        }

        #region IDepartmentService 成员

        public IList<Department> GetAllDepartments()
        {
            return repository.GetAll<Department>();
        }

        #endregion
    }
}
