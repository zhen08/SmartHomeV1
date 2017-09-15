using System;
using SmartVideo.Transport;
using Thrift.Protocol;
using Thrift.Transport;

namespace SmartVideoCamera
{
    public class LocalClient
    {
        private static LocalClient _instance;
        public static LocalClient Instance => _instance ?? (_instance = new LocalClient());

        private TTransport transport;
        private SmartVideoLocalService.Client client;

        public LocalClient()
        {
            transport = new TFramedTransport(new TSocket("tx1.local", 7709, 10000));
            client = new SmartVideoLocalService.Client(new TBinaryProtocol(transport));
        }

        public bool UploadMedia(MediaStruct media)
        {
            try
            {
                Console.WriteLine("Uploading media, size:{0}", media.Data.Length);
                transport.Open();
                client.uploadMedia(media);
                transport.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transport.Close();
            }
            return false;
        }
    }

}
