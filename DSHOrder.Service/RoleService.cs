using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class RoleService:IRoleService
    {
        RoleRepository repository = null;

        public RoleService()
        {
            repository = new RoleRepository();
        }

        public IList<Entity.Role> GetAllRoles()
        {
            return repository.GetAll<Entity.Role>();
        }     
    }
}
