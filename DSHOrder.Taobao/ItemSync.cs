using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using System.Configuration;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Service.Interface;
using Quartz;

namespace DSHOrder.Taobao
{
    public class ItemSync : BaseSync, IJob
    {
        // 参照ThreeMonthsOrderSync.cs
        private const int HOUR_SECTION = 24;
        private const long PAGE_SIZE = 200;   
        private const string TOP_REQ_ITEM_FIELDS = "detail_url, price, num_iid, title";
        private const string TOP_REQ_ITEM_BANNER = "regular_shelved, never_on_shelf, sold_out,off_shelf,for_shelved,violation_off_shelf";
        private const string SYS_LAST_SYNC_TIME_KEY = "lastItemSyncTime";

        protected IGroupByItemService gbiService;
        protected IGroupByGroupService gbgService;
        protected ISystemParamService spService;
        protected List<GroupByItem> groupByItems;
        protected string userName;
        protected DateTime? lastSyncTime;

        public ItemSync()
        {
            gbiService = new GroupByItemService();
            gbgService = new GroupByGroupService();
            spService = new SystemParamService();
            groupByItems = new List<GroupByItem>();
        }

        public void Execute(IJobExecutionContext context)
        {
            InnerExecute(context.JobDetail.JobDataMap.GetString("userName"));
        }

        public bool InnerExecute(string name)
        {
            try
            {
               
                this.userName = name;
                Log.Info("ItemSync is Beginning.");

                lastSyncTime = GetLastSyncTime(SYS_LAST_SYNC_TIME_KEY);

                if (!lastSyncTime.HasValue)
                {
                    return GetAllItems();
                }

                return GetIncrementItems();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ":" + ex.StackTrace);
                //throw;
                return false;
            }           
        }

        private bool GetIncrementItems()
        {
            bool isSuccess = true;

            DateTime end = GetTaobaoDateTime();
            DateTime start = lastSyncTime.Value;

            isSuccess = GetItemsByPeriod(start, end);
            if (!isSuccess) {
                Log.ErrorFormat("Retrieve IncrementItems fail.({0}-{1})", start.ToString(), end.ToString());
                return false;
            } 

            gbiService.Add(groupByItems);
            SetLastSyncTime(SYS_LAST_SYNC_TIME_KEY, end);
            
            Log.Info("ItemSync Success.");
            return true;
        }

        private bool GetAllItems()
        {
            bool isSuccess = true;

            DateTime end = GetTaobaoDateTime();
            DateTime start = end.AddMonths(-3);

            IList<DateTime[]> timeList = SplitTimeByHours(start, end, HOUR_SECTION);
            foreach (DateTime[] dts in timeList)
            {
                isSuccess = isSuccess && GetItemsByPeriod(dts[0], dts[1]);

                if (!isSuccess) {
                    Log.ErrorFormat("Retrieve All Items fail.({0}-{1})", dts[0].ToString(), dts[1].ToString());
                    return false;
                } 
            }

            gbiService.Add(groupByItems);
            SetLastSyncTime(SYS_LAST_SYNC_TIME_KEY, end);

            return true;
        }

