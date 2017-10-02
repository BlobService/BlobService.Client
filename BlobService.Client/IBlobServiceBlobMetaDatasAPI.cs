using BlobService.Client.DTO;
using RestEase;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobServiceBlobMetaDatasAPI
    {
        [Get("/blobs/{id}/metadata")]
        Task<Response<IEnumerable<BlobMetaDataDTO>>> GetMetaDataAsync([Path] string id);

        [Post("/blobs/{blobId}/metadata")]
        Task<Response<BlobMetaDataDTO>> SetMetaDataAsync(
                [Path] string blobId,
                [Body] BlobMetaDataDTO model);

        [Put("/blobs/{blobId}/metadata")]
        Task<Response<BlobMetaDataDTO>> UpdateMetaDataAsync(
                [Path] string blobId,
                [Body] BlobMetaDataDTO model
            );

        [Delete("/blobs/{blobId}/metadata")]
        Task<HttpResponseMessage> DeleteMetaDataAsync([Path] string blobId, string key);
    }
}
