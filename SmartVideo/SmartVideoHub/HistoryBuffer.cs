using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartVideo.Transport;

namespace SmartVideoHub
{
    public class HistoryBuffer
    {
        private static HistoryBuffer _instance;
        public static HistoryBuffer Instance => _instance ?? (_instance = new HistoryBuffer());

        private const int MaxBufferDepth = 2;

        private Dictionary<string, List<MediaStruct>> buffer;

        public HistoryBuffer()
        {
            buffer = new Dictionary<string, List<MediaStruct>>();
        }

        public void AddMedia(MediaStruct media)
        {
            if (!buffer.ContainsKey(media.DeviceId)) {
                buffer.Add(media.DeviceId,new List<MediaStruct>());
            }
            var mediaList = buffer[media.DeviceId];
            mediaList.Add(media);
            //File.WriteAllBytes(media.FileName,media.Data);
            while (mediaList.Count>MaxBufferDepth) {
                //File.Delete(mediaList[0].FileName);
                mediaList.RemoveAt(0);
            }
        }

        public IEnumerable<MediaStruct> GetHistory(string deviceId)
        {
            if (!buffer.ContainsKey(deviceId)) {
                return Enumerable.Empty<MediaStruct>();
            }
            return buffer[deviceId];
        }
    }
}
