using BlobService.Client.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Client.UnitTests
{
    public class SeedData
    {
        public List<ContainerDTO> Containers { get; } = new List<ContainerDTO>();

        public List<BlobDTO> Blobs { get; } = new List<BlobDTO>();
    }
}
