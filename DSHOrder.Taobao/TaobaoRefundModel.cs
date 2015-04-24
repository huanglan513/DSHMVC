using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using System.Configuration;

namespace DSHOrder.Taobao
{
    public class TaobaoRefundModel:BaseTaobaoModel
    {
        /// <summary>
        /// Get the details about the refund by ID.
        /// </summary>
        /// <param name="refundID"></param>
        /// <returns></returns>
        public Refund GetRefundByRefundID(long refundID)
        {
            RefundGetRequest request = new RefundGetRequest();
            request.Fields = "refund_id, alipay_no, tid, oid, buyer_nick, seller_nick, total_fee, status, created, refund_fee, good_status, has_good_return, payment, reason, desc, num_iid, title, price, num, good_return_time, company_name, sid, address, shipping_type, refund_remind_timeout";
            request.RefundId = refundID;
            RefundGetResponse response = client.Execute(request, taobaoConfig.TestSesionKey);
            return response.Refund;
        }

        /// <summary>
        /// Get the list of Refund include those information:
        /// refund_id, tid, title, buyer_nick, seller_nick, total_fee, status, created, refund_fee, oid, good_status, company_name, sid, payment, reason, desc, has_good_return, modified, order_status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="buyerNick"></param>
        /// <param name="startModifiedTime"></param>
        /// <param name="endModifiedTime"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Refund> GetRefunds(Constants.RefundStatus? status, string buyerNick, DateTime? startModifiedTime, DateTime? endModifiedTime, long? pageNo, long? pageSize)
        {
            RefundsReceiveGetRequest request = new RefundsReceiveGetRequest();
            request.Fields = "refund_id, tid, title, buyer_nick, seller_nick, total_fee, status, created, refund_fee, oid, good_status, company_name, sid, payment, reason, desc, has_good_return, modified, order_status, num_iid";
            request.Status = status.HasValue ? status.ToString() : string.Empty;
            request.BuyerNick = buyerNick;
            request.StartModified = startModifiedTime;
            request.EndModified = endModifiedTime;
            request.PageNo = pageNo;
            request.PageSize = pageSize;
            RefundsReceiveGetResponse response = client.Execute(request, taobaoConfig.TestSesionKey);
            return response.TotalResults > 0 ? response.Refunds : null;
        }

    }
}
