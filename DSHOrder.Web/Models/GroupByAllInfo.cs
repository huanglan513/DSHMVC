using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using System.ComponentModel.DataAnnotations;
using DSHOrder.Common;

namespace DSHOrder.Web.Models
{
    public class GroupByAllInfo
    {
        public IList<GroupByItem> GroupByItemList { get; set; }

        public GroupByGroup GroupByGroupEntity { get; set; }

        public IList<GroupByCity> GroupByCityList { get; set; }

        public int KeyId { get; set; }

        public int? IndustryID { get; set; }

        public int? DepartmentID { get; set; }

        public List<int> Sellers { get; set; }

        [Required(ErrorMessage = "第一次打款日期不能为空")]
        [Display(Name = "回访日期")]   
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "第一次打款日期格式错误，应为:yyyy-MM-dd")]
        public DateTime? FirstPaymentDate { get; set; }

        [Required(ErrorMessage = "第二次打款日期不能为空")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "第二次打款日期格式错误，应为:yyyy-MM-dd")]
        [Compare("FirstPaymentDate", ValidationCompareOperator.GreaterThanEqual, ValidationDataType.Date, ErrorMessage = "第二次打款日期必须等于或晚于第一次")]
        public DateTime? SecondPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ThirdPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ForthPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FifthPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SixthPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SeventhPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EighthPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NinthPaymentDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TenthPaymentDate { get; set; }

        public int? CityID { get; set; }

        public int? DistrictID { get; set; }

   
    }
}