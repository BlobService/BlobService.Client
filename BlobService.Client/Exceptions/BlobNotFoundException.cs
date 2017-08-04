using System;

namespace BlobService.Client.Exceptions
{
    public class BlobNotFoundException : BlobServiceException
    {
        public BlobNotFoundException()
        {
        }

        public BlobNotFoundException(string message) : base(message)
        {
        }

        public BlobNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}