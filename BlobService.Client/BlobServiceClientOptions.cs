using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Client
{
    public class BlobServiceClientOptions
    {
        public static BlobServiceClientOptions Default => new BlobServiceClientOptions { ThrowExceptionOnNotFound = true };
        public bool ThrowExceptionOnNotFound { get; set; }

    }
}
