using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    #region Models

    public class LogOnModel
    {
        [Required(ErrorMessage="用户名不能为空！")]
        [Display(Name = "用户名:")]
        public string UserName { get; set; }

        [Required(ErrorMessage="密码不能为空！")]
        [DataType(DataType.Password)]
        [Display(Name = "密码:")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }


    public class UserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "用户名不能为空！")]
        [Display(Name = "用户名:")]
        public string UserName { get; set; }

        [Display(Name = "性别:")]
        public string Sex { get; set; }

        [Display(Name = "称呼:")]
        public string Title { get; set; }

      
        [Required(ErrorMessage = "密码不能为空！")]        
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-z0-9_-]{6,20}$",
            ErrorMessage = "密码格式有误。有效格式为：输入长度为：6-20位，字符：只能为数字、字符")]
        [Display(Name = "密码:")]
        public string Password { get; set; }

       
        [DataType(DataType.Password)]
        [Display(Name = "确认密码:")]
        [Compare("Password", ErrorMessage = "两次密码输入不一致！")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage="至少选择一个部门！") ]
        [Display(Name="所属部门:")]
        public List<int> Departments { get; set; }

        [Display(Name = "用户角色:")]
        public int RoleId { get; set; }

        [Display(Name = "城市:")]
        public int? CityID { get; set; }

        [Display(Name = "客户名称:")]
        public int? CustomerID { get; set; }

        [Display(Name="状态:")]
        public bool Status { get; set; }
    }

    public class UserEditModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "用户名不能为空！")]
        [Display(Name = "用户名:")]
        public string UserName { get; set; }

        [Display(Name = "性别:")]
        public string Sex { get; set; }

        [Display(Name = "称呼:")]
        public string Title { get; set; }      

        [Required(ErrorMessage = "至少选择一个部门！")]
        [Display(Name = "所属部门:")]
        public List<int> Departments { get; set; }

        [Display(Name = "用户角色:")]
        public int RoleId { get; set; }

        [Display(Name = "城市:")]
        public int? CityID { get; set; }

        [Display(Name = "客户名称:")]
        public int? CustomerID { get; set; }

        [Display(Name = "状态:")]
        public bool Status { get; set; }
    }

    #endregion
}
