using BlobService.Client.DTO;
using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
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
            throw new NotImplementedException();
        }

        public Task<Response<ContainerDTO>> CreateContainerAsync([BodyAttribute] ContainerDTO container)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteContainerAsync([PathAttribute] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ContainerDTO>> GetContainerByNameAsync([PathAttribute] string name)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<BlobDTO>>> ListContainerBlobsAsync([PathAttribute] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<ContainerDTO>>> ListContainersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response<ContainerDTO>> UpdateContainerAsync([BodyAttribute] ContainerDTO container)
        {
            throw new NotImplementedException();
        }
    }
}