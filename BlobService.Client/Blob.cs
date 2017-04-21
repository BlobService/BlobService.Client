using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class Blob
    {
        public string Id { get; set; }
        public int SizeInBytes { get; set; }
        public string FileName { get; set; }
        public string DownloadUrl { get; }
        public string MimeType { get; }
    }
}
