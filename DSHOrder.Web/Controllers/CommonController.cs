using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DSHOrder.Web.Controllers
{
    public class CommonController : Controller
    {
        DSHOrder.Service.Interface.IFunctionService functionService;
        DSHOrder.Service.Interface.IUserService userService;
        IList<Entity.Function> functions;

        //TODO temp value
        public const int CUSTOMER_LIST = 7;
        public const int STAFF_LIST = 6;
        public const int PRODUCT_LIST = 8;

        protected override void Initialize(RequestContext requestContext)
        {
            functionService = new DSHOrder.Service.FunctionService();
            userService = new DSHOrder.Service.UserService();
            functions = functionService.GetAllFunctions();

            base.Initialize(requestContext);
        }

        //
        // GET: /Common/

        [ChildActionOnly]
        public ActionResult Menu()
        {
            Entity.User user = userService.GetUserByName(User.Identity.Name);
            Entity.UserRole ur = user.UserRole.SingleOrDefault<Entity.UserRole>();

            //if (ur == null)
            //{
            //    return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
            //}

            IList<DSHOrder.Entity.Function> list = functionService.GetAllMenus(ur.RoleID.Value);

            // hard code： 业务员、总经理都可以提交申请单
            if (ur.Role.RoleName == "总经理")
            {
                //string FunctionName = "团购申请";
                //Entity.Function f = functionService.GetByName(FunctionName);
                //list.Add(f);

                string FunctionName = "业务员填写团购申请表";
                Entity.Function f = functionService.GetByName(FunctionName);
                list.Add(f);
            }


            return PartialView(list);
        }

        public ActionResult Navigation(int functionId)
        {
            Entity.Function f = functions.SingleOrDefault<Entity.Function>(p => p.FunctionID == functionId);

            if (f.Url.EndsWith(".axd"))
            {
                //TODO need to check and redirect a new url.
                return View();
                //return Redirect("../" + f.Url);
            }

            String[] strs = f.Url.Split('/');

            if (strs.Length == 2)
            {
                return RedirectToAction(strs[1], strs[0]);
            }
            else if (strs.Length == 3)
            {
                return RedirectToAction(strs[1], strs[0], new { id = strs[2] });
            }

            return null;


            //switch (functionId)
            //{
            //    case STAFF_LIST:
            //        return RedirectToAction("Index", "User");
            //    case CUSTOMER_LIST:
            //        return RedirectToAction("Index", "Customer");
            //    case PRODUCT_LIST:
            //        return RedirectToAction("Index", "GroupBy");
            //    default:
            //        return RedirectToAction("Index", "Home");
            //}
        }

    }
}
