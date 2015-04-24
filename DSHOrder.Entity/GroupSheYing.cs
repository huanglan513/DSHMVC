using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Common;

namespace DSHOrder.Entity
{
    [MetadataType(typeof(GroupSheYingMetaData))]
    public partial class GroupSheYing
    {

    }


    public class GroupSheYingMetaData
    {
        [Required(ErrorMessage = "必填")]
        public int SheYingType { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PaiSheDays { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PaiSheAddressInner { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PaiSheAddressOut { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PreDays { get; set; }

        [Required(ErrorMessage = "必填")]
        public int QuJinDays { get; set; }

        [Required(ErrorMessage = "必填")]
        public int ExpireDate { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PaiSheCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JingXiuCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int InDisCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JingXiuPriceExta { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JingXiuPriceExtaRuCe { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Size1 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int GongYi1 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Size2 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int GongYi2 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Size3 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int GongYi3 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeCount1 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeSize1 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeGongYi1 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeCount2 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeSize2 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int RuCeGongYi2 { get; set; }

        [Required(ErrorMessage = "必填")]
        public int ClothCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int HuaZhuangType { get; set; }

        [Required(ErrorMessage = "必填")]
        public int BaiTai { get; set; }

        [Required(ErrorMessage = "必填")]
        public int CaiZhi { get; set; }

        [Required(ErrorMessage = "必填")]
        public int HaiBaoCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int HaiBaoSize { get; set; }

        [Required(ErrorMessage = "必填")]
        public int QianBaoCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int SongJieMao { get; set; }

        [Required(ErrorMessage = "必填")]
        public int BaoMenPiao { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JieDaiCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int CanJiaJWY { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Other { get; set; }

        [Required(ErrorMessage = "必填")]
        public int IfBelowScore { get; set; }

        [Required(ErrorMessage = "必填")]
        public int YiPayJia { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Memo { get; set; }
    }
}
