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
        public static string _serverIP { get; private set; }
        //for checktime

        public static int _checkCount { get; set; }
        public static int _checkAutoCount { get; set; }

        public static Random _tmpRandom = new Random(100000);
        public static Dictionary<string, bool> _checkCurrRfid { get; set; }
        public static Dictionary<string, bool> _checkAutoRfid { get; set; }

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
        /// <summary>
        /// 保存日记
        /// </summary>
        /// <param name="message">RFID</param>
        /// <param name="mod_id">操作名称</param>
        /// <param name="types">0:入库、1：出库、2：上架、3：基本信息)</param>
        /// <param name="result">0/1</param>
        public static void saveLog(string mod_id, string message, short types, short result)
        {
            try
            {
                //init mysql db
                using (var db = new MysqlDbContext())
                {

                    var tmpLog = new t_syslogrecd();
                    var guid = Guid.NewGuid();
                    var rand = new Random();
                    tmpLog.log_id = DateTime.Now.ToString("yyyyMMddHHmmss") + "R" + rand.Next(100000).ToString();

                    tmpLog.operatorid = "SocketRFID";
                    tmpLog.message = message;// "RFID采集";
                    tmpLog.type = types;
                    tmpLog.result = result;
                    tmpLog.mod_id = mod_id;//"StocketRFID";
                    tmpLog.adduser = _serverIP;
                    tmpLog.addtime = DateTime.Now;
                    tmpLog.org_no = "SocketRFID";
                    db.t_syslogrecd.Add(tmpLog);

                    db.SaveChanges();

                    logger.DebugFormat("*******SAVE LOG******");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public static void saveVideoSet(string message, short channel)
        {
            try
            {
                //init mysql db
                using (var db = new MysqlDbContext())
                {

                    var tmpt_videodata = new t_videodata();
                    var guid = Guid.NewGuid();
                    var rand = new Random();
                    tmpt_videodata.video_no = DateTime.Now.ToString("yyyyMMddHHmmss") + "R" + rand.Next(100000).ToString();

                    tmpt_videodata.message = message;
                    tmpt_videodata.font_size = 20;
                    tmpt_videodata.font_type = 20;
                    tmpt_videodata.site = 0;
                    tmpt_videodata.Channel = channel;
                    tmpt_videodata.adduser = _serverIP;
                    tmpt_videodata.addtime = DateTime.Now;
                    tmpt_videodata.Version = "V1";
                    tmpt_videodata.status = 0;
                    tmpt_videodata.remark = "";
                    db.t_videodata.Add(tmpt_videodata);

                    db.SaveChanges();

                    logger.DebugFormat("*******SAVE Video Set******");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public static void testDB()
        {
            try
            {
                using (var db = new MysqlDbContext())
                {
                    //var tmpNewt_checkresult = new t_checkresult();
                    //var tmpNewt_checkdetailresult = new t_checkdetailresult();

                    //var tmpGuidId = Guid.NewGuid().ToString();
                    //#region 主表
                    //tmpNewt_checkresult.check_id = tmpGuidId;
                    //tmpNewt_checkresult.check_date = DateTime.Now;
                    //tmpNewt_checkresult.bespeak_no = "RFID";
                    //tmpNewt_checkresult.bespeak_date = "8:00:00";
                    //tmpNewt_checkresult.user_no = "点检";
                    //tmpNewt_checkresult.user_nm = "点检";
                    //tmpNewt_checkresult.status = 1;
                    //tmpNewt_checkresult.remark = "点检";

                    //tmpNewt_checkresult.addtime = DateTime.Now;
                    //tmpNewt_checkresult.adduser = "StocketRFID";
                    //tmpNewt_checkresult.updtime = DateTime.Now;
                    //tmpNewt_checkresult.upduser = "StocketRFID";


                    //db.t_checkresult.Add(tmpNewt_checkresult);
                    //var saveflag2 = db.SaveChanges();
                    //#endregion

                    var tmpLog = new t_syslogrecd();
                    //tmpLog.log_id = new Random(10000).Next(1000000);
                    tmpLog.operatorid = "SocketRFID";
                    tmpLog.message = "RFID采集";
                    tmpLog.type = 1;
                    tmpLog.result = 1;
                    tmpLog.mod_id = "StocketRFID";
                    tmpLog.adduser = "StocketRFID";
                    tmpLog.addtime = DateTime.Now;
                    tmpLog.org_no = "dd";
                    db.t_syslogrecd.Add(tmpLog);
                    var saveflag2 = db.SaveChanges();
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
            _serverIP = setValue("ServerIP", "127.0.0.1");
            _checkCount = 0;
            _checkCurrRfid = new Dictionary<string, bool>();
            _checkAutoCount = 0;
            _checkAutoRfid = new Dictionary<string, bool>();

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
