using System;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using SmartVideo.Transport;
using SmartVideo.Model.Document;
using Shell.Execute;
using System.Threading.Tasks;

namespace SmartVideoHub
{
    public static class DetectNet
    {
        public const string VIDEO_FILE_NAME = @"/dev/shm/detect.mp4";
        public const string START_FILE_NAME = @"/dev/shm/detect.start";
        public const string OUTPUT_FILE_NAME = @"/dev/shm/detect.out";
        public const string THUMBNAIL_FILE_NAME = @"/dev/shm/detect.jpg";

        public static DetectionResult DetectMedia(MediaStruct media)
        {
            var startTime = DateTime.UtcNow;
            var result = new DetectionResult();
            result.Media = media;
            result.Document = new MediaDocument { DeviceId = media.DeviceId, MediaTimeStamp = media.TimeStamp, VideoBlob = media.BlobName + ".MP4", ThumbnailBlob = media.BlobName + ".JPG" };
            if (File.Exists(OUTPUT_FILE_NAME))
            {
                File.Delete(OUTPUT_FILE_NAME);
            }
            File.WriteAllBytes(VIDEO_FILE_NAME, media.Data);
            File.WriteAllText(START_FILE_NAME, ".");
            for (int i = 0; i < 3000; i++)
            {
                if (File.Exists(OUTPUT_FILE_NAME))
                {
                    break;
                }
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            if (!File.Exists(OUTPUT_FILE_NAME))
            {
                Console.WriteLine("Detection Timeout!!");
                StartDetectDaemon();
                return null;
            }
            if (File.Exists(THUMBNAIL_FILE_NAME))
            {
                result.Thumbnail = File.ReadAllBytes(THUMBNAIL_FILE_NAME);
            }
            var lines = File.ReadAllLines(OUTPUT_FILE_NAME);
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                var numOfPedBoxes = Convert.ToInt32(fields[2]);
                var numOfFaceBoxes = Convert.ToInt32(fields[4]);
                if ((numOfPedBoxes + numOfFaceBoxes > 0) && (fields.Length == ((numOfPedBoxes + numOfFaceBoxes) * 4) + 5))
                {
                    var frame = new Frame();
                    frame.FrameNumber = Convert.ToInt32(fields[0]);
                    for (int i = 5; i < (5 + (numOfPedBoxes * 4)); i += 4)
                    {
                        int x1 = Convert.ToInt32(fields[i]);
                        int y1 = Convert.ToInt32(fields[i + 1]);
                        int x2 = Convert.ToInt32(fields[i + 2]);
                        int y2 = Convert.ToInt32(fields[i + 3]);
                        if ((x2 - x1 > 48) && (y2 - y1 > 96))
                        {
                            frame.PedestrianBoxes.Add(new BoundingBox("pedestrian",x1,y1,x2,y2));
                        }
                    }
                    for (int i = 5 + (numOfPedBoxes * 4); i < (5 + ((numOfPedBoxes + numOfFaceBoxes) * 4)); i += 4)
                    {
						int x1 = Convert.ToInt32(fields[i]);
						int y1 = Convert.ToInt32(fields[i + 1]);
						int x2 = Convert.ToInt32(fields[i + 2]);
						int y2 = Convert.ToInt32(fields[i + 3]);
						if ((x2 - x1 > 60) && (y2 - y1 > 60))
						{
							frame.PedestrianBoxes.Add(new BoundingBox("face", x1, y1, x2, y2));
						}
                    }
                    if ((frame.PedestrianBoxes.Count > 0) || (frame.FaceBoxes.Count > 0))
                    {
                        result.Document.Frames.Add(frame);
                    }
                }
            }
            Console.WriteLine("Detection done in {0} seconds for {1}", DateTime.UtcNow.Subtract(startTime).TotalSeconds, media.DeviceId);
            result.Document.ActiveMedia = (result.Document.Frames.Count > 0);
            return result;
        }

        public static void StartDetectDaemon()
        {
            var launcher = new ProgramLauncher();
            launcher.Launch("killall", "detectnet-daemon");
            Thread.Sleep(15000);
            foreach (var file in Directory.GetFiles(@"/dev/shm/").Where(f => f.Contains("detect")))
            {
                File.Delete(file);
            }
            if (File.Exists(OUTPUT_FILE_NAME))
            {
                File.Delete(OUTPUT_FILE_NAME);
            }
            if (File.Exists(VIDEO_FILE_NAME))
            {
                File.Delete(VIDEO_FILE_NAME);
            }
            if (File.Exists(START_FILE_NAME))
            {
                File.Delete(START_FILE_NAME);
            }
            Task.Run(() =>
            {
                launcher.Launch("detectnet-daemon", "");
            });
            Console.WriteLine("Detectnet daemon started.");
        }


        class DetectResult
        {
            public int Frame { get; set; }
            public string Pedestrian { get; set; }
            public int PedestrianDetected { get; set; }
            public string Face { get; set; }
            public int FaceDetected { get; set; }
        }
    }
}
