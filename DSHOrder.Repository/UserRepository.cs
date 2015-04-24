using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class UserRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityUser; }
        }

        public IList<User> GetUsersByPaging(Pagination paging)
        {
            var query = from q in this.CreateQuery<User>()
                        orderby q.UserName
                        select q;
            return paging.ParseQueryPaging<User>(query).ToList();
        }

        public int MarkDelete(int[] ids)
        {
            var userList = this.CreateQuery<User>().Where<User>(p => ids.Contains(p.UserID)).ToList();
            foreach (var user in userList)
            {
                user.DeleteInd = 1;
            }
            return this.SaveChanges();
        }
    }
}
