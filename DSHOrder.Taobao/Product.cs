using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Domain;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Service.Interface;

namespace DSHOrder.Taobao
{
    public class Product
    {
        ITopClient client = null;
        public Product()
        {
            //if (client == null)
            //{
            //    client = new DefaultTopClient(Constants.URL, Constants.APP_KEY, Constants.APP_SECRET, Constants.PARSE_TYPE);
            //}
        }

        public List<Top.Api.Domain.Item> GetAllProducts()
        {
            //ItemsGetRequest req = new ItemsGetRequest();
            //req.Fields = "num_iid,title,nick,pic_url,cid,price,type,delist_time,post_fee,score,volume";
            //req.Nicks = Constants.SELLER_NICK;
            //ItemsGetResponse response = client.Execute(req);
            //return response.TotalResults > 0 ? response.Items : null;
            throw new NotImplementedException();

        }

        public int ConvertTaobaoDataToDb(List<Item> items, string userName)
        {
            List<GroupByItem> list = new List<GroupByItem>();

            foreach (Item item in items)
            {
                
            }

            return -1;
        }

        public bool GetProdutsFromTaobao(string userName)
        {
            int count = 0;
            List<Top.Api.Domain.Item> list = GetAllProducts();
            if (list.Count > 0)
            {
                count = ConvertTaobaoDataToDb(list, userName);
            }
            return count > 0 ? true : false;
        }
    }
}
