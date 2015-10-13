using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using log4net;

using MysqlDbContext = ClassLibraryApi.AnXinWH.AnXinWH;
using MySql.Data.MySqlClient;
using ClassLibraryApi.AnXinWH;

namespace AnXinWH.SocketRFIDService
{
    public class TcpListernerThread
    {
        public readonly ILog logger;
        #region attr
        /// <summary>
        /// 记录机台当前状态是自动还是手动
        /// </summary>
        public static Hashtable s_hasDeviceState = new Hashtable();

        /// <summary>
        /// 记录机台上报信号的最新时间
        /// </summary>
        public static Hashtable s_hasDeviceLastReportTime = new Hashtable();

        /// <summary>
        /// 记录机台上一次开模时间
        /// </summary>
        public static Hashtable s_hasDeviceLastSecondReportTime = new Hashtable();

        /// <summary>
        /// 记录机台连接时间对象//
        /// </summary>
        public static Hashtable s_hasConnectObject = new Hashtable();
        /// <summary>
        /// 线程池
        /// </summary>
        public static TaskPool pool = new TaskPool();

        /// <summary>
        /// 设备ID与成型机ID绑定表
        /// </summary>
        public static Hashtable s_hasReportIdToDeviceId = new Hashtable();

        /// <summary>
        /// 设备ID与成型机IP绑定表
        /// </summary>
        public static Hashtable s_hasReportIdToIP = new Hashtable();

        /// <summary>
        /// 设备ID与成型机IP绑定表
        /// </summary>
        public static Hashtable s_hasReportIPId = new Hashtable();

        public static object lockerRT = new object();//添加一个对象作为锁,锁s_hasDeviceLastReportTime

        public static object locker = new object();//添加一个对象作为锁

        public static object lockerRTs = new object();
        public static bool s_bolWork = false;

        #endregion

        /// <summary>
        /// N 分钟内扫描无效
        /// </summary>
        public static Dictionary<string, bool> _tmpListScanRFID = new Dictionary<string, bool>();
        public TcpListernerThread()
        {
            logger = LogManager.GetLogger(GetType());
        }
        #region 用TcpListener监听(测试)

        public static System.Net.Sockets.TcpListener mSocketL;
        public static System.Threading.Thread mThread;
        public static String mPort = "";
        public static String mIP = "127.0.0.1";
        public static int mBlockLog = 0;
        public static Encoding encoding = Encoding.GetEncoding("GB2312"); //解码器（可以用于汉字）         private Socket client;         private string data = null; 
        //private byte[] receiveBytes = new byte[1024];//服务器端设置缓冲区         private int recCount; 

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static System.Net.Sockets.Socket ms;
        public static System.Net.IPAddress ipAddress = null;
        //启动开始开始时间//
        public static DateTime dtStatrt = DateTime.Now;
        public static DateTime _dtStatrtClearRFIDID = DateTime.Now;

        public void GetMessage()
        {
            //如果newsock不为空，那说明不是第一次进来，那么只需要开启一下状态就好
            if (mSocketL != null)
            {
                lock (locker)//锁
                {
                    s_bolWork = true;
                }
                return;
            }

            try
            {
                //init mysql db
                using (var db = new MysqlDbContext())
                {
                    var tmpcont = db.m_users.Count();
                    logger.DebugFormat("********************{0} 个用户.", tmpcont);
                }

                ipAddress = IPAddress.Parse(mIP);

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, int.Parse(mPort));

                // 生成一个TCP的socket
                ms = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                ms.Bind(localEndPoint);
                ms.Listen(100);


                //启动开始开始时间//
                dtStatrt = DateTime.Now;
                _dtStatrtClearRFIDID = DateTime.Now;

                while (true)
                {
                    try
                    {
                        allDone.Reset();
                        //异步接收//
                        ms.BeginAccept(new AsyncCallback(AcceptCallback), ms);
                        allDone.WaitOne();

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ms.Dispose();
                logger.Error(ex.Message.ToString());
            }
            finally
            {
                mSocketL = null;
            }
        }



        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                logger.Debug("******AcceptCallBack.");

                allDone.Set();

                Socket sc = (Socket)ar.AsyncState;
                Socket socket = sc.EndAccept(ar);

                // 造一个容器，并用于接收命令.

                StateObject state = new StateObject();

                state.workSocket = socket;


                IPEndPoint clientipe = (IPEndPoint)socket.RemoteEndPoint;
                string w_strIP = clientipe.Address.ToString();


                logger.DebugFormat("******AcceptCallBack.Enter Ip: {0}.", w_strIP);

                //IP如果存在于列表中，则需要去读设备的初始状态
                //if (s_hasReportIdToIP.ContainsKey(w_strIP) == true)
                //{
                //    string[] w_chardata;
                //    w_chardata = strToTochar("55" + s_hasReportIdToIP[w_strIP] + "AA000120");

                //    byte[] msgBuff = strToToHexByte("55" + s_hasReportIdToIP[w_strIP] + "AA000120" + Sum_CRC8(w_chardata) + "16");

                //    socket.Send(msgBuff);
                //}

                socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,

                 new AsyncCallback(ReadCallback), state);

