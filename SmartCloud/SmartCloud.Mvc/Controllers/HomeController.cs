using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartCloud.Mvc.Models.HomeViewModels;
using SmartCloud.Mvc.Services;
using Microsoft.Azure.Documents.Client;
using SmartVideo.Model.Document;
using Microsoft.Azure.Documents.Linq;

namespace SmartCloud.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICloudStorageService _cloudService;
        private DocumentClient documentClient;
        private Uri eventVideoCollectionUri;
        private Uri trainingImageCollectionUri;

        public HomeController(ICloudStorageService cloudStorage)
        {
            _cloudService = cloudStorage;
            documentClient = new DocumentClient(new Uri(Constants.DocumentDbEndPointUri), Constants.DocumentDbAccessKey);
            eventVideoCollectionUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDbDatabaseName, Constants.DocumentDbEventVideoCollectionName);
            trainingImageCollectionUri = UriFactory.CreateDocumentCollectionUri(Constants.DocumentDbDatabaseName, Constants.DocumentTrainingImageCollectionName);
        }

        public async Task<IActionResult> Index(string device)
        {
            ViewBag.user = User.Identity.Name;
            var result = new List<VideoIndexViewModel>();
            IDocumentQuery<MediaDocument> query;
            if (string.IsNullOrEmpty(device))
            {
                query = documentClient
                    .CreateDocumentQuery<MediaDocument>(eventVideoCollectionUri, new FeedOptions { MaxItemCount = -1 })
                    .OrderByDescending(d => d.MediaTimeStamp)
                    .Take(120)
                    .AsDocumentQuery();
            }
            else
            {
                query = documentClient
                    .CreateDocumentQuery<MediaDocument>(eventVideoCollectionUri)
                    .Where(d => d.DeviceId.Equals(device))
                    .OrderByDescending(d => d.MediaTimeStamp)
                    .Take(60)
                    .AsDocumentQuery();
            }
            while (query.HasMoreResults)
            {
                var res = await query.ExecuteNextAsync<MediaDocument>();
                foreach (var document in res)
                {
                    if (!(string.IsNullOrEmpty(device))||(!result.Any(r => r.DeviceId.Equals(document.DeviceId))))
                    {
                        var video = new VideoIndexViewModel();
                        video.Id = document.Id;
                        video.DeviceId = document.DeviceId;
                        video.VideoTime = TimeZoneInfo.ConvertTime(new DateTime(2008, 5, 24).AddSeconds(document.MediaTimeStamp), TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById(@"New Zealand Standard Time"));
                        video.VideoBlob = document.VideoBlob;
                        video.ThumbnailBlob = document.ThumbnailBlob;
                        video.ExportedTraining = document.ExportedTraining;
                        if (document.Frames.Count > 0)
                        {
                            video.BoundingBoxs = document.Frames.Where(f => f.PedestrianBoxes.Count > 0).First().PedestrianBoxes;
                        }
                        result.Add(video);
                    }
                }

            }
            return View(result.OrderBy(r=>r.DeviceId));
        }

        public async Task<IActionResult> TrainingGood(string id)
        {
            var mediaQuery = documentClient
                .CreateDocumentQuery<MediaDocument>(eventVideoCollectionUri)
                .Where(d => d.Id.Equals(id));
            foreach (var media in mediaQuery)
            {
                var trainingDoc = new TrainingDocument();
                trainingDoc.ImageBlob = media.ThumbnailBlob;
                foreach (var ped in media.Frames.First().PedestrianBoxes)
                {
                    trainingDoc.PedestrianBoxes.Add(new BoundingBox("Pedestrian", ped.x1, ped.y1, ped.x2, ped.y2));
                }
                await documentClient.CreateDocumentAsync(trainingImageCollectionUri, trainingDoc);
                media.ExportedTraining = true;
                await documentClient.UpsertDocumentAsync(eventVideoCollectionUri, media);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TrainingBad(string id)
        {
            var mediaQuery = documentClient
                .CreateDocumentQuery<MediaDocument>(eventVideoCollectionUri)
                .Where(d => d.Id.Equals(id));
            foreach (var media in mediaQuery)
            {
                var trainingDoc = new TrainingDocument();
                trainingDoc.ImageBlob = media.ThumbnailBlob;
                foreach (var ped in media.Frames.First().PedestrianBoxes)
                {
                    trainingDoc.PedestrianBoxes.Add(new BoundingBox("DontCare", ped.x1, ped.y1, ped.x2, ped.y2));
                }
                await documentClient.CreateDocumentAsync(trainingImageCollectionUri, trainingDoc);
                media.ExportedTraining = true;
                await documentClient.UpsertDocumentAsync(eventVideoCollectionUri, media);
            }
            return RedirectToAction("Index");
        }

        public IActionResult PlayVideo(string blob)
        {
            return View("PlayVideo", blob);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
