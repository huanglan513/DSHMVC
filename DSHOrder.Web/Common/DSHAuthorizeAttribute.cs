using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace DSHOrder.Web.Common
{
    public class DSHAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return base.AuthorizeCore(httpContext);
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = httpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }            
 
            //var rolesInSetting = ConfigurationManager.AppSettings["RolesFor:" + Roles]; 
            //var usersInSetting = ConfigurationManager.AppSettings["UsersFor:" + Users];
 
            //if (!String.IsNullOrEmpty(usersInSetting)) {
            //    var users = usersInSetting.Split(newchar[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //  if (users != null|| users.Length > 0 && users.Contains(User.Identity.Name, StringComparer.OrdinalIgnoreCase))
            //    return true; 
            //} 
 
            //if (String.IsNullOrEmpty(rolesInSetting))
            //    returnfalsevar roles = rolesInSetting.Split(newchar[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
            return false;
        }

        private static bool UserInRole(string role) {
            //if (user.UserInRole(role))
            //{
            //    return true;
            //}
            return false;
        }
    }
}