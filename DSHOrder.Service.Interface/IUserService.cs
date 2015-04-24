using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service.Interface
{
    public interface IUserService
    {
        IList<User> GetUsersByDepartmentID(int departmentID);

        int MinPasswordLength { get; }
        bool ValidateUser(string userName, string password);
        UserManageStatus CreateUser(DSHOrder.Entity.User user);
        UserManageStatus UpdateUser(DSHOrder.Entity.User user);
        IList<User> GetAllUsers();
        IList<User> GetUsersByPaging(Pagination paging);
        int MarkDelete(int[] ids);
        User GetUserByID(int userID);
        User GetUserByName(string userName);

        IList<User> GetUsersByCityId(int cityId);
    }
}
