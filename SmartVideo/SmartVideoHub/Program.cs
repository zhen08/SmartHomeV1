using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartVideoHub
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            CheckLocalBackup();
            DetectNet.StartDetectDaemon();
            LocalServer.StartServer();
            StartCloudTask();
            while (true)
            {
                var media = MediaQueue.Instance.Dequeue();

                if (media != null)
                {
                    DetectionResultQueue.Instance.Enqueue(DetectNet.DetectMedia(media));
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private static void CheckLocalBackup()
        {
            Task.Run(() =>
            {
                var files = Directory.GetFiles(Constants.LocalStorageFolder).Where(f => f.Contains("MP4")).ToList();
                foreach (var file in files)
                {
                    if (DateTime.UtcNow.Subtract(File.GetCreationTimeUtc(file)).TotalDays>7) {
                        File.Delete(file);
                    }
                }
            });
        }

        private static void StartCloudTask()
        {
            Task.Run(() =>
            {
                Dictionary<string, DateTime> lastUpdate = new Dictionary<string, DateTime>();
                Console.WriteLine("Starting Cloud Submission Task");
                var storage = new CloudStorage();
                while (true)
                {
                    try
                    {
                        DetectionResult detectionResult = DetectionResultQueue.Instance.Dequeue();
                        if (detectionResult != null)
                        {
                            var deviceId = detectionResult.Document.DeviceId;
                            if (!lastUpdate.ContainsKey(deviceId))
                            {
                                lastUpdate.Add(deviceId, DateTime.MinValue);
                            }
                            if (detectionResult.Document.ActiveMedia)
                            {
                                lastUpdate[deviceId] = DateTime.UtcNow;
                                storage.UploadMediaToEventHistory(detectionResult).Wait();
                            }
                            else
                            {
                                if (DateTime.UtcNow.Subtract(lastUpdate[deviceId]).TotalMinutes >= 60)
                                {
                                    lastUpdate[deviceId] = DateTime.UtcNow;
                                    storage.UploadMediaToEventHistory(detectionResult).Wait();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Thread.Sleep(10000);
                    }
                    Thread.Sleep(100);
                }
            });
        }
    }
}
