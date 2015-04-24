using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Quartz;

namespace DSHOrder.Taobao
{
    public class DefaultProvider
    {
        //protected override System.Collections.Specialized.NameValueCollection GetSchedulerProperties()
        //{   
        //    var properties = base.GetSchedulerProperties();
        //    //properties.Add("test1", "test1value");
        //    return properties;
        //}

        //protected override void InitScheduler(IScheduler scheduler)
        //{
        //    ////Item Sync
        //    //JobDetail jobDetail1 = new JobDetail("FetchTaobaoItems", null, typeof(DSHOrder.Taobao.ItemSync));
        //    //jobDetail1.JobDataMap.Add("userName", "system");

        //    //Trigger trigger1 = TriggerUtils.MakeMinutelyTrigger(30, 1);
        //    //trigger1.StartTimeUtc = DateTime.UtcNow;
        //    //trigger1.Name = "ItemsTrigger";
        //    //scheduler.ScheduleJob(jobDetail1, trigger1);

        //    ////Three Months Order Sync
        //    //JobDetail jobDetail2 = new JobDetail("FetchTaobaoAllOrders", null, typeof(DSHOrder.Taobao.ThreeMonthsOrderSync));
        //    //jobDetail2.JobDataMap.Add("userName", "computer");

        //    //Trigger trigger2 = new SimpleTrigger("AllOrderTrigger", null);
        //    //scheduler.ScheduleJob(jobDetail2, trigger2);

        //    ////Increment Order Sync
        //    //JobDetail jobDetail3 = new JobDetail("FetchTaobaoIncrementOrders", null, typeof(DSHOrder.Taobao.IncrementOrderSync));
        //    //jobDetail3.JobDataMap.Add("userName", "computer");

        //    //Trigger trigger3 = TriggerUtils.MakeMinutelyTrigger(30, 1);
        //    //trigger3.StartTimeUtc = DateTime.UtcNow;
        //    //trigger3.Name = "IncrementOrdersTrigger";
        //    //scheduler.ScheduleJob(jobDetail3, trigger3);

        //    ////GroupByItemOrderMap Sync
        //    //JobDetail jobDetail4 = new JobDetail("MapGroupByItemAndOrders", null, typeof(DSHOrder.Taobao.GroupByItemOrderMapSync));
        //    //jobDetail4.JobDataMap.Add("userName", "computer");

        //    //Trigger trigger4 = new SimpleTrigger("OrderMapperTrigger", null);
        //    //scheduler.ScheduleJob(jobDetail4, trigger4);

        //    //scheduler.PauseAll();
        //}
    }
}
