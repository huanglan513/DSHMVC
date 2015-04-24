using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Web.Common;

namespace DSHOrder.Web.Controllers
{
    public class SystemController : Controller
    {
        //DSHOrder.Taobao.Order order = new Taobao.Order();

        //
        // GET: /System/

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Taobao()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Taobao")]
        [MultiButton(Name = "getOne")]
        public ActionResult GetOneForTaobao(FormCollection collection)
        {
            //DateTime startTime = DateTime.Parse(collection["txtStartTime"]);
            //DateTime endTime = DateTime.Parse(collection["txtEndTime"]);
            //bool rst = order.GetIncrementOrderFromTaobao(User.Identity.Name, startTime, endTime);
            //if (rst)
            //{
            //    ViewBag.OutputResult = "成功获取数据！";
            //}
            //else
            //{
            //    ViewBag.OutputResult = "获取数据失败！";
            //}
            return View();
        }

        [HttpPost]
        [ActionName("Taobao")]
        [MultiButton(Name = "getAll")]
        public ActionResult GetAllForTaobao()
        {
            //bool rst = order.GetOrdersFromTaobao(User.Identity.Name);
            //if (rst)
            //{
            //    ViewBag.OutputResult = "成功获取数据！";
            //}
            //else
            //{
            //    ViewBag.OutputResult = "获取数据失败！";
            //}
            return View();
        }

    }
}
