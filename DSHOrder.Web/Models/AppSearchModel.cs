using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using System.ComponentModel.DataAnnotations;
using DSHOrder.Common;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Common.Application.GroupByGroup;
using System.Collections;

namespace DSHOrder.Web.Models
{
    public class AppSearchModel
    {
        public GBGFlowEnumFLowNode CurrentNode { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }

        public int? CustomerCityId { get; set; }
        public int? PortalId { get; set; }
        public string GroupByCodeOrName { get; set; }
        public string Sale { get; set; }
        public string CustomerName { get; set; }
        public GBGFlowEnumSearchType SearchType { get; set; }


        public List<ResultItem> ResultItems { get; set; }
        public PagedList<ResultItem> Pager { get; set; }

        public AppSearchModel()
        {
            this.ResultItems = new List<ResultItem>();
        }


        public IList<GroupByPortal> ListGroupByPortal
        {
            get
            {
                IGroupByPortalService service = new GroupByPortalService();
                return service.GetAllPortals();
            }
        }

        public SelectList PortalList
        {
            get
            {
                IGroupByPortalService service = new GroupByPortalService();
                var listPortals = service.GetAllPortals();

                Entity.GroupByPortal portal = new Entity.GroupByPortal();
                portal.GroupByPortalID = -1;
                portal.PortalName = "请选择城市";
                listPortals.Insert(0, portal);

                var list = new SelectList(listPortals, "GroupByPortalID", "PortalName");

                return list;
            }
        }

        public SelectList SearchTypeList
        {
            get
            {
                ArrayList al = new ArrayList();

                switch (CurrentNode)
                {
                    case GBGFlowEnumFLowNode.None:
                        break;
                    case GBGFlowEnumFLowNode.YWYTXTGSQB:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node2Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node2Type2, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node2Type3, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node2Type4, al);
                        break;
                    case GBGFlowEnumFLowNode.BMJLJZJYS:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node3Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node3Type2, al);
                        break;
                    case GBGFlowEnumFLowNode.FKBES:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node4Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node4Type2, al);
                        break;
                    case GBGFlowEnumFLowNode.ZJLSP:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node5Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node5Type2, al);
                        break;
                    case GBGFlowEnumFLowNode.XSZLTJTZSP:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node6Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node6Type2, al);
                        break;
                    case GBGFlowEnumFLowNode.TZSP:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node7Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node7Type2, al);

                        break;
                    case GBGFlowEnumFLowNode.CHB:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node8Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node8Type2, al);

                        break;
                    case GBGFlowEnumFLowNode.SJB:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node9Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node9Type2, al);

                        break;
                    case GBGFlowEnumFLowNode.XSZLQRTL:
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node10Type1, al);
                        AddEnumToSelectList(GBGFlowEnumSearchType.Node10Type2, al);

                        break;
                    case GBGFlowEnumFLowNode.END:
                        break;
                    default:
                        break;
                }

                var list = new SelectList(al, "Type", "Desc");

                return list;
            }
        }

        private void AddEnumToSelectList(Enum e, ArrayList al)
        {
            al.Add(new { Type = e, Desc = EnumDescriptionAttribute.GetEnumDescription(e) });
        }

        public SelectList CityList
        {
            get
            {
                ICityService cityService = new CityService();
                IList<City> cityList = cityService.GetAllCities();

                Entity.City city = new Entity.City();
                city.CityID = -1;
                city.CityName = "请选择城市";
                cityList.Insert(0, city);

                var list = new SelectList(cityList, "CityID", "CityName");

                return list;
            }
        }



    }

    public class ResultItem
    {
        public int GroupByGroupId { get; set; }
        public int GroupByItemId { get; set; }
        public string GroupByGroupName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string Sales { get; set; }
        public string AllCities { get; set; }
    }
}