using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Common;

namespace DSHOrder.Entity
{
    public enum GroupBySearchStatus
    {
        GroupBying = 1,
        GroupByed = 2,
        GroupBypay = 3,
        GroupBypending = 4
    }

    public enum GroupByGroupStatus
    { 
        Returning = 0,
        Returned = 1,
        FirstPay = 2,
        SecondPay = 3
    }

    [MetadataType(typeof(GroupByItemMetaData))]
    public partial class GroupByItem
    {
    }

    public class GroupByItemMetaData
    {
        [DisplayName("上线日期")]
        [Required(ErrorMessage="上线日期不能为空")]
        [DataType(DataType.Date,ErrorMessage="上线日期的格式不正确，应为yyyy-MM-dd")]
        public DateTime? StartDay { get; set; }

        [DisplayName("下线日期")]
        [Required(ErrorMessage = "下线日期不能为空")]
        [DataType(DataType.Date, ErrorMessage = "下线日期的格式不正确，应为yyyy-MM-dd")]
        [Compare("StartDay", ValidationCompareOperator.GreaterThanEqual, ValidationDataType.Date, ErrorMessage = "下线日期必须等于或晚于上线日期")]
        public DateTime? EndDay { get; set; }

        [DisplayName("有效日期")]
        [Required(ErrorMessage = "有效日期不能为空")]
        [DataType(DataType.Date, ErrorMessage = "有效日期的格式不正确，应为yyyy-MM-dd")]
        public DateTime? ExpireDay { get; set; }

        [DisplayName("团购最少数量")]
        [Required(ErrorMessage = "团购最少数量不能为空")]
        [Range(1,10000000,ErrorMessage="团购最少数量必须为正数,且范围在1~10000000")]
        public int? LowestNum { get; set; }

        [DisplayName("团购最大数量")]
        [Required(ErrorMessage = "团购最大数量不能为空")]
        [Range(1, 10000000, ErrorMessage = "团购最大数量必须为正数,且范围在1~10000000")]
        [Compare("LowestNum", ValidationCompareOperator.GreaterThanEqual, ValidationDataType.Integer, ErrorMessage = "团购最大数量必须等于或多于团购最少数量")]
        public int? HighestNum { get; set; }

    

        [DisplayName("团购价")]
        [Required(ErrorMessage = "团购价不能为空")]
        [Range(0, 10000000, ErrorMessage = "团购价必须为正数,且范围在1~10000000")]
        //[Compare("OriginalPrice", ValidationCompareOperator.GreaterThanEqual, ValidationDataType.Double, ErrorMessage = "团购价必须等于或多于底价")]
        public decimal? SellingPrice { get; set; }

        [DisplayName("总销售量")]
        public int? SellNum { get; set; }

        [DisplayName("团购编号")]
        [Required(ErrorMessage = "团购编号不能为空")]
        [StringLength(50, ErrorMessage = "团购编号长度不能超过50个字符")]
        public string GroupByItemCodeNo { get; set; }

        [DisplayName("退款总计")]
        public decimal? RefundPrice { get; set; }

        [DisplayName("评分级别")]
        public decimal? RankingValue { get; set; }

        [DisplayName("评分次数")]
        public decimal? RankingNum { get; set; }






        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveTZMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveTZProcessTZTime { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveTZGroupByFlowNo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "请上传设计文档")]
        public int ApproveCHBUploadFileID { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveCHBMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveSJBMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveXSZLMemo { get; set; }

    }
}
