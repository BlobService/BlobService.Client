using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobServiceClient
    {
        string Endpoint { get; }
        Task<Blob> AddBlobAsync(string containerId, string fileName, Stream file);
        Task<Blob> UpdateBlobAsync(string blobId, string fileName, Stream file);
        Task<Blob> GetBlobByIdAsync(string blobId);
        Task DeleteBlobAsync(string blobId);
        Task<Container> GetContainerByIdAsync(string containerId);
        Task<Container> GetContainerByNameAsync(string containerName, bool creatIfDoesntExist = true);
        Task<Container> AddContainerAsync(string containerName);
        Task DeleteContainerAsync(string containerId);
        Task<IEnumerable<Blob>> ListBlobsAsync(string containerId);
    }
}
