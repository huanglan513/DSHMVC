using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;
using System.Linq.Expressions;

namespace DSHOrder.Repository
{
    public class GroupByGroupRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityGroupByGroup; }
        }

        public List<GroupByPaymentInfo> GetPaymentList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID, int groupByPortalID)
        {
            IQueryable<GroupByPaymentInfo> query = null;
            IQueryable<KeyClass> queryItem = null;
            if (status == 0)     //查询申请付款的记录
            {
                if (groupPaymentStatus == 0)       //查询已处理为下架（物流和话费类处理为停止退款），还有付款需要申请的记录
                {
                    queryItem = from t in this.CreateQuery<Payment>()
                                where t.DeleteInd == 0 && (!t.ApprovalStatus.HasValue || t.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Disagree)
                                group t by t.GroupByItemID into cc
                                join q in this.CreateQuery<GroupByItem>() on cc.Key equals q.GroupByItemID
                                join p in this.CreateQuery<GroupByGroup>() on q.GroupByGroupID equals p.GroupByGroupID
                                join s in this.CreateQuery<SubIndustry>() on p.SubIndustryID equals s.SubIndustryID
                                join m in this.CreateQuery<Industry>() on s.IndustryID equals m.IndustryID
                                where q.DeleteInd == 0 && p.DeleteInd == 0 &&
                                     (((m.IndustryName.Contains("物流") || m.IndustryName.Contains("话费")) && q.Status >= (int)Utils.GroupByItemStatus.StopRefund) ||
                                     (!(m.IndustryName.Contains("物流") || m.IndustryName.Contains("话费")) && q.Status >= (int)Utils.GroupByItemStatus.Shelf))
                                select new KeyClass
                                {
                                    GroupByItemID = cc.Key
                                };

                }
                else if (groupPaymentStatus == 1)     //查询需要申请打款的记录，此处指计划打款日期离今天在预设置的时间提醒范围内。不一定是已经转为下架的记录
                {
                    var queryParam = from p in this.CreateQuery<SystemParam>()
                                     where p.SystemName == "PaymentWarningDay"
                                     select p.SystemValue;
                    List<string> sysValueList = queryParam.ToList();
                    int day = 7;
                    if (sysValueList.Count == 0 || !int.TryParse(sysValueList[0], out day))
                    {
                        day = 7;
                    }
                    DateTime startDate = DateTime.Today.AddDays(-day);
                    DateTime endDate = DateTime.Today.AddDays(1);
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && (!q.ApprovalStatus.HasValue || q.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Disagree)
                                    && q.PaymentDeadline.HasValue && q.PaymentDeadline.Value >= startDate && q.PaymentDeadline.Value < endDate
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                {
                                    GroupByItemID = cc.Key
                                };
                }
                else if (groupPaymentStatus == 2)  //查询已经申请过付款的记录
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && q.ApprovalStatus.HasValue
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                {
                                    GroupByItemID = cc.Key
                                };
                }
            }
            else if (status == 1)    //查询在打款审批中的记录
            {
                if (groupPaymentStatus == 0)   //查询待审批的记录
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && q.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.PendingApproval
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                    {
                                        GroupByItemID = cc.Key
                                    };
                }
                else if (groupPaymentStatus == 1)  //查询已审批的记录
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && (q.ApprovalStatus.HasValue && q.ApprovalStatus != (int)Utils.ApprovalPaymentStatus.PendingApproval)
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                {
                                    GroupByItemID = cc.Key
                                };
                }
            }
            else            //查询申请打款已审批通过，但是财务还未打款的记录
            {
                if (groupPaymentStatus == 0)  //查询全部的记录，包括已付款完毕，已付款部分，等待付款
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && q.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Agree
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                    {
                                        GroupByItemID = cc.Key
                                    };
                }
                else if (groupPaymentStatus == 1) //查询等待付款的记录
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && q.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Agree && (!q.PaymentStatus.HasValue || q.PaymentStatus == (int)Utils.PaymentStatus.NotPayment)
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                    {
                                        GroupByItemID = cc.Key
                                    };
                }
                else if (groupPaymentStatus == 2) //查询已付款部分的记录
                {
                    queryItem = from q in this.CreateQuery<Payment>()
                                where q.DeleteInd == 0 && q.ApprovalStatus == (int)Utils.ApprovalPaymentStatus.Agree && q.PaymentStatus == (int)Utils.PaymentStatus.Payment
                                group q by q.GroupByItemID into cc
                                select new KeyClass
                                    {
                                        GroupByItemID = cc.Key
                                    };
                }
                else if (groupPaymentStatus == 3) //查询已付款完毕的记录
                {
                    queryItem = from q in this.CreateQuery<GroupByItem>()
                                where q.Status == (int)Utils.GroupByItemStatus.Paied
                                select new KeyClass
                                {
                                    GroupByItemID = q.GroupByItemID
                                };
                }
            }
            if (!string.IsNullOrEmpty(seller))
            {
                queryItem = from q in queryItem
                            join m in this.CreateQuery<GroupByItem>() on q.GroupByItemID equals m.GroupByItemID
                            join p in this.CreateQuery<GroupByGroup>() on m.GroupByGroupID equals p.GroupByGroupID
                            join s in this.CreateQuery<GroupBySales>() on p.GroupByGroupID equals s.GroupByGroupID
                            join t in this.CreateQuery<User>() on s.UserID equals t.UserID
                            where t.UserName.Contains(seller) && s.DeleteInd == 0 && p.DeleteInd == 0
                            select q;
            }
            query = from a in queryItem
                    join p in this.CreateQuery<GroupByItem>() on a.GroupByItemID equals p.GroupByItemID
                    join q in this.CreateQuery<GroupByGroup>() on p.GroupByGroupID equals q.GroupByGroupID
                    join o in this.CreateQuery<GroupByPortal>() on p.GroupByPortalID equals o.GroupByPortalID
                    join s in this.CreateQuery<Customer>() on q.CustomerID equals s.CustomerID
                    join g in this.CreateQuery<City>() on s.CityID equals g.CityID
                    where (q.GroupByGroupName.Contains(groupByName) || q.GroupByCodeNo.Contains(groupByName)) && s.CustomerName.Contains(customerName)
                          && (customerCityID == 0 || s.CityID == customerCityID) && (groupByPortalID == 0 || p.GroupByPortalID == groupByPortalID)
                          && q.DeleteInd == 0 && p.DeleteInd == 0 && s.DeleteInd == 0
                    orderby q.GroupByGroupName
                    select new GroupByPaymentInfo
                    {
                        GroupByGroupID = q.GroupByGroupID,
                        GroupByGroupName = q.GroupByGroupName,
                        GroupByCodeNo = q.GroupByCodeNo,
                        CustomerID = s.CustomerID,
                        CustomerName = s.CustomerName,
                        GroupByPortal = o.PortalName,
                        CustomerCityName = g.CityName,
                        StartDate = p.StartDay,
                        EndDate = p.EndDay,
                        ItemStatus = p.Status,
                        GroupByItemID = p.GroupByItemID,
                        SubIndustryID = q.SubIndustryID
                    };
            var list = paging.ParseQueryPaging<GroupByPaymentInfo>(query).ToList();

            var querySales = (from q in list
                              join p in this.CreateQuery<GroupBySales>() on q.GroupByGroupID equals p.GroupByGroupID
                              join s in this.CreateQuery<User>() on p.UserID equals s.UserID
                              select new
                              {
                                  GroupByGroupID = q.GroupByGroupID,
                                  UserName = s.UserName
                              }).Distinct();
            var salesList = querySales.ToList();

            foreach (GroupByPaymentInfo item in list)
            {
                string salesName = string.Empty;
                foreach (var salesItem in salesList)
                {
                    if (salesItem.GroupByGroupID == item.GroupByGroupID &&!salesName.Contains(salesItem.UserName+","))
                    {
                        salesName=salesName+(salesItem.UserName + ",");
                    }
                }
                if (salesName.Length > 0)
                {
                    item.Seller = salesName.Remove(salesName.Length - 1, 1).ToString();
                }
            }

            List<int> groupByItemIDList = new List<int>();
            int prevItemByItemID = 0;
            foreach (GroupByPaymentInfo item in list)
            {
                if (item.GroupByItemID != prevItemByItemID)
                {
                    groupByItemIDList.Add(item.GroupByItemID);
                    prevItemByItemID = item.GroupByItemID;
                }
            }
            if (groupByItemIDList.Count > 0)
            {
                int[] groupByIDArray = groupByItemIDList.ToArray();
                var queryPayment = from q in this.CreateQuery<Payment>()
                                   where groupByIDArray.Contains(q.GroupByItemID.Value) && q.ApprovalStatus.HasValue
                                   select q;
                var paymentList = queryPayment.ToList();
                int prevItem = 0;
                int count = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    GroupByPaymentInfo item = list[i];
                    if (item.GroupByItemID != prevItem)
                    {
                        string strPayment = string.Empty;
                        foreach (Payment payment in paymentList)
                        {
                            if (payment.GroupByItemID == item.GroupByItemID)
                            {
                                strPayment += payment.ApplyDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "(" + payment.PaymentType + ")" + ",";
                            }
                        }
                        if (strPayment != string.Empty)
                        {
                            item.ApplyDateInfo = strPayment.Remove(strPayment.Length - 1);
                        }
                        prevItem = item.GroupByGroupID;

                        if (i != 0)
                        {
                            list[i - count].PortalCount = count;
                            count = 1;
                        }
                        if (i == list.Count - 1)
                        {
                            list[i - (count - 1)].PortalCount = count;
                        }
                        if (list.Count == 1)
                        {
                            list[0].PortalCount = count;
                        }
                    }
                    else
                    {
                        count++;
                        item.ApplyDateInfo = list[i - 1].ApplyDateInfo;
                        if (i == list.Count - 1)
                        {
                            list[i - (count - 1)].PortalCount = count;
                        }
                    }
                }
            }
            return list;
        }


        public List<GroupByPaymentInfo> GetShelfList(Pagination paging, string groupByName, string seller, string customerName, int groupPaymentStatus, int status, int customerCityID, int groupByPortalID)
        {
            IQueryable<GroupByPaymentInfo> query = null;
            IQueryable<GroupByItem> queryItem = null;
            if (status == 0)     //未到下架日期的团购
            {
                queryItem = from q in this.CreateQuery<GroupByItem>()
                            where q.DeleteInd == 0 && (!q.EndDay.HasValue || q.EndDay > DateTime.Today)
                            select q;
            }
            else if (status == 1)       //已到下架日期的团购
            {
                queryItem = from q in this.CreateQuery<GroupByItem>()
                            where q.DeleteInd == 0 && q.EndDay <= DateTime.Today
                            select q;
            }
            if (!string.IsNullOrEmpty(seller))
            {
                queryItem = from q in queryItem
                            join p in this.CreateQuery<GroupByGroup>() on q.GroupByGroupID equals p.GroupByGroupID
                            join s in this.CreateQuery<GroupBySales>() on p.GroupByGroupID equals s.GroupByGroupID
                            join t in this.CreateQuery<User>() on s.UserID equals t.UserID
                            where t.UserName.Contains(seller) && s.DeleteInd == 0 && p.DeleteInd == 0
                            select q;
            }
            query = from p in queryItem
                    join q in this.CreateQuery<GroupByGroup>() on p.GroupByGroupID equals q.GroupByGroupID
                    join o in this.CreateQuery<GroupByPortal>() on p.GroupByPortalID equals o.GroupByPortalID
                    join s in this.CreateQuery<Customer>() on q.CustomerID equals s.CustomerID
                    join g in this.CreateQuery<City>() on s.CityID equals g.CityID
                    where (q.GroupByGroupName.Contains(groupByName) || q.GroupByCodeNo.Contains(groupByName)) && s.CustomerName.Contains(customerName)
                          && (customerCityID == 0 || s.CityID == customerCityID) && (groupByPortalID == 0 || p.GroupByPortalID == groupByPortalID)
                          && q.DeleteInd == 0 && p.DeleteInd == 0 && s.DeleteInd == 0
                    orderby p.EndDay descending, q.GroupByGroupName
                    select new GroupByPaymentInfo
                    {
                        GroupByGroupID = q.GroupByGroupID,
                        GroupByGroupName = q.GroupByGroupName,
                        GroupByCodeNo = q.GroupByCodeNo,
                        CustomerID = s.CustomerID,
                        CustomerName = s.CustomerName,
                        GroupByPortal = o.PortalName,
                        CustomerCityName = g.CityName,
                        StartDate = p.StartDay,
                        EndDate = p.EndDay,
                        ItemStatus = p.Status,
                        GroupByItemID = p.GroupByItemID
                    };
            var list = paging.ParseQueryPaging<GroupByPaymentInfo>(query).ToList();


            var querySales = (from q in list
                            join p in this.CreateQuery<GroupBySales>() on q.GroupByGroupID equals p.GroupByGroupID
                            join s in this.CreateQuery<User>() on p.UserID equals s.UserID
                            select new
                            {
                                GroupByGroupID = q.GroupByGroupID,
                                UserName = s.UserName
                            }).Distinct();
            var salesList = querySales.ToList();

            foreach (GroupByPaymentInfo item in list)
            {
                StringBuilder salesName = new StringBuilder();
                foreach (var salesItem in salesList)
                {
                    if (salesItem.GroupByGroupID == item.GroupByGroupID)
                    {
                        salesName.Append(salesItem.UserName + ",");
                    }
                }
                if (salesName.Length > 0)
                {
                    item.Seller = salesName.Remove(salesName.Length - 1, 1).ToString();
                }
            }

            return list;
        }

        public List<GroupByGroup> SearchGroupBy(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string SearchType)
        {
            GroupByCodeOrName = string.IsNullOrEmpty(GroupByCodeOrName) ? "" : GroupByCodeOrName.Trim();
            Sale = string.IsNullOrEmpty(Sale) ? "" : Sale.Trim();
            CustomerName = string.IsNullOrEmpty(CustomerName) ? "" : CustomerName.Trim();

            var t = this.CreateQuery<GroupByGroup>();
            // extCondition = (r) => r.GroupByGroupID > 0;
            //t = t.Where(r => extCondition(r));

            var query = from r in t
                        where
                            ((string.IsNullOrEmpty(GroupByCodeOrName) || (r.GroupByCodeNo.IndexOf(GroupByCodeOrName) >= 0) || r.GroupByGroupName.IndexOf(GroupByCodeOrName) >= 0))
                            && (string.IsNullOrEmpty(CustomerName) || r.Customer.CustomerName.IndexOf(CustomerName) >= 0)
                            && (string.IsNullOrEmpty(Sale) || r.GroupBySales.AsEnumerable().Any(s => s.DeleteInd == 0 && s.User.UserName.IndexOf(Sale) >= 0))
                            && (!CustomerCityId.HasValue || CustomerCityId.Value == -1 || r.Customer.CityID == CustomerCityId)
                            && (!PortalId.HasValue || PortalId.Value == -1 || r.GroupByItem.Any(i => i.DeleteInd == 0 && i.GroupByPortalID == PortalId))
                            // && (r.CurrentNode == CurrentNode)
                            && r.DeleteInd == 0
                        select r;

            query = FixSearchType(query, SearchType);

            query = query.OrderBy(r => r.LastModifyTime);

            var list = paging.ParseQueryPaging<GroupByGroup>(query).ToList();

            return list;
        }

        private IQueryable<GroupByGroup> FixSearchType(IQueryable<GroupByGroup> query, string strSearchType)
        {
            // from r in query where r .GroupByGroupID > 0 select r;

            IQueryable<GroupByGroup> qReturn = null;

            #region EnumDescription

            //[EnumDescription("未审批的团购")]
            //Node7Type1,
            //[EnumDescription("已审批的团购")]
            //Node7Type2,

            //[EnumDescription("未设计的团购")]
            //Node8Type1,
            //[EnumDescription("已设计的团购")]
            //Node8Type2,

            //[EnumDescription("未处理的团购")]
            //Node9Type1,
            //[EnumDescription("已处理的团购")]
            //Node9Type2,

            //[EnumDescription("未处理的团购")]
            //Node10Type1,
            //[EnumDescription("已处理的团购")]
            //Node10Type2

            #endregion

            qReturn = query;

            switch (strSearchType)
            {
                case "Node2Type1":
                    //[EnumDescription("全部团购")]
                    //Node2Type1 = 1,
                    qReturn = query;
                    break;
                case "Node2Type2":
                    //[EnumDescription("进行中的团购")]
                    //Node2Type2,
                    // qReturn = from r in query where 
                     
                    break;
                case "Node2Type3":
                    //[EnumDescription("往期团购")]
                    //Node2Type3,

                    break;
                case "Node2Type4":
                    //[EnumDescription("未开始的团购")]
                    //Node2Type4,

                    break;
                case "Node3Type1":
                    //[EnumDescription("未审批的团购")]
                    //Node3Type1,

                    break;
                case "Node3Type2":
                    //[EnumDescription("已审批的团购")]
                    //Node3Type2,

                    break;
                case "Node4Type1":
                    //[EnumDescription("未审批的团购")]
                    //Node4Type1,

                    break;
                case "Node4Type2":
                    //[EnumDescription("已审批的团购")]
                    //Node4Type2,

                    break;
                case "Node5Type1":
                    //[EnumDescription("未审批的团购")]
                    //Node5Type1,

                    break;
                case "Node5Type2":
                    //[EnumDescription("已审批的团购")]
                    //Node5Type2,

                    break;
                case "Node6Type1":
                    //[EnumDescription("未提交的团购")]
                    //Node6Type1,

                    break;
                case "Node6Type2":
                    //[EnumDescription("已提交的团购")]
                    //Node6Type2,

                    break;
                default:
                    break;
            }

            return qReturn;
        }

        public List<GroupByItem> SearchGroupByItem(Pagination paging, int? CustomerCityId, int? PortalId, string GroupByCodeOrName, string Sale, string CustomerName, string CurrentNode)
        {
            GroupByCodeOrName = string.IsNullOrEmpty(GroupByCodeOrName) ? "" : GroupByCodeOrName.Trim();
            Sale = string.IsNullOrEmpty(Sale) ? "" : Sale.Trim();
            CustomerName = string.IsNullOrEmpty(CustomerName) ? "" : CustomerName.Trim();

            var query = from r in this.CreateQuery<GroupByGroup>()
                        join item in this.CreateQuery<GroupByItem>() on r.GroupByGroupID equals item.GroupByGroupID
                        where
                            ((string.IsNullOrEmpty(GroupByCodeOrName) || (r.GroupByCodeNo.IndexOf(GroupByCodeOrName) >= 0) || r.GroupByGroupName.IndexOf(GroupByCodeOrName) >= 0))
                            && (string.IsNullOrEmpty(CustomerName) || r.Customer.CustomerName.IndexOf(CustomerName) >= 0)
                            && (string.IsNullOrEmpty(Sale) || r.GroupBySales.AsEnumerable().Any(s => s.DeleteInd == 0 && s.User.UserName.IndexOf(Sale) >= 0))
                            && (!CustomerCityId.HasValue || CustomerCityId.Value == -1 || r.Customer.CityID == CustomerCityId)
                            && (!PortalId.HasValue || PortalId.Value == -1 || r.GroupByItem.Any(i => i.DeleteInd == 0 && i.GroupByPortalID == PortalId))
                            && (r.CurrentNode == CurrentNode)
                            && r.DeleteInd == 0
                        select item;

            query = query.OrderBy(r => r.LastModifyTime);

            var list = paging.ParseQueryPaging<GroupByItem>(query).ToList();

            return list;
        }
    }

    class KeyClass
    {
        public int? GroupByItemID { get; set; }

        public int? GroupByGroupID { get; set; }
    }
}
