using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using System.Configuration;
using Common.Logging;

namespace DSHOrder.Taobao
{
    public class BaseSync
    {
        private const string OFFICAL_URL = "http://gw.api.taobao.com/router/rest";
        private const string SANDBOX_URL = "http://gw.api.tbsandbox.com/router/rest";


        protected static ITopClient client = null;

        private readonly ILog log;

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected ILog Log
        {
            get { return log; }
        }

        public BaseSync()
        {
            if (client == null)
            { 
                if (TaobaoConfig.IsSandBox)
                {
                    client = new DefaultTopClient(SANDBOX_URL, TaobaoConfig.AppKey, TaobaoConfig.AppSecret);
                }
                else
                {
                    client = new DefaultTopClient(OFFICAL_URL, TaobaoConfig.AppKey, TaobaoConfig.AppSecret);
                }
            }
            log = LogManager.GetLogger(typeof(BaseSync));
        }

        protected DateTime GetTaobaoDateTime()
        {
            TimeGetRequest request = new TimeGetRequest();
            TimeGetResponse rsp = client.Execute<TimeGetResponse>(request);
            if (rsp.IsError)
            {
                Log.ErrorFormat("Error in GetTaobaoDateTime: {0}.{1}", rsp.ErrMsg, rsp.SubErrMsg);
                return DateTime.Now;    
            }
            return Convert.ToDateTime(rsp.Time);
        }

        protected long GetPageCount(long totalCount, long pageSize)
        {
            return (totalCount + pageSize - 1) / pageSize;
        }

        protected IList<DateTime[]> SplitTimeByHours(DateTime start, DateTime end, int hours)
        {
            IList<DateTime[]> dtList = new List<DateTime[]>();
            while (start < end)
            {
                DateTime _end = start.AddHours(hours);
                if (_end > end)
                {
                    _end = end;
                }
                dtList.Add(new[] { start, _end });
                start = _end;
            }
            return dtList;
        }
    }

    
}
