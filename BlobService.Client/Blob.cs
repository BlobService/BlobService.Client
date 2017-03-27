using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class Blob : IBlob
    {
        public RestClient Client { get; internal set; }
        public string Id { get; internal set; }

        public int SizeInBytes { get; internal set; }

        public string FileName { get; internal set; }

        public string DownloadUrl { get; internal set; }

        public string MimeType { get; internal set; }

        public IBlobContainer Container { get; internal set; }

        public async Task<bool> DeleteAsync()
        {
            var request = new RestRequest("blobs/delete/{id}", Method.DELETE);
            request.AddParameter("id", Id, ParameterType.UrlSegment);
            var result = await Client.ExecuteAsync(request);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
