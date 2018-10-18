using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IOrganisationPhotoApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);
    }

    public class OrganisationPhotoApi : IOrganisationPhotoApi
    {
        private readonly IDocumentDBRepository<Organisation> _organisationRepository;
        private readonly IOrganisationService _organisationService;

        public OrganisationPhotoApi(IOrganisationService organisationService,
            IDocumentDBRepository<Organisation> organisationRepository)
        {
            _organisationService = organisationService;
            _organisationRepository = organisationRepository;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            var documentId = req.GetQueryNameValuePairs().Where(w => w.Key == "documentId").First().Value;
            var PhotoId = req.GetQueryNameValuePairs().Where(w => w.Key == "photoId").First().Value;
            if (string.IsNullOrWhiteSpace(documentId) || string.IsNullOrWhiteSpace(PhotoId))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var mediaResponse = await _organisationRepository.GetAttachmentMediaAsync(documentId, PhotoId);
            if (mediaResponse == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(mediaResponse.Media);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            if (req.Content.IsMimeMultipartContent() == false)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            Stream fileStream = null;
            var streamProvider = await req.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());
            Organisation organisation = null;
            var filename = string.Empty;
            string organisationId = string.Empty;

            foreach (HttpContent content in streamProvider.Contents)
            {
                Stream stream = content.ReadAsStreamAsync().Result;
                string name = content.Headers.ContentDisposition.FileName;

                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    filename = content.Headers.ContentDisposition.FileName;
                    log.Info($"Filename: {content.Headers.ContentDisposition.FileName}");
                    fileStream = stream;
                }
                else
                {
                    var contents = new StreamReader(stream).ReadToEnd();
                    if (content.Headers.ContentDisposition.Name == "\"organisation\"")
                    {
                        organisation = JsonConvert.DeserializeObject<Organisation>(contents);
                    }

                    log.Info($"{content.Headers.ContentDisposition.Name}: {contents}");
                }
            }

            if (string.IsNullOrWhiteSpace(organisation.id))
            {
                organisation = await _organisationService.CreateAsync(organisation, req);
            }

            var attachment = await _organisationRepository.AddAttachment(organisation.id, fileStream, "image/jpeg", filename);

            organisation.PhotoId = attachment.Resource.Id;
            await _organisationService.UpdateAsync(organisation, req);

            return req.CreateResponse(HttpStatusCode.OK, organisation);
        }
    }
}