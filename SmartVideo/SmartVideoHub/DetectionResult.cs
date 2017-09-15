using System;
using SmartVideo.Model.Document;
using SmartVideo.Transport;

namespace SmartVideoHub
{
    public class DetectionResult
    {
        public MediaStruct Media;
        public byte[] Thumbnail;
        public MediaDocument Document;
    }
}
