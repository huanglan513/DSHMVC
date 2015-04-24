using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Common;

namespace DSHOrder.Entity
{
    [MetadataType(typeof(GroupGeneralMetaData))]
    public partial class GroupGeneral
    {
    }


    public class GroupGeneralMetaData
    {
        // [Required(ErrorMessage = "可供使用人数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int UserCountPerTicket { get; set; }

        // [Required(ErrorMessage = "服务产品描述为空")]
        [Required(ErrorMessage = "必填")]
        public int Description { get; set; }

        // [Required(ErrorMessage = "团购券数量不能为空")]
        [Required(ErrorMessage = "必填")]
        public int TicketCountPerTime { get; set; }

        // [Required(ErrorMessage = "预约天数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int DayCountAhead { get; set; }

        // [Required(ErrorMessage = "每天可接待人数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int TimeCountPerDay { get; set; }

        // [Required(ErrorMessage = "有效期起始日期不能为空")]
        [Required(ErrorMessage = "必填")]
        public int EffectiveDayBegin { get; set; }

        // [Required(ErrorMessage = "有效期终止日期不能为空")]
        [Required(ErrorMessage = "必填")]
        public int EffectiveDayEnd { get; set; }

        // [Required(ErrorMessage = "团购数量上限人数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int UpperLimitCount { get; set; }

        // [Required(ErrorMessage = "接待时间不能为空")]
        [Required(ErrorMessage = "必填")]
        public int ReceiveTime { get; set; }

        // [Required(ErrorMessage = "是否参加聚无忧不能为空")]
        [Required(ErrorMessage = "必填")]
        public int JoinJWY { get; set; }

        public int Other { get; set; }

        // [Required(ErrorMessage = "评分不能为空")]
        [Required(ErrorMessage = "必填")]
        public int IfBelowScoreTM { get; set; }

        // [Required(ErrorMessage = "人数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int YiPayJiaTM { get; set; }

        // [Required(ErrorMessage = "百分率不能为空")]
        [Required(ErrorMessage = "必填")]
        public int IfBelowScoreTB { get; set; }

        // [Required(ErrorMessage = "人数不能为空")]
        [Required(ErrorMessage = "必填")]
        public int YiPayJiaTB { get; set; }


    }


}
