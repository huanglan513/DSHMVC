using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DSHOrder.Entity
{
    public class CustomerMetaData
    {
        [Required(ErrorMessage = "商家公司全称不能为空")]
        [StringLength(100)]
        [DisplayName("商家公司全称")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "商家联系人不能为空")]
        [StringLength(50)]
        [DisplayName("商家联系人")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "商家联系电话不能为空")]
        [StringLength(50)]
        [DisplayName("商家联系电话")]
        public string ContactPhoneNo { get; set; }

        [Required(ErrorMessage = "商家Email不能为空")]
        [StringLength(50)]
        [DisplayName("商家Email")]
        public string ContactEmail { get; set; }

        //[Range(1, 10)]
        [Required(ErrorMessage = "收款银行不能为空")]
        [StringLength(100)]
        [DisplayName("收款银行")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "银行账户不能为空")]
        [StringLength(50)]
        [DisplayName("银行账户")]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = "收款人不能为空")]
        [StringLength(50)]
        [DisplayName("收款人")]
        public string BankReceiver { get; set; }

        [Required(ErrorMessage = "地址不能为空")]
        [StringLength(200)]
        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("所在城市")]
        public string CityID { get; set; }

        [DisplayName("所在区")]
        public string DistrictID { get; set; }

        [DisplayName("结算方式")]
        public string DefaultPaymentType { get; set; }

        [DisplayName("页面设计联系人")]
        public string PageDesignContact { get; set; }

        [DisplayName("固定电话")]
        public string PageDesignPhone { get; set; }

        [DisplayName("Email")]
        public string PageDesignEmail { get; set; }

        [DisplayName("QQ")]
        public string PageDesignQQ { get; set; }

        [DisplayName("联系电话")]
        public string PageDesignMobile { get; set; }

        [DisplayName("页面执行联系人")]
        public string PageExecuteContact { get; set; }

        [DisplayName("固定电话")]
        public string PageExecutePhone { get; set; }

        [DisplayName("Email")]
        public string PageExecuteEmail { get; set; }

        [DisplayName("QQ")]
        public string PageExecuteQQ { get; set; }

        [DisplayName("联系电话")]
        public string PageExecuteMobile { get; set; }

        [DisplayName("投诉处理联系人")]
        public string ComplaintHandlingContact { get; set; }

        [DisplayName("固定电话")]
        public string ComplaintHandlingPhone { get; set; }

        [DisplayName("Email")]
        public string ComplaintHandlingEmail { get; set; }

        [DisplayName("QQ")]
        public string ComplaintHandlingQQ { get; set; }

        [DisplayName("联系电话")]
        public string ComplaintHandlingMobile { get; set; }

        [DisplayName("财务联系人")]
        public string FinancialContact { get; set; }

        [DisplayName("固定电话")]
        public string FinancialPhone { get; set; }

        [DisplayName("Email")]
        public string FinancialEmail { get; set; }

        [DisplayName("QQ")]
        public string FinancialQQ { get; set; }

        [DisplayName("联系电话")]
        public string FinancialMobile { get; set; }

        [DisplayName("营业执照")]
        public string BusinessLisenceImg { get; set; }
        [DisplayName("税务登记证")]
        public string TaxRegisterCertificateImg { get; set; }
        [DisplayName("卫生许可证")]
        public string HealthPermitImg { get; set; }
        [DisplayName("经营许可证")]
        public string LisenceImg { get; set; }
        [DisplayName("其他专业资质")]
        public string OtherProfQualificationImg { get; set; }
        [DisplayName("授权书")]
        public string PowerofAttorneyImg { get; set; }
        [DisplayName("外部环境图")]
        public string ExternalEnvImg { get; set; }
        [DisplayName("内部环境图")]
        public string InternalEnvImg { get; set; }

        [DisplayName("是否认证")]
        public bool IsCertified { get; set; }

        [Required(ErrorMessage = "品牌名称不能为空")]
        [StringLength(100)]
        [DisplayName("品牌名称")]
        public string BrandName { get; set; }

       // [Required(ErrorMessage = "月结日不能为空")]
        [DisplayName("月结日")]
        public DateTime DefaultPaymentDate { get; set; }

        [DisplayName("点评/口碑等上的评价（网址）")]
        public string Evaluation { get; set; }

    }
}