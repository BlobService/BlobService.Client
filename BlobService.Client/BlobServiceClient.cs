using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class BlobServiceClient : IBlobServiceClient
    {
        public string Endpoint { get; internal set; }
        private RestClient _client;
        public BlobServiceClient(string endpoint)
        {
            Endpoint = endpoint;
            _client = new RestClient(Endpoint);
        }

        public async Task<IBlobContainer> GetContainerByIdAsync(string id)
        {
            var request = new RestRequest("containers/get/{id}", Method.GET);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            var result = await _client.ExecuteTaskAsync<BlobContainer>(request);
            var container = result.Data;
            if (container != null)
            {
                container.Client = _client;
            }
            return container;
        }

        public async Task<IBlobContainer> GetContainerByNameAsync(string name, bool createIfNotExist = true)
        {
            var request = new RestRequest("containers/get", Method.GET);
            request.AddParameter("name", name);
            var result = await _client.ExecuteTaskAsync<BlobContainer>(request);
            var container = result.Data;
            if (container != null)
            {
                container.Client = _client;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NotFound && createIfNotExist)
            {
                var createRequest = new RestRequest("containers/add", Method.POST);
                createRequest.RequestFormat = DataFormat.Json;
                createRequest.AddBody(new { name = "name" });
                var createResult = await _client.ExecuteTaskAsync<BlobContainer>(request);
                container = createResult.Data;
                if (container != null)
                {
                    container.Client = _client;
                }
            }
            return container;
        }
    }
}
