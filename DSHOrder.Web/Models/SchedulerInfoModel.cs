using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class SchedulerInfoModel
    {
        [Display(Name = "状态")]
        public bool Started { get; set; }

        [Display(Name = "调度器名称")]
        public string SchedulerName { get; set; }

        [Display(Name = "是否远程")]
        public bool SchedulerRemote { get; set; }

        [Display(Name = "已执行任务数量")]
        public int NumberOfJobsExecuted { get; set; }

        [Display(Name = "总的任务数量")]
        public int NumberOfTotalJobs { get; set; }

        [Display(Name = "是否关闭")]
        public bool Shutdown { get; set; }

        public IList<JobInfoModel> JobInfos { get; set; }

        [Display(Name = "启动时间")]
        public DateTimeOffset? RunningSince { get; set; }
    }
}