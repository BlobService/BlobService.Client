using BlobService.Client.DTO;
using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlobService.Client.UnitTests.Mocks
{
    public class ContainersApiMock : IBlobServiceContainersAPI
    {
        private ApiMockOptions _options;
        private SeedData _seedData;

        public ContainersApiMock(SeedData data, ApiMockOptions options = null)
        {
            _options = options ?? new ApiMockOptions();
            this._seedData = data;
        }

        public Task<Response<BlobDTO>> AddBlobAsync([Path] string containerId,
            [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition,
            [Header("Content-Type")] MediaTypeHeaderValue contentType,
            [Body] Stream file)
        {
            Response<BlobDTO> response = null;
            if (_options.ThrowInternalServerError)
            {
                response = new Response<BlobDTO>("Very Bad Error",
                    new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError),
                    () => null);
            }
            else
            {
                var newId = Guid.NewGuid().ToString("D");
                var newBlob = new BlobDTO()
                {
                    ContainerId = containerId,
                    Id = newId,
                    MimeType = contentType.MediaType,
                    OrigFileName = contentDisposition.FileName,
                    SizeInBytes = file.Length,
                    DownloadRelativeUrl = new Uri($"/blobs/{newId}/raw", UriKind.Relative)
                };
                _seedData.Blobs.Add(newBlob);

                response = new Response<BlobDTO>("Very Bad Error",
                    new HttpResponseMessage(HttpStatusCode.OK),
                    () => newBlob);
            }
            return Task.FromResult(response);
        }

        public Task<Response<ContainerDTO>> CreateContainerAsync([Body] ContainerDTO container)
        {
            container.Id = Guid.NewGuid().ToString("D");
            this._seedData.Containers.Add(container);
            var apiResponse = new Response<ContainerDTO>("TestContent",
                new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                () => container);
            return Task.FromResult(apiResponse);
        }

        public Task<HttpResponseMessage> DeleteContainerAsync([PathAttribute] string id)
        {
            if (this._options.ThrowInternalServerError)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
            }

            HttpResponseMessage response = null;
            var container = _seedData.Containers.Where(c => c.Id == id).FirstOrDefault();

            if (container != null)
            {
                _seedData.Containers.Remove(container);
                response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            else
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return Task.FromResult(response);
        }

        public Task<Response<ContainerDTO>> GetContainerByNameAsync([PathAttribute] string name)
        {
            var container = _seedData.Containers.Where(c => c.Name == name).FirstOrDefault();
            Response<ContainerDTO> response = null;
            if (container != null)
            {
                response = new Response<ContainerDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => container);
            }
            else
            {
                response = new Response<ContainerDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.NotFound), () => new ContainerDTO());
            }

            return Task.FromResult(response);
        }

        public Task<Response<IEnumerable<BlobDTO>>> ListContainerBlobsAsync([Path] string id)
        {
            var container = _seedData.Containers.Where(c => c.Id == id).FirstOrDefault();
            Response<IEnumerable<BlobDTO>> response = null;
            if (container != null)
            {
                var blobs = _seedData.Blobs.Where(b => b.ContainerId == container.Id).AsEnumerable();
                response = new Response<IEnumerable<BlobDTO>>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => blobs);
            }
            else
            {
                response = new Response<IEnumerable<BlobDTO>>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.NotFound), () => Enumerable.Empty<BlobDTO>());
            }

            return Task.FromResult(response);
        }

        public Task<Response<IEnumerable<ContainerDTO>>> ListContainersAsync()
        {
            var response = new Response<IEnumerable<ContainerDTO>>("TestContent",
                new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                () => _seedData.Containers.AsEnumerable());
            return Task.FromResult(response);
        }

        public Task<Response<ContainerDTO>> UpdateContainerAsync([Body] ContainerDTO container)
        {
            var targetContainer = _seedData.Containers.Where(c => c.Id == container.Id).FirstOrDefault();
            Response<ContainerDTO> response = null;
            if (targetContainer != null)
            {
                targetContainer.Name = container.Name;
                response = new Response<ContainerDTO>("Test",
                    new HttpResponseMessage(HttpStatusCode.OK),
                    () => targetContainer);
            }
            else
            {
                response = new Response<ContainerDTO>("Test",
                    new HttpResponseMessage(HttpStatusCode.NotFound),
                    () => new ContainerDTO());
            }

            return Task.FromResult(response);
        }
    }
}