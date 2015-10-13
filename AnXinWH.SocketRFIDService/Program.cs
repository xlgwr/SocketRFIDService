//#define Dev
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using log4net;
using System.Reflection;


namespace AnXinWH.SocketRFIDService
{

    public static class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  <!--出入库标记，0：入库，1：出库-->
        /// </summary>
        public static string _sysType { get; private set; }
        public static string _sysCompareMin { get; private set; }

        public static void setSysConfig()
        {
            try
            {
                _sysType = System.Configuration.ConfigurationManager.AppSettings["sysType"].ToString();
            }
            catch (Exception ex)
            {
                _sysType = "0";
                logger.Error(ex);
            }

            //sysCompareMin
            try
            {
                _sysCompareMin = System.Configuration.ConfigurationManager.AppSettings["sysCompareMin"].ToString();
            }
            catch (Exception ex)
            {
                _sysCompareMin = "5";
                logger.Error(ex);
            }
        }

#if Dev
        private static void Main(string[] args)
        {
            try
            {
                setSysType();

                Test test = new Test();
                test.OnStart();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

#else
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            try
            {
                setSysConfig();
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                 { 
                new Service1() 
                  };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

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
