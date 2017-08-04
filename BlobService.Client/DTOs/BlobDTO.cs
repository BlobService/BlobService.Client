using System;

namespace BlobService.Client.DTO
{
    public class BlobDTO
    {
        public string Id { get; internal set; }
        public long SizeInBytes { get; internal set; }
        public string OrigFileName { get; internal set; }
        public Uri DownloadRelativeUrl { get; internal set; }
        public string MimeType { get; internal set; }
        public string ContainerId { get; internal set; }
    }
}