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

namespace DSHOrder.Taobao
{
    public class TaobaoItemModel:BaseTaobaoModel
    {
        /// <summary>
        /// Get the taobao item information by number.
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public Item GetItemByID(long no)
        {
            ItemGetRequest request = new ItemGetRequest();
            request.Fields = "detail_url,num_iid,title,nick,type,cid,seller_cids,props,input_pids,input_str,desc,pic_url,num,valid_thru,list_time,delist_time,stuff_status,location,price,post_fee,express_fee,ems_fee,has_discount,freight_payer,has_invoice,has_warranty,has_showcase,modified,increment,approve_status,postage_id,product_id,auction_point,property_alias,item_img,prop_img,sku,video,outer_id,is_virtual";
            request.NumIid = no;
            ItemGetResponse response = client.Execute(request);
            return response.Item;
        }

        /// <summary>
        /// Get the list of the item part information:
        /// num_iid,title,nick,pic_url,cid,price,type,delist_time,post_fee,score,volume.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="categoryId"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Item> GetAllItems(string title, int? categoryId, string orderBy, long? pageNo, long? pageSize)
        {
            TaobaoConfig taobaoConfig = (TaobaoConfig)ConfigurationManager.GetSection("TaobaoConfig");
            ItemsGetRequest request = new ItemsGetRequest();
            //request.Fields = "num_iid,title,nick,pic_url,cid,price,type,delist_time,post_fee,score,volume";
            request.Fields = "num_iid";
            request.Nicks = taobaoConfig.SellerNick;
            request.Cid = categoryId;
            request.Q = title;
            request.OrderBy = orderBy;
            request.PageNo = pageNo;
            request.PageSize = pageSize;            
            ItemsGetResponse response = client.Execute(request);

            List<Item> list = null;
            if (response.TotalResults > 0)
            {
                list = new List<Item>();
                foreach (Item i in response.Items)
                {
                    Item item = GetItemByID(i.NumIid);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Get the list of the item part information:
        /// num_iid,title,nick,pic_url,cid,price,type,delist_time,post_fee,score,volume.
        /// Because it needs to match the DB, it will effect the perfermance.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="categoryId"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Item> GetLastestItems(string title, int? categoryId, long? pageNo, long? pageSize)
        {            
            ItemsGetRequest request = new ItemsGetRequest();
            //request.Fields = "num_iid,title,nick,pic_url,cid,price,type,delist_time,post_fee,score,volume";
            request.Fields = "num_iid";
            request.Nicks = taobaoConfig.SellerNick;
            request.Cid = categoryId;
            request.Q = title;
            request.OrderBy = "delist_time";
            request.PageNo = pageNo;
            request.PageSize = pageSize;
            ItemsGetResponse response = client.Execute(request);

            List<Item> list = null;
            if (response.TotalResults > 0)
            {               
                IGroupByItemService gbiService = new GroupByItemService();
                foreach (Item i in response.Items)
                {
                    GroupByItem gbi = gbiService.GetByToaobaoProductId(i.NumIid.ToString());
                    if (gbi == null)
                    {
                        if (list == null)
                        {
                            list = new List<Item>();
                        }
                        Item item = GetItemByID(i.NumIid);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public static GroupByItem BuildGroupByItem(IGroupByGroupService gbgService, Item item, string name)
        {
            GroupByItem gbi = new GroupByItem();
            gbi.GroupByPortalID = Constants.TAOBAO_PORTAL_ID;
            gbi.URL = item.DetailUrl;
            //FIXME 此价格不是团购价格。
            gbi.SellingPrice = Convert.ToDecimal(item.Price);
            gbi.CreateBy = name;
            gbi.CreateTime = DateTime.Now;
            gbi.LastModifyBy = name;
            gbi.LastModifyTime = DateTime.Now;
            gbi.DeleteInd = 0;
            gbi.TaobaoProductID = item.NumIid.ToString();
            GroupByGroup gbg = gbgService.GetByName(item.Title);
            if (gbg == null)
            {
                gbg = new GroupByGroup();
                gbg.GroupByGroupName = item.Title;
                gbg.CreateBy = name;
                gbg.CreateTime = DateTime.Now;
                gbg.LastModifyBy = name;
                gbg.LastModifyTime = DateTime.Now;
                gbg.DeleteInd = 0;
                gbi.GroupByGroup = gbg;
            }
            else
            {
                gbi.GroupByGroupID = gbg.GroupByGroupID;
            }
            return gbi;
        }

        /// <summary>
        /// Store the taobao product data into DB.
        /// </summary>
        /// <param name="srcList"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<GroupByItem> StoreItemsToDb(List<Item> srcList, string name)
        {
            List<GroupByItem> gbiList = new List<GroupByItem>();

            List<Item> items = new List<Item>();

            IGroupByItemService gbiService = new GroupByItemService();
            IGroupByGroupService gbgService = new GroupByGroupService();
            foreach (Item i in srcList)
            {
                GroupByItem gbi = new GroupByItem();
                gbi.GroupByPortalID = Constants.TAOBAO_PORTAL_ID;
                gbi.URL = i.DetailUrl;                
                //FIXME 此价格不是团购价格。
                gbi.SellingPrice = Convert.ToDecimal(i.Price);                
                gbi.CreateBy = name;
                gbi.CreateTime = DateTime.Now;
                gbi.LastModifyBy = name;
                gbi.LastModifyTime = DateTime.Now;
                gbi.DeleteInd = 0;
                gbi.TaobaoProductID = i.NumIid.ToString();                
                GroupByGroup gbg = gbgService.GetByName(i.Title);
                if (gbg == null)
                {
                    gbg = new GroupByGroup();
                    gbg.GroupByGroupName = i.Title;
                    gbg.CreateBy = name;
                    gbg.CreateTime = DateTime.Now;
                    gbg.LastModifyBy = name;
                    gbg.LastModifyTime = DateTime.Now;
                    gbg.DeleteInd = 0;
                    gbi.GroupByGroup = gbg;
                }
                else {
                    gbi.GroupByGroupID = gbg.GroupByGroupID;
                }
                gbiList.Add(gbi);
            }

            int count = gbiService.Add(gbiList);
            return count > 0 ? gbiList : null;
        }
    }
}
