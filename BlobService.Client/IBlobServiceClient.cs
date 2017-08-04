using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobServiceClient
    {
        Uri BaseUri { get; }

        Task<BlobContainer> CreateContainerAsync(string containerName);

        Task<Blob> GetBlobReferenceAsync(string containerName, string blobId);

        Task<BlobContainer> GetContainerReferenceAsync(string containerName);

        Task<IEnumerable<BlobContainer>> ListContainersAsync();
    }
}