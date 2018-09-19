using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Core.Service.Search;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface ISearchApi : IHttpApi
        {
            Task<SearchResults<U>> Search<T, U>(HttpRequestMessage req, TraceWriter log) where T : BaseModel where U : class;
        }

        public class SearchApi : ISearchApi
        {
            private readonly ISearchService _searchService;
            private readonly IUserDigestService _userDigestService;

            public SearchApi(ISearchService searchService,
                IUserDigestService userDigestService)
            {
                _searchService = searchService;
                _userDigestService = userDigestService;
            }

            public async Task<SearchResults<U>> Search<T, U>(HttpRequestMessage req, TraceWriter log) where T : BaseModel where U : class
            {
                var token = req.GetQueryNameValuePairs().Where(w => w.Key == "token").FirstOrDefault().Value;
                var sort = req.GetQueryNameValuePairs().Where(w => w.Key == "sort").FirstOrDefault().Value;
                var sortAscending = req.GetQueryNameValuePairs().Where(w => w.Key == "sortAscending").FirstOrDefault().Value;
                var pageSize = req.GetQueryNameValuePairs().Where(w => w.Key == "pageSize").FirstOrDefault().Value;

                var filterNames = req.GetQueryNameValuePairs().Where(w => w.Key == "f").Select(q => q.Value).ToList();
                var values = req.GetQueryNameValuePairs().Where(w => w.Key == "v").Select(q => q.Value).ToList();

                var filters = new List<GridQueryFilter>();
                for (int i = 0; i < filterNames.Count; i++)
                {
                    filters.Add(new GridQueryFilter() { column = filterNames[i], value = values.ElementAtOrDefault(i) });
                }

                var gridQuery = new GridQuery()
                {
                    filters = filters,
                    token = token,
                    sort = sort,
                };

                int page;
                if (string.IsNullOrWhiteSpace(pageSize) == false && int.TryParse(pageSize, out page))
                {
                    gridQuery.pageSize = page;
                }

                bool sortAsc = false;
                bool.TryParse(sortAscending, out sortAsc);
                gridQuery.sortAscending = sortAsc;

                var container = ContainerRegistrar.GetContainer();
                var queryBuilder = container.GetInstance<IQueryBuilder<T, U>>();

                var userDigest = await _userDigestService.CurrentUserAsync(req);
                var searchResults = await _searchService.Search<T, U>(queryBuilder, token, gridQuery, userDigest);

                return searchResults;
            }
        }
    }