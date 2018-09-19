using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public static class SearchUtilities
    {
        public static string[] GetIdsFromNameValuePairs(string filterValue)
        {
            return filterValue.Split(new[] { '~' }).Select(s => s.Substring(0, s.IndexOf("|"))).ToArray();
        }

        public static T[] GetEnumsFromValue<T>(string filterValue)
        {
            return filterValue.Split(new[] { '~' }).Select(s => (T)Enum.Parse(typeof(T), s)).ToArray();
        }

        public static string[] GetStringsFromList(string filterValue)
        {
            return filterValue.Split(new[] { '~' });
        }
    }
}
