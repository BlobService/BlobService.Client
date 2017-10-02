using Newtonsoft.Json;
using System;

namespace BlobService.Client.DTO
{
    public class BlobDTO
    {
        [JsonProperty("id")]
        public string Id { get; internal set; }
        [JsonProperty("sizeInBytes")]
        public long SizeInBytes { get; internal set; }
        [JsonProperty("origFileName")]
        public string OrigFileName { get; internal set; }
        [JsonProperty("downloadRelativeUrl")]
        public Uri DownloadRelativeUrl { get; internal set; }
        [JsonProperty("mimeType")]
        public string MimeType { get; internal set; }
        [JsonProperty("containerId")]
        public string ContainerId { get; internal set; }
    }
}