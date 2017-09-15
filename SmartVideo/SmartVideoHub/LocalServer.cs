using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SmartVideo.Transport;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace SmartVideoHub
{
    public static class LocalServer
    {
        public static void StartServer()
        {
            Task.Run(() =>
            {
				try
				{
                    Console.WriteLine("Starting Thrift Server.");
					TProtocolFactory protocolFactory = new TBinaryProtocol.Factory(true, true);
					TServerTransport servertrans = new TServerSocket(new TcpListener(System.Net.IPAddress.Any, 7709), 10000);
					TTransportFactory transportFactory = new TFramedTransport.Factory();
					SmartVideoLocalServiceHandler hostServiceHandler = new SmartVideoLocalServiceHandler();
                    SmartVideoLocalService.Processor processor = new SmartVideoLocalService.Processor(hostServiceHandler);

					TServer serverEngine = new TThreadPoolServer(processor, servertrans, transportFactory, protocolFactory);
					serverEngine.Serve();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}

			});
        }
    }
}
