using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCloud.Mvc.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private CloudBlobClient blobClient;
        private CloudStorageAccount storageAccount;

        public CloudStorageService()
        {
            storageAccount = CloudStorageAccount.Parse(Constants.CloudStorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<Stream> GetBlob(string container, string blob)
        {
            var result = new MemoryStream();
            var cont = blobClient.GetContainerReference(container);

            CloudBlockBlob blockBlob = cont.GetBlockBlobReference(blob);
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.DownloadToStreamAsync(result);
                result.Position = 0;
            }
            
            return result;
        }

        public async Task<IEnumerable<string>> ListAll(string container)
        {
            var cont = blobClient.GetContainerReference(container);
            List<string> result = new List<string>();
            var blobs = await cont.ListBlobsSegmentedAsync(new BlobContinuationToken());
            foreach (var b in blobs.Results)
            {
                var bloburi = b.Uri.ToString();
                result.Add(bloburi.Substring(bloburi.LastIndexOf('/') + 1));
            }
            return result;
        }
    }
}
