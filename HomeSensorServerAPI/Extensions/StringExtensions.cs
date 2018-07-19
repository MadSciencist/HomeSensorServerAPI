using System;

namespace HomeSensorServerAPI.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string s) where T : struct
        {
            return Enum.TryParse(s, out T newValue) ? newValue : default(T);
        }
    }
}
