using BlobService.Client.DTO;
using RestEase;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlobService.Client.UnitTests.Mocks
{
    public class BlobsApiMock : IBlobServiceBlobsAPI
    {
        private SeedData _seedData;

        private ApiMockOptions _options;

        public BlobsApiMock(SeedData data, ApiMockOptions options = null)
        {
            _options = options ?? new ApiMockOptions();
            this._seedData = data;
        }

        public Task<HttpResponseMessage> DeleteBlobAsync([Path] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BlobDTO>> GetBlobByIdAsync([Path] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Stream>> RawBlobAsync([Path] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BlobDTO>> UpdateBlobAsync([Path] string id, [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition, [Header("Content-Type")] MediaTypeHeaderValue contentType, [Body] Stream file)
        {
            throw new NotImplementedException();
        }
    }
}