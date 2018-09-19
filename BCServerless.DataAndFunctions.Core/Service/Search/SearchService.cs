using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public interface ISearchService
    {
        Task<SearchResults<U>> Search<T, U>(IQueryBuilder<T, U> queryBuilder, string token, GridQuery gridQuery, UserDigest userDigest, bool includeCounts = true)
            where T : BaseModel
            where U : class;
    }

    public class SearchService : ISearchService
    {
        private readonly IDocumentClient _documentClient;
        private readonly IApplicationConfig _applicationConfig;
        private readonly IUserDigestService _userDigestService;

        public SearchService(IDocumentClient documentClient, IApplicationConfig applicationConfig, IUserDigestService userDigestService)
        {
            _documentClient = documentClient;
            _applicationConfig = applicationConfig;
            _userDigestService = userDigestService;
        }                    

        public async Task<SearchResults<U>> Search<T, U>(IQueryBuilder<T, U> queryBuilder, string token, GridQuery gridQuery, UserDigest userDigest, bool includeCounts = true) where T : BaseModel where U : class
        {
            var repo = new DocumentDBRepository<T>(_documentClient, _applicationConfig, _userDigestService);
            var searchResults = new SearchResults<U>();
            IQueryable<T> query;

            token = token ?? gridQuery?.token;
            if (string.IsNullOrEmpty(token))
            {
                query = repo.GetQueryable(gridQuery?.pageSize ?? -1);
            }
            else
            {
                var continuationToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
                query = repo.GetQueryable(gridQuery?.pageSize ?? -1, continuationToken);
            }
           
            var resultQuery = queryBuilder.GetQuery(query, gridQuery, userDigest);

            IDocumentQuery<U> docQuery = resultQuery.AsDocumentQuery();

            if (docQuery.HasMoreResults)
            {
                var result = await docQuery.ExecuteNextAsync<U>();
                var list = new List<U>();

                list.AddRange(result);
                searchResults.results = list;

                if (!string.IsNullOrEmpty(result.ResponseContinuation))
                {
                    var continuationToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(result.ResponseContinuation));

                    searchResults.token = continuationToken;
                }
            }

            if (includeCounts)
            {
                var countQuery = repo.GetQueryable();
                var countResultsQuery = queryBuilder.GetQuery(countQuery, gridQuery, userDigest);

                searchResults.count = await countResultsQuery.CountAsync();

                var totalQuery = repo.GetQueryable();
                var totalResultsQuery = queryBuilder.GetTotal(totalQuery, userDigest);

                searchResults.total = await totalResultsQuery.CountAsync();
            }

            return searchResults;
        }        
    }
}
