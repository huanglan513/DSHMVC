using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Xml.Serialization;
using System.Data.Objects.DataClasses;

namespace DSHOrder.Common
{
    public static class EntityObjectHelper
    {
        private static object _LockObject = new object();

        private static ConcurrentDictionary<Type, Type> s_dynamicTypes = new ConcurrentDictionary<Type, Type>();

        private static Func<Type, Type> s_dynamicTypeCreator = new Func<Type, Type>(CreateDynamicType);

        public static object ToDynamic(this EntityObject o, bool IsSimple = false) 
        {
            lock (_LockObject)
            {
                var entityType = o.GetType();
                _Properties = GetProperties(entityType, IsSimple);

                var dynamicType = s_dynamicTypes.GetOrAdd(entityType, s_dynamicTypeCreator);

                var dynamicObject = Activator.CreateInstance(dynamicType);
                foreach (var entityProperty in _Properties)
                {
                    var value = entityProperty.GetValue(o, null);
                    dynamicType.GetField(entityProperty.Name).SetValue(dynamicObject, value);
                }

                return dynamicObject;
            }
        }


        private static Type CreateDynamicType(Type type)
        {
            var asmName = new AssemblyName("DynamicAssembly_" + Guid.NewGuid());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule("DynamicModule_" + Guid.NewGuid());

            var typeBuilder = moduleBuilder.DefineType(type.GetType() + "$DynamicType", TypeAttributes.Public);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            foreach (var entityProperty in _Properties)
            {
                typeBuilder.DefineField(entityProperty.Name, entityProperty.PropertyType, FieldAttributes.Public);
            }

            return typeBuilder.CreateType();
        }

        private static PropertyInfo[] _Properties = null;
        private static PropertyInfo[] GetProperties(Type type, bool IsSimple)
        {
            var ps = type.GetProperties();

            if (!IsSimple)
            {
                return ps;
            }
            else
            {
                var r = (from p in ps
                         where p.PropertyType.BaseType.FullName == "System.ValueType"
                            || p.PropertyType.FullName == "System.String"
                         select p).ToArray();

                return r;
            }
        }


        public static T FixEntity<T>(this T eo) where T : EntityObject
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(eo.GetType());
                serializer.Serialize(stream, eo);

                stream.Seek(0, SeekOrigin.Begin);

                T eoReturn = (T)serializer.Deserialize(stream);

                return eoReturn;
            }
        }


    }
}
