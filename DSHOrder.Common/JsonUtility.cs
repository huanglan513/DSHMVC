using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;


namespace DSHOrder.Common
{
    /// <summary>
    ///JsonHelper 的摘要说明
    /// </summary>
    public static class JsonUtility
    {
        /// <summary>
        /// 生成Json格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                // szJson = FixDate(szJson);

                return szJson;
            }
        }

        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }


        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string szJson)
        {
            return ParseFromJson<T>(szJson);
        }


        /// <summary> 
        /// 返回对象序列化 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            string strReturn = serialize.Serialize(obj);
            // strReturn = FixDate(strReturn);

            return strReturn;
        }

        /// <summary> 
        /// 控制深度 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <param name="recursionDepth">深度</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.RecursionLimit = recursionDepth;

            string strReturn = serialize.Serialize(obj);
            // strReturn = FixDate(strReturn);

            return strReturn;
        }

        /// <summary> 
        /// DataTable转为json 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            return ToJson(dic);
        }


    }

}
