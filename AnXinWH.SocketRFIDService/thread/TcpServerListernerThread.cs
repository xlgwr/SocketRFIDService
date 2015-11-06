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
using AnXinWH.SocketRFIDService.Led;
using AnXinWH.SocketRFIDService.Model;

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
        public static Dictionary<string, bool> _tmpCurrCheckPoint = new Dictionary<string, bool>();
        public TcpListernerThread()
        {
            logger = LogManager.GetLogger(GetType());
        }
        #region 用TcpListener监听(测试)

        public static System.Net.Sockets.TcpListener mSocketL;
        public static System.Threading.Thread mThread;
        public static Random _tmpRandom = new Random(100000);
        public static String mPort = "8088";
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

        public static string _tmpPreCheckTime = "";
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
                        logger.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ms != null)
                {
                    ms.Dispose();
                }
                logger.Error(ex);
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
        ///  <!--0：入库，1：报警/点检，2：出库-->
        ///  出入库标记更新
        ///  status 状态 smallint 默认1:可用 0:不可用
        /// </summary>
        /// <param name="sysType"></param>
        /// <returns></returns>
        public bool toDoSomeThing(string tmpStrRFID, string tmpMoveFlag, string RFIDClientIP)
        {
            string sysType = "";
            m_terminaldevice tmpDevice = null;

            try
            {
                logger.DebugFormat("*******#############IP:{0},移动标记：{1}，RFID:{2}.", RFIDClientIP, tmpMoveFlag, tmpStrRFID);

                using (var db = new MysqlDbContext())
                {
                    tmpDevice = db.m_terminaldevice.Where(m => m.ModelNo.Equals("500") && m.SerialNoIPAddr.Equals(RFIDClientIP)).FirstOrDefault();


                    if (tmpDevice != null)
                    {
                        //实际入库表
                        var tmpcontStockIn = db.t_stockinctnnodetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 2).Count();
                        //库存明细表
                        var tmpcontStockOut = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 1).Count();

                        sysType = tmpDevice.param3;

                        if (!sysType.Equals("1"))
                        {
                            if (tmpcontStockIn > 0)
                            {
                                sysType = "0";
                            }
                            else
                            {
                                if (tmpcontStockOut > 0)
                                {
                                    sysType = "2";
                                }
                            }
                        }
                        logger.DebugFormat("*******############# {0},开始处理操作：{1},IP:{2}", tmpDevice.param3, tmpDevice.TerminalName, RFIDClientIP);
                    }
                    else
                    {
                        logger.ErrorFormat("*******#############****Error: 没有找到 {0} 对应的设备。", RFIDClientIP);
                        return false;
                    }
                }

                switch (sysType)
                {
                    case "0":
                        #region stock in

                        if (_tmpListScanRFID.Keys.Contains(tmpStrRFID))
                        {

                            if (_tmpListScanRFID[tmpStrRFID])
                            {
                                logger.DebugFormat("#***********已扫并处理OK，不做处理。RFID: {0}", tmpStrRFID);
                                logger.InfoFormat("#***********已扫并处理OK，不做处理。RFID: {0}", tmpStrRFID);

                                //sendTxtToLED(tmpItemRFID);
                                return true;
                            }
                            else
                            {
                                // sendTxtToLED(tmpItemRFID);// + "已扫"
                                logger.DebugFormat("#***********已扫但处理失败，重做处理。RFID: {0}", tmpStrRFID);

                            }

                        }
                        else
                        {
                            _tmpListScanRFID.Add(tmpStrRFID, false);
                        }


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
                                    var tmpSendLedMsg = tmpStrRFID + "入库";
                                    sendTxtToLED(tmpSendLedMsg, tmpDevice);
                                    Program.saveLog("入库", "0入库:" + tmpStrRFID, 0, 1);
                                    return true;
                                }

                                logger.DebugFormat("*error开始入库*失败，系统错误,请联系管理员**************************************************************", tmpStrRFID);
                                Program.saveLog("入库", "0入库:" + tmpStrRFID, 0, 0);
                                return false;
                            }
                            logger.DebugFormat("*开始入库*失败，未查到[可入库/有效的]实际入库明细记录,RFID:{0}**************************************************************", tmpStrRFID);
                            return false;
                        }
                        break;
                        #endregion
                    case "2":
                        #region stock out

                        if (_tmpListScanRFID.Keys.Contains(tmpStrRFID))
                        {

                            if (_tmpListScanRFID[tmpStrRFID])
                            {
                                logger.DebugFormat("#***********已扫并处理OK，不做处理。RFID: {0}", tmpStrRFID);
                                logger.InfoFormat("#***********已扫并处理OK，不做处理。RFID: {0}", tmpStrRFID);

                                //sendTxtToLED(tmpItemRFID);
                                return true;
                            }
                            else
                            {
                                // sendTxtToLED(tmpItemRFID);// + "已扫"
                                logger.DebugFormat("#***********已扫但处理失败，重做处理。RFID: {0}", tmpStrRFID);

                            }

                        }
                        else
                        {
                            _tmpListScanRFID.Add(tmpStrRFID, false);
                        }

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
                                var tmpstockdetailForOut = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 1).FirstOrDefault();

                                if (tmpstockdetailForOut == null)
                                {
                                    logger.DebugFormat("*****无效RFID:{0}.", tmpStrRFID);
                                    return false;
                                }
                                #endregion

                                #region 货物出库明细
                                //货物出库明细
                                t_stockoutdetail tmpStockoutDetails = null;
                                var strremark = "";
                                if (string.IsNullOrEmpty(tmpstockdetailForOut.receiptNo))
                                {
                                    #region 没有仓单号
                                    logger.DebugFormat("*开始出库**********#########没有仓单号,货物编号:{0},托盘号:{1}，rfid_no:{2}", tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);
                                    ////***********报警*********

                                    //var tmpNewAlerm = new t_alarmdata();
                                    //tmpNewAlerm.recd_id = DateTime.Now.ToString("yyyyMMddhhmmss") + "D" + _tmpRandom.Next(100000).ToString() + "R" + tmpStrRFID;
                                    //tmpNewAlerm.alarm_type = "Alarm_06";
                                    //tmpNewAlerm.depot_no = "0";
                                    //tmpNewAlerm.cell_no = tmpStrRFID;
                                    //tmpNewAlerm.begin_time = DateTime.Now;
                                    //tmpNewAlerm.over_time = DateTime.Now;
                                    //tmpNewAlerm.remark = "RFID:" + tmpStrRFID + "无出库指示。";
                                    //tmpNewAlerm.status = 1;
                                    //tmpNewAlerm.addtime = DateTime.Now;
                                    //tmpNewAlerm.adduser = "StocketRFID";
                                    //tmpNewAlerm.updtime = DateTime.Now;
                                    //tmpNewAlerm.upduser = "StocketRFID";
                                    //db.t_alarmdata.Add(tmpNewAlerm);
                                    //var saveflag = db.SaveChanges();

                                    //if (saveflag > 0)
                                    //{
                                    //    logger.DebugFormat("********报警 保存完成。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3}.", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag);
                                    //}
                                    //else
                                    //{

                                    //    logger.DebugFormat("********报警 保存失败。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3}", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag);
                                    //}
                                    //var tmpLedMsg = tmpStrRFID + "无出库指示.";
                                    //sendTxtToLED(tmpLedMsg, tmpDevice);
                                    //return false;
                                    #endregion

                                    tmpStockoutDetails = db.t_stockoutdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.prdct_no.Equals(tmpstockdetailForOut.prdct_no) && m.status == 1).FirstOrDefault();
                                    strremark = "RFID:" + tmpStrRFID + ",无单号,没有查到[有效的]货物出库明细记录。";
                                }
                                else
                                {
                                    tmpStockoutDetails = db.t_stockoutdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.receiptNo.Equals(tmpstockdetailForOut.receiptNo) && m.prdct_no.Equals(tmpstockdetailForOut.prdct_no) && m.status == 1).FirstOrDefault();
                                    strremark = "RFID:" + tmpStrRFID + ",仓单号:" + tmpstockdetailForOut.receiptNo + ",没有查到[有效的]货物出库明细记录。";
                                }
                                logger.DebugFormat("*开始出库**********#########共有{0}条记录.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpcont, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                if (tmpStockoutDetails == null)
                                {
                                    logger.DebugFormat("*error开始出库**********{0}#########没有查到[有效的]货物出库明细记录.仓单号:{1},货物编号:{2},托盘号:{3}，rfid_no:{4}", tmpcont, tmpstockdetailForOut.receiptNo, tmpstockdetailForOut.prdct_no, tmpstockdetailForOut.ctnno_no, tmpStrRFID);

                                    var tmpNewAlerm = new t_alarmdata();
                                    tmpNewAlerm.recd_id = DateTime.Now.ToString("yyyyMMddhhmmss") + "D" + _tmpRandom.Next(100000).ToString() + "R" + tmpStrRFID;
                                    tmpNewAlerm.alarm_type = "Alarm_06";
                                    tmpNewAlerm.depot_no = "0";
                                    tmpNewAlerm.cell_no = tmpStrRFID;
                                    tmpNewAlerm.begin_time = DateTime.Now;
                                    tmpNewAlerm.over_time = DateTime.Now;
                                    tmpNewAlerm.remark = strremark;
                                    tmpNewAlerm.status = 1;
                                    tmpNewAlerm.addtime = DateTime.Now;
                                    tmpNewAlerm.adduser = "StocketRFID";
                                    tmpNewAlerm.updtime = DateTime.Now;
                                    tmpNewAlerm.upduser = "StocketRFID";
                                    db.t_alarmdata.Add(tmpNewAlerm);
                                    var saveflag = db.SaveChanges();

                                    if (saveflag > 0)
                                    {

                                        logger.DebugFormat("********报警 保存完成。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3}.", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag);

                                    }
                                    else
                                    {

                                        logger.DebugFormat("********报警 保存失败。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3}", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag);
                                    }
                                    var tmpLedMsg = tmpStrRFID + "无出库指示.";
                                    sendTxtToLED(tmpLedMsg, tmpDevice);

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
                                    tmpNewStockDetails.adduser = "RFIDStockOut";
                                    tmpNewStockDetails.updtime = DateTime.Now;
                                    tmpNewStockDetails.upduser = "RFIDStockOut";
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

                                    var tmpLedMsg = "仓单" + tmpstockdetailForOut.receiptNo + "托盘" + tmpstockdetailForOut.ctnno_no;//shelf_no;

                                    sendTxtToLED(tmpLedMsg, tmpDevice);

                                    Program.saveLog("出库", "2出库:" + tmpStrRFID, 1, 1);
                                    return true;
                                }

                                logger.DebugFormat("*error开始出库*失败，系统错误,请联系管理员**************************************************************", tmpStrRFID);
                                Program.saveLog("出库", "2出库:" + tmpStrRFID, 1, 0);
                                return false;

                                #endregion
                            }

                            logger.DebugFormat("*error开始出库*失败，未查到[可出库/有效的]库存明细表记录,RFID:{0}**************************************************************", tmpStrRFID);
                            return false;
                        }
                        break;
                        #endregion
                    case "1":
                        var tmpShelf = "没找到对应货架。";
                        var tmpStockDetail = new t_stockdetail();
                        #region 报警
                        if (tmpMoveFlag.Equals("1"))
                        {
                            using (var db = new MysqlDbContext())
                            {

                                tmpStockDetail = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID)).FirstOrDefault();
                                if (tmpStockDetail != null)
                                {
                                    tmpShelf = tmpStockDetail.shelf_no;
                                }
                                var tmpNewAlerm = new t_alarmdata();
                                tmpNewAlerm.recd_id = DateTime.Now.ToString("yyyyMMddhhmmss") + "D" + _tmpRandom.Next(100000).ToString() + "R" + tmpStrRFID;
                                tmpNewAlerm.alarm_type = "Alarm_04";
                                tmpNewAlerm.depot_no = "0";
                                tmpNewAlerm.cell_no = tmpStrRFID;
                                tmpNewAlerm.begin_time = DateTime.Now;
                                tmpNewAlerm.over_time = DateTime.Now;
                                tmpNewAlerm.remark = "RFID:" + tmpStrRFID + "移动了。货架号：" + tmpShelf;
                                tmpNewAlerm.status = 1;
                                tmpNewAlerm.addtime = DateTime.Now;
                                tmpNewAlerm.adduser = "StocketRFID";
                                tmpNewAlerm.updtime = DateTime.Now;
                                tmpNewAlerm.upduser = "StocketRFID";
                                db.t_alarmdata.Add(tmpNewAlerm);
                                var saveflag = db.SaveChanges();

                                if (saveflag > 0)
                                {
                                    Program.saveLog("移动报警", "2移动报警:" + tmpStrRFID, 3, 1);
                                    logger.DebugFormat("********报警 保存完成。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3},货架：{4}.", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag, tmpShelf);

                                }
                                else
                                {
                                    Program.saveLog("移动报警", "2移动报警:" + tmpStrRFID, 3, 1);
                                    logger.DebugFormat("********报警 保存失败。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3},货架：{4}", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag, tmpShelf);
                                }

                            }
                        }
                        #endregion

                        #region 点检
                        using (var db = new MysqlDbContext())
                        {

                            tmpStockDetail = db.t_stockdetail.Where(m => m.rfid_no.Equals(tmpStrRFID) && m.status == 1).FirstOrDefault();

                            var tmpCheckPoint = db.m_checkpoint.OrderBy(m => m.checktime).ToList();
                            var isToCheck = currTimeExit(tmpCheckPoint, tmpStrRFID);

                            if (isToCheck.isIn)
                            {
                                var tmpNewt_checkresult = new t_checkresult();
                                var tmpNewt_checkdetailresult = new t_checkdetailresult();

                                var tmpGuidId = Guid.NewGuid().ToString();
                                #region 主表
                                tmpNewt_checkresult.check_id = tmpGuidId;
                                tmpNewt_checkresult.check_date = isToCheck.checktimeNow;
                                tmpNewt_checkresult.bespeak_no = "";//RFID
                                tmpNewt_checkresult.bespeak_date = isToCheck.checktime;
                                tmpNewt_checkresult.user_no = Program._serverIP; //"";//点检
                                tmpNewt_checkresult.user_nm = tmpStrRFID;
                                tmpNewt_checkresult.status = 1;

                                tmpNewt_checkresult.remark = "点检:" + isToCheck.checktime + ",收到 RFID:" + tmpStrRFID;

                                tmpNewt_checkresult.addtime = DateTime.Now;
                                tmpNewt_checkresult.adduser = "StocketRFID";
                                tmpNewt_checkresult.updtime = DateTime.Now;
                                tmpNewt_checkresult.upduser = "StocketRFID";


                                db.t_checkresult.Add(tmpNewt_checkresult);

                                var saveflag2 = db.SaveChanges();

                                #endregion
                                #region 主表明细
                                tmpNewt_checkdetailresult.check_id = tmpGuidId;
                                tmpNewt_checkdetailresult.out_item_no = "1";
                                tmpNewt_checkdetailresult.rfid_no = tmpStrRFID;

                                if (tmpStockDetail != null)
                                {
                                    tmpNewt_checkdetailresult.prdct_no = tmpStockDetail.prdct_no;
                                    tmpNewt_checkdetailresult.receiptNo = tmpStockDetail.receiptNo;
                                    tmpNewt_checkdetailresult.qty = tmpStockDetail.qty;
                                    tmpNewt_checkdetailresult.nwet = tmpStockDetail.nwet;
                                    tmpNewt_checkdetailresult.gwet = tmpStockDetail.gwet;
                                    tmpNewt_checkdetailresult.cell_no = tmpStockDetail.shelf_no;
                                    tmpNewt_checkdetailresult.remark = "点检:" + isToCheck.checktime + ",RFID:" + tmpStrRFID;
                                    tmpNewt_checkdetailresult.status = 1;

                                    tmpNewt_checkdetailresult.addtime = DateTime.Now;
                                    tmpNewt_checkdetailresult.adduser = "StocketRFID";
                                    tmpNewt_checkdetailresult.updtime = DateTime.Now;
                                    tmpNewt_checkdetailresult.upduser = "StocketRFID";

                                    db.t_checkdetailresult.Add(tmpNewt_checkdetailresult);

                                    saveflag2 += db.SaveChanges();
                                }

                                #endregion

                                if (saveflag2 > 0)
                                {
                                    Program.saveLog("点检", "2点检:" + tmpStrRFID, 3, 1);

                                    logger.DebugFormat("********点检 保存完成。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3},货架：{4}.", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag2, tmpShelf);

                                }
                                else
                                {
                                    Program.saveLog("点检", "2点检:" + tmpStrRFID, 3, 0);

                                    logger.DebugFormat("********点检 保存失败。IP:{0},移动标记：{1}，RFID:{2}.SaveFlag:{3},货架：{4}", RFIDClientIP, tmpMoveFlag, tmpStrRFID, saveflag2, tmpShelf);
                                }
                            }
                        }
                        return false;
                        break;
                        #endregion

                    default:
                        break;
                }


                return false;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0}:{1},flag:{2},rfid_no:{3}", 481, ex, sysType, tmpStrRFID);
                throw ex;
                return false;
            }
        }
        CheckTime currTimeExit(IList<m_checkpoint> tmpCheckPoint, string tmpStrRFID)
        {
            var tmpCheckTime = new CheckTime();
            tmpCheckTime.isIn = false;
            try
            {
                var currDateTime = DateTime.Now;
                var currTotalMin = currDateTime.Hour * 60 + currDateTime.Minute;
                var tmpallTime = "";
                foreach (var item in tmpCheckPoint)
                {
                    var tmpTime = item.checktime.Split(':');
                    var tmpstartTime = Convert.ToInt32(tmpTime[0]) * 60 + Convert.ToInt32(tmpTime[1]);
                    if (currTotalMin >= tmpstartTime && currTotalMin <= (tmpstartTime + 2))
                    {
                        logger.DebugFormat("******当前时间：{0} 在时间：{1} (>=5分钟内）,开始点检采集数据。", currDateTime, item.checktime);
                        tmpCheckTime.checktime = item.checktime;

                        if (string.IsNullOrEmpty(_tmpPreCheckTime))
                        {
                            _tmpPreCheckTime = item.checktime;
                            tmpCheckTime.checktimePre = "";

                        }
                        else
                        {

                            if (item.checktime.Equals(_tmpPreCheckTime))
                            {
                                _tmpPreCheckTime = item.checktime;
                                tmpCheckTime.checktimePre = "";
                            }
                            else
                            {
                                tmpCheckTime.checktimePre = _tmpPreCheckTime;
                                logger.InfoFormat("*******0点检：{0}，上次：{1}。", item.checktime, _tmpPreCheckTime);
                                //to 检查上一次是否未检查到的RFID
                                #region 点检报警检查
                                try
                                {
                                    using (var dbs = new MysqlDbContext())
                                    {
                                        var isExitCheckRFID = dbs.Database.SqlQuery<int>("select count(*) from t_checkresult where DATE_FORMAT(check_date,'%Y%m%d')=DATE_FORMAT(NOW(),'%Y%m%d') and bespeak_date=@p0", _tmpPreCheckTime).FirstOrDefault();

                                        if (isExitCheckRFID > 0)
                                        {
                                            var isCheckCount = dbs.t_checkresult.Where(m => m.bespeak_date.Equals(_tmpPreCheckTime) && m.status == 1).Count();

                                            if (isCheckCount <= 0)
                                            {
                                                logger.DebugFormat("******点检报警检查*****当前点检时间：{0},上次点检时间：{1} 已检查 点检报警 过 。{2}", item.checktime, _tmpPreCheckTime, isCheckCount);
                                            }
                                            else
                                            {
                                                logger.DebugFormat("******点检报警检查*****当前点检时间：{0},上次点检时间：{1} 开始 检查点检报警 。{2}", item.checktime, _tmpPreCheckTime, isCheckCount);

                                                var tmpStockDetails = dbs.t_stockdetail.Where(m => m.status == 1).ToList();

                                                foreach (var tmpstock in tmpStockDetails)
                                                {
                                                    //check is ist
                                                    var tmpPreCheckCurr = dbs.t_checkresult.Where(m => m.bespeak_date.Equals(_tmpPreCheckTime) && m.user_nm.Equals(tmpstock.rfid_no)).Count();

                                                    logger.DebugFormat("**db:{0}.RFID:{1}", tmpPreCheckCurr, tmpstock.rfid_no);
                                                    if (tmpPreCheckCurr <= 0)
                                                    {
                                                        logger.DebugFormat("**db Add:{0}.RFID:{1}", tmpPreCheckCurr, tmpstock.rfid_no);

                                                        var isExitCheckSaveAleraRFID = dbs.Database.SqlQuery<int>("select count(*) from t_alarmdata where DATE_FORMAT(begin_time,'%Y%m%d')=DATE_FORMAT(NOW(),'%Y%m%d') and depot_no=@p0 and cell_no=@p1", _tmpPreCheckTime, tmpstock.rfid_no).FirstOrDefault();

                                                        if (isExitCheckSaveAleraRFID <= 0)
                                                        {
                                                            //报警
                                                            var tmpNewAlerm = new t_alarmdata();
                                                            tmpNewAlerm.recd_id = DateTime.Now.ToString("yyyyMMddhhmmss") + "D" + _tmpRandom.Next(100000).ToString() + "R" + tmpstock.rfid_no;
                                                            tmpNewAlerm.alarm_type = "Alarm_05";
                                                            tmpNewAlerm.depot_no = _tmpPreCheckTime;
                                                            tmpNewAlerm.cell_no = tmpstock.rfid_no;
                                                            tmpNewAlerm.begin_time = DateTime.Now;
                                                            tmpNewAlerm.over_time = DateTime.Now;
                                                            tmpNewAlerm.remark = "RFID:" + tmpstock.rfid_no + ",没有点检到。点检时间：" + _tmpPreCheckTime;
                                                            tmpNewAlerm.status = 1;
                                                            tmpNewAlerm.addtime = DateTime.Now;
                                                            tmpNewAlerm.adduser = "StocketRFID";
                                                            tmpNewAlerm.updtime = DateTime.Now;
                                                            tmpNewAlerm.upduser = "StocketRFID";
                                                            dbs.t_alarmdata.Add(tmpNewAlerm);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        dbs.Database.ExecuteSqlCommand("update t_checkresult set t_checkresult.`status`='0' where user_nm=@p0", tmpstock.rfid_no);
                                                    }


                                                }
                                                var saveflag = dbs.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            logger.DebugFormat("******点检报警检查*****当前点检时间：{0},上次点检时间：{1} 当天没有点检记录，Now:{2}.", item.checktime, _tmpPreCheckTime, DateTime.Now);

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    // throw;
                                    logger.ErrorFormat("*******0点检：{0}，上次：{1}。Error:{2}", item.checktime, _tmpPreCheckTime, ex);
                                    continue;
                                }

                                #endregion
                            }

                        }
                        //logger.InfoFormat("*******1点检：{0}，上次：{1}。", item.checktime, _tmpPreCheckTime);

                        tmpCheckTime.checktimeNow = currDateTime;
                        tmpCheckTime.isIn = true;

                        using (var dbs = new MysqlDbContext())
                        {
                            var isExitRFID = dbs.Database.SqlQuery<t_checkresult>("select * from t_checkresult where DATE_FORMAT(check_date,'%Y%m%d')=DATE_FORMAT(NOW(),'%Y%m%d') and bespeak_date=@p0 and user_nm=@p1", item.checktime, tmpStrRFID).FirstOrDefault();
                            if (isExitRFID != null)
                            {
                                tmpCheckTime.isIn = false;
                                logger.DebugFormat("******当前时间：{0},点检时间：{1}，RFID:{2} 已点检过 。", currDateTime, item.checktime, tmpStrRFID);
                            }
                            else
                            {
                                logger.DebugFormat("******当前时间：{0},点检时间：{1}，RFID:{2} 未点检过 。", currDateTime, item.checktime, tmpStrRFID);
                            }
                        }
                        return tmpCheckTime;
                        break;
                    }
                    else
                    {
                        logger.DebugFormat("Other Set: 点检：{0}，上次：{1}。", item.checktime, _tmpPreCheckTime);
                        _tmpPreCheckTime = item.checktime;

                    }
                    tmpallTime += item.checktime + ",";

                }
                logger.DebugFormat("******当前时间：{0} 不在【{1}】内(5分钟内）。", currDateTime, tmpallTime);
                return tmpCheckTime;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("****获取RFID点检时间失败，{0}", ex);
                throw ex;
            }
            return tmpCheckTime;
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

                        logger.DebugFormat("*******************_______*********************#read {0} client {1} bytes, RFID: {2},Buffer16:{3}.", tw_strIP, bytesReadLength, tmpItemRFID, String.Join(",", item));
                        logger.InfoFormat("#read {0} client {1} bytes,RFID: {2}, Buffer16:{3}.", tw_strIP, bytesReadLength, tmpItemRFID, String.Join(",", item));


                        //处理数据
                        //test to send back    
                        //handler.Send(state.buffer, 0, bytesReadLength, SocketFlags.None);

                        //todo test
                        var tmpResule = toDoSomeThing(tmpItemRFID, tmpMoveFlag, tw_strIP);
                        //if (tmpResule)
                        //{
                        //    sendTxtToLED(tmpItemRFID);
                        //}
                        _tmpListScanRFID[tmpItemRFID] = tmpResule;

                        //logger.InfoFormat("SQL result:{0}.", tmpResule);

                    }

                    //close current workSocket

                    logger.DebugFormat("**##客户socket读取数据结束,Close current Connect IP:{0}*************************************************************", tw_strIP);
                    logger.Debug("**************************************(^_^)分割线(^_^)*************************************************************");

                    handler.Close();

                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
                    logger.Error(ex);
                }



            }

        }
        #region LED
        private const int WM_LED_NOTIFY = 1025;
        CLEDSender LEDSender = new CLEDSender();
        public void sendTxtToLED(string tmpTxt, m_terminaldevice deviceLED)
        {
            try
            {
                logger.DebugFormat("****Start to Send LED TXT:{0}", tmpTxt);
                TSenderParam param = new TSenderParam();
                ushort K;

                GetDeviceParam(ref param.devParam, deviceLED);

                param.notifyMode = LEDSender.NOTIFY_EVENT;
                // param.wmHandle = (UInt32)Handle;
                param.wmMessage = WM_LED_NOTIFY;

                K = (ushort)LEDSender.Do_MakeRoot(LEDSender.ROOT_PLAY, LEDSender.COLOR_MODE_DOUBLE, LEDSender.SURVIVE_ALWAYS);
                LEDSender.Do_AddChapter(K, 30000, LEDSender.WAIT_CHILD);
                LEDSender.Do_AddRegion(K, 0, 0, 128, 32, 0);

                //第1页面
                LEDSender.Do_AddLeaf(K, 1000, LEDSender.WAIT_CHILD);
                //16点阵字体"01234567890123456789"
                LEDSender.Do_AddString(K, 0, 0, 512, 16, LEDSender.V_TRUE, 0,
                    tmpTxt,
                    LEDSender.FONT_SET_16, 0xff, 1, 1, 2, 1, 0, 1, 1000);

                //send
                Parse(LEDSender.Do_LED_SendToScreen(ref param, K), tmpTxt);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }
        private void Parse(Int32 K, string tmpTxt)
        {
            if (K == LEDSender.R_DEVICE_READY) logger.InfoFormat("**LED:{0},正在执行命令或者发送数据...", tmpTxt);
            else if (K == LEDSender.R_DEVICE_INVALID) logger.InfoFormat("**LED:{0},打开通讯设备失败(串口不存在、或者串口已被占用、或者网络端口被占用)", tmpTxt);
            else if (K == LEDSender.R_DEVICE_BUSY) logger.InfoFormat("**LED:{0},设备忙，正在通讯中...", tmpTxt);
        }
        private void GetDeviceParam(ref TDeviceParam param, m_terminaldevice deviceLED)
        {
            try
            {
                logger.DebugFormat("*******send LDK,IP:{0},Port:{1}.", deviceLED.param1, deviceLED.param2);
                //param.devType = LEDSender.DEVICE_TYPE_UDP;

                //param.comPort = (ushort)Convert.ToInt16(0);
                //param.comSpeed = (ushort)38400;
                //param.locPort = (ushort)Convert.ToInt16(Program._locPort);
                //param.rmtHost = Program._rmtHost; ;
                //param.rmtPort = (ushort)Convert.ToInt16(Program._rmtPort);
                //param.dstAddr = (ushort)Convert.ToInt16(Program._dstAddr); 


                param.devType = LEDSender.DEVICE_TYPE_UDP;

                param.comPort = (ushort)Convert.ToInt16(1);
                param.comSpeed = (ushort)19200;
                param.locPort = (ushort)Convert.ToInt16(8881);



                param.rmtHost = deviceLED.param1;// "192.168.1.199";
                param.rmtPort = (ushort)Convert.ToInt16(deviceLED.param2); //6666;



                param.dstAddr = 0;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("获取LED参数失败,Error:{0}", ex);
                throw ex;
            }

        }
        #endregion
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
