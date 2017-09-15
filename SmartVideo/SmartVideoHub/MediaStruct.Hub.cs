using System;
namespace SmartVideo.Transport
{
    public partial class MediaStruct
    {
        public string FileName => String.Format("{0}_{1}.{2}", DeviceId, TimeStamp, MediaType);
        public string BlobName => String.Format("{0}_{1}", DeviceId, TimeStamp);
    }
}
