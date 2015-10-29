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


using MysqlDbContext = ClassLibraryApi.AnXinWH.AnXinWH;
using MySql.Data.MySqlClient;
using ClassLibraryApi.AnXinWH;

namespace AnXinWH.SocketRFIDService
{

    public static class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static string _sysCompareMin { get; private set; }

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
        public static void testDB()
        {
            try
            {
                using (var db = new MysqlDbContext())
                {
                    var tmpNewt_checkresult = new t_checkresult();
                    var tmpNewt_checkdetailresult = new t_checkdetailresult();

                    var tmpGuidId = Guid.NewGuid().ToString();
                    #region 主表
                    tmpNewt_checkresult.check_id = tmpGuidId;
                    tmpNewt_checkresult.check_date = DateTime.Now;
                    tmpNewt_checkresult.bespeak_no = "RFID";
                    tmpNewt_checkresult.bespeak_date = "8:00:00";
                    tmpNewt_checkresult.user_no = "点检";
                    tmpNewt_checkresult.user_nm = "点检";
                    tmpNewt_checkresult.status = 1;
                    tmpNewt_checkresult.remark = "点检";

                    tmpNewt_checkresult.addtime = DateTime.Now;
                    tmpNewt_checkresult.adduser = "StocketRFID";
                    tmpNewt_checkresult.updtime = DateTime.Now;
                    tmpNewt_checkresult.upduser = "StocketRFID";


                    db.t_checkresult.Add(tmpNewt_checkresult);
                    var saveflag2 = db.SaveChanges();
                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static void setSysConfig()
        {       

            _sysCompareMin = setValue("sysCompareMin", "5");
           
        }

#if Dev
        private static void Main(string[] args)
        {
            try
            {

                //testDB();
                //return;

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
