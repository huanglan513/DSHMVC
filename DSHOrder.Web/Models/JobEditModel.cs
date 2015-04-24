using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class JobEditModel
    {
        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "任务名称")]
        public String JobName { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "任务组")]
        public String JobGroup { get; set; }

        [Display(Name = "任务描述")]
        public String JobDescription { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "程序集")]
        public String JobAssembly { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "任务类名")]
        public String JobClassName { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "触发器名称")]
        public String TriggerName { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "触发器组")]
        public String TriggerGroup { get; set; }

        [Display(Name = "触发器描述")]
        public String TriggerDescription { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "触发时间")]
        public DateTime StartTime { get; set; }

        [Display(Name = "中止时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "开启中止時間")]
        public bool IsNeedEndTime { get; set; }

        [Required(ErrorMessage = "不能为空!")]
        [Display(Name = "表达式")]
        public String CronExpression { get; set; }
    }
}