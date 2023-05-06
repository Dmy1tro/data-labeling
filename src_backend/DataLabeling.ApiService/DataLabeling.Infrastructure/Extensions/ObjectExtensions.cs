using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataLabeling.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsEmpty<T>(this T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                return true;
            }

            switch (value)
            {
                case string str when string.IsNullOrEmpty(str):
                    return true;

                case ICollection c when c.Count == 0:
                case Array a when a.Length == 0:
                case IEnumerable e when e.Cast<object>().Any() == false:
                    return true;
            }

            return false;
        }

        public static bool IsNullOrDefault<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default);
        }
    }
}
