using BlobService.Client.UnitTests.Helpers;
using BlobService.Client.Util;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlobService.Client.UnitTests
{
    public class BlobTests
    {
        [Fact]
        public async Task DeleteBlobAsync_ShouldSuccess()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);

            var targetBlob = data.Blobs.First();
            var blobId = targetBlob.Id;
            var containerName = data.Containers.Where(c => c.Id == targetBlob.ContainerId).First().Name;
            var blob = await client.GetBlobReferenceAsync(containerName, blobId);

            await blob.DeleteAsync();
            Assert.DoesNotContain(data.Blobs, b => b.Id == blobId);
        }

        [Fact]
        public async Task ReadBlob_ShouldSuccess()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);

            var targetBlob = data.Blobs.First();
            var blobId = targetBlob.Id;
            var containerName = data.Containers.Where(c => c.Id == targetBlob.ContainerId).First().Name;
            var blob = await client.GetBlobReferenceAsync(containerName, blobId);

            using (var rawBlob = await blob.OpenReadAsync())
            {
                using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
                {
                    Assert.Equal(test_Stream.Length, rawBlob.Length);
                }
            }
        }

        [Fact]
        public async Task UpdateBlobData_ShouldSuccess()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);

            var targetBlob = data.Blobs.First();
            var blobId = targetBlob.Id;
            var containerName = data.Containers.Where(c => c.Id == targetBlob.ContainerId).First().Name;
            var blob = await client.GetBlobReferenceAsync(containerName, blobId);

            using (var newStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever+whatever=love")))
            {
                var newFilename = "newFile.png";
                var newContentType = MIMETypeMapper.GetMimeMapping(newFilename);
                await blob.UpdateBlobAsync(newStream, newFilename, newContentType);

                Assert.Equal(blob.SizeInBytes, newStream.Length);
                Assert.Equal(blob.MimeType, newContentType);
                Assert.Equal(blob.FileName, newFilename);
            }
        }

        [Fact]
        public async Task DownloadToStreamAsync_ShouldSuccess()
        {
            var data = BlobServiceClientBuilder.GenerateSeedData();
            var client = BlobServiceClientBuilder.GetBlobServiceClientMock(data);

            var targetBlob = data.Blobs.First();
            var blobId = targetBlob.Id;
            var containerName = data.Containers.Where(c => c.Id == targetBlob.ContainerId).First().Name;
            var blob = await client.GetBlobReferenceAsync(containerName, blobId);

            var blobOriginalStream = await blob.OpenReadAsync();

            var targetStream = new MemoryStream();

            await blob.DownloadToStreamAsync(targetStream);

            targetStream.Position = 0;
            blobOriginalStream.Position = 0;
            Assert.True(TestHelper.FileStreamEquals(targetStream, blobOriginalStream));
        }
    }
}