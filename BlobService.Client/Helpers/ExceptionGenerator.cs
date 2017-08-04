using BlobService.Client.Exceptions;
using BlobService.Client.Util;

namespace BlobService.Client.Helpers
{
    internal static class ExceptionGenerator
    {
        internal static BlobServiceException GeneralFailureException(string message)
        {
            return new BlobServiceException(string.Format(Consts.GeneralFailureMsg, message));
        }

        internal static BlobServiceException InvalidOperationException(string operation,
                                                                        string objectName, string objectType)
        {
            return new InvalidServiceOperationException(
                string.Format(Consts.InvalidOperationError, operation, objectType, objectName));
        }

        internal static ContainerNotFoundException ContainerNotFoundException(string message)
        {
            return new ContainerNotFoundException(message);
        }

        internal static BlobNotFoundException BlobNotFoundException(string blobId)
        {
            return new BlobNotFoundException(string.Format(Consts.BlobNotFoundError, blobId));
        }
    }
}