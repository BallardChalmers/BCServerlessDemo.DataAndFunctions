using System.Collections.Generic;

namespace BCServerlessDemo.DataAndFunctions.Core.Model
{
    public class SearchResults<T>
    {
        public IList<T> results { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public string token { get; set; }
    }
}
