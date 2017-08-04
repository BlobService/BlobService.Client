using BlobService.Client.DTO;
using BlobService.Client.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Client.UnitTests
{
    public static class BlobServiceClientBuilder
    {
        public static SeedData GenerateSeedData()
        {
            var sd = new SeedData();

            sd.Containers.Add(new ContainerDTO()
            {
                Id = "8e78d631-811b-49af-b361-6a41357ea9c4",
                Name = "TestCont#1"
            });

            sd.Containers.Add(new ContainerDTO()
            {
                Id = "a2707716-8e6e-4b80-bcda-130f3886052e",
                Name = "TestCont#2"
            });

            sd.Blobs.Add(new BlobDTO()
            {
                Id = "c6f02c43-c0db-4e9e-a2d2-31535e536a7a",
                ContainerId = "a2707716-8e6e-4b80-bcda-130f3886052e",
                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                OrigFileName = "Test.docx",
                SizeInBytes = 33579,
                DownloadRelativeUrl = new Uri("/blobs/c6f02c43-c0db-4e9e-a2d2-31535e536a7a/raw")
            });
            return sd;
        }

        public static BlobServiceClient GetBlobServiceClientMock(SeedData data)
        {
            IBlobServiceContainersAPI containerApi = new ContainersApiMock(data);
            IBlobServiceBlobsAPI blobsApi = new BlobsApiMock(data);

            return new BlobServiceClient(new Uri(""), containerApi, blobsApi);
        }
    }
}
