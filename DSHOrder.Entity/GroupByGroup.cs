using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Common;

namespace DSHOrder.Entity
{
    [MetadataType(typeof(GroupByGroupMetaData))]
    public partial class GroupByGroup
    {



    }
    public class GroupByGroupMetaData
    {
        [DisplayName("团购名称")]
        [Required(ErrorMessage = "团购名称不能为空")]
        [StringLength(100, ErrorMessage = "团购名称长度不能超过100个字符")]
        public string GroupByGroupName { get; set; }

        [DisplayName("团购合同编号")]
        [Required(ErrorMessage = "团购合同号不能为空")]
        [StringLength(50, ErrorMessage = "团购合同号长度不能超过50个字符")]
        public string GroupByContractNo { get; set; }

        [DisplayName("团购编号")]
        [StringLength(50, ErrorMessage = "团购编号长度不能超过50个字符")]
        public string GroupByCodeNo { get; set; }

        [DisplayName("团购描述")]
        [Required(ErrorMessage = "团购描述不能为空")]
        public string GroupByContent { get; set; }

        [DisplayName("底价")]
        [Required(ErrorMessage = "底价不能为空")]
        [Range(0, 10000000, ErrorMessage = "底价必须为正数,且范围在1~10000000")]
        public decimal? OriginalPrice { get; set; }

        [DisplayName("最终利润")]
        public decimal FinalProfit { get; set; }

        [DisplayName("商家公司全称")]
        [Required(ErrorMessage = "商家公司全称不能为空")]
        public int? CustomerID { get; set; }

        [DisplayName("行业细类")]
        public int? SubIndustryID { get; set; }

        [DisplayName("产品市场价")]
        [Required(ErrorMessage = "产品市场价不能为空")]
        public decimal? PriceMarket { get; set; }

        [DisplayName("产品结算价")]
        [Required(ErrorMessage = "产品结算价不能为空")]
        public decimal? PriceSettlement { get; set; }

        [DisplayName("建议上线价格")]
        [Required(ErrorMessage = "建议上线价格不能为空")]
        public decimal? PriceSuggest { get; set; }

        [DisplayName("其它服务费用")]
        [Required(ErrorMessage = "其它服务费用不能为空")]
        public decimal? PriceOther { get; set; }

        [DisplayName("参团店数")]
        public int? ShopCount { get; set; }

        [DisplayName("参团店名")]
        public string ShopNames { get; set; }

        [DisplayName("结算方式")]
        public int? SettlementType { get; set; }
        [DisplayName("结算日")]
        public DateTime? SettlementDate { get; set; }


        [DisplayName("产品图片1")]
        [Required(ErrorMessage = "产品图片1不能为空")]
        public int? ProductPicture1FileID { get; set; }

        [DisplayName("产品图片2")]
        [Required(ErrorMessage = "产品图片2不能为空")]
        public int? ProductPicture2FileID { get; set; }

        [DisplayName("资料路径")]
        public int? MaterialFileID { get; set; }

        [DisplayName("其他历史案例")]
        public string OtherHistryCase { get; set; }

        [DisplayName("点评/口碑等网上评价(网址)")]
        public string DPOrKBAddress { get; set; }







        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveBMJLMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveFKRBMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveZJLMemo { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "必填")]
        public int ApproveXSZLMemo { get; set; }

        //[DisplayName("xxxxxx")]
        //[Required(ErrorMessage = "必填")]
        //public int ApproveXSZLCrossCityID { get; set; }

        //[DisplayName("xxxxxx")]
        //[Required(ErrorMessage = "必填")]
        //public int ApproveXSZLCrossCityUserID { get; set; }

        [DisplayName("xxxxxx")]
        [Required(ErrorMessage = "请上传合同文件")]
        public int ApproveXSZLContractFileID { get; set; }



    }
}
