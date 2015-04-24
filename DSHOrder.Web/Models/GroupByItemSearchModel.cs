using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using Webdiyer.WebControls.Mvc;
using System.Web.Mvc;

namespace DSHOrder.Web.Models
{
    public class GroupByItemSearchModel
    {
        /// <summary>
        /// 团购编号
        /// </summary>
        public string GroupByNumber { get; set; }

        /// <summary>
        /// 业务人员名称
        /// </summary>
        public string GroupBySale { get; set; }

        /// <summary>
        /// 团购名称
        /// </summary>
        public string GroupByName { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 团购状态
        /// </summary>
        public int? GroupByStatus { get; set; }

        /// <summary>
        /// 团购平台
        /// </summary>
        public int? PortalSelected { get; set; }
        public IEnumerable<SelectListItem> PortalList { get; set; }

        /// <summary>
        /// 城市列表
        /// </summary>
        public int? CitySelected { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }

        ///// <summary>
        ///// 团购商品列表
        ///// </summary>
        //public PagedList<GroupByGroup> GroupByGroupList { get; set; }

        /// <summary>
        /// 团购商品列表
        /// </summary>
        public PagedList<GroupByItem> GroupByItemList { get; set; }


    }
}