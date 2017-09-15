using System;
using System.IO;
using SmartVideo.Transport;

namespace SmartVideoHub
{
    public class SmartVideoLocalServiceHandler : SmartVideoLocalService.Iface
    {
        public bool uploadMedia(MediaStruct media)
        {
            try {
                Console.WriteLine("Receiving media from : {0}, size : {1}",media.DeviceId, media.Data.Length);
                MediaQueue.Instance.Enqueue(media);
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}
