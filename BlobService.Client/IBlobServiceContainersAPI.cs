using BlobService.Client.DTO;
using RestEase;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobService.Client
{
    [AllowAnyStatusCode]
    public interface IBlobServiceContainersAPI
    {
        [Get("/containers")]
        Task<Response<IEnumerable<ContainerDTO>>> ListContainersAsync();

        [Get("/containers/{name}")]
        Task<Response<ContainerDTO>> GetContainerByNameAsync([Path] string name);

        [Post("/containers")]
        Task<Response<ContainerDTO>> CreateContainerAsync([Body] ContainerDTO container);

        [Put("/containers")]
        Task<Response<ContainerDTO>> UpdateContainerAsync([Body] ContainerDTO container);

        [Delete("/containers/{id}")]
        Task<HttpResponseMessage> DeleteContainerAsync([Path] string id);

        [Get("/containers/{id}/blobs")]
        Task<Response<IEnumerable<BlobDTO>>> ListContainerBlobsAsync([Path] string id);

        [Post("/containers/{containerId}/blobs")]
        Task<Response<BlobDTO>> AddBlobInternalAsync(
               [Path] string containerId,
               [Body] HttpContent content
           );
    }
}