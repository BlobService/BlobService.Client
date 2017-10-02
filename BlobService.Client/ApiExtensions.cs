using BlobService.Client.DTO;
using RestEase;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public static class ApiExtensions
    {
        public static async Task<Response<BlobDTO>> UpdateBlobAsync(this IBlobServiceBlobsAPI api,
                                                string id,
                                                ContentDispositionHeaderValue contentDisposition,
                                                MediaTypeHeaderValue contentType,
                                                Stream file)
        {
            var content = new MultipartFormDataContent();
            var fileByteContent = await ReadFullyAsync(file);
            var fileContent = new ByteArrayContent(fileByteContent);
            fileContent.Headers.ContentDisposition = contentDisposition;
            fileContent.Headers.ContentType = contentType;
            content.Add(fileContent, "file");

            return await api.UpdateBlobInternalAsync(id, content);
        }

        public static async Task<Response<BlobDTO>> AddBlobAsync(this IBlobServiceContainersAPI api,
                                              string containerId,
                                              ContentDispositionHeaderValue contentDisposition,
                                              MediaTypeHeaderValue contentType,
                                              Stream file)
        {
            var content = new MultipartFormDataContent();
            var fileByteContent = await ReadFullyAsync(file);
            var fileContent = new ByteArrayContent(fileByteContent);
            fileContent.Headers.ContentDisposition = contentDisposition;
            fileContent.Headers.ContentType = contentType;
            content.Add(fileContent, "file");

            return await api.AddBlobInternalAsync(containerId, content);
        }

        public static async Task<byte[]> ReadFullyAsync(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.Position = 0;
                await input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}