using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace DSHOrder.Taobao
{
    public class TaobaoTradeRateModel:BaseTaobaoModel
    {
        /// <summary>
        /// Get the list of the product rate.
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TradeRate> GetTradeRate(long productID, long? pageNo, long? pageSize)
        {
            TraderatesSearchRequest request = new TraderatesSearchRequest();
            request.NumIid = productID;
            request.PageNo = pageNo;
            request.PageSize = pageSize;
            TraderatesSearchResponse response = client.Execute(request);            
            return response.TotalResults > 0? response.TradeRates: null; 
        }        
        
    }
}
