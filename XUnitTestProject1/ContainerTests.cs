using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using BlobService.Client.Exceptions;
using BlobService.Client.UnitTests.Mocks;

namespace BlobService.Client.UnitTests
{
    public class ContainerTests
    {
        

        //BlobServiceClient clientReal = new BlobServiceClient(new Uri(@"http://localhost:50101/"));
        [Fact]
        public async Task RetrieveContainerByName()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);
            var containerName = "TestContainer";
            var container = await client.GetContainerReferenceAsync(containerName);
            Assert.Equal(container.Name, containerName);
        }

        //[Fact]
        //public async Task TryRetrieveNonExistingContainer_ShouldThrowException()
        //{
        //    BlobServiceClient client = clientReal;
        //    var containerName = "FakeContainer";

        //    Exception ex = await Assert.ThrowsAsync<ContainerNotFoundException>(
        //        async () => await client.GetContainerReferenceAsync(containerName));
        //}

        //[Fact]
        //public async Task ListContainers()
        //{
        //    BlobServiceClient client = clientReal;
        //    var containers = await client.ListContainersAsync();
        //    Assert.Equal(containers.Count(), 5);
        //}

        //[Fact]
        //public async Task CreateContainer()
        //{
        //    BlobServiceClient client = clientReal;
        //    var containerName = "NewContainer";

        //    BlobContainer newContainer = new BlobContainer(containerName, client);
        //    await newContainer.CreateAsync();

        //    var container = await client.GetContainerReferenceAsync(containerName);
        //}

        //[Fact]
        //public async Task CreateContainer_ShouldRetrunArgumentNullException()
        //{
        //    BlobServiceClient client = clientReal;
        //    string containerName = null;

        //    BlobContainer newContainer = new BlobContainer(containerName, client);
           
        //    Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(
        //        async () => await newContainer.CreateAsync());
        //}

        //[Fact]
        //public async Task CreateContainer_ShouldReturnArgumentException()
        //{
        //    BlobServiceClient client = clientReal;
        //    string containerName = string.Empty;

        //    BlobContainer newContainer = new BlobContainer(containerName, client);

        //    Exception ex = await Assert.ThrowsAsync<ArgumentException>(
        //        async () => await newContainer.CreateAsync());
        //}

        //[Fact]
        //public async Task CheckContainerExists()
        //{
        //    BlobServiceClient client = clientReal;
        //    string containerName = "TestContainer";
        //    BlobContainer newContainer = new BlobContainer(containerName, client);

        //    var exists = await newContainer.ExistsAsync();

        //    Assert.True(exists);
        //}

        //[Fact]
        //public async Task CheckContainerExists_ShouldReturnFalse()
        //{
        //    BlobServiceClient client = clientReal;
        //    string containerName = "Fake Name";
        //    BlobContainer newContainer = new BlobContainer(containerName, client);

        //    var exists = await newContainer.ExistsAsync();

        //    Assert.False(exists);
        //}

        //[Fact]
        //public async Task DeleteContainer()
        //{

        //}
    }
}
