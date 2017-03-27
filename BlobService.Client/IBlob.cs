using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public interface IBlob
    {
        string Id { get; }
        int SizeInBytes { get; }
        string FileName { get; }
        string DownloadUrl { get; }
        string MimeType { get; }
        IBlobContainer Container { get; }
        Task<bool> DeleteAsync();
    }
}
