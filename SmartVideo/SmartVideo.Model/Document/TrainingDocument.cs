using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SmartVideo.Model.Document
{
    public class TrainingDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string ImageBlob { get; set; }
        public List<BoundingBox> PedestrianBoxes { get; set; }

        public TrainingDocument()
        {
            PedestrianBoxes = new List<BoundingBox>();
        }
    }
}
