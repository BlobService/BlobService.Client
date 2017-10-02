using Newtonsoft.Json;

namespace BlobService.Client.DTO
{
    public class BlobMetaDataDTO
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
