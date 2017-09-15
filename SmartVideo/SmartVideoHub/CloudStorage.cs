using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SmartVideo.Transport;
using PCLCrypto;
using SmartVideo.Model.Document;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace SmartVideoHub
{
    public class CloudStorage
    {
        private CloudBlobContainer eventContainer;

        public CloudStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.CloudStorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            eventContainer = blobClient.GetContainerReference(Constants.EventHistoryContainer);
            eventContainer.CreateIfNotExists();
            eventContainer.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off });
        }

        public async Task UploadMediaToEventHistory(DetectionResult detectionResult)
        {
            try
            {
                File.WriteAllBytes(Path.Combine(Constants.LocalStorageFolder, detectionResult.Media.FileName), detectionResult.Media.Data);
				var startTime = DateTime.UtcNow;
                CloudBlockBlob blockBlob;
                blockBlob = eventContainer.GetBlockBlobReference(detectionResult.Document.VideoBlob);
                await blockBlob.UploadFromByteArrayAsync(detectionResult.Media.Data, 0, detectionResult.Media.Data.Length);

                blockBlob = eventContainer.GetBlockBlobReference(detectionResult.Document.ThumbnailBlob);
                await blockBlob.UploadFromByteArrayAsync(detectionResult.Thumbnail, 0, detectionResult.Thumbnail.Length);

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsJsonAsync(Constants.DocumentPostingUri, detectionResult.Document);
                Console.WriteLine("Uploaded data from {0} in {1} seconds", detectionResult.Media.DeviceId, DateTime.UtcNow.Subtract(startTime).TotalSeconds);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
