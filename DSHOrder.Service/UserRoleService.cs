using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class UserRoleService:Interface.IUserRoleService
    {
        UserRoleRepository userRoleRepo = null;
        
        public UserRoleService()
        {
            userRoleRepo = new UserRoleRepository();
        }

        public IList<Entity.UserRole> GetAllUserRoles()
        {
            return userRoleRepo.GetAll<Entity.UserRole>();
        }

        public Entity.UserRole GetRoleByUserID(int userID)
        {
            return userRoleRepo.GetBy<Entity.UserRole>(p => p.UserID == userID);
        }

        #region IUserRoleService 成员


        public bool UserInRole(string strUser, string strRole)
        {
            return userRoleRepo.UserInRole(strUser, strRole);
        }

        #endregion

        #region IUserRoleService 成员


        public Entity.UserRole GetRoleByUserName(string strUser)
        {
            return userRoleRepo.GetRoleByUserName(strUser);
        }

        #endregion
    }
}
