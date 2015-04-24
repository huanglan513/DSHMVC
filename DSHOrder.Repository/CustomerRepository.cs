using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class CustomerRepository : ObjectRepository
    {

        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityCustomer; }
        }

        public IList<CustomerExtend> GetCustomersByNameAndCity(Pagination paging, string customerName, int cityID, int viewType, int isCertifiedSearch = 100)
        {
            bool? isCertified = null;
            if (isCertifiedSearch == 0)
                isCertified = false;
            else if (isCertifiedSearch == 1)
                isCertified = true;

            IQueryable<CustomerExtend> query;
            if (viewType == 0)
            {
                query = from q in this.CreateQuery<Customer>()
                        join p in this.CreateQuery<City>() on q.CityID equals p.CityID
                        where q.CustomerName.Contains(customerName) && (cityID == 0 || q.CityID == cityID) && (!isCertified.HasValue || q.IsCertified == isCertified) && q.DeleteInd == 0
                        join c in
                            (from s in this.CreateQuery<GroupByGroup>()
                             group s by s.CustomerID into a
                             select new
                             {
                                 CustomerID = a.Key,
                                 GroupCount = a.Count()
                             }) on q.CustomerID equals c.CustomerID into b
                        from d in b.DefaultIfEmpty()
                        orderby q.CustomerName
                        select new CustomerExtend
                        {
                            CustomerID = q.CustomerID,
                            CustomerName = q.CustomerName,
                            ContactName = q.ContactName,
                            ContactPhoneNo = q.ContactPhoneNo,
                            CityName = p.CityName,
                            IsCertified = q.IsCertified,
                            GroupCount = d.GroupCount
                        };
            }
            else
            {
                // var queryCustomer = from q in this.CreateQuery<Customer>()
                //          where q.CustomerName.Contains(customerName) && (cityID == 0 || q.CityID == cityID) && q.DeleteInd == 0
                //          orderby q.CustomerName
                //              select q;
                //var queryRank=from q in queryCustomer
                //        join p in this.CreateQuery<GroupByGroup>() on q.CustomerID equals p.CustomerID
                //        join s in this.CreateQuery<GroupByItem>()  on p.GroupByGroupID equals s.GroupByGroupID
                //        where p.DeleteInd==0 && s.DeleteInd==0
                //        select new 
                //        {
                //              CustomerID = q.CustomerID,
                //              RankValue=s.RankingValue
                //        } into t
                //        group t by t.CustomerID into cc
                //        select new
                //        {
                //            CustomerID=cc.Key,
                //            AvgRankValue=cc.Average(m=>m.RankValue)
                //        };
                //  query=from q in queryCustomer
                //        join p in queryRank on q.CustomerID equals p.CustomerID
                //        join s in this.CreateQuery<City>() on q.CityID equals s.CityID
                //        select new CustomerExtend
                //        {
                //             CustomerID = q.CustomerID,
                //              CustomerName = q.CustomerName,
                //              ContactName = q.ContactName,
                //              ContactPhoneNo = q.ContactPhoneNo,
                //              CityName = s.CityName
                //        };

                //var queryGroupByItem = from q in this.CreateQuery<GroupByItem>()
                //                       where q.DeleteInd==0
                //        group q by q.GroupByGroupID into cc
                //        select new
                //        {
                //            GroupByGroupID = cc.Key,
                //            AvgRankValueGroup = cc.Average(p => p.RankingValue)
                //        };
                // var queryAvgRankValue=from q in this.CreateQuery<GroupByGroup>()
                //                       join p in queryGroupByItem on q.GroupByGroupID equals p.GroupByGroupID 
                //                       where q.DeleteInd==0
                //                       select new
                //                       {
                //                           CustomerID=q.CustomerID,
                //                           AvgRankValueGroup=p.AvgRankValueGroup
                //                       } into s;


                var queryRankByCustomer = from q in this.CreateQuery<GroupByGroup>()
                                          join p in this.CreateQuery<GroupByItem>() on q.GroupByGroupID equals p.GroupByGroupID
                                          where q.DeleteInd == 0 && p.DeleteInd == 0
                                          select new
                                          {
                                              CustomerID = q.CustomerID,
                                              RankValue = p.RankingValue ?? 0
                                          } into s
                                          group s by s.CustomerID into cc
                                          select new
                                          {
                                              CustomerID = cc.Key,
                                              AvgRankValue = cc.Average(t => t.RankValue)
                                          };
                query = from p in queryRankByCustomer
                        join q in this.CreateQuery<Customer>() on p.CustomerID equals q.CustomerID
                        join s in this.CreateQuery<City>() on q.CityID equals s.CityID
                        where q.CustomerName.Contains(customerName) && (cityID == 0 || q.CityID == cityID) && (!isCertified.HasValue || q.IsCertified == isCertified) && p.AvgRankValue < 10 && q.DeleteInd == 0
                        orderby q.CustomerName
                        select new CustomerExtend
                        {
                            CustomerID = q.CustomerID,
                            CustomerName = q.CustomerName,
                            ContactName = q.ContactName,
                            ContactPhoneNo = q.ContactPhoneNo,
                            CityName = s.CityName,
                            AvgRankValue = p.AvgRankValue,
                            IsCertified = q.IsCertified
                        };
            }

            return paging.ParseQueryPaging<CustomerExtend>(query).ToList();
        }

        public int MarkDeleted(int[] ids)
        {
            var customerList = this.CreateQuery<Customer>().Where(p => ids.Contains(p.CustomerID)).ToList();
            foreach (Customer item in customerList)
            {
                item.DeleteInd = 1;
            }
            return this.SaveChanges();

        }

        public List<CooperationRecord> GetCooperationHistory(int customerID, Pagination paging)
        {
            var query = from q in this.CreateQuery<GroupByItem>()
                        join p in this.CreateQuery<GroupByGroup>() on q.GroupByGroupID equals p.GroupByGroupID
                        join s in this.CreateQuery<GroupByPortal>() on q.GroupByPortalID equals s.GroupByPortalID
                        where p.CustomerID == customerID && q.DeleteInd == 0 && p.DeleteInd == 0
                        orderby p.GroupByGroupID descending, s.GroupByPortalID
                        select new CooperationRecord
                        {
                            PortalName = s.PortalName,
                            GroupByGroupName = p.GroupByGroupName,
                            StartDay = q.StartDay,
                            EndDay=q.EndDay,
                            OriginalPrice=p.OriginalPrice,
                            SellPrice=q.SellingPrice,
                            ActualSellNum = q.ActualSellNum ?? 0,
                            RankValue = q.RankingValue ?? 0,
                            Url = q.URL,
                            ActualTotalProfit=q.ActualTotalProfit,
                            OtherCharge=q.OtherCharge
                        };
            return paging.ParseQueryPaging<CooperationRecord>(query).ToList();
        }

        public List<Customer> SearchCustomer(string CustomerName, int CityID, int IsCertified, int subIndustryId)
        {
            bool? bCertified = null;
            if (IsCertified != -1)
            {
                if (IsCertified == 1)
                {
                    bCertified = true;
                }
                else if (IsCertified == 0)
                {
                    bCertified = false;
                }
            }
            CustomerName = CustomerName.Trim();

            var query = from r in this.CreateQuery<Customer>()
                        where (string.IsNullOrEmpty(CustomerName) || r.CustomerName.IndexOf(CustomerName) >= 0)
                        && (CityID == -1 || r.CityID.Value == CityID)
                        && (bCertified == null || r.IsCertified.Value == bCertified.Value)
                        && (subIndustryId == -1 || r.SubIndustryID.Value == subIndustryId)
                        select r;

            return query.ToList();
        }
    }
}
