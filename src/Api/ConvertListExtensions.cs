using System;
using System.Linq;

namespace CleanTemplate.Api
{
    public static class ConvertListExtensions
    {
        public static int[] ToInt32Array(this string Ids)
        {
            if (string.IsNullOrEmpty(Ids))
            {
                return new int[] { };
            } 
            else
            {
                var parsedIds = Ids.Split(',');
                var idList = parsedIds.Select(x => Convert.ToInt32(x)).ToArray();
                return idList;
            }
        }
    }
}
