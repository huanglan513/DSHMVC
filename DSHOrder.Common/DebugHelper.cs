using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace DSHOrder.Common
{
    public class DebugHelper
    {
        public static void WriteObject(object o)
        {
            var entityType = o.GetType();

            var ps = from p in entityType.GetProperties()
                     where p.CanWrite
                     select p;

            foreach (var entityProperty in ps)
            {
                try
                {
                    var value = entityProperty.GetValue(o, null);
                    System.Diagnostics.Debug.Write(string.Concat(entityProperty.Name, ":"));
                    System.Diagnostics.Debug.Write(value.ToString());
                }
                catch 
                {
                    System.Diagnostics.Debug.Write("[发生异常]");
                }
                finally
                {
                    System.Diagnostics.Debug.Write("\n");
                }
            }

            if (o is NameValueCollection)
            {
                var nvc = o as NameValueCollection;

                foreach (var item in nvc.AllKeys)
                {
                    try
                    {
                        System.Diagnostics.Debug.Write(string.Concat(item, ":"));
                        System.Diagnostics.Debug.Write(nvc[item]);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.Write("[发生异常]");
                    }
                    finally
                    {
                        System.Diagnostics.Debug.Write("\n");
                    }
                }
            }
        }

    }
}
