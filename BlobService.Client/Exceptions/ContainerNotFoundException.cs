using System;

namespace BlobService.Client.Exceptions
{
    public class ContainerNotFoundException : BlobServiceException
    {
        public ContainerNotFoundException()
        {
        }

        public ContainerNotFoundException(string message) : base(message)
        {
        }

        public ContainerNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}