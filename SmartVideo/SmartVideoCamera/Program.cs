using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartVideo.Transport;
using Shell.Execute;
using System.Threading;

namespace SmartVideoCamera
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string machineName = Environment.MachineName;//.Substring(0, Environment.MachineName.IndexOf('.'));
            Console.WriteLine("SmartVideo Camera Daemon started on {0}", machineName);

            Task.Run(() =>
            {
                Console.WriteLine("Launching video capturing task");
                var launcher = new ProgramLauncher();
                launcher.Launch("killall", "raspivid");
                Thread.Sleep(5000);
                try
                {
                    foreach (var file in Directory.GetFiles(@"/dev/shm/").Where(f => f.Contains("h264") || f.Contains("mp4")))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                launcher.Launch("raspivid", " -n -ih -w 1024 -h 768 -fps 10 -b 0 -qp 20 -t 0 -sg 60000 -o /dev/shm/v%08d.h264");
            });

            var conversionLauncher = new ProgramLauncher();
            while (true)
            {
                try
                {
                    var files = Directory.GetFiles(@"/dev/shm/").Where(f => f.Contains("h264")).OrderBy(f => f);
                    foreach (var file in files)
                    {
                        if (file != files.Last())
                        {
                            string mp4file = file.Replace("h264", "mp4");
                            if (!File.Exists(mp4file))
                            {
                                conversionLauncher.Launch("MP4Box", String.Format("-fps 10 -add {0} {1} ", file, mp4file));
                                Thread.Sleep(3000);
                            }
                            var media = new MediaStruct();
                            media.DeviceId = machineName;
                            media.TimeStamp = (Int32)File.GetLastWriteTime(file).Subtract(new DateTime(2008, 5, 24)).TotalSeconds;
                            media.MediaType = MEDIATYPE.MP4;
                            media.Data = File.ReadAllBytes(mp4file);
                            if (LocalClient.Instance.UploadMedia(media))
                            {
                                File.Delete(file);
                                File.Delete(mp4file);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Thread.Sleep(1000);
            }
        }
    }
}