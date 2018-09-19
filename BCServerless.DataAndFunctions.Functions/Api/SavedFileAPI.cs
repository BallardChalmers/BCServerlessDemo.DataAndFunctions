using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IFileApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);
    }

    public class FileApi : IFileApi
    {
        private readonly IDocumentDBRepository<SavedFile> _savedFileRepository;
        private readonly IFileService _fileService;

        public FileApi(IDocumentDBRepository<SavedFile> savedFileRepository, IFileService fileService)
        {
            _savedFileRepository = savedFileRepository;
            _fileService = fileService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            var fileId = req.GetQueryNameValuePairs().Where(w => w.Key == "fileId").First().Value;
            if (string.IsNullOrWhiteSpace(fileId))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var savedFile = await _savedFileRepository.GetItemAsync(fileId);
            if (savedFile == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var mediaResponse = await _savedFileRepository.GetAttachmentMediaAsync(fileId, savedFile.attachmentId);

            if (mediaResponse == null)
            {
                req.CreateResponse(HttpStatusCode.NoContent);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(mediaResponse.Media);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = savedFile.fileName;
            return response;
        }

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("FileApi.Post method called.");

            if (req.Content.IsMimeMultipartContent() == false)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            Stream fileStream = null;
            var multipartStreamProvider = await req.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());

            string documentId = string.Empty, fileName = string.Empty, name = string.Empty;

            foreach (HttpContent content in multipartStreamProvider.Contents)
            {
                Stream stream = content.ReadAsStreamAsync().Result;

                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    fileName = content.Headers.ContentDisposition.FileName.Trim('"');
                    log.Info($"Filename: {content.Headers.ContentDisposition.FileName}");
                    fileStream = stream;
                }
                else
                {
                    var contents = new StreamReader(stream).ReadToEnd();
                    if (content.Headers.ContentDisposition.Name == "\"documentId\"")
                    {
                        documentId = contents;
                    }
                    if (content.Headers.ContentDisposition.Name == "\"name\"")
                    {
                        name = contents;
                    }
                }
            }

            var savedFile = await _fileService.AddFileAsync(fileStream, documentId, fileName, name, req);
            return req.CreateResponse(HttpStatusCode.Created, savedFile);
        }
    }
}
