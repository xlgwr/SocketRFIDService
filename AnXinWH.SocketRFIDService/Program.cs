//#define Dev
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;

namespace AnXinWH.SocketRFIDService
{
    static class Program
    {
#if Dev
        private static void Main(string[] args)
        {
            Test test = new Test();
            test.OnStart();
        }

#else
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
        }
#endif
    }
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
