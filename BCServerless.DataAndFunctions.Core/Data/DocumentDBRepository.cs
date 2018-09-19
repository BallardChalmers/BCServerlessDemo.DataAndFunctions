using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data
{
    public interface IDocumentDBRepository<T> where T : BaseModel
    {
        IQueryable<T> GetQueryable(int maxItems);
        IQueryable<T> GetQueryable(int maxItems, string requestContinuation);
        Task<IEnumerable<S>> GetResultsAsync<S>(IQueryable<S> query);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task InitializeAsync();
        Task<T> CreateOrUpdateItemAsync(T item, HttpRequestMessage req);
        Task<T> CreateItemAsync(T item, HttpRequestMessage req);
        Task<T> UpdateItemAsync(string id, T item, HttpRequestMessage req);
        Task<T> DeleteItemAsync(string id, T item, HttpRequestMessage req);
        Task<ResourceResponse<Attachment>> AddAttachment(string id, System.IO.Stream attachment, string contentType, string filename);
        Task<MediaResponse> GetAttachmentMediaAsync(string documentId, string attachmentId);
    }

    public class DocumentDBRepository<T> : IDocumentDBRepository<T> where T : BaseModel
    {
        protected readonly IDocumentClient _documentClient;
        protected readonly IApplicationConfig _applicationConfig;
        protected IUserDigestService _userDigestService;

        protected string TypeName => typeof(T).Name;
        protected Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(_applicationConfig.Database, _applicationConfig.Collection);
        public ChangeRecord changeRecord;

        public DocumentDBRepository(IDocumentClient documentClient, IApplicationConfig applicationConfig, IUserDigestService userDigestService)
        {
            _documentClient = documentClient;
            _applicationConfig = applicationConfig;
            _userDigestService = userDigestService;
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine("Endpoint: " + _applicationConfig.Endpoint);
            Console.WriteLine("authKey: " + _applicationConfig.AuthKey);

            await CreateDatabaseIfNotExistsAsync();
            await CreateCollectionIfNotExistsAsync(_applicationConfig.Collection);
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(GetDocumentUri(id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _documentClient.CreateDocumentQuery<T>(CollectionUri, new FeedOptions { MaxItemCount = -1 }).Where(predicate);
            query = query.Where(w => w.type == TypeName);

            var documentQuery = query.AsDocumentQuery();

            List<T> results = new List<T>();
            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            //Note - The AsDocumentQuery() method takes no notice of the Json Serializer settings so produces broken queries unless the object properties are lower case!
            IDocumentQuery<T> query = _documentClient.CreateDocumentQuery<T>(CollectionUri, new FeedOptions { MaxItemCount = -1 }).Where(w => w.type == TypeName).AsDocumentQuery();
            

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public IQueryable<T> GetQueryable(int maxItems = -1)
        {
            return _documentClient.CreateDocumentQuery<T>(CollectionUri,
                new FeedOptions { MaxItemCount = maxItems })
                .Where(w => w.type == TypeName);
        }

        public IQueryable<T> GetQueryable(int maxItems, string requestContinuation)
        {
            return _documentClient.CreateDocumentQuery<T>(CollectionUri,
                new FeedOptions { MaxItemCount = maxItems, RequestContinuation = requestContinuation })
                .Where(w => w.type == TypeName);
        } 

        public async Task<IEnumerable<S>> GetResultsAsync<S>(IQueryable<S> query)
        {
            var documentQuery = query.AsDocumentQuery();
            List<S> results = new List<S>();

            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<S>());
            }

            return results;
        }

        public async Task<T> CreateOrUpdateItemAsync(T item, HttpRequestMessage req)
        {
            return (item.id == null || GetItemAsync(item.id).Result == null)
                ? await CreateItemAsync(item, req)
            : await UpdateItemAsync(item.id, item, req);
        }

        public async Task<T> CreateItemAsync(T item, HttpRequestMessage req)
        {
            
            //TODO: Should be trigger or stored proc.
            var digest = await _userDigestService.CurrentUserAsync(req);
            var now = DateTime.Now;

            item.changedById = digest.Id;
            item.changedByName = digest.DisplayName;
            item.changedOn = now;
            
            var document = await _documentClient.CreateDocumentAsync(CollectionUri, item);
            
            var changeRecord = await AddChangeRecord($"Added {item.name}", digest, "Created", null, item, now);
            var newItem = (T)(dynamic)document.Resource;

            return newItem;
        }

        public async Task<T> DeleteItemAsync(string id, T item, HttpRequestMessage req)
        {
            //TODO: Should be trigger or stored proc.
            var before = await GetItemAsync(id);
            var digest = await _userDigestService.CurrentUserAsync(req);
            var now = DateTime.Now;

            item.changedById = digest.Id;
            item.changedByName = digest.DisplayName;
            item.changedOn = now;

            var doc = await _documentClient.ReplaceDocumentAsync(GetDocumentUri(id), item);

            var changeRecord = await AddChangeRecord($"Removed {item.name}", digest, "Deleted", before, null, now);

            var deletedItem = (T)(dynamic)doc.Resource;
            return deletedItem;
        }

        public async Task<T> UpdateItemAsync(string id, T item, HttpRequestMessage req)
        {
            //TODO: Should be trigger or stored proc.
            var before = await GetItemAsync(id);
            var digest = await _userDigestService.CurrentUserAsync(req);
            var now = DateTime.Now;

            item.changedById = digest.Id;
            item.changedByName = digest.DisplayName;
            item.changedOn = now;

            var doc = await _documentClient.ReplaceDocumentAsync(GetDocumentUri(id), item);

            var changeRecord = await AddChangeRecord($"Changed {item.name}", digest, "Updated", before, item, now);

            var updatedItem = (T)(dynamic)doc.Resource;
            return updatedItem;
        }

        public async Task<ResourceResponse<Attachment>> AddAttachment(string id, System.IO.Stream attachment, string contentType, string filename)
        {
            //TODO: Change tracking
            return await _documentClient.CreateAttachmentAsync(UriFactory.CreateDocumentUri(_applicationConfig.Database, _applicationConfig.Collection, id), attachment,
                new MediaOptions { ContentType = contentType });
        }

        public async Task<MediaResponse> GetAttachmentMediaAsync(string documentId, string attachmentId)
        {
            var attachmentResponse = await _documentClient.ReadAttachmentAsync(UriFactory.CreateAttachmentUri(_applicationConfig.Database, _applicationConfig.Collection,
                documentId, attachmentId));
            MediaResponse response = null;

            if (attachmentResponse != null)
            {
                response = await _documentClient.ReadMediaAsync(attachmentResponse.Resource.MediaLink);
            }

            return response;
        }

        private async Task<ChangeRecord> AddChangeRecord(string name, UserDigest user, string action, T before, T after, DateTime now)
        {

            var changeRecord = new ChangeRecord
            {
                name = name,
                changedById = user.Id,
                changedByName = user.DisplayName,
                changedOn = now,
                subjectDescription = after != null ? after.type : before.type,
                subjectId = after != null ? after.id : before.id,
                subjectType = after != null ? after.type : before.type,
                action = action,
                before = before,
                after = after
            };         

            var doc = await _documentClient.CreateDocumentAsync(CollectionUri, changeRecord);
            return (ChangeRecord)(dynamic)doc.Resource;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_applicationConfig.Database));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Database { Id = _applicationConfig.Database });
                }
                else
                {
                    throw;
                }
            }
        }

        private  async Task CreateCollectionIfNotExistsAsync(string CollectionId)
        {
            try
            {
                await _documentClient.ReadDocumentCollectionAsync(CollectionUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_applicationConfig.Database),
                        new DocumentCollection { Id = _applicationConfig.Collection, IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 }) },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }  

        private Uri GetDocumentUri(string documentId)
        {        
            return UriFactory.CreateDocumentUri(_applicationConfig.Database, _applicationConfig.Collection, documentId);
        }        

    }
}
