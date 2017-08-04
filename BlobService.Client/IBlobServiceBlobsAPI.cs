using BlobService.Client.DTO;
using RestEase;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlobService.Client
{
    [AllowAnyStatusCode]
    public interface IBlobServiceBlobsAPI
    {
        [Get("/blobs/{id}")]
        Task<Response<BlobDTO>> GetBlobByIdAsync([Path] string id);

        [Get("/blobs/{id}/raw")]
        Task<Response<Stream>> RawBlobAsync([Path] string id);

        [Put("/blobs/{id}")]
        Task<Response<BlobDTO>> UpdateBlobAsync(
                [Path] string id,
                [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition,
                [Header("Content-Type")] MediaTypeHeaderValue contentType,
                [Body] Stream file
            );

        [Delete("blobs/{id}")]
        Task<HttpResponseMessage> DeleteBlobAsync([Path] string id);
    }
}