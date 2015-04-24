using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Web.Common;

namespace DSHOrder.Web.Controllers
{
    public class TaobaoController : BaseController
    {
        ISystemParamService sysService = new SystemParamService();
               
        public ActionResult Items()
        {
            Entity.SystemParam sys = sysService.GetSystemRecordByName("ItemInited");
            if (sys == null || sys.SystemValue.Equals("false"))
            {
                ViewBag.IsEmptyItems = true;
            }            
            return View();
        }

        [HttpPost]
        [ActionName("Items")]
        [MultiButton(Name = "getlatest")]   
        public ActionResult GetLatestItems(FormCollection collection)
        {
            bool rst = false;
            List<Entity.GroupByItem> list = new List<Entity.GroupByItem>();
            long? pageIndex = null;
            long? pageSize = 40;

            if (!string.IsNullOrEmpty(collection["PageIndex"]))
            {
                pageIndex = Convert.ToInt64(collection["PageIndex"]);
            }

            //Taobao.TaobaoItemModel model = new Taobao.TaobaoItemModel();
            //List<Top.Api.Domain.Item> items = model.GetLastestItems(null, null, pageIndex, pageSize);
            //if (items != null && items.Count > 0)
            //{
            //    list = model.StoreItemsToDb(items, User.Identity.Name);
            //    rst = list.Count > 0;
            //}

            ViewBag.OutputResult = rst ? "更新获取数据成功！" : "更新数据失败！";
            return View(list);
        }

        [HttpPost]
        [ActionName("Items")]
        [MultiButton(Name = "getAll")]
        public ActionResult GetAllItems(FormCollection collection)
        {
            bool rst = false;
            List<Entity.GroupByItem> list = new List<Entity.GroupByItem>();
            ViewBag.IsEmptyItems = true;
            long? pageIndex = null;
            long? pageSize = 40;

            if (!string.IsNullOrEmpty(collection["PageIndex"]))
            {
                pageIndex = Convert.ToInt64(collection["PageIndex"]);
            }

            //Taobao.TaobaoItemModel model = new Taobao.TaobaoItemModel();
            //List<Top.Api.Domain.Item> items = model.GetAllItems(null, null, null, pageIndex, pageSize);
            //if (items != null && items.Count > 0)
            //{
            //    list = model.StoreItemsToDb(items, User.Identity.Name);
            //    rst = list.Count > 0;
            //}

            if (rst)
            {
                Entity.SystemParam sys = sysService.GetSystemRecordByName("ItemInited");
                if (sys != null)
                {
                    sys.SystemValue = "true";
                    rst = sysService.UpdateSystemValue(sys);
                    ViewBag.IsEmptyItems = false;
                }
            }

            ViewBag.OutputResult = rst ? "更新获取数据成功！" : "更新数据失败！";
            return View(list);
        }

        public ActionResult Orders()
        {
            Entity.SystemParam sys = sysService.GetSystemRecordByName("OrderInited");
            if (sys == null || sys.SystemValue.Equals("false"))
            {
                ViewBag.IsEmptyOrders = true;
            }
            return View();
        }
      
        [HttpPost]
        [ActionName("Orders")]
        [MultiButton(Name = "getlatest")]
        public ActionResult GetLatestOrders(FormCollection collection)
        {
            bool rst = false;
            List<Entity.OrderDetail> list = null;
            long? pageIndex = null;
            long? pageSize = 40;

            if (!string.IsNullOrEmpty(collection["PageIndex"]))
            {
                pageIndex = Convert.ToInt64(collection["PageIndex"]);
            }

            //Taobao.TaobaoTradeModel model = new Taobao.TaobaoTradeModel();
            //List<Top.Api.Domain.Trade> items = model.GetLatestTrads(DateTime.Now.AddDays(-1), DateTime.Now, pageIndex, pageSize);
            //if (items != null && items.Count > 0)
            //{
            //    list = model.MigrateTradesToOrders(items);
            //    list = model.StoreOrdersToDb(list, User.Identity.Name);
            //    rst = list.Count > 0;
            //}

            ViewBag.OutputResult = rst ? "更新获取数据成功！" : "更新数据失败！";
            return View(list);
        }

        [HttpPost]
        [ActionName("Orders")]
        [MultiButton(Name = "getAll")]
        public ActionResult GetAllOrders(FormCollection collection)
        {
            bool rst = false;
            List<Entity.OrderDetail> list = null;
            ViewBag.IsEmptyOrders = true;
            long? pageIndex = null;
            long? pageSize = 40;

            if (!string.IsNullOrEmpty(collection["PageIndex"]))
            {
                pageIndex = Convert.ToInt64(collection["PageIndex"]);
            }

            //Taobao.TaobaoTradeModel model = new Taobao.TaobaoTradeModel();

            //List<Top.Api.Domain.Trade> items = model.GetAllTrades(null, null, null, null, null, pageIndex, pageSize);          
            //if (items != null && items.Count > 0)
            //{
            //    list = model.MigrateTradesToOrders(items);
            //    list = model.StoreOrdersToDb(list, User.Identity.Name);
            //    rst = list.Count > 0;
            //}

            if (rst)
            {
                Entity.SystemParam sys = sysService.GetSystemRecordByName("OrderInited");
                if (sys != null)
                {
                    sys.SystemValue = "true";
                    rst = sysService.UpdateSystemValue(sys);
                    ViewBag.IsEmptyOrders = false;
                }
            }

            ViewBag.OutputResult = rst ? "更新获取数据成功！" : "更新数据失败！";
            return View(list);
        }

    }   
}
