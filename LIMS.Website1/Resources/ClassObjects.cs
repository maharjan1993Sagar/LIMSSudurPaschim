using LIMS.Website1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace LIMS.Website1.Resources
{
    public static class ClassObjects
    {
        //Get a List of the properties from a type
        public static PropertyInfo[] ListOfPropertiesFromInstance(Type AType)
        {
            if (AType == null) return null;
            return AType.GetProperties(BindingFlags.Public);
        }

        public static string GetDisplayName(PropertyInfo propInfo)
        {
            DisplayNameAttribute dp = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault();
            if (dp != null)
            {
                return dp.DisplayName;

            }
            else
            {
                return null;
            }
        }

        //Get a List of the properties from a instance of a class
        public static PropertyInfo[] ListOfPropertiesFromInstance(object InstanceOfAType)
        {
            if (InstanceOfAType == null) return null;
            Type TheType = InstanceOfAType.GetType();
            return TheType.GetProperties(BindingFlags.Public);
        }

        //purrfect for usage example and Get a Map of the properties from a instance of a class
        public static Dictionary<string, PropertyInfo> DictionaryOfPropertiesFromInstance(object InstanceOfAType)
        {
            if (InstanceOfAType == null) return null;
            Type TheType = InstanceOfAType.GetType();
            PropertyInfo[] Properties = TheType.GetProperties(BindingFlags.Public);
            Dictionary<string, PropertyInfo> PropertiesMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo Prop in Properties)
            {
                PropertiesMap.Add(Prop.Name, Prop);
            }
            return PropertiesMap;
        }

        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace.ToLower(), nameSpace.ToLower(), StringComparison.OrdinalIgnoreCase))
                      .ToArray();
        }
    }
}
