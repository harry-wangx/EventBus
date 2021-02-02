using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.EventBus
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }

        public static string GetFullName(this Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                return $"{type.FullName.Remove(type.FullName.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                return type.FullName;
            }
        }

        public static string GetFullName(this object @object)
        {
            return @object.GetType().GetFullName();
        }
    }
}
