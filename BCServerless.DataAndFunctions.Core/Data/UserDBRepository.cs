using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data
{
    public interface IUserDBRepository
    {
        IQueryable<UserDB> GetQueryable(int maxItems);
        IQueryable<UserDB> GetQueryable(int maxItems, string requestContinuation);
        Task<IEnumerable<S>> GetResultsAsync<S>(IQueryable<S> query);
        Task<UserDB> GetItemAsync(string id);
        Task<IEnumerable<UserDB>> GetItemsAsync();
        Task<IEnumerable<UserDB>> GetItemsAsync(Expression<Func<UserDB, bool>> predicate);
        Task<UserDB> CreateItemAsync(UserDB item, IUserDigestService userDigestService, HttpRequestMessage req);
        Task<UserDB> CreateOrUpdateItemAsync(UserDB item, IUserDigestService userDigestService, HttpRequestMessage req);
        Task<UserDB> UpdateItemAsync(string id, UserDB item, IUserDigestService userDigestService, HttpRequestMessage req);
    }

    public class UserDBRepository : DocumentDBRepository<UserDB>, IUserDBRepository
    {
        public UserDBRepository(IDocumentClient documentClient, IApplicationConfig applicationConfig) : base(documentClient, applicationConfig, null)
        {    

        }

        public async Task<UserDB> CreateOrUpdateItemAsync(UserDB item, IUserDigestService userDigestService, HttpRequestMessage req)
        {
            _userDigestService = userDigestService;
            var dbresult = await CreateOrUpdateItemAsync(item, req);
            return dbresult;
        }

        public async Task<UserDB> CreateItemAsync(UserDB item, IUserDigestService userDigestService, HttpRequestMessage req)
        {
            _userDigestService = userDigestService;
            var createResult = await CreateItemAsync(item, req);
            return createResult;
        }

        public async Task<UserDB> UpdateItemAsync(string id, UserDB item, IUserDigestService userDigestService, HttpRequestMessage req)
        {
            _userDigestService = userDigestService;
            var updateResult = await UpdateItemAsync(id, item, req);
            return updateResult;
        }     
    }
}
