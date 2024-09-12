using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CUAFunding.Common.Helpers
{
    public static class DynamicHelper
    {
        public static bool IsValidProperty(this System.Type entityType, string propertyName, bool throwExceptionIfNotFound = true)
        {
            var prop = entityType.GetProperty(
                propertyName,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance);

            if (prop == null && throwExceptionIfNotFound)
            {
                throw new NotSupportedException($"Property {propertyName} doesn`t exist.");
            }
            var isValid = prop != null;

            return isValid;
        }

    }
}
