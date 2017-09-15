using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartVideo.Model.Document
{
    public class MediaDocument
    {
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		public string DeviceId { get; set; }
        public int MediaTimeStamp { get; set; }
        public string VideoBlob { get; set; }
		public string ThumbnailBlob { get; set; }
		public List<Frame> Frames { get; set; }
        public bool ActiveMedia { get; set; }
        public bool ExportedTraining { get; set; }
        public MediaDocument()
        {
            Frames = new List<Frame>();
        }
	}
}
