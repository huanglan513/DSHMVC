using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using System.Configuration;

namespace DSHOrder.Taobao
{
    public class BaseTaobaoModel
    {
        private const string OFFICAL_URL = "http://gw.api.taobao.com/router/rest";
        private const string SANDBOX_URL = "http://gw.api.tbsandbox.com/router/rest";

        protected static ITopClient client = null;
        protected static TaobaoConfig taobaoConfig = null;

        public BaseTaobaoModel()
        {
            if (client == null) {
                taobaoConfig = (TaobaoConfig)ConfigurationManager.GetSection("TaobaoConfig");
                if (taobaoConfig.IsSandBox)
                {
                    client = new DefaultTopClient(SANDBOX_URL, taobaoConfig.AppKey, taobaoConfig.AppSecret);
                }
                else {
                    client = new DefaultTopClient(OFFICAL_URL, taobaoConfig.AppKey, taobaoConfig.AppSecret);
                }               
            }
        }

    }
}
