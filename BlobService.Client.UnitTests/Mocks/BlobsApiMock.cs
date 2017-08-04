using BlobService.Client.DTO;
using RestEase;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Client.UnitTests.Mocks
{
    public class BlobsApiMock : IBlobServiceBlobsAPI
    {
        private SeedData _seedData;

        private ApiMockOptions _options;

        public BlobsApiMock(SeedData data, ApiMockOptions options = null)
        {
            _options = options ?? new ApiMockOptions();
            this._seedData = data;
        }

        public Task<HttpResponseMessage> DeleteBlobAsync([Path] string id)
        {
            if (this._options.ThrowInternalServerError)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
            }

            HttpResponseMessage response = null;
            var blob = _seedData.Blobs.Where(b => b.Id == id).FirstOrDefault();

            if (blob != null)
            {
                _seedData.Blobs.Remove(blob);
                response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            else
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return Task.FromResult(response);
        }

        public Task<Response<BlobDTO>> GetBlobByIdAsync([Path] string id)
        {
            if (this._options.ThrowInternalServerError)
            {
                return Task.FromResult(new Response<BlobDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError), () => new BlobDTO()));
            }
            var blob = _seedData.Blobs.Where(b => b.Id == id).FirstOrDefault();
            Response<BlobDTO> response = null;
            if (blob != null)
            {
                response = new Response<BlobDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => blob);
            }
            else
            {
                response = new Response<BlobDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.NotFound), () => new BlobDTO());
            }

            return Task.FromResult(response);
        }

        public Task<Response<Stream>> RawBlobAsync([Path] string id)
        {
            if (this._options.ThrowInternalServerError)
            {
                return Task.FromResult(new Response<Stream>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError), () => new MemoryStream()));
            }

            var blob = _seedData.Blobs.Where(b => b.Id == id).FirstOrDefault();
            var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));

            Response<Stream> response = null;
            if (blob != null)
            {
                response = new Response<Stream>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => test_Stream);
            }
            else
            {
                response = new Response<Stream>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.NotFound), () => new MemoryStream());
            }
            return Task.FromResult(response);
        }

        public Task<Response<BlobDTO>> UpdateBlobAsync([Path] string id, [Header("Content-Disposition")] ContentDispositionHeaderValue contentDisposition, [Header("Content-Type")] MediaTypeHeaderValue contentType, [Body] Stream file)
        {
            if (this._options.ThrowInternalServerError)
            {
                return Task.FromResult(new Response<BlobDTO>("Test", new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError), () => new BlobDTO()));
            }
            var targetBlob = _seedData.Blobs.Where(b => b.Id == id).FirstOrDefault();
            Response<BlobDTO> response = null;
            if (targetBlob != null)
            {
                targetBlob.MimeType = contentType.MediaType;
                targetBlob.OrigFileName = contentDisposition.FileName.Trim('"');
                targetBlob.SizeInBytes = file.Length;

                response = new Response<BlobDTO>("Test",
                    new HttpResponseMessage(HttpStatusCode.OK),
                    () => targetBlob);
            }
            else
            {
                response = new Response<BlobDTO>("Test",
                    new HttpResponseMessage(HttpStatusCode.NotFound),
                    () => new BlobDTO());
            }

            return Task.FromResult(response);
        }
    }
}