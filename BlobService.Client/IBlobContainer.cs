using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobContainer
    {
        string Id { get; }
        string Name { get; }
        Task<IEnumerable<IBlob>> ListBlobsAsync();
        Task<IBlob> GetBlobAsync(string blobId);
        Task<bool> UploadBlobAsync(byte[] blob);
        Task<bool> RemoveBlobAsync(string blobId);
        Task<bool> DeleteAsync();
    }
}
