using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Taobao
{
    public class Constants
    {
        public const string PARSE_TYPE = "xml";
        //public const string URL = @"http://gw.api.taobao.com/router/rest";
        //public const string APP_KEY = "12298546";
        //public const string APP_SECRET = "9d39823cf635167f0ad57a1521318204";
        //public const string SESSION_KEY = "4121737de5337386a0f2e4ba31dfe024da0a49969965efwfTtww71151";
        //public const string SELLER_NICK = "都市惠生活服务";

        public const int TAOBAO_PORTAL_ID = 2;

        //Refund Status
        public enum RefundStatus
        {
            WAIT_SELLER_AGREE,      //买家已经申请退款，等待卖家同意
            WAIT_BUYER_RETURN_GOODS,        //卖家已经同意退款，等待买家退货
            WAIT_SELLER_CONFIRM_GOODS,      //买家已经退货，等待卖家确认收货
            SELLER_REFUSE_BUYER,        //卖家拒绝退款
            CLOSED,     //退款关闭
            SUCCESS,     //退款成功
            NO_REFUND       //不需要退款
        }

        public enum TradeStatus
        {
            TRADE_NO_CREATE_PAY,        //没有创建支付宝交易
            WAIT_BUYER_PAY,         //等待买家付款
            WAIT_SELLER_SEND_GOODS,     //等待卖家发货,即:买家已付款
            WAIT_BUYER_CONFIRM_GOODS,       //等待买家确认收货,即:卖家已发货
            TRADE_BUYER_SIGNED,     //买家已签收,货到付款专用
            TRADE_FINISHED,     //交易成功
            TRADE_CLOSED,       //交易关闭
            TRADE_CLOSED_BY_TAOBAO,     //交易被淘宝关闭
            ALL_WAIT_PAY,   //包含：WAIT_BUYER_PAY、TRADE_NO_CREATE_PAY
            ALL_CLOSED      //包含：TRADE_CLOSED、TRADE_CLOSED_BY_TAOBAO
        }

        public enum RateStatus
        {
            RATE_UNBUYER,       //买家未评
            RATE_UNSELLER,      //卖家未评
            RATE_BUYER_UNSELLER,        //买家已评，卖家未评
            RATE_UNBUYER_SELLER     //买家未评，卖家已评
        }    
    }
}
