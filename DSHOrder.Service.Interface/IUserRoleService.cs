using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Service.Interface
{
    public interface IUserRoleService
    {
        IList<Entity.UserRole> GetAllUserRoles();
        Entity.UserRole GetRoleByUserID(int userID);

        bool UserInRole(string strUser, string strRole);

        Entity.UserRole GetRoleByUserName(string strUser);
    }
}
