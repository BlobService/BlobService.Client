using BlobService.Client.DTO;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobService.Client.UnitTests.Mocks
{
    public class BlobMetaDatasApiMock : IBlobServiceBlobMetaDatasAPI
    {

        private SeedData _seedData;

        private ApiMockOptions _options;

        public BlobMetaDatasApiMock(SeedData data, ApiMockOptions options = null)
        {
            _options = options ?? new ApiMockOptions();
            this._seedData = data;
        }

        public Task<HttpResponseMessage> DeleteMetaDataAsync([Path] string blobId, string key)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<BlobMetaDataDTO>>> GetMetaDataAsync([Path] string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BlobMetaDataDTO>> SetMetaDataAsync([Path] string blobId, [Body] BlobMetaDataDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BlobMetaDataDTO>> UpdateMetaDataAsync([Path] string blobId, [Body] BlobMetaDataDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
