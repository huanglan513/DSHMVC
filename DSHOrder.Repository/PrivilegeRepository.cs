using System;
using System.Collections.Generic;
using System.Linq;
using DSHOrder.Entity;
using System.Text;

namespace DSHOrder.Repository
{
    public class PrivilegeRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityPrivilege; }
        }

        public bool ExistPrivilege(int roleID, string routeUrl)
        {
            var query = from q in this.CreateQuery<Privilege>()
                        where q.RoleID == roleID && q.Function.Url.Equals(routeUrl, StringComparison.OrdinalIgnoreCase)
                        select q;
            if(query.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
