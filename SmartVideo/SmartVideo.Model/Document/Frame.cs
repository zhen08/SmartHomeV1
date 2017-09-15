using System;
using System.Collections.Generic;

namespace SmartVideo.Model.Document
{
    public class Frame
    {
        public int FrameNumber { get; set; }
        public List<BoundingBox> PedestrianBoxes { get; set; }
		public List<BoundingBox> FaceBoxes { get; set; }

        public Frame()
        {
            PedestrianBoxes = new List<BoundingBox>();
            FaceBoxes = new List<BoundingBox>();
        }
	}
}
