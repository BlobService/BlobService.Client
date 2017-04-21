using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobServiceApi
    {
        #region blobs api

        [Get("/blobs/{id}")]
        Task<Blob> GetBlobByIdAsync(string id);

        [Get("/blobs/{id}/download")]
        Task<string> DownloadBlobAsync(string id);

        [Post("/containers/{containerId}/blobs")]
        Task<Blob> AddBlobAsync(
                string containerId, 
                [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition,
                [Header("Content-Type")] MediaTypeHeaderValue contentType,
                [Body] Stream file
            );

        [Put("/blobs/{id}")]
        Task<Blob> UpdateBlobAsync(
                string id,
                [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition,
                [Header("Content-Type")] MediaTypeHeaderValue contentType,
                [Body] Stream file
            );

        [Delete("blobs/{id}")]
        Task DeleteBlobAsync(string id);

        #endregion

        #region containers api

        [Get("/containers")]
        Task<IEnumerable<Container>> GetAllContainersAsync();

        [Get("/containers/{id}")]
        Task<Container> GetContainerByIdAsync(string id);

        [Get("/containers/?name={name}")]
        Task<Container> GetContainerByNameAsync(string name);

        [Post("/containers")]
        Task<Container> AddContainerAsync([Body] Container container);

        [Delete("/containers/{id}")]
        Task DeleteContainerAsync(string id);

        [Get("/containers/{id}/blobs")]
        Task<IEnumerable<Blob>> ListBlobsAsync(string id);

        #endregion
    }
}
