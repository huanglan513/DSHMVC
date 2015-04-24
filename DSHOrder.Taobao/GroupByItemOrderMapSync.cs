using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using Quartz;
using Common.Logging;

namespace DSHOrder.Taobao
{
    public class GroupByItemOrderMapSync : IJob
    {
        protected IOrderDetailService odService;
        protected IGroupByItemService gbiService;

        private readonly ILog log;

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected ILog Log
        {
            get { return log; }
        }

        public GroupByItemOrderMapSync()
        {
            odService = new OrderDetailService();
            gbiService = new GroupByItemService();
            log = LogManager.GetLogger(typeof(BaseSync));
        }

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("GroupByItemOrderMapSync is Beginning.");
            try
            {
                IList<OrderDetail> ods = odService.GetEmptyGroupByItem();
                if (ods == null) return;
                foreach (OrderDetail od in ods)
                {
                    GroupByItem gbi = gbiService.GetByToaobaoProductId(od.TaobaoProductID);
                    if (gbi != null)
                    {
                        od.GroupByItemID = gbi.GroupByItemID;
                    }
                }

                odService.Update(ods);
            }
            catch (Exception ex)
            {
                 Log.Error(ex.Message + ":" + ex.StackTrace);
                //throw;
            }

            Log.Info("GroupByItemOrderMapSync is Successful.");
        }

    }
}
