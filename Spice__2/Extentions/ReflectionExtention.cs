using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Extentions
{
    public static class ReflectionExtention
    {
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
    }
}
