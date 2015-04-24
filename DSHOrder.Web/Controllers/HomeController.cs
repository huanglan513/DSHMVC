using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Web.Models;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Web.Routing;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Web.Controllers
{
    public class HomeController : ApplicationController
    {
        IUserService userService { get; set; }
        IFormsAuthenticationService faService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (faService == null) { faService = new FormsAuthenticationService(); }
            if (userService == null) { userService = new UserService(); }

            base.Initialize(requestContext);
        }

        public ActionResult Security()
        {
            return View();
        }

        public ActionResult Index()
        {
            //  ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
            //return RedirectToAction("Index", "GroupBy");
        }

        public ActionResult Willcome()
        {
            return View();
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (userService.ValidateUser(model.UserName, model.Password))
                {
                    faService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // return RedirectToAction("Index", "Home");
                        return RedirectToAction("Willcome", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码不正确");
                }
            }

            return View(model);
        }


        public ActionResult LogOff()
        {
            faService.SignOut();

            return RedirectToAction("LogOn", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                IUserService service = new UserService();
                User user = service.GetUserByName(this.User.Identity.Name);
                if (user.Password.Equals(model.OldPassword))
                {
                    user.Password = model.ConfirmPassword;
                    UserManageStatus status = service.UpdateUser(user);
                    if (status == UserManageStatus.Success)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "保存失败");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "旧密码不正确");
                }
            }
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
