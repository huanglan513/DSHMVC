using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Web.Common;

namespace DSHOrder.Web.Controllers
{
    public class BaseController : Controller
    {
        AuthorizeHelper authorizeHelper { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ////set work context to admin mode
            //EngineContext.Current.Resolve<IWorkContext>().IsAdmin = true;
            if (authorizeHelper == null) { authorizeHelper = new AuthorizeHelper(); }

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
        }

        protected bool ValidateRole(string action)
        {
            return authorizeHelper.IsInRole(this, action);
        }

    }
}
