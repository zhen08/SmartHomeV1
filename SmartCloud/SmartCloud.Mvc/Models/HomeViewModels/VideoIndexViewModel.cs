using SmartVideo.Model.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCloud.Mvc.Models.HomeViewModels
{
    public class VideoIndexViewModel
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime VideoTime { get; set; }
        public string VideoBlob { get; set; }
        public string ThumbnailBlob { get; set; }
        public bool ExportedTraining { get; set; }
        public IEnumerable<BoundingBox> BoundingBoxs { get; set; }
    }
}
