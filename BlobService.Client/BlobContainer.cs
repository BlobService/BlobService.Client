using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class BlobContainer : IBlobContainer
    {
        public RestClient Client { get; set; }
        public string Id { get; internal set; }

        public string Name { get; internal set; }

        public async Task<bool> DeleteAsync()
        {
            var request = new RestRequest("containers/delete/{id}", Method.DELETE);
            request.AddParameter("id", Id, ParameterType.UrlSegment);
            var result = await Client.ExecuteTaskAsync(request);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<IBlob> GetBlobAsync(string blobId)
        {
            var request = new RestRequest("blobs/get/{id}", Method.GET);
            request.AddParameter("id", blobId, ParameterType.UrlSegment);
            var result = await Client.ExecuteTaskAsync<Blob>(request);
            var blob = result.Data;
            if (blob != null)
            {
                blob.Client = Client;
                blob.Container = this;
                // blob.DownloadUrl TODO
            }
            return blob;
        }

        public async Task<IEnumerable<IBlob>> ListBlobsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveBlobAsync(string blobId)
        {
            var request = new RestRequest("blobs/delete/{id}", Method.DELETE);
            request.AddParameter("id", blobId, ParameterType.UrlSegment);
            var result = await Client.ExecuteTaskAsync(request);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> UploadBlobAsync(byte[] blob)
        {
            throw new NotImplementedException();
        }
    }
}
