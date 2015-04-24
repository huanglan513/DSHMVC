using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quartz;
using Quartz.Impl;
using DSHOrder.Web.Models;
using Quartz.Impl.Matchers;

namespace DSHOrder.Web.Controllers
{
    public class SchedulerAdminController : BaseController
    {
        private IScheduler sched;

        public SchedulerAdminController()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
        }

        public ActionResult Index()
        {
            SchedulerMetaData metaData = sched.GetMetaData();
            SchedulerInfoModel model = new SchedulerInfoModel();
            int totalJobs = 0;

            model.Started = metaData.Started;
            model.SchedulerName = metaData.SchedulerName;
            model.SchedulerRemote = metaData.SchedulerRemote;
            model.NumberOfJobsExecuted = metaData.NumberOfJobsExecuted;
            model.Shutdown = metaData.Shutdown;
            model.RunningSince = metaData.RunningSince;
            model.JobInfos = new List<JobInfoModel>();

            if (metaData.Shutdown || !metaData.Started)
            {
                return View(model);
            }

            IList<string> groups = sched.GetJobGroupNames();           

            foreach (string g in groups)
            {
                Quartz.Collection.ISet<JobKey> keys = sched.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(g));

                foreach (JobKey j in keys)
                {
                    IJobDetail job = sched.GetJobDetail(j);
                    JobInfoModel jim = new JobInfoModel();
                    jim.Job = job;
                    jim.Triggers = sched.GetTriggersOfJob(j);

                    model.JobInfos.Add(jim);
                    totalJobs++;
                }
            }

            model.NumberOfTotalJobs = totalJobs;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string action)
        {
            switch (action)
            {
                case "停止":
                    sched.Shutdown();
                    break;
                case "继续":
                    sched.ResumeAll();
                    break;
                case "暂停":
                    sched.PauseAll();
                    break;
                default:
                    break;
            }
            return RedirectToAction("Index", "SchedulerAdmin");
        }

        public ActionResult Edit()
        {
            JobEditModel model = new JobEditModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(JobEditModel model)
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.Load(model.JobAssembly);
            Type t = a.GetType(model.JobClassName);

            IJobDetail job = JobBuilder.Create(t).WithIdentity(model.JobName, model.JobGroup).WithDescription(model.JobDescription)
                                        .UsingJobData(new JobDataMap()).Build();
            TriggerBuilder triggerBuilder = TriggerBuilder.Create().WithIdentity(model.TriggerName, model.TriggerGroup)
                                    .WithDescription(model.TriggerDescription).WithCronSchedule(model.CronExpression).StartAt(model.StartTime);

            if (model.IsNeedEndTime)
            {
                triggerBuilder = triggerBuilder.EndAt(model.EndTime);
            }

            ITrigger trigger = triggerBuilder.Build();
            sched.ScheduleJob(job, trigger);

            return View(model);
        }

    }
}