        private bool GetItemsByPeriod(DateTime start, DateTime end)
        {
            ItemsInventoryGetRequest iRequest = new ItemsInventoryGetRequest();
            iRequest.Fields = "num_iid";
            iRequest.Banner = TOP_REQ_ITEM_BANNER;
            iRequest.StartModified = start;
            iRequest.EndModified = end;            
            iRequest.PageNo = 1;
            iRequest.PageSize = PAGE_SIZE;


            //This is for inventory items.
            ItemsInventoryGetResponse iResponse = client.Execute<ItemsInventoryGetResponse>(iRequest, TaobaoConfig.TestSesionKey);
            if (iResponse.IsError)
            {
                Log.Error(iResponse.ErrMsg + ". " + iResponse.SubErrMsg);
                //TODO throw new NotImplementedException();
                return false;
            }

            long pageCount = GetPageCount(iResponse.TotalResults, iRequest.PageSize.GetValueOrDefault());
            for (; pageCount > 0; pageCount--)
            {
                iRequest.PageNo = pageCount;
                iResponse = client.Execute<ItemsInventoryGetResponse>(iRequest, TaobaoConfig.TestSesionKey);
                if (iResponse.IsError)
                {
                    Log.Error("Inventory items: " + iResponse.ErrMsg + ". " + iResponse.SubErrMsg);
                    //TODO throw new NotImplementedException();
                    return false;
                }

                foreach (Item i in iResponse.Items)
                {
                    Item item = GetFullItemInfo(i.NumIid);
                    if (item != null)
                    {
                        MapDSHGroupByItem(item);
                    }
                }
            }

            //This is for onsale items.
            ItemsOnsaleGetRequest oRequest = new ItemsOnsaleGetRequest();
            oRequest.Fields = "num_iid";
            oRequest.PageNo = 1;
            oRequest.PageSize = PAGE_SIZE;

            ItemsOnsaleGetResponse oResponse = client.Execute<ItemsOnsaleGetResponse>(oRequest, TaobaoConfig.TestSesionKey);
            if (oResponse.IsError)
            {
                Log.Error("Onsale items: " + oResponse.ErrMsg + ". " + oResponse.SubErrMsg);
                //TODO throw new NotImplementedException();
                return false;
            }
            pageCount = GetPageCount(oResponse.TotalResults, oRequest.PageSize.GetValueOrDefault());
            for (; pageCount > 0; pageCount--)
            {
                oRequest.PageNo = pageCount;
                oResponse = client.Execute<ItemsOnsaleGetResponse>(oRequest, TaobaoConfig.TestSesionKey);
                if (oResponse.IsError)
                {
                    Log.Error("Onsale items: " + oResponse.ErrMsg + ". " + oResponse.SubErrMsg);
                    //TODO throw new NotImplementedException();
                    return false;
                }

                foreach (Item i in oResponse.Items)
                {
                    Item item = GetFullItemInfo(i.NumIid);
                    if (item != null)
                    {
                        MapDSHGroupByItem(item);
                    }
                }
            }
            return true;
        }

        private void MapDSHGroupByItem(Item item)
        {
            GroupByItem gbi = new GroupByItem();
            gbi.GroupByPortalID = Constants.TAOBAO_PORTAL_ID;
            gbi.URL = item.DetailUrl;
            //TODO 此价格不是团购价格。
            gbi.SellingPrice = Convert.ToDecimal(item.Price);           
            gbi.TaobaoProductID = item.NumIid.ToString();
            
            //TODO 匹配方式需要修改,不能用title.
            GroupByGroup gbg = gbgService.GetByName(item.Title);
            if (gbg == null)
            {
                gbg = new GroupByGroup();
                gbg.GroupByGroupName = item.Title;
                gbg.Status = 0;

                gbg.CreateBy = userName;
                gbg.CreateTime = DateTime.Now;
                gbg.LastModifyBy = userName;
                gbg.LastModifyTime = DateTime.Now;
                gbg.DeleteInd = default(int);

                gbi.GroupByGroup = gbg;               
            }
            else
            {
                gbi.GroupByGroupID = gbg.GroupByGroupID;
            }

            gbi.CreateBy = userName;
            gbi.CreateTime = DateTime.Now;
            gbi.LastModifyBy = userName;
            gbi.LastModifyTime = DateTime.Now;
            gbi.DeleteInd = default(int);

            groupByItems.Add(gbi);
        }

        private Item GetFullItemInfo(long numIid)
        {
            ItemGetRequest request = new ItemGetRequest();
            request.Fields = TOP_REQ_ITEM_FIELDS;
            request.NumIid = numIid;

            ItemGetResponse response = client.Execute<ItemGetResponse>(request, TaobaoConfig.TestSesionKey);
            if (response.IsError)
            {
                Log.Error(response.ErrMsg + ". " + response.SubErrMsg);
                //TODO throw new NotImplementedException();
                return null;
            }
            return response.Item;
        }


        private DateTime? GetLastSyncTime(string sysName)
        {
            SystemParam sysParam = spService.GetSystemRecordByName(sysName);
            if (sysParam != null && !string.IsNullOrEmpty(sysParam.SystemValue))
            {
                return Convert.ToDateTime(sysParam.SystemValue);
            }
            return null;
        }

        private void SetLastSyncTime(string sysName, DateTime end)
        {
            IList<SystemParam> sysParams = spService.GetAllSystemData();
            SystemParam sp = sysParams.First<SystemParam>(p => p.SystemName.Equals(sysName));
            if (sp == null) 
            {
                sp = new SystemParam();                
                sp.SystemID = sysParams.Count + 1;
                sp.SystemName = sysName;                
            }
            sp.SystemValue = end.ToString();
            spService.UpdateSystemValue(sp);
        }
    }
}
