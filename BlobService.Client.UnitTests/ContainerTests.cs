using BlobService.Client.Exceptions;
using BlobService.Client.UnitTests.Mocks;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlobService.Client.UnitTests
{
    public class ContainerTests
    {
        [Fact]
        public async Task DeleteContainer()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            string containerName = data.Containers[0].Name;

            BlobContainer container = await client.GetContainerReferenceAsync(containerName);
            await container.DeleteAsync();

            Assert.False(data.Containers.Any(c => c.Name == container.Name));
        }

        [Fact]
        public async Task DeleteContainer_ShouldReturnServerError()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var apiMockOptiosn = new ApiMockOptions() { ThrowInternalServerError = true };

            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data, apiMockOptiosn);

            string containerName = data.Containers[data.Containers.Count - 1].Name;

            BlobContainer container = await client.GetContainerReferenceAsync(containerName);

            Exception ex = await Assert.ThrowsAsync<BlobServiceException>(
                async () => await container.DeleteAsync());
        }

        [Fact]
        public async Task UploadBlob()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var targetContainerName = data.Containers.First().Name;
            var container = await client.GetContainerReferenceAsync(targetContainerName);
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                var blob = await container.UploadBlobAsync(test_Stream, "IloveAramMCP.png");
                Assert.NotNull(blob);
            }
        }

        [Fact]
        public async Task ListBlobs()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var targetContainerName = "TestCont#2";
            var container = await client.GetContainerReferenceAsync(targetContainerName);

            var blobs = await container.ListBlobsAsync();

            Assert.Equal(blobs.Count(), 1);
        }

        [Fact]
        public async Task UpdateContainer()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var targetContainerName = "TestCont#2";
            var container = await client.GetContainerReferenceAsync(targetContainerName);

            await container.RenameAsync(targetContainerName + "_New");

            Assert.Equal(container.Name, targetContainerName + "_New");
        }
    }
}