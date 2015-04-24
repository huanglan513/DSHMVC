using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace DSHOrder.Web.Common
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
        //    // construct job info
        //    //JobDetail jobDetail = new JobDetail("FetchTaobaoItems", null, typeof(DSHOrder.Taobao.ItemSync));
        //    //jobDetail.JobDataMap.Add("userName", "computer");

        //    //Trigger trigger = TriggerUtils.MakeMinutelyTrigger(30, 1);
        //    //trigger.StartTimeUtc = DateTime.UtcNow;
        //    //trigger.Name = "HalfHourTrigger";
        //    //scheduler.ScheduleJob(jobDetail, trigger);
        //}
    }
}