using System;
namespace SmartVideo.Model.Document
{
    public class BoundingBox
    {
        public string Label { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }

        public BoundingBox()
        {
            Label = "";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
        }

        public BoundingBox(string label, int x1, int y1, int x2, int y2) {
            this.Label = label;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}
