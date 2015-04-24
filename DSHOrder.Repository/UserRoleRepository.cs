using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class UserRoleRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityUserRole; }
        }

        public bool UserInRole(string strUser, string strRole)
        {
            var rows = from r in this.CreateQuery<UserRole>()
                       where r.Role.RoleName.Trim().ToUpper() == strRole.Trim().ToUpper()
                         && r.User.UserName.Trim().ToUpper() == strUser.Trim().ToUpper()
                       select r;

            return rows.Count() > 0;
        }



        public UserRole GetRoleByUserName(string strUser)
        {
            var rows = from r in this.CreateQuery<UserRole>()
                       where r.User.UserName.Trim().ToUpper() == strUser.Trim().ToUpper()
                       select r;

            return rows.SingleOrDefault();
        }
    }
}
