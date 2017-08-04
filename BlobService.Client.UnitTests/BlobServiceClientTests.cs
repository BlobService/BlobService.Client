using BlobService.Client.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlobService.Client.UnitTests
{
    public class BlobServiceClientTests
    {
        [Fact]
        public async Task RetrieveContainerByName()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var containerName = data.Containers[0].Name;
            var container = await client.GetContainerReferenceAsync(containerName);
            Assert.Equal(container.Name, containerName);
        }

        [Fact]
        public async Task RetrieveContainerByName_NonExistingContainer()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var containerName = "VeryFakeName";
            Exception ex = await Assert.ThrowsAsync<ContainerNotFoundException>(
                async () => await client.GetContainerReferenceAsync(containerName));
        }

        [Fact]
        public async Task ListContainers()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var containers = await client.ListContainersAsync();
            Assert.Equal(containers.Count(), data.Containers.Count);
        }

        [Fact]
        public async Task CreateContainer()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var containerName = "NewContainer";

            BlobContainer newContainer = await client.CreateContainerAsync(containerName);
            Assert.NotEqual(newContainer.Id, string.Empty);
        }

        [Fact]
        public async Task CreateContainer_ShouldRetrunArgumentNullException()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            string containerName = null;

            BlobContainer newContainer = null;

            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => newContainer = await client.CreateContainerAsync(containerName));
        }

        [Fact]
        public async Task CreateContainer_ShouldReturnArgumentExceptionAsync()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            string containerName = string.Empty;

            BlobContainer newContainer = null;

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(
                async () => newContainer = await client.CreateContainerAsync(containerName));
        }

        [Fact]
        public async Task GetBlobReferenceAsync()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var targetBlob = data.Blobs.First();
            var blobId = targetBlob.Id;
            var containerName = data.Containers.Where(c => c.Id == targetBlob.ContainerId).First().Name;
            var blob = await client.GetBlobReferenceAsync(containerName, blobId);

            Assert.Equal(targetBlob.OrigFileName, blob.FileName);
        }
    }
}