                dtStatrt = DateTime.Now;

            }
            catch (Exception ex)
            {
                logger.Error("******AcceptCallback:{0}.", ex);
            }

        }

        public void Send(Socket handler, byte[] data)
        {

            // 消息格式转换.

            byte[] byteData = data;

            // 开始发送数据给远程目标.

            handler.BeginSend(byteData, 0, byteData.Length, 0,

                new AsyncCallback(SendCallback), handler);

        }

        public void SendCallback(IAsyncResult ar)
        {



            // 从state对象获取socket.

            Socket handler = (Socket)ar.AsyncState;

            //完成数据发送

            int bytesSent = handler.EndSend(ar);

            logger.DebugFormat("######Sent {0} bytes to client.", bytesSent);

            //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            handler.Shutdown(SocketShutdown.Both);

            handler.Close();

        }
        /// <summary>
        ///  <!--出入库标记，0：入库，1：出库-->
        ///  出入库标记更新
        ///  status 状态 smallint 默认1:可用 0:不可用
        /// </summary>
        /// <param name="sysType"></param>
        /// <returns></returns>
        public bool changeInOrOutStock(string sysType, string tmpStrRFID)
        {
            try
            {
                switch (sysType)
                {
                    case "0":
                        //0：入库
                        //查实际入库明细(2),有-->更新为(1);
                        using (var db = new MysqlDbContext())
                        {
                            var tmpcont = db.t_stockinctnnodetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 2).Count();

                            if (tmpcont > 0)
                            {
                                var tmpModelin = db.t_stockinctnnodetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 2).First();

                                logger.DebugFormat("#开始入库**********#########共有{0}条记录.入库单号:{1},货物编号：{2},托盘号:{3}，rfid_no:{4}", tmpcont, tmpModelin.stockin_id, tmpModelin.prdct_no, tmpModelin.ctnno_no, tmpStrRFID);
                                logger.Debug("*********(2--->1)[status 1:可用 0:不可用 2：卸料]**********");

                                string tmpUpdateSQL = "update t_stockinctnnodetail set STATUS='1' WHERE STATUS='2' and rfid_no=@rfid_no";
                                var tmpRetunCount = db.Database.ExecuteSqlCommand(tmpUpdateSQL, new MySqlParameter("@rfid_no", tmpStrRFID.Trim()));

                                if (tmpRetunCount > 0)
                                {
                                    logger.DebugFormat("*#入库**********#########t_stockinctnnodetail: 更新成功入库标记，共有{0}条已更新.入库单号:{1},货物编号：{2},托盘号:{3}，rfid_no:{4}", tmpRetunCount, tmpModelin.stockin_id, tmpModelin.prdct_no, tmpModelin.ctnno_no, tmpStrRFID);

                                    return true;
                                }

                                logger.DebugFormat("*error开始入库*失败，系统错误,请联系管理员**************************************************************", tmpStrRFID);
                                return false;
                            }
                            logger.DebugFormat("*开始入库*失败，未查到[可入库/有效的]实际入库明细记录,RFID:{0}**************************************************************", tmpStrRFID);
                            return false;
                        }
                        break;

                    case "1":
                        //1：出库
                        using (var db = new MysqlDbContext())
                        {
                            //库存明细表
                            //1:在库可用,0:不可用(出库更新库存时更新(1-->0)

                            //查库存明细表(1),得:仓单号.
                            //(有-->查申.货物出库明细(仓单号)
                            //      [有-->更新库存表,库存明细表(0),无-->报警]),
                            //无-->报警)                            
                            var tmpcont = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 1).Count();
                            if (tmpcont > 0)
                            {

                                #region 查库存明细表
                                //查库存明细表
                                var tmpstockdetailForOut = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 1).First();

                                if (string.IsNullOrEmpty(tmpstockdetailForOut.receiptNo))
                                {
                                    logger.DebugFormat("*开始出库**********#########没有仓单号,货物编号:{0},托盘号:{1}，rfid_no:{2}", tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);
                                    //***********报警*********
                                    return false;
                                }
                                logger.DebugFormat("*开始出库**********#########共有{0}条记录.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpcont, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                #endregion
                                #region 货物出库明细
                                //货物出库明细
                                var tmpStockoutDetails = db.t_stockoutdetail.Where(m => m.receiptNo.Equals(tmpstockdetailForOut.receiptNo) && m.prdct_no.Equals(tmpstockdetailForOut.prdct_no) && m.status == 1).FirstOrDefault();

                                if (tmpStockoutDetails == null)
                                {
                                    logger.DebugFormat("*error开始出库**********{0}#########没有查到[有效的]货物出库明细记录.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpcont, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                    return false;
                                }
                                #endregion
                                #region 插入明细记录.
                                //插入明细记录.
                                var tmpExitStockDetails = db.t_stockoutctnnodetail.Find(new object[]{
                                    tmpStockoutDetails.stockout_id,
                                    tmpstockdetailForOut.prdct_no,
                                    tmpstockdetailForOut.rfid_no,
                                    tmpstockdetailForOut.ctnno_no
                                });
                                if (tmpExitStockDetails == null)
                                {
                                    var tmpNewStockDetails = new t_stockoutctnnodetail();
                                    tmpNewStockDetails.stockout_id = tmpStockoutDetails.stockout_id;

                                    tmpNewStockDetails.prdct_no = tmpstockdetailForOut.prdct_no;
                                    tmpNewStockDetails.rfid_no = tmpstockdetailForOut.rfid_no;
                                    tmpNewStockDetails.ctnno_no = tmpstockdetailForOut.ctnno_no;
                                    tmpNewStockDetails.receiptNo = tmpstockdetailForOut.receiptNo;
                                    tmpNewStockDetails.pqty = tmpstockdetailForOut.pqty;
                                    tmpNewStockDetails.qty = tmpstockdetailForOut.qty;
                                    tmpNewStockDetails.nwet = tmpstockdetailForOut.nwet;
                                    tmpNewStockDetails.gwet = tmpstockdetailForOut.gwet;
                                    tmpNewStockDetails.adduser = "rfid";
                                    tmpNewStockDetails.updtime = DateTime.Now;
                                    tmpNewStockDetails.upduser = "rfid";
                                    tmpNewStockDetails.addtime = DateTime.Now;
                                    tmpNewStockDetails.status = 1;

                                    db.t_stockoutctnnodetail.Add(tmpNewStockDetails);

                                    //db.SaveChanges();
                                }


                                #endregion
                                #region 更新库存
                                //更新库存
                                var tmpstock = db.t_stock.Where(m => m.prdct_no.Equals(tmpstockdetailForOut.prdct_no)).FirstOrDefault();

                                if (tmpstock != null)
                                {
                                    logger.DebugFormat("*开始出库**********#########更新库存前:箱数:{0},数量:{1},重量:{2},净重:{3}", tmpstock.pqty, tmpstock.qty, tmpstock.gwet, tmpstock.nwet);

                                    tmpstock.pqty -= tmpstockdetailForOut.pqty;
                                    tmpstock.qty -= tmpstockdetailForOut.qty;
                                    tmpstock.gwet -= tmpstockdetailForOut.gwet;
                                    tmpstock.nwet -= tmpstockdetailForOut.nwet;

                                    //if (tmpflagsave > 0)
                                    //{
                                    //    logger.DebugFormat("*开始出库*********#########Save flag:{0}.", tmpflagsave);
                                    //    logger.DebugFormat("*开始出库**********#########更新库存成功,货物编号:{0},托盘号:{1}，rfid_no:{2}", tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);
                                    //    logger.DebugFormat("*开始出库**********#########更新库存后:箱数:{0},数量:{1},重量:{2},净重:{3}", tmpstock.pqty, tmpstock.qty, tmpstock.gwet, tmpstock.nwet);

                                    //}
                                    //else
                                    //{
                                    //    logger.DebugFormat("*error开始出库**********#########更新库存失败,货物编号:{0},托盘号:{1}，rfid_no:{2}", tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                    //    return false;
                                    //}
                                }
                                else
                                {
                                    logger.DebugFormat("*error开始出库**********#########没有库存,货物编号:{0},托盘号:{1}，rfid_no:{2}", tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                    return false;
                                }

                                #endregion

                                #region 更新明细状态
                                //更新明细状态 
                                string tmpUpdateSQL = "update t_stockdetail set STATUS='0' WHERE STATUS='1' and rfid_no=@rfid_no";
                                var tmpRetunCount = db.Database.ExecuteSqlCommand(tmpUpdateSQL, new MySqlParameter("@rfid_no", tmpStrRFID.Trim()));

                                if (tmpRetunCount > 0)
                                {
                                    //logger.DebugFormat("*success出库明细**********#########t_stockdetail: 更新成功明细出库标记，共有{0}条已更新.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpRetunCount, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);
                                    logger.DebugFormat("*success出库**********#########:更新成功，共有{0}条已更新.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpRetunCount, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                    var tmpflagsave = db.SaveChanges();

                                    return true;
                                }

                                logger.DebugFormat("*error开始出库*失败，系统错误,请联系管理员**************************************************************", tmpStrRFID);
                                return false;

                                #endregion
                            }

                            logger.DebugFormat("*error开始出库*失败，未查到[可出库/有效的]库存明细表记录,RFID:{0}**************************************************************", tmpStrRFID);
                            return false;
                        }
                        break;

                    default:
                        break;
                }


                return false;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0}:{1},flag:{2},rfid_no:{3}", 240, ex, sysType, tmpStrRFID);
                return false;
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {

            String content = String.Empty;

            // 从异步state对象中获取state和socket对象.

            StateObject state = (StateObject)ar.AsyncState;

            Socket handler = state.workSocket;

            List<string[]> tmpGetList = new List<string[]>();

            try
            {


                // 从客户socket读取数据.

                int bytesReadLength = handler.EndReceive(ar);

                if (bytesReadLength > 0)
                {
                    IPEndPoint tclientipe = (IPEndPoint)handler.RemoteEndPoint;
                    string tw_strIP = tclientipe.Address.ToString();

                    byte[] tmpBuffer = new byte[bytesReadLength];
                    Array.Copy(state.buffer, tmpBuffer, bytesReadLength);

                    var to16 = tmpBuffer.ToList().Select(m => m.ToString("X")).ToArray();

                    //len:9
                    //A5,1,6,81,42,25,0,B7,B5
                    //get it 81,42,25

                    if (to16.Length > 9)
                    {

                        int tmplen = to16.Length;
                        while (tmplen > 0)
                        {
                            string[] tmp169 = new string[9];
                            tmplen -= 9;
                            Array.Copy(to16, tmplen, tmp169, 0, 9);
                            tmpGetList.Add(tmp169);
                        }
                    }
                    else
                    {
                        tmpGetList.Add(to16);
                    }

                    logger.DebugFormat("#*************Get {0} 个 RFID.", tmpGetList.Count());
                    logger.InfoFormat("#*************Get {0}  个 RFID.", tmpGetList.Count());
                    foreach (var item in tmpGetList)
                    {
                        string[] toChar = new string[3];//tmpBuffer.ToList().Select(m => (Char)m).ToArray();
                        Array.Copy(item, 3, toChar, 0, 3);
                        string tmpMoveFlag = item[6];

                        var tmpItemRFID = String.Join("", toChar);

                        if (_tmpListScanRFID.Keys.Contains(tmpItemRFID))
                        {

                            if (_tmpListScanRFID[tmpItemRFID])
                            {
                                logger.DebugFormat("#***********已扫并处理OK，不做处理。RFID: {0}, Buffer16:{1}.", tmpItemRFID, String.Join(",", item));
                                logger.InfoFormat("#***********已扫并处理OK，不做处理。RFID: {0}, Buffer16:{1}.", tmpItemRFID, String.Join(",", item));

                                continue;
                            }
                            else
                            {
                                logger.DebugFormat("#***********已扫但处理失败，重做处理。RFID: {0}, Buffer16:{1}.", tmpItemRFID, String.Join(",", item));

                            }

                        }
                        else
                        {
                            _tmpListScanRFID.Add(tmpItemRFID, false);
                        }

                        logger.DebugFormat("*******************_______*********************#read {0} client {1} bytes, RFID: {2},Buffer16:{3}.", tw_strIP, bytesReadLength, tmpItemRFID, String.Join(",", item));
                        logger.InfoFormat("#read {0} client {1} bytes,RFID: {2}, Buffer16:{3}.", tw_strIP, bytesReadLength, tmpItemRFID, String.Join(",", item));
                        if (tmpMoveFlag.Equals("1"))
                        {
                            logger.InfoFormat("#read {0} Move Flag.", tmpMoveFlag);
                        }
                        logger.DebugFormat("#read {0} Move Flag.", tmpMoveFlag);

                        
                        //处理数据
                        //test to send back    
                        //handler.Send(state.buffer, 0, bytesReadLength, SocketFlags.None);

                        //todo test
                        var tmpResule = changeInOrOutStock(Program._sysType, tmpItemRFID);
                        _tmpListScanRFID[tmpItemRFID] = tmpResule;
                        //logger.InfoFormat("SQL result:{0}.", tmpResule);

                    }

                    //close current workSocket

                    logger.DebugFormat("**##客户socket读取数据结束,Close current Connect IP:{0}*************************************************************", tw_strIP);

                    handler.Close();

                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                if (handler.Connected == true)
                {
                    //logger.DebugFormat("***********BeginReceive,{0}", handler);
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,

                             new AsyncCallback(ReadCallback), state);
                }

                try
                {
                    if (DateTime.Compare(DateTime.Now, _dtStatrtClearRFIDID.AddSeconds(5)) > 0)
                    {
                        logger.DebugFormat("****{0} 分钟到了。清理采集的数据个数：{1}。", Program._sysCompareMin, _tmpListScanRFID.Count());
                        _tmpListScanRFID.Clear();
                        _dtStatrtClearRFIDID = DateTime.Now;
                    }
                    if (DateTime.Compare(DateTime.Now, dtStatrt.AddHours(1)) > 0)
                    {
                        logger.DebugFormat("***********hander close.{0}", handler);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }



            }

        }



        private void ProcessDate(Object obj)
        {
            try
            {


                int bytesRec;
                string w_strIP = "";
                System.Net.Sockets.Socket sck = (System.Net.Sockets.Socket)obj;
                IPEndPoint clientipe = (IPEndPoint)sck.RemoteEndPoint;
                w_strIP = clientipe.Address.ToString();

                DateTime w_TimeStart = DateTime.Now;
                byte[] receiveBytes = new byte[1024];

                // SqlClass _sqlclass=new SqlClass();



                //有采集数据就不用记录心跳数据//
                DateTime dtimeHead = DateTime.Now;

                //启动开始开始时间//
                DateTime dtStatrt = DateTime.Now;

                //IP如果存在于列表中，则需要去读设备的初始状态
                if (s_hasReportIdToIP.ContainsKey(w_strIP) == true)
                {
                    string[] w_chardata;
                    w_chardata = strToTochar("55" + s_hasReportIdToIP[w_strIP] + "AA000120");

                    byte[] msgBuff = strToToHexByte("55" + s_hasReportIdToIP[w_strIP] + "AA000120" + Sum_CRC8(w_chardata) + "16");
                    sck.Send(msgBuff);
                }

                while (true)
                {
                    try
                    {
                        bytesRec = sck.Receive(receiveBytes);

                        //停止服务的话就不往下走了
                        if (s_bolWork != true)
                        {
                            //接着监听
                            continue;
                        }



                        if (bytesRec > 0)
                        {

                            //处理数据

                            int i = bytesRec;

                            List<Hashtable> Lists = new List<Hashtable>();
                            Hashtable hs = new Hashtable();
                            Hashtable hsport = new Hashtable();
                            Hashtable hsTwo = new Hashtable();
                            byte[] w_bytes = new byte[15];
                            //byte[] device = new byte[4];
                            TimeSpan ts = new TimeSpan();

                            string device = "";
                            int indexStart = 0;
                            int indexLength = 15;
                            int index = 0;
                            int Count = 1;
                            int J;

                            int Start = 0x55;
                            int End = 0x16;
                            int Upload = 0x2f;
                            int Heart = 0x3f;
                            int Read = 0x20;

                            int intdevicePort = 1;
                            while (index < i)
                            {
                                indexStart = indexLength * (Count - 1);


                                for (J = 0; J < 15; J++)
                                {
                                    if (receiveBytes[indexStart + J] == Start)
                                    {
                                        w_bytes = new byte[15];
                                        hs = new Hashtable();
                                        device = "";
                                    }

                                    w_bytes[J] = receiveBytes[indexStart + J];

                                    if (J > 0 && J < 5)
                                    {
                                        device = device + (IntToStrHex(int.Parse(receiveBytes[J].ToString()).ToString("X"), 2));
                                    }

                                    if (receiveBytes[indexStart + J] == End && (J == 14 || J == 6 || J == 12))
                                    {

                                        //类型区分//

                                        if (J == 6 && w_bytes[5] == Heart)//1：心跳
                                        {
                                            hs.Add("flag", "1");
                                        }
                                        else if (w_bytes[8] == Upload || w_bytes[8] == Read)// 0;上报电文//
                                        {
                                            hs.Add("flag", "0");
                                        }

                                        else//错误类型//
                                        {
                                            hs.Add("flag", "99");
                                        }

                                        //设备名称//
                                        hs.Add("ReportID", device);

                                        //心跳电文不用解析 设备端口号和高低电平//
                                        if (hs["flag"].ToString() == "0")
                                        {

                                            if (w_bytes[8] != Read)
                                            {
                                                hsport = GetPort(w_bytes[9], w_bytes[10]);
                                            }
                                            else
                                            {
                                                hsport = GetReadPort(w_bytes[9]);
                                            }
                                        }

                                        //设备端口号为0的都不处理
                                        if (intdevicePort != 0)
                                        {

                                            //Lists.Add(hs);

                                            //合并
                                            foreach (var item in hsport.Keys)
                                            {

                                                if (int.Parse(item.ToString()) == 2)//自动
                                                {
                                                    if (int.Parse(hsport[item].ToString()) == 1)//高电平时
                                                    {
                                                        Hashtable hsTemp = new Hashtable();
                                                        hsTemp.Add("flag", hs["flag"]);
                                                        hsTemp.Add("ReportID", hs["ReportID"]);
                                                        hsTemp.Add("State", 1);  //添个手动低电平
                                                        hsTemp.Add("DC", 0);

                                                        Lists.Add(hsTemp);
                                                    }
                                                }

                                                hsTwo.Add("flag", hs["flag"]);
                                                hsTwo.Add("ReportID", hs["ReportID"]);
                                                hsTwo.Add("State", item);
                                                hsTwo.Add("DC", int.Parse(hsport[item].ToString()));

                                                Lists.Add(hsTwo);

                                                if (int.Parse(item.ToString()) == 2)//自动
                                                {
                                                    if (int.Parse(hsport[item].ToString()) == 0)//低电平时
                                                    {
                                                        Hashtable hsTemp = new Hashtable();
                                                        hsTemp.Add("flag", hs["flag"]);
                                                        hsTemp.Add("ReportID", hs["ReportID"]);
                                                        hsTemp.Add("State", 1);  //添个手动高电平
                                                        hsTemp.Add("DC", 1);

                                                        Lists.Add(hsTemp);
                                                    }
                                                }

                                                //心跳间隔//
                                                dtimeHead = DateTime.Now;

                                            }

                                            if (hsport.Count == 0)
                                            {
                                                //3分钟内未有采集数据上传 就执行心跳数据//
                                                ts = DateTime.Now - dtimeHead;

                                                //if (ts.Minutes > 3)
                                                //{
                                                Lists.Add(hs);
                                                //}
                                            }


                                        }

                                        index = index + J + 1;

                                        Count++;

                                        break;
                                    }

                                }



                            }

                            //更新设备的上报时间
                            if (s_hasDeviceLastReportTime.ContainsKey(device) == true)
                            {
                                lock (lockerRT)//锁
                                {
                                    s_hasDeviceLastReportTime[device] = DateTime.Now;
                                }


                            }


                            //for (int j = 0; j < Lists.Count; j++)
                            //{
                            //    using (SqlClass _sqlclass = new SqlClass())
                            //    {
                            //        BusDaService w_busDaService = new BusDaService(_sqlclass);

                            //        w_busDaService.RunDataWork((Hashtable)Lists[j]);
                            //    }
                            //}

                            receiveBytes = new byte[1024];

                            if (DateTime.Compare(DateTime.Now, dtStatrt.AddHours(1)) > 0)
                            {
                                sck.Shutdown(SocketShutdown.Both);
                                sck.Close();
                            }
                        }

                    }
                    catch (Exception)
                    {
                        sck.Shutdown(SocketShutdown.Both);
                        sck.Close();

                    }
                    finally
                    {
                        receiveBytes = new byte[1024];
                    }
                    System.Threading.Thread.Sleep(100);

                }
            }
            catch (Exception)
            {


            }

        }

        /// <summary>   
        /// 16进制字符串转字节数组   
        /// </summary>   
        /// <param name="hexString"></param>   
        /// <returns></returns>   
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }

        /// <summary>   
        ///字符串转数组   
        /// </summary>   
        /// <param name="hexString"></param>   
        /// <returns></returns>   
        private static string[] strToTochar(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            string[] returnBytes = new string[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = hexString.Substring(i * 2, 2).Trim();
            return returnBytes;
        }

        /// <summary>
        /// 算校验码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string Sum_CRC8(string[] data)
        {
            int iSum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                //iSum =iSum+ int.Parse(data[i].ToString("d"));
                iSum = iSum + Convert.ToInt32(data[i], 16);
            }

            iSum %= 256;
            return iSum.ToString("x");
        }

        //获取高低电平//
        private int GetHight(int port, int hight)
        {
            try
            {
                int Retrun = port & hight;

                if (Retrun == 0)
                    return 0;

                else return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string IntToStrHex(string i_Hex, int i_Cnt)
        {
            //int i;
            string Return = i_Hex;

            if (i_Hex.Length < i_Cnt)
            {
                for (int i = 0; i < i_Cnt - i_Hex.Length; i++)
                {
                    Return = "0" + Return;
                }
            }
            return Return;
        }


        private static Hashtable GetPort(int port, int hight)
        {
            try
            {
                if (port == 0) return new Hashtable();

                string Retrun = IntToStrHex(Convert.ToString(hight, 2), 8);

                string StringTo2 = IntToStrHex(Convert.ToString(port, 2), 8);

                Hashtable has = new Hashtable();

                int intRow = 1;
                string strSubstring = "";

                for (int i = 7; i >= 0; i--)
                {
                    strSubstring = StringTo2.Substring(i, 1);

                    if (strSubstring == "1")
                    {
                        has.Add(intRow, Retrun.Substring(i, 1));
                    }

                    intRow++;
                }
                return has;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Hashtable GetReadPort(int port)
        {
            try
            {
                string StringTo2 = IntToStrHex(Convert.ToString(port, 2), 8);

                Hashtable has = new Hashtable();

                string strSubstring = "";

                strSubstring = StringTo2.Substring(6, 1);

                if (strSubstring == "1")
                {

                    has.Add(2, 1);
                }
                else
                {
                    has.Add(2, 0);
                }
                return has;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 用TcpListener监听(测试)

    }
}
