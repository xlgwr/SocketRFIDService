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
        public static string _locPort { get; private set; }
        public static string _rmtHost { get; private set; }
        public static string _rmtPort { get; private set; }
        public static string _dstAddr { get; private set; }

        public static string setValue(string strname, string value)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[strname].ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return value;
            }
        }
        public static void setSysConfig()
        {
            _sysType = setValue("sysType", "0");
            _sysCompareMin = setValue("sysCompareMin", "5");
            _locPort = setValue("locPort", "8881");
            _rmtHost = setValue("rmtHost", "192.168.1.199");
            _rmtPort = setValue("rmtPort", "6666");
            _dstAddr = setValue("dstAddr", "0");
        }

#if Dev
        private static void Main(string[] args)
        {
            try
            {
                setSysConfig();

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
