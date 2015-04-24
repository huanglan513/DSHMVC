using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IDepartmentService
    {
        IList<Department> GetAllDepartments();
    }
}
