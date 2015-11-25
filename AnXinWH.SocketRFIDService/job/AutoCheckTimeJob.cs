using AnXinWH.SocketRFIDService.Basic;
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
using AnXinWH.SocketRFIDService.Model;

namespace AnXinWH.SocketRFIDService.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class AutoCheckTimeJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            logger.DebugFormat("*********开始运行 自动点检 job:{0}.下次job时间：{1}.", context.JobDetail.Key, context.NextFireTimeUtc);
            try
            {
                using (var db = new MysqlDbContext())
                {
                    var tmpCheckPoint = db.m_checkpoint.OrderBy(m => m.checktime).ToList();
                    var isToCheck = currTimeExit(tmpCheckPoint);
                    if (!string.IsNullOrEmpty(isToCheck))
                    {
                        logger.DebugFormat("*************开始自动点检0,点检时间：{0}。***************", isToCheck);
                        //check add today
                        #region check add today

                        var sqlExit = "select count(*) from t_checkresult a where DATE_FORMAT(a.addtime,'%Y%m%d')=DATE_FORMAT(now(),'%Y%m%d') and a.check_date='" + isToCheck + "'";
                        var tmpExit = db.Database.SqlQuery<int>(sqlExit).FirstOrDefault();

                        if (tmpExit > 0)
                        {
                            logger.DebugFormat("*************系统已经 自动点检过1,点检时间：{0}。***************", isToCheck);
                            Program._checkAutoRfid.Clear();
                            Program._checkAutoCount = 1;
                            return;
                        }
                        #endregion
                        switch (Program._checkAutoCount)
                        {
                            case 0:
                                Program._checkAutoRfid.Clear();
                                Program._checkAutoCount = 1;
                                logger.DebugFormat("*************开始自动点检1,点检时间：{0}。***************", isToCheck);
                                break;
                            case 1:
                                #region save checklog
                                var m2odelofStockDetails2 = db.t_stockdetail.Where(m => m.status == 1).ToList();
                                if (m2odelofStockDetails2.Count > 0)
                                {
                                    //yyyy-MM-dd HH:mm:ss 
                                    var tmpGuidId = DateTime.Now.ToString("yyyyMMddHHmmss") + "A" + Program._tmpRandom.Next(100000).ToString(); //Guid.NewGuid().ToString();
                                    var tmpNewt_checkresult = new t_checkresult();

                                    tmpNewt_checkresult.bespeak_no = "";
                                    //主
                                    tmpNewt_checkresult.check_id = tmpGuidId;
                                    tmpNewt_checkresult.check_date = isToCheck;
                                    tmpNewt_checkresult.user_no = "";// Program._serverIP; //"";//点检
                                    tmpNewt_checkresult.user_nm = "";//tmpStrRFID;
                                    tmpNewt_checkresult.status = 1; //1表示正常点检  0表示报警点检
                                    tmpNewt_checkresult.checktype = 1;//1表示自动点检  0手动点检


                                    tmpNewt_checkresult.remark = "";//"自动点检:" + isToCheck + ",收到 RFID:" + item.rfid_no;

                                    tmpNewt_checkresult.addtime = DateTime.Now;
                                    tmpNewt_checkresult.adduser = Program._serverIP;
                                    tmpNewt_checkresult.updtime = DateTime.Now;
                                    tmpNewt_checkresult.upduser = Program._serverIP;

                                    db.t_checkresult.Add(tmpNewt_checkresult);

                                    foreach (var item in m2odelofStockDetails2)
                                    {

                                        var tmpNewt_checkdetailresult = new t_checkdetailresult();

                                        #region  //明细
                                        tmpNewt_checkdetailresult.check_id = tmpGuidId;
                                        tmpNewt_checkdetailresult.out_item_no = "1";
                                        tmpNewt_checkdetailresult.rfid_no = item.rfid_no;
                                        tmpNewt_checkdetailresult.prdct_no = item.prdct_no;
                                        tmpNewt_checkdetailresult.receiptNo = item.receiptNo;
                                        tmpNewt_checkdetailresult.qty = item.qty;
                                        tmpNewt_checkdetailresult.nwet = item.nwet;
                                        tmpNewt_checkdetailresult.gwet = item.gwet;
                                        tmpNewt_checkdetailresult.cell_no = item.shelf_no;
                                        tmpNewt_checkdetailresult.remark = "";// "自动点检报警:RFID:" + item.rfid_no + ",仓位号：" + item.shelf_no + ",没有点检到。点检时间：" + isToCheck;

                                        tmpNewt_checkdetailresult.addtime = DateTime.Now;
                                        tmpNewt_checkdetailresult.adduser = Program._serverIP;//Program._serverIP;
                                        tmpNewt_checkdetailresult.updtime = DateTime.Now;
                                        tmpNewt_checkdetailresult.upduser = Program._serverIP;
                                        #endregion

                                        if (Program._checkAutoRfid.Keys.Contains(item.rfid_no))
                                        {
                                            #region 明细表
                                            tmpNewt_checkdetailresult.status = 0;//0:正常，1：点检报警，2：补点
                                            db.t_checkdetailresult.Add(tmpNewt_checkdetailresult);
                                            #endregion
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
                                            tmpNewAlerm.param1 = isToCheck;
                                            tmpNewAlerm.param2 = item.rfid_no;
                                            tmpNewAlerm.remark = "";// "自动点检报警:RFID:" + item.rfid_no + ",仓位号：" + item.shelf_no + ",没有点检到。点检时间：" + isToCheck;
                                            tmpNewAlerm.status = 1;
                                            tmpNewAlerm.addtime = DateTime.Now;
                                            tmpNewAlerm.adduser = Program._serverIP;
                                            tmpNewAlerm.updtime = DateTime.Now;
                                            tmpNewAlerm.upduser = Program._serverIP;
                                            db.t_alarmdata.Add(tmpNewAlerm);

                                            Program.saveLog("自动点检报警", "2自动点检报警:" + isToCheck, 3, 1);
                                            logger.DebugFormat("********自动点检报警 保存完成。RFID:{0}.仓位号：{1}.", item.rfid_no, item.shelf_no);

                                            #endregion

                                        }
                                    }
                                    var saveflag = db.SaveChanges();
                                    if (saveflag > 0)
                                    {
                                        Program.saveLog("自动点检", "2自动点检:" + isToCheck, 3, 1);
                                        logger.DebugFormat("********自动点检 保存完成。checkTime:{0}.", isToCheck);

                                    }
                                    else
                                    {
                                        Program.saveLog("自动点检", "2自动点检:" + isToCheck, 3, 0);
                                        logger.DebugFormat("********自动点检 保存失败。checkTime:{0}.", isToCheck);
                                    }
                                }
                                #endregion
                                //end
                                Program._checkAutoRfid.Clear();
                                Program._checkAutoCount = 0;
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        Program._checkAutoRfid.Clear();
                        Program._checkAutoCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("######运行出错误：{0}", ex);
            }
        }

        string currTimeExit(IList<m_checkpoint> tmpCheckPoint)
        {
            try
            {
                // var tmpCheckPoint = db.m_checkpoint.OrderBy(m => m.checktime).ToList();              

                var currDateTime = DateTime.Now;
                var currTotalMin = currDateTime.Hour * 60 + currDateTime.Minute;
                foreach (var item in tmpCheckPoint)
                {
                    var tmpTime = item.checktime.Split(':');
                    var tmpstartTime = Convert.ToInt32(tmpTime[0]) * 60 + Convert.ToInt32(tmpTime[1]);
                    if (currTotalMin >= tmpstartTime && currTotalMin <= (tmpstartTime + 3))
                    {
                        logger.DebugFormat("******当前时间：{0} 在时间：{1} (>=3分钟内）,开始点检采集数据。", currDateTime, item.checktime);
                        return item.checktime;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("****获取RFID点检时间失败，{0}", ex);
                throw ex;
            }
        }
    }
}