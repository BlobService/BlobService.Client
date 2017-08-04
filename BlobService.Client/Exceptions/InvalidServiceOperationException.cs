using System;

namespace BlobService.Client.Exceptions
{
    public class InvalidServiceOperationException : BlobServiceException
    {
        public InvalidServiceOperationException()
        {
        }

        public InvalidServiceOperationException(string message) : base(message)
        {
        }

        public InvalidServiceOperationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}