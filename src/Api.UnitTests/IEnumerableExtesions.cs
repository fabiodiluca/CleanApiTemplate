﻿using System.Collections.Generic;

namespace Api.UnitTests
{
    public static class IEnumerableExtesions
    {
        public static List<T> ToList<T>(this IEnumerable<T> enumerable)
        {
            return new List<T>(enumerable);
        }
    }
}
