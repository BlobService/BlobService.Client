using Newtonsoft.Json;

namespace BlobService.Client.DTO
{
    public class ContainerDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}