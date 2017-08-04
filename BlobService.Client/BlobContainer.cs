using BlobService.Client.DTO;
using BlobService.Client.Helpers;
using BlobService.Client.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class BlobContainer
    {
        private IBlobServiceContainersAPI _containersApi;

        internal BlobContainer(string containerName, string id, IBlobServiceClient blobServiceClient)
        {
            Utility.AssertNotNullOrEmpty(nameof(containerName), containerName);
            Utility.AssertNotNullOrEmpty(nameof(id), id);
            this.Name = containerName;
            this.ServiceClient = blobServiceClient;
            this.Id = id;
            _containersApi = ((BlobServiceClient)blobServiceClient).ContainersApiClient;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public IBlobServiceClient ServiceClient { get; private set; }

        public virtual Task DeleteAsync()
        {
            return this.DeleteAsync(CancellationToken.None);
        }

        public virtual async Task DeleteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var serviceResponse = await _containersApi.DeleteContainerAsync(this.Id);

            if (false == serviceResponse.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.Content?.ToString());
            }
        }

        public string GetSharedAccessSignature()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<Blob>> ListBlobsAsync()
        {
            return this.ListBlobsAsync(CancellationToken.None);
        }

        public virtual async Task<IEnumerable<Blob>> ListBlobsAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw ExceptionGenerator.InvalidOperationException("ListBlobs", this.Name, "Container");
            }

            var serviceResponse = await _containersApi.ListContainerBlobsAsync(this.Id);
            cancellationToken.ThrowIfCancellationRequested();

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent.ToString());
            }

            var containerBlobDTOs = serviceResponse.GetContent();
            return containerBlobDTOs.Select(b => new Blob(this.ServiceClient, this, b.Id, b.OrigFileName, b.MimeType, b.SizeInBytes, b.DownloadRelativeUrl));
        }

        public virtual Task<Blob> UploadBlobAsync(Stream file, string fileName)
        {
            string contentType = MIMETypeMapper.GetMimeMapping(fileName);
            return UploadBlobAsync(file, fileName, contentType);
        }

        public virtual Task<Blob> UploadBlobAsync(Stream file, string fileName, string contentType)
        {
            return UploadBlobAsync(file, fileName, contentType, CancellationToken.None);
        }

        public virtual async Task<Blob> UploadBlobAsync(Stream file, string fileName, string contentType, CancellationToken cancellationToken)
        {
            var contentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"" };
            var contentMimeType = new MediaTypeHeaderValue(contentType);
            var serviceResponse = await _containersApi.AddBlobAsync(this.Id, contentDisposition, contentMimeType, file);

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent);
            }
            var newBlob = serviceResponse.GetContent();
            return new Blob(this.ServiceClient, this, newBlob.Id, newBlob.OrigFileName,
                                newBlob.MimeType, newBlob.SizeInBytes, newBlob.DownloadRelativeUrl);
        }

        public virtual Task RenameAsync(string newName)
        {
            return RenameAsync(newName, CancellationToken.None);
        }

        public virtual async Task RenameAsync(string newName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var containerDto = new ContainerDTO()
            {
                Id = Id,
                Name = newName
            };
            var serviceResponse = await _containersApi.UpdateContainerAsync(containerDto);

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent?.ToString());
            }

            var updatedContainer = serviceResponse.GetContent();

            this.Id = updatedContainer.Id;
            this.Name = updatedContainer.Name;
        }
    }
}