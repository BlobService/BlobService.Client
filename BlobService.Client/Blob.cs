using BlobService.Client.Helpers;
using BlobService.Client.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BlobService.Client
{
    public class Blob
    {
        private IBlobServiceBlobsAPI _blobApi;
        private IBlobServiceBlobMetaDatasAPI _metaDataApi;

        internal Blob(IBlobServiceClient client, BlobContainer container, string blobId,
                        string filename, string mimeType, long sizeInBytes, Uri downloadUrl)
        {
            Utility.AssertNotNull(nameof(client), client);
            Utility.AssertNotNull(nameof(container), container);
            Utility.AssertNotNullOrEmpty(nameof(blobId), blobId);
            Utility.AssertNotNullOrEmpty(nameof(filename), filename);
            Utility.AssertNotNullOrEmpty(nameof(mimeType), mimeType);
            Utility.AssertNotNull(nameof(downloadUrl), downloadUrl);

            this.ServiceClient = client;
            this.Id = blobId;
            this.Container = container;
            this.FileName = filename;
            this.MimeType = mimeType;
            this.SizeInBytes = sizeInBytes;
            this.DownloadUrl = downloadUrl;

            this._blobApi = ((BlobServiceClient)ServiceClient).BlobsApiClient;
            this._metaDataApi = ((BlobServiceClient)ServiceClient).MetaDatasApiClient;
        }

        public BlobContainer Container { get; }
        public Uri DownloadUrl { get; private set; }
        public string FileName { get; private set; }
        public string Id { get; private set; }
        public string MimeType { get; private set; }
        public IBlobServiceClient ServiceClient { get; private set; }
        public long SizeInBytes { get; private set; }

        public virtual Task DeleteAsync()
        {
            return this.DeleteAsync(CancellationToken.None);
        }

        public virtual Task DeleteMetaDataAsync(string key)
        {
            return this.DeleteMetaDataAsync(key, CancellationToken.None);
        }

        public virtual async Task DeleteMetaDataAsync(string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var serviceResponse = await _metaDataApi.DeleteMetaDataAsync(this.Id, key);

            if (false == serviceResponse.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.Content.ToString());
            }
        }

        public virtual Task<Dictionary<string, string>> GetMetaDataAsync()
        {
            return this.GetMetaDataAsync(CancellationToken.None);
        }

        public virtual Task SetMetaDataAsync(string key, string value)
        {
            return this.SetMetaDataAsync(CancellationToken.None, key, value);
        }

        public virtual async Task SetMetaDataAsync(CancellationToken cancellationToken, string key, string value)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var serviceResponse = await _metaDataApi.SetMetaDataAsync(this.Id, new DTO.BlobMetaDataDTO
            {
                Key = key,
                Value = value
            });

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.ResponseMessage.Content.ToString());
            }
        }

        public virtual async Task<Dictionary<string, string>> GetMetaDataAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var serviceResponse = await _metaDataApi.GetMetaDataAsync(this.Id);

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent);
            }

            var mds = serviceResponse.GetContent().ToList();

            var mdDictionary = new Dictionary<string, string>();
            foreach (var md in mds)
            {
                mdDictionary.Add(md.Key, md.Value);
            }

            return mdDictionary;
        }

        public virtual async Task DeleteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var serviceResponse = await _blobApi.DeleteBlobAsync(this.Id);

            if (false == serviceResponse.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.Content.ToString());
            }
        }

        public virtual Task DownloadToFileAsync(string path, FileMode mode)
        {
            return this.DownloadToFileAsync(path, mode, CancellationToken.None);
        }

        public virtual async Task DownloadToFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var blobFile = new FileStream(path, mode))
            {
                await this.DownloadToStreamAsync(blobFile, cancellationToken);
            }
        }

        public virtual Task DownloadToStreamAsync(Stream target)
        {
            return this.DownloadToStreamAsync(target, CancellationToken.None);
        }

        public virtual async Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blobStream = await this.OpenReadAsync(cancellationToken);
            await blobStream.CopyToAsync(target);
        }

        public string GetSharedAccessSignature()
        {
            throw new NotImplementedException();
        }

        public virtual Task<Stream> OpenReadAsync()
        {
            return this.OpenReadAsync(CancellationToken.None);
        }

        public virtual async Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var serviceResponse = await _blobApi.RawBlobAsync(this.Id);
            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                if (serviceResponse.ResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw ExceptionGenerator.BlobNotFoundException(this.Id);
                }
                else
                {
                    throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent);
                }
            }

            return serviceResponse.GetContent();
        }

        public virtual Task UpdateBlobAsync(Stream file, string fileName, string contentType)
        {
            return this.UpdateBlobAsync(file, fileName, contentType, CancellationToken.None);
        }

        public virtual async Task UpdateBlobAsync(Stream file, string fileName, string contentType, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var contentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"", Name = "file" };
            var contentMimeType = new MediaTypeHeaderValue(contentType);
            var serviceResponse = await _blobApi.UpdateBlobAsync(this.Id, contentDisposition, contentMimeType, file);
            var updatedBlob = serviceResponse.GetContent();

            if (false == serviceResponse.ResponseMessage.IsSuccessStatusCode)
            {
                throw ExceptionGenerator.GeneralFailureException(serviceResponse.StringContent);
            }

            FileName = updatedBlob.OrigFileName;
            Id = updatedBlob.Id;
            MimeType = updatedBlob.MimeType;
            SizeInBytes = updatedBlob.SizeInBytes;
        }
    }
}