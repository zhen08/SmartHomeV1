using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartCloud.Mvc.Services;

namespace SmartCloud.Mvc.Controllers
{
    [Authorize]
    [Route("Video")]
    public class VideoController : Controller
    {
        private ICloudStorageService _cloudService;

        public VideoController(ICloudStorageService cloudStorage)
        {
            _cloudService = cloudStorage;
        }

        public async Task<IActionResult> Video(string blob)
        {
            return File(await _cloudService.GetBlob("eventhistory", blob), "video/mp4");
        }
    }
}