using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Common;

namespace DSHOrder.Entity
{
    [MetadataType(typeof(GroupLvYouMetaData))]
    public partial class GroupLvYou
    {

    }


    public class GroupLvYouMetaData
    {
        [Required(ErrorMessage = "必填")]
        public int LYCPXLAP { get; set; }

        [Required(ErrorMessage = "必填")]
        public int FYYBH { get; set; }

        [Required(ErrorMessage = "必填")]
        public int FYWBH { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JiuDianJB { get; set; }

        [Required(ErrorMessage = "必填")]
        public int CanBiao { get; set; }

        [Required(ErrorMessage = "必填")]
        public int PreDays { get; set; }

        [Required(ErrorMessage = "必填")]
        public int KeJieDaiCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int JiaTongGongJu { get; set; }

        //[Required(ErrorMessage = "必填")]
        //public int JiaTongGongJuOther { get; set; }

        [Required(ErrorMessage = "必填")]
        public int ChuTuanTime { get; set; }

        [Required(ErrorMessage = "必填")]
        public int ExpireDate { get; set; }

        [Required(ErrorMessage = "必填")]
        public int Shopping { get; set; }

        //[Required(ErrorMessage = "必填")]
        //public int ShoppingCount { get; set; }

        [Required(ErrorMessage = "必填")]
        public int ChenTuanReShu { get; set; }

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
