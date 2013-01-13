using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLRoller.Specify
{
    public static class Extensions
    {
        public static IList<T> GetCustomAttributes<T>(this PropertyInfo pi, bool inherit)
        {
            var customAttributes = pi.GetCustomAttributes(typeof (T), inherit);
            return customAttributes.Select(customAttribute => (T) customAttribute).ToList();
        }
    }
}