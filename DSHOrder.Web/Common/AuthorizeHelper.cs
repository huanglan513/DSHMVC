using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Service;
using System.Web.Mvc;

namespace DSHOrder.Web.Common
{
    public class AuthorizeHelper
    {
        PrivilegeService pservice = new PrivilegeService();
        public bool IsInRole(Controller controller, string inputAction)
        {   
            if (!controller.User.Identity.IsAuthenticated)
            {
                return false;
            }    
            string userName = controller.User.Identity.Name;
            string controllerName = controller.RouteData.Values["controller"].ToString();
            string actionName = inputAction;
            if(string.IsNullOrEmpty(inputAction)){
                actionName = controller.RouteData.Values["action"].ToString();
            }            
            string routeUrl = controllerName + "/" + actionName;
            return pservice.ValidatePrivilige(userName, routeUrl);            
        }
    }
}