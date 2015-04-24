using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Repository;
using DSHOrder.Service.Interface;

namespace DSHOrder.Service
{
    public class PrivilegeService : IPrivilegeService
    {
        PrivilegeRepository repos = null;

        public PrivilegeService()
        {
            repos = new PrivilegeRepository();
        }

        public bool ValidatePrivilige(string userName, string routeUrl)
        {
            Entity.User user = repos.CreateQuery<Entity.User>().SingleOrDefault<Entity.User>(p => p.UserName.Equals(userName));
            if (user == null || user.UserRole.Count() <= 0) return false;
            bool rst = false;
            foreach (var userRole in user.UserRole)
            {
                rst = repos.ExistPrivilege(userRole.RoleID.Value, routeUrl);
                if (rst) return true;
            }
            return rst;
        }
    }
}
