using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Common
{
    public class Utils
    {
       

        public enum ApprovalPaymentStatus
        {
            PendingApproval=1, //审批中
            Agree=2, //同意
            Disagree=3   //拒绝
        }

        public enum PaymentStatus
        {
            NotPayment=0,  //未打款
            Payment=1   //已打款
        }

        public enum GroupByItemStatus
        {
            Shelf=9,       //已下架
            StopRefund=10,    //停止退款
            Paied=14        //已付款完毕
        }
    }
}
