﻿using AnXinWH.SocketRFIDService.Basic;
using AnXinWH.SocketRFIDService.DAL;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.IO;


using MysqlDbContext = ClassLibraryApi.AnXinWH.AnXinWH;
using ClassLibraryApi.AnXinWH;

namespace AnXinWH.SocketRFIDService.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class AutoGetJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            logger.DebugFormat("*********开始运行 手动点检 job:{0}.下次job时间：{1}.", context.JobDetail.Key, context.NextFireTimeUtc);

            try
            {
                using (var db = new MysqlDbContext())
                {
                    //var tmpcont = db.m_users.Count();
                    //logger.DebugFormat("********************{0} 个用户.",tmpcont);
                    var m1odelchecktime1 = db.m_parameter.Where(m => m.paramkey.Equals("SetCheck") && m.paramtype == 1).FirstOrDefault();
                    if (m1odelchecktime1 != null)
                    {
                        logger.DebugFormat("*************开始手动点检,点检时间：{0}。***************", m1odelchecktime1.paramvalue);

                        //check add today
                        #region check add today

                        var sqlExit = "select count(*) from t_checkresult a where DATE_FORMAT(a.addtime,'%Y%m%d')=DATE_FORMAT(now(),'%Y%m%d') and a.check_date='" + m1odelchecktime1.paramvalue + "'";
                        var tmpExit = db.Database.SqlQuery<int>(sqlExit).FirstOrDefault();

                        if (tmpExit > 0)
                        {
                            logger.DebugFormat("*************系统已经手动点检过1,点检时间：{0}。***************", m1odelchecktime1.paramvalue);
                            Program._checkAutoRfid.Clear();
                            Program._checkAutoCount = 1;
                            m1odelchecktime1.paramtype = 0;
                            var saveflag2 = db.SaveChanges();
                            if (saveflag2 > 0)
                            {
                                Program.saveLog("手动点检", "2系统已经手动点检过:" + m1odelchecktime1.paramvalue, 3, 1);

                                logger.DebugFormat("********手动点检标记 更新完成。{0}.", m1odelchecktime1.paramvalue);

                            }
                            else
                            {
                                Program.saveLog("手动点检", "2系统已经手动点检过:" + m1odelchecktime1.paramvalue, 3, 0);

                                logger.DebugFormat("********手动点检标记 更新失败。{0}.", m1odelchecktime1.paramvalue);
                            }
                            return;
                        }
                        #endregion

                        switch (Program._checkCount)
                        {
                            case 0:
                                Program._checkCurrRfid.Clear();
                                Program._checkCount = 1;
                                break;
                            case 1:
                                #region save checklog
                                var m2odelofStockDetails2 = db.t_stockdetail.Where(m => m.status == 1).ToList();
                                if (m2odelofStockDetails2.Count > 0)
                                {
                                    var tmpGuidId = DateTime.Now.ToString("yyyyMMddHHmmss") + "D" + Program._tmpRandom.Next(100000).ToString(); //Guid.NewGuid().ToString();
                                    //主
                                    var tmpNewt_checkresult = new t_checkresult();
                                    tmpNewt_checkresult.bespeak_no = "";

                                    tmpNewt_checkresult.check_id = tmpGuidId;
                                    tmpNewt_checkresult.check_date = m1odelchecktime1.paramvalue;
                                    tmpNewt_checkresult.user_no = "";// Program._serverIP; //"";//点检
                                    tmpNewt_checkresult.user_nm = "";//tmpStrRFID;
                                    tmpNewt_checkresult.status = 1; //1表示正常点检  0表示报警点检
                                    tmpNewt_checkresult.checktype = 0;//1表示手动点检  0手动点检


                                    tmpNewt_checkresult.remark = "";// "手动点检:" + m1odelchecktime1.paramvalue + ",收到 RFID:" + item.rfid_no;

                                    tmpNewt_checkresult.addtime = DateTime.Now;
                                    tmpNewt_checkresult.adduser = Program._serverIP;
                                    tmpNewt_checkresult.updtime = DateTime.Now;
                                    tmpNewt_checkresult.upduser = Program._serverIP;

                                    db.t_checkresult.Add(tmpNewt_checkresult);
                                    foreach (var item in m2odelofStockDetails2)
                                    {
                                        var tmpNewt_checkdetailresult = new t_checkdetailresult();

                                        #region 明细表
                                        //明细表
                                        tmpNewt_checkdetailresult.check_id = tmpGuidId;
                                        tmpNewt_checkdetailresult.out_item_no = "1";
                                        tmpNewt_checkdetailresult.rfid_no = item.rfid_no;

                                        tmpNewt_checkdetailresult.prdct_no = item.prdct_no;
                                        tmpNewt_checkdetailresult.receiptNo = item.receiptNo;
                                        tmpNewt_checkdetailresult.qty = item.qty;
                                        tmpNewt_checkdetailresult.nwet = item.nwet;
                                        tmpNewt_checkdetailresult.gwet = item.gwet;

                                        tmpNewt_checkdetailresult.cell_no = item.shelf_no;
                                        tmpNewt_checkdetailresult.remark = "";// "手动点检:" + m1odelchecktime1.paramvalue + ",收到 RFID:" + item.rfid_no;

                                        tmpNewt_checkdetailresult.addtime = DateTime.Now;
                                        tmpNewt_checkdetailresult.adduser = Program._serverIP;//Program._serverIP;
                                        tmpNewt_checkdetailresult.updtime = DateTime.Now;
                                        tmpNewt_checkdetailresult.upduser = Program._serverIP;

                                        #endregion
                                        if (Program._checkCurrRfid.Keys.Contains(item.rfid_no))
                                        {

                                            tmpNewt_checkdetailresult.status = 0;//0:正常，1：点检报警，2：补点
                                            db.t_checkdetailresult.Add(tmpNewt_checkdetailresult);
                                            
                                        }
                                        else
                                        {
                                            //点检报警
                                            #region 明细表

                                            tmpNewt_checkdetailresult.status = 1;//0:正常，1：点检报警，2：补点
                                            db.t_checkdetailresult.Add(tmpNewt_checkdetailresult);
                                            #endregion

                                            #region 报警
                                            //报警
                                            var tmpNewAlerm = new t_alarmdata();
                                            tmpNewAlerm.recd_id = tmpGuidId + "R" + item.rfid_no;
                                            tmpNewAlerm.alarm_type = "Alarm_05";
                                            tmpNewAlerm.depot_no = "";// _tmpPreCheckTime;
                                            tmpNewAlerm.cell_no = item.shelf_no;//tmpRfidShelf;
                                            tmpNewAlerm.begin_time = DateTime.Now;
                                            tmpNewAlerm.over_time = DateTime.Now;
                                            tmpNewAlerm.param1 = m1odelchecktime1.paramvalue;
                                            tmpNewAlerm.param2 = item.rfid_no;
                                            tmpNewAlerm.remark = "";//"手动点检报警:RFID:" + item.rfid_no + ",仓位号：" + item.shelf_no + ",没有点检到。点检时间：" + m1odelchecktime1.paramvalue;
                                            tmpNewAlerm.status = 1;
                                            tmpNewAlerm.addtime = DateTime.Now;
                                            tmpNewAlerm.adduser = Program._serverIP;
                                            tmpNewAlerm.updtime = DateTime.Now;
                                            tmpNewAlerm.upduser = Program._serverIP;
                                            db.t_alarmdata.Add(tmpNewAlerm);

                                            Program.saveLog("手动点检报警", "2手动点检报警:" + m1odelchecktime1.paramvalue, 3, 1);
                                            logger.DebugFormat("********手动点检报警 保存完成。RFID:{0}.仓位号：{1}.", item.rfid_no, item.shelf_no);

                                            #endregion

                                        }
                                    }
                                    //change flag;
                                    m1odelchecktime1.paramtype = 0;
                                    var saveflag2 = db.SaveChanges();
                                    if (saveflag2 > 0)
                                    {
                                        Program.saveLog("手动点检", "2手动点检标记更新:" + m1odelchecktime1.paramvalue, 3, 1);

                                        logger.DebugFormat("********手动点检标记 更新完成。{0}.", m1odelchecktime1.paramvalue);

                                    }
                                    else
                                    {
                                        Program.saveLog("手动点检", "2手动点检标记更新:" + m1odelchecktime1.paramvalue, 3, 0);

                                        logger.DebugFormat("********手动点检标记 更新失败。{0}.", m1odelchecktime1.paramvalue);
                                    }
                                }
                                #endregion
                                //end
                                Program._checkCurrRfid.Clear();
                                Program._checkCount = 0;
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        Program._checkCurrRfid.Clear();
                        Program._checkCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("######运行出错误：{0}", ex);
            }
        }
    }
}