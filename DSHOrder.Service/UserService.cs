using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;
using System.Web.Security;
using DSHOrder.Common;

namespace DSHOrder.Service
{
    public class UserService : IUserService
    {
        private const int minRequiredPasswordLength = 8;

        UserRepository userRepo = null;

        public UserService()
        {
            userRepo = new UserRepository();
        }

        #region IUserService 成员

        public IList<User> GetAllUsers()
        {
            var query = userRepo.CreateQuery<User>().Where(p => p.DeleteInd == 0);
            return query.ToList();
        }

        public IList<User> GetUsersByDepartmentID(int departmentID)
        {
            var query = from q in userRepo.CreateQuery<User>()
                        join p in userRepo.CreateQuery<UserDepartment>() on q.UserID equals p.UserID
                        where p.DepartmentID == departmentID && q.DeleteInd == 0
                        select q;
            return query.ToList();
        }
        #endregion

        public int MinPasswordLength
        {
            get
            {
                return minRequiredPasswordLength;
            }
        }

        public User GetUserByID(int userID)
        {
            return userRepo.GetBy<User>(p => p.UserID == userID);
        }

        public User GetUserByName(string userName)
        {
            return userRepo.GetBy<User>(p => p.UserName == userName);
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            string encodedPassword = CryptoPassword(password);

            var query = from q in userRepo.CreateQuery<User>()
                        join p in userRepo.CreateQuery<UserRole>() on q.UserID equals p.UserID
                        where q.UserName == userName && q.Password == encodedPassword
                        select q;

            return query.Count() > 0;
        }


        public UserManageStatus CreateUser(User user)
        {
            //Duplicate User            
            //if (isExistUserName(user.UserName)) throw new ArgumentException("用户名已存在!", "user");
            if (isExistUserName(user.UserName)) return UserManageStatus.DuplicateUser;


            //Create the user.
            user = userRepo.Add<User>(user, true);

            int rstUser = userRepo.SaveChanges();
            return rstUser > 0 ? UserManageStatus.Success : UserManageStatus.Fail;
        }

        public UserManageStatus UpdateUser(User user)
        {
            //if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            userRepo.Update<User>(user, true);

            int rstUser = userRepo.SaveChanges();
            return rstUser > 0 ? UserManageStatus.Success : UserManageStatus.Fail;
        }

        private string CryptoPassword(string password)
        {
            //TODO to be implemented
            //hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strPlainText))
            return password;
        }

        private bool isExistUserName(string userName)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            var query = from q in userRepo.CreateQuery<User>()
                        where q.UserName == userName && q.DeleteInd == 0
                        select q;

            return query.Count() > 0;
        }

        public IList<User> GetUsersByPaging(Pagination paging)
        {
            return userRepo.GetUsersByPaging(paging);
        }

        public int MarkDelete(int[] ids)
        {  
            return userRepo.MarkDelete(ids);
        }



        #region IUserService 成员


        public IList<User> GetUsersByCityId(int cityId)
        {
            var users = from u in userRepo.CreateQuery<User>()
                        where cityId == -1 || u.CityID == cityId
                        select u;

            return users.ToList();
        }

        #endregion
    }

}
