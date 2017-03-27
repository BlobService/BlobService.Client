using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlobServiceClient
    {
        string Endpoint { get; }
        Task<IBlobContainer> GetContainerByNameAsync(string name, bool createIfNotExist = true);
        Task<IBlobContainer> GetContainerByIdAsync(string id);
    }
}
