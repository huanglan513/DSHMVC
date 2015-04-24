using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Web.Common.Application.GroupByGroup;

namespace DSHOrder.Web
{
    public static class ComUtil
    {
        public enum CodeType
        {
            RefundType = 1,
            PaymentType = 2,
            RefundStatus = 3,
            IssueType = 4,
            OrderType = 5,
            GoodType = 6,
            UploadFileType = 9,
            JoinJWY = 10,
            // 销售助理提交团长审批
            XSZLTJTZSP = 15,
            // 团长审批结果
            TZSPJG = 16,
            // 设计状态
            SJZT = 17,
            // 设计是否确认完成
            SJSFWC = 18,
            // 团链是否确认
            TLSFQR = 19,
            // 摄影类别
            SYLB = 20,
            // 是否赠送假睫毛
            SFZSJJM = 21,
            // 是否包含门票
            SFBHMP = 22,
            // 是否进购物店
            SFJGWD = 23,
            // 交通工具
            JTGJ = 24
        }

        public enum BankType
        {
            中国银行 = 1,
            招商银行,
            中国工商银行,
            中国农业银行,
            中国建设银行,
            平安银行,
            深圳发展银行
        }

        public enum ApplyPaymentType
        {
            UnApply = 0,  //未申请打款
            Applied = 1,  //打款审批中
            UnPaid = 2     //打款申请审批同意，但是未支付款项
        }
        public static IEnumerable<SelectListItem> GetCityList()
        {
            ICityService cityService = new CityService();
            IList<City> cityList = cityService.GetAllCities();
            return cityList.Select<City, SelectListItem>(o => new SelectListItem { Text = o.CityName, Value = o.CityID.ToString() });
        }

        public static IEnumerable<SelectListItem> GetPortalList()
        {
            IGroupByPortalService portalService = new GroupByPortalService();
            IList<GroupByPortal> portalList = portalService.GetAllPortals();
            return portalList.Select<GroupByPortal, SelectListItem>(o => new SelectListItem { Text = o.PortalName, Value = o.GroupByPortalID.ToString() });
        }

        public static IEnumerable<SelectListItem> GetUserList()
        {
            IUserService userService = new UserService();
            IList<User> userList = userService.GetAllUsers();
            return userList.Select<User, SelectListItem>(o => new SelectListItem { Text = o.UserName, Value = o.UserID.ToString() });
        }

        public static IEnumerable<SelectListItem> GetIndustryList()
        {
            IIndustryService industryService = new IndustryService();
            IList<Industry> Industrys = industryService.GetAllIndustries();
            return Industrys.Select<Industry, SelectListItem>(o => new SelectListItem { Text = o.IndustryName, Value = o.IndustryID.ToString() });
        }


        public static IEnumerable<SelectListItem> GetSubIndustryList(int IndustryID)
        {
            ISubIndustryService subIndustryService = new SubIndustryService();
            IList<SubIndustry> SubIndustrys = subIndustryService.GetSubIndustriesByIndustryID(IndustryID);
            return SubIndustrys.Select<SubIndustry, SelectListItem>(o => new SelectListItem { Text = o.SubIndustryName, Value = o.SubIndustryID.ToString() });
        }

    }
}