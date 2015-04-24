using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using DSHOrder.Service.Interface;
using DSHOrder.Service;

namespace DSHOrder.Web.Models
{
    public class GroupByItemFlowInfo
    {
        public GroupByGroup GroupByGroup = null;
        public GroupByItem GroupByItem = null;

        public GroupByItemFlowInfo(GroupByGroup GroupByGroup)
        {
            this.GroupByGroup = GroupByGroup;
        }
        public GroupByItemFlowInfo(GroupByItem GroupByItem)
        {
            this.GroupByItem = GroupByItem;
            this.GroupByGroup = GroupByItem.GroupByGroup;
        }

        public string GetCities(int GroupByItemId)
        {
            string strReturn = "";

            IGroupByCityService serivce = new GroupByCityService();
            IList<GroupByCity> rows = serivce.GetCitiesByPortalId(GroupByItemId);

            rows.ToList().ForEach(r => strReturn += r.City.CityName + ",");
            strReturn = strReturn.Trim(',');

            return strReturn;
        }

        public string GetAllCities()
        {
            string strReturn = "";

            this.GroupByGroup.GroupByItem.ToList().ForEach(m =>
                    {
                        strReturn += string.Concat(m.GroupByPortal.PortalName, ":", this.GetCities(m.GroupByItemID), "\n");
                    }
                );

            return strReturn.Trim();
        }


        public string GetCustomerName()
        {
            string strReturn = "";
            if (this.GroupByGroup.Customer != null)
            {
                strReturn = GroupByGroup.Customer.CustomerName;
            }

            return strReturn;
        }


        public object GetCustomerCity()
        {
            string strReturn = "";
            if (this.GroupByGroup.Customer != null && this.GroupByGroup.Customer.City != null)
            {
                strReturn = this.GroupByGroup.Customer.City.CityName;
            }

            return strReturn;
        }


        public string GetSales()
        {
            string strReturn = "";
            if (this.GroupByGroup.GroupBySales != null)
            {
                this.GroupByGroup.GroupBySales.ToList().ForEach(r => strReturn += string.Concat(r.User.UserName, ","));
            }

            return strReturn.Trim(',');
        }



        public static List<GroupByItemFlowInfo> GetList(List<GroupByGroup> liGroupByGroup)
        {
            List<GroupByItemFlowInfo> liReturn = new List<GroupByItemFlowInfo>();
            liGroupByGroup.ForEach(r => liReturn.Add(new GroupByItemFlowInfo(r)));
            return liReturn;
        }
        public static List<GroupByItemFlowInfo> GetList(List<GroupByItem> liGroupByItem)
        {
            List<GroupByItemFlowInfo> liReturn = new List<GroupByItemFlowInfo>();
            liGroupByItem.ForEach(r => liReturn.Add(new GroupByItemFlowInfo(r)));
            return liReturn;
        }


    }
}