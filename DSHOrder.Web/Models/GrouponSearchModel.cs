using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DSHOrder.Web.Extensions;

namespace DSHOrder.Web.Models
{
    public class GrouponSearchModel
    {
        /// <summary>
        /// 城市列表
        /// </summary>
        [DisplayName("商家所在城市")]
        [DropdownList("City")]
        public string CitySelected { get; set; }

        /// <summary>
        /// 团购平台
        /// </summary>
        [DisplayName("团购平台")]
        [DropdownList("Portal")]
        public string PortalSelected { get; set; }

        /// <summary>
        /// 团购编号/名称
        /// </summary>
        [DisplayName("团购编号/名称")]
        public string GroupByName { get; set; }

        /// <summary>
        /// 业务人员名称
        /// </summary>
        [DisplayName("业务员")]
        public string GroupBySale { get; set; }     

        /// <summary>
        /// 商家名称
        /// </summary>
        [DisplayName("商家公司全称")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 团购状态
        /// </summary>        
        public string GroupByStatus { get; set; }

        /// <summary>
        /// 团购商品列表
        /// </summary>
        public PagedList<GroupByItem> GroupByItemList { get; set; }

        /// <summary>
        /// 列表的页数，注意:此项为弱引用：
        /// PageIndexParameterName = "PageIndex"
        /// 在cshtml上并没有强引用
        /// </summary>
        public int? PageIndex { get; set; }
    }
}