using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class StartFormModel
    {
        public IEnumerable<SelectListItem> Users = null;
        public IEnumerable<SelectListItem> Industrys = null;
        public IEnumerable<SelectListItem> SubIndustrys = new List<SelectListItem>();

        public StartFormModel()
        { 
        
        }

        public StartFormModel(string strUserName)
        {
            IUserService service = new UserService();
            User user = service.GetUserByName(strUserName);
            this._UserID = user.UserID;

            Users = ComUtil.GetUserList();

            Industrys = ComUtil.GetIndustryList();
        }

        private int _UserID = -1;
        [Display(Name = "业务员")]
        [Required(ErrorMessage = "业务员不能为空")]
        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }

        [Display(Name = "所属行业")]
        [Required(ErrorMessage = "所属行业不能为空")]
        public int IndustryID
        {
            get;
            set;
        }
        public string IndustryName
        {
            get;
            set;
        }

        [Display(Name = "行业细类")]
        [Required(ErrorMessage = "行业细类不能为空")]
        public int SubIndustryID
        {
            get;
            set;
        }
        public string SubIndustryName
        {
            get;
            set;
        }
    }
}