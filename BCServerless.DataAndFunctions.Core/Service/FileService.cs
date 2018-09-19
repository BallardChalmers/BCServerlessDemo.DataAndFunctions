using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IFileService
    {
        Task<SavedFile> AddFileAsync(Stream fileStream, string documentId, string fileName, string name, HttpRequestMessage req);
    }

    public class FileService : IFileService
    {
        private readonly IDocumentDBRepository<SavedFile> _SavedFileRepository;

        public FileService(IDocumentDBRepository<SavedFile> SavedFileRepository)
        {
            _SavedFileRepository = SavedFileRepository;
        }

        public async Task<SavedFile> AddFileAsync(Stream fileStream, string documentId, string fileName, string name, HttpRequestMessage req)
        {
            if (string.IsNullOrEmpty(documentId))
            {
                documentId = Guid.NewGuid().ToString();
            }

            var SavedFile = new SavedFile()
            {
                name = string.IsNullOrEmpty(name) ? fileName : name,
                fileName = fileName,
                documentId = documentId,
            };
            SavedFile = await _SavedFileRepository.CreateItemAsync(SavedFile, req);

            var resourceResponse = await _SavedFileRepository.AddAttachment(SavedFile.id, fileStream, "application/octet-stream", SavedFile.fileName);
            SavedFile.attachmentId = resourceResponse.Resource.Id;
            SavedFile = await _SavedFileRepository.UpdateItemAsync(SavedFile.id, SavedFile, req);

            return SavedFile;
        }

    }
}
