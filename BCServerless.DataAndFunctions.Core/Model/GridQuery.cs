using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Model
{
    public class GridQueryFilter
    {
        public string column { get; set; }
        public string value { get; set; }
    }

    public class GridQuery
    {
        public GridQuery()
        {
            filters = new List<GridQueryFilter>();
            fixedFilters = new List<GridQueryFilter>();
        }

        public string sort { get; set; }
        public bool sortAscending { get; set; }
        public List<GridQueryFilter> filters { get; set; }
        public List<GridQueryFilter> fixedFilters { get; set; }
        public int? pageSize { get; set; }
        public string token { get; set; }
    }
}
