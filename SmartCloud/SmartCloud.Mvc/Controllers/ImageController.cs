using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartCloud.Mvc.Services;

namespace SmartCloud.Mvc.Controllers
{
    [Authorize]
    [Route("Image")]
    public class ImageController : Controller
    {
        private ICloudStorageService _cloudService;
        public ImageController(ICloudStorageService cloudStorage)
        {
            _cloudService = cloudStorage;
        }

        public async Task<IActionResult> Image(string blob)
        {
            return File(await _cloudService.GetBlob("eventhistory", blob), "image/jpeg");
        }
    }
}