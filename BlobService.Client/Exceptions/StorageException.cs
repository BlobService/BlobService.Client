using System;

namespace BlobService.Client.Exceptions
{
    public class BlobServiceException : Exception
    {
        public BlobServiceException()
        {
        }

        public BlobServiceException(string message) : base(message)
        {
        }

        public BlobServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public int HttpStatusCode { get; internal set; }
    }
}