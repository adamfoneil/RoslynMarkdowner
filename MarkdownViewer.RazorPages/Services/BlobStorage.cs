using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkdownViewer.App.Services
{
    public class BlobStorage
    {
        private readonly StorageCredentials _credentials;
        private readonly string _containerName;

        public BlobStorage(StorageCredentials credentials, string containerName)
        {
            _credentials = credentials;
            _containerName = containerName;
        }

        public async Task SaveAsync<T>(string userName, T @object, string name)
        {
            var container = GetContainer();
            var blob = GetBlob(container, userName, name);
            var json = JsonConvert.SerializeObject(@object, Formatting.Indented);
            await blob.UploadTextAsync(json);
            blob.Properties.ContentType = "application/json";
            await blob.SetPropertiesAsync();
        }

        public async Task<T> GetAsync<T>(string userName, string name)
        {
            var container = GetContainer();
            var blob = GetBlob(container, userName, name);
            string json = await blob.DownloadTextAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<IEnumerable<CloudBlockBlob>> ListContentAsync(string userName)
        {
            var container = GetContainer();

            List<CloudBlockBlob> results = new List<CloudBlockBlob>();

            var token = default(BlobContinuationToken);
            do
            {
                var segment = await container.ListBlobsSegmentedAsync(userName, true, BlobListingDetails.All, null, token, null, null);
                results.AddRange(segment.Results.OfType<CloudBlockBlob>());
                token = segment.ContinuationToken;
            } while (token != null);

            return results;
        }

        private CloudBlockBlob GetBlob(CloudBlobContainer container, string userName, string blobName)
        {
            return container.GetBlockBlobReference($"{userName}/{blobName}");
        }

        private CloudBlobContainer GetContainer()
        {
            var account = new CloudStorageAccount(_credentials, true);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(_containerName);
        }
    }
}
