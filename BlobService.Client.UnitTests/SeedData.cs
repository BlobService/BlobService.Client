using BlobService.Client.DTO;
using System.Collections.Generic;

namespace BlobService.Client.UnitTests
{
    public class SeedData
    {
        public List<ContainerDTO> Containers { get; } = new List<ContainerDTO>();

        public List<BlobDTO> Blobs { get; } = new List<BlobDTO>();
    }
}