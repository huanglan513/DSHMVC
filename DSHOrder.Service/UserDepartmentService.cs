using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class UserDepartmentService:Interface.IUserDepartmentService
    {
        IRepository roleDepartmentRepo = null;

        public UserDepartmentService()
        {
            roleDepartmentRepo = new UserDepartmentRepository();
        }

        public IList<Entity.UserDepartment> GetAllUserDepartments()
        {
            return roleDepartmentRepo.GetAll<Entity.UserDepartment>();
        }
    

    }
}
