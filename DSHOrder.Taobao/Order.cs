using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Service.Interface;

namespace DSHOrder.Taobao
{
    public class Order
    {
        ITopClient client = null;
        public Order() {
            //if (client == null)
            //{
            //    client = new DefaultTopClient(Constants.URL, Constants.APP_KEY, Constants.APP_SECRET, Constants.PARSE_TYPE);
            //}
        }

        public List<Top.Api.Domain.Trade> GetAllSoldOrders() 
        {
            //TradesSoldGetRequest req = new TradesSoldGetRequest();
            //req.Fields = "seller_nick,buyer_nick,title,type,created,sid,tid,seller_rate,buyer_rate,status,payment,discount_fee,adjust_fee,post_fee,total_fee,pay_time,end_time,modified,consign_time,buyer_obtain_point_fee,point_fee,real_point_fee,received_payment,commission_fee,pic_path,num_iid,num_iid,num,price,cod_fee,cod_status,shipping_type,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,orders.title,orders.pic_path,orders.price,orders.num,orders.iid,orders.num_iid,orders.sku_id,orders.refund_status,orders.status,orders.oid,orders.total_fee,orders.payment,orders.discount_fee,orders.adjust_fee,orders.sku_properties_name,orders.item_meal_name,orders.buyer_rate,orders.seller_rate,orders.outer_iid,orders.outer_sku_id,orders.refund_id,orders.seller_type";
            //TradesSoldGetResponse response = client.Execute(req, Constants.SESSION_KEY);
            //return response.TotalResults > 0? response.Trades : null;
            throw new NotImplementedException();
        }

        public List<Top.Api.Domain.Trade> GetIncrementSoldOrders(DateTime startTime, DateTime endTime)
        {
            //TradesSoldIncrementGetRequest req = new TradesSoldIncrementGetRequest();
            //req.Fields = "seller_nick,buyer_nick,title,type,created,sid,tid,seller_rate,buyer_rate,status,payment,discount_fee,adjust_fee,post_fee,total_fee,pay_time,end_time,modified,consign_time,buyer_obtain_point_fee,point_fee,real_point_fee,received_payment,commission_fee,pic_path,num_iid,num_iid,num,price,cod_fee,cod_status,shipping_type,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,orders.title,orders.pic_path,orders.price,orders.num,orders.iid,orders.num_iid,orders.sku_id,orders.refund_status,orders.status,orders.oid,orders.total_fee,orders.payment,orders.discount_fee,orders.adjust_fee,orders.sku_properties_name,orders.item_meal_name,orders.buyer_rate,orders.seller_rate,orders.outer_iid,orders.outer_sku_id,orders.refund_id,orders.seller_type";
            //req.StartModified = startTime;
            //req.EndModified = endTime;
            //TradesSoldIncrementGetResponse response = client.Execute(req, Constants.SESSION_KEY);            
            //return response.TotalResults > 0 ? response.Trades : null;            
            throw new NotImplementedException();
        }

        public int ConvertTaobaoDataToDb(List<Top.Api.Domain.Trade> trades, string userName)
        {
            List<OrderDetail> items = new List<OrderDetail>();

            foreach (Top.Api.Domain.Trade trade in trades)
            {
                foreach (Top.Api.Domain.Order order in trade.Orders)
                {
                    OrderDetail item = new OrderDetail();
                    item.GroupByItemID = 1;
                    item.BuyerID = null;
                    item.BuyerNick = trade.BuyerNick;
                    item.ShippingType = trade.ShippingType;
                    item.Tid = trade.Tid.ToString();
                    item.Oid = order.Oid.ToString();
                    item.AlipayNo = trade.BuyerAlipayNo;
                    item.TotalFee = Decimal.Parse(trade.TotalFee);
                    item.OrderStatus = 0;//order.Status;
                    item.RefundStatus = 0; // order.RefundStatus;
                    item.Payment = Decimal.Parse(trade.Payment);
                    item.Address = trade.ReceiverAddress;
                    //item.PayTime = DateTime.Parse(trade.PayTime);
                    item.CreateBy = userName;
                    item.CreateTime = DateTime.Now;
                    item.LastModifyBy = userName;
                    item.LastModifyTime = DateTime.Now;
                    items.Add(item);
                }                            
            }

            IOrderDetailService service = new OrderDetailService();
            return service.FillOrder(items);
        }

        public bool GetIncrementOrderFromTaobao(string userName, DateTime startTime, DateTime endTime)
        {
            int count = 0;
            List<Top.Api.Domain.Trade> list = GetIncrementSoldOrders(startTime, endTime);
            if (list.Count > 0)
            {
                count = ConvertTaobaoDataToDb(list, userName);
            }
            return count > 0 ? true : false;
        }

        public bool GetOrdersFromTaobao(string userName)
        {
            int count = 0;
            List<Top.Api.Domain.Trade> list = GetAllSoldOrders();
            if (list.Count > 0)
            {
                count = ConvertTaobaoDataToDb(list, userName);
            }
            return count > 0 ? true : false;
        }
    }
}
