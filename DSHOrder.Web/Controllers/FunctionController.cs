using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Service;

namespace DSHOrder.Web.Controllers
{
    public class FunctionController : ApplicationController
    {
        IFunctionService service = null;
        public FunctionController()
        {
            service = new FunctionService();
        }
        //
        // GET: /Function/

        public ActionResult Index()
        {
            return View(service.GetAllFunctions());
        }

    }
}
