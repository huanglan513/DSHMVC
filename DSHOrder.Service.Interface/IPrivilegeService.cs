using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Service.Interface
{
    public interface IPrivilegeService
    {
        bool ValidatePrivilige(string userName, string routeUrl);
    }
}
