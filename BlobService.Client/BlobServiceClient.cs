using BlobService.Client.DTO;
using BlobService.Client.Helpers;
using BlobService.Client.Util;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class BlobServiceClient : IBlobServiceClient
    {
        public BlobServiceClient(Uri baseUri) :
            this(baseUri,
                RestClient.For<IBlobServiceContainersAPI>(baseUri),
                RestClient.For<IBlobServiceBlobsAPI>(baseUri)
                )
        {
        }

        internal BlobServiceClient(Uri baseUri, IBlobServiceContainersAPI containersApi,
                                    IBlobServiceBlobsAPI blobsApi)
        {
            this.BaseUri = baseUri;
            ContainersApiClient = containersApi;
            BlobsApiClient = blobsApi;
        }

        /// <summary>
        /// Gets the Blob service endpoint.
        /// </summary>
        /// <value>An object of type <see cref="BaseUri"/> containing Blob service URI.</value>
        public Uri BaseUri { get; private set; }

        internal IBlobServiceBlobsAPI BlobsApiClient { get; private set; }
        internal IBlobServiceContainersAPI ContainersApiClient { get; private set; }

        public virtual async Task<Blob> GetBlobReferenceAsync(string containerName, string blobId)
        {
            Utility.AssertNotNullOrEmpty(nameof(blobId), blobId);
            var retrieveBlobApiCallresult = await this.BlobsApiClient.GetBlobByIdAsync(blobId);

            if (false == retrieveBlobApiCallresult.ResponseMessage.IsSuccessStatusCode)
            {
                if (retrieveBlobApiCallresult.ResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw ExceptionGenerator.BlobNotFoundException(blobId);
                }
                else
                {
                    throw ExceptionGenerator.GeneralFailureException(retrieveBlobApiCallresult.StringContent);
                }
            }

            var blobDTO = retrieveBlobApiCallresult.GetContent();

            var container = await GetContainerReferenceAsync(containerName);

            var blob = new Blob(this, container, blobDTO.Id, blobDTO.OrigFileName,
                                    blobDTO.MimeType, blobDTO.SizeInBytes, blobDTO.DownloadRelativeUrl);

            return blob;
        }

        public virtual async Task<BlobContainer> GetContainerReferenceAsync(string containerName)
        {
            Utility.AssertNotNullOrEmpty(nameof(containerName), containerName);
            var response = await ContainersApiClient.GetContainerByNameAsync(containerName);
            if (response.ResponseMessage.IsSuccessStatusCode == false)
            {
                if (response.ResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw ExceptionGenerator.ContainerNotFoundException($"Container {containerName} not found.");
                }

                throw ExceptionGenerator.GeneralFailureException(response.StringContent);
            }

            var containerDTO = response.GetContent();
            return new BlobContainer(containerDTO.Name, containerDTO.Id, this);
        }

        public virtual async Task<IEnumerable<BlobContainer>> ListContainersAsync()
        {
            var containersResponse = await ContainersApiClient.ListContainersAsync();
            if (!containersResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(containersResponse.StringContent);
            }
            var conainersDTO = containersResponse.GetContent();
            return conainersDTO.Select(c => new BlobContainer(c.Name, c.Id, this));
        }

        public virtual async Task<BlobContainer> CreateContainerAsync(string containerName)
        {
            var serviceResponse = await ContainersApiClient.CreateContainerAsync(
                                            new ContainerDTO() { Name = containerName });
            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent);
            }

            var container = serviceResponse.GetContent();
            return new BlobContainer(container.Name, container.Id, this);
        }
    }
}