using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BlobService.Client
{
    public class BlobServiceClient : IBlobServiceClient
    {
        public string Endpoint { get; set; }

        private IBlobServiceApi _blobApi;
        public BlobServiceClient(string endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _blobApi = RestClient.For<IBlobServiceApi>(endpoint);
        }

        public async Task<Blob> GetBlobByIdAsync(string blobId)
        {
            var blob = await _blobApi.GetBlobByIdAsync(blobId);
            return blob;
        }

        public async Task DeleteBlobAsync(string blobId)
        {
            await _blobApi.DeleteBlobAsync(blobId);
        }

        public async Task<Container> GetContainerByIdAsync(string containerId)
        {
            var container = await _blobApi.GetContainerByIdAsync(containerId);
            return container;
        }

        public async Task<Container> GetContainerByNameAsync(string containerName, bool creatIfDoesntExist = true)
        {
            var container = await _blobApi.GetContainerByNameAsync(containerName);
            if(container == null)
            {
                container = await AddContainerAsync(containerName);
            }
            return container;
        }

        public async Task<Container> AddContainerAsync(string containerName)
        {
            var container = await _blobApi.AddContainerAsync(new Container()
            {
                Name = containerName
            });
            return container;
        }

        public async Task DeleteContainerAsync(string containerId)
        {
            await _blobApi.DeleteContainerAsync(containerId);
        }

        public async Task<IEnumerable<Blob>> ListBlobsAsync(string containerId)
        {
            var blobs = await _blobApi.ListBlobsAsync(containerId);
            return blobs;
        }

        public async Task<Blob> AddBlobAsync(string containerId, string fileName, Stream file)
        {
            var contentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"" };
            var contentType = new MediaTypeHeaderValue("application/octet-stream");
            var blob = await _blobApi.AddBlobAsync(containerId, contentDisposition, contentType, file);
            return blob;
        }

        public async Task<Blob> UpdateBlobAsync(string blobId, string fileName, Stream file)
        {
            var contentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"" };
            var contentType = new MediaTypeHeaderValue("application/octet-stream");
            var blob = await _blobApi.UpdateBlobAsync(blobId, contentDisposition, contentType, file);
            return blob;
        }
    }
}
