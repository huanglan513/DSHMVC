using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace DSHOrder.Taobao
{
    public class TaobaoConfig : ConfigurationSection
    {
        //[ConfigurationProperty("appKey", IsRequired = true)]
        //public string AppKey
        //{
        //    get { return this["appKey"].ToString(); }
        //    set { this["appKey"] = value; }
        //}

        //[ConfigurationProperty("appSecret", IsRequired = true)]
        //public string AppSecret
        //{
        //    get { return this["appSecret"].ToString(); }
        //    set { this["appSecret"] = value; }
        //}

        //[ConfigurationProperty("testSessionKey")]
        //public string TestSesionKey
        //{
        //    get { return this["testSessionKey"].ToString(); }
        //    set { this["testSessionKey"] = value; }
        //}

        //[ConfigurationProperty("sellerNick", IsRequired = true)]
        //public string SellerNick
        //{
        //    get { return this["sellerNick"].ToString(); }
        //    set { this["sellerNick"] = value; }
        //}

        //[ConfigurationProperty("isSandbox")]
        //public bool IsSandBox
        //{
        //    get { return Convert.ToBoolean(this["isSandbox"].ToString()); }
        //    set { this["isSandBox"] = value; }
        //}

        private const string PrefixTaobaoConfiguration = "taobao";

        private const string KeyAppKey = PrefixTaobaoConfiguration + ".appKey";
        private const string KeyAppSecret = PrefixTaobaoConfiguration + ".appSecret";
        private const string KeyTestSessionKey = PrefixTaobaoConfiguration + ".testSessionKey";
        private const string KeySellerNick = PrefixTaobaoConfiguration + ".sellerNick";
        private const string KeyIsSandbox = PrefixTaobaoConfiguration + ".isSandbox";

        private const string DefaultKeyAppKey = "12298546";
        private const string DefaultKeyAppSecret = "9d39823cf635167f0ad57a1521318204";
        private const string DefaultKeyTestSessionKey = "4122720de5337386a0f2e4ba31dhWgs5xQJfe024da0a4996996571151";
        private const string DefaultKeySellerNick = "都市惠生活服务";
        private const string DefaultKeyIsSandbox = "false";

        private static readonly NameValueCollection configuration;

        static TaobaoConfig()
        {
            configuration = ConfigurationManager.GetSection("taobao") as NameValueCollection;
        }

        public static string AppKey
        {
            get { return GetConfigurationOrDefault(KeyAppKey, DefaultKeyAppKey); }
        }

        public static string AppSecret
        {
            get { return GetConfigurationOrDefault(KeyAppSecret, DefaultKeyAppSecret); }

        }

        public static string TestSesionKey
        {
            get { return GetConfigurationOrDefault(KeyTestSessionKey, DefaultKeyTestSessionKey); }

        }

        public static string SellerNick
        {
            get { return GetConfigurationOrDefault(KeySellerNick, DefaultKeySellerNick); }

        }

        public static bool IsSandBox
        {
            get
            {
                string retValue = null;
                if (configuration != null)
                {
                    retValue = configuration[KeyIsSandbox];
                }

                if (retValue == null || retValue.Trim().Length == 0)
                {
                    retValue = DefaultKeyIsSandbox;
                }
                return System.Convert.ToBoolean(retValue);
            }
        }

        /// <summary>
        /// Returns configuration value with given key. If configuration
        /// for the does not exists, return the default value.
        /// </summary>
        /// <param name="configurationKey">Key to read configuration with.</param>
        /// <param name="defaultValue">Default value to return if configuration is not found</param>
        /// <returns>The configuration value.</returns>
        private static string GetConfigurationOrDefault(string configurationKey, string defaultValue)
        {
            string retValue = null;
            if (configuration != null)
            {
                retValue = configuration[configurationKey];
            }

            if (retValue == null || retValue.Trim().Length == 0)
            {
                retValue = defaultValue;
            }
            return retValue;
        }
    }
}
