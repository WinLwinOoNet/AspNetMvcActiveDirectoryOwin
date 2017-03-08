using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetMvcActiveDirectoryOwin.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get static class's field values
        /// </summary>
        /// <param name="target">Type of static class</param>
        /// <returns>List of string</returns>
        public static IList<string> GetFieldValues(this Type target)
        {
            return target.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => x.GetValue(null).ToString())
                .ToList();
        }
    }
}
