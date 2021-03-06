﻿using AnXinWH.SocketRFIDService.Basic;
using AnXinWH.SocketRFIDService.DAL;
using AnXinWH.SocketRFIDService.Job;
using log4net;
using Quartz;
using Quartz.Impl;
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

using MysqlDbContext = ClassLibraryApi.AnXinWH.AnXinWH;
using MySql.Data.MySqlClient;

namespace AnXinWH.SocketRFIDService
{
    public partial class Service1 : ServiceBase
    {

        private readonly ILog logger;
        public static IScheduler scheduler;
        private readonly WinLogWirter winlogger;


        public Service1()
        {
            InitializeComponent();

            winlogger = new WinLogWirter();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            logger = LogManager.GetLogger(GetType());
            scheduler = StdSchedulerFactory.GetDefaultScheduler();

        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {

                Exception ex = e.ExceptionObject as Exception;
                winlogger.LogEvent("来自“AnXinWH.SocketRFIDService”的全局异常。" + ex.Message + "详细信息如下："
                                    + Environment.NewLine + "［InnerException］" + ex.InnerException
                                    + Environment.NewLine + "［Source］" + ex.Source
                                    + Environment.NewLine + "［TargetSite］" + ex.TargetSite
                                    + Environment.NewLine + "［StackTrace］" + ex.StackTrace);
            }
            catch { }
        }

        protected override void OnStart(string[] args)
        {

            try
            {
                scheduler.Start();
                AllMsg("Quartz服务成功启动.");


                //
                Working();

                DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTimeOffset.Now);

                //get
                #region satrtAutoGetXml 自动点检 job
                var evenTime = 60;
                logger.DebugFormat("******** 自动点检 **********周期:{0} 秒。", evenTime);
                IJobDetail AutoCheckTimeJob = JobBuilder.Create<AutoCheckTimeJob>().WithIdentity("AutoCheckTimeJob", "AutoCheckTimeJobGroup").Build();

                ITrigger AutoCheckTimeJob_trigger = TriggerBuilder.Create()
                    .WithIdentity("AutoCheckTimeJobTrigger", "AutoCheckTimeJobGroup")
                    .StartAt(runTime)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(evenTime).RepeatForever())
                    .Build();
                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(AutoCheckTimeJob, AutoCheckTimeJob_trigger);
                #endregion
                //get
                #region satrtAutoGetXml 手动点检 job
                var evenTime2 = 30;
                logger.DebugFormat("******** 手动点检 **********周期:{0} 秒。", evenTime2);
                IJobDetail AutoGetXml_job = JobBuilder.Create<AutoGetJob>().WithIdentity("AutoGetJob", "AutoGetJobGroup").Build();

                ITrigger AutoGetXml_trigger = TriggerBuilder.Create()
                    .WithIdentity("AutoGetJobTrigger", "AutoGetJobGroup")
                    .StartAt(runTime)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(evenTime2).RepeatForever())
                    .Build();
                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(AutoGetXml_job, AutoGetXml_trigger);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void AllMsg(string message)
        {
            logger.Debug(message);
            logger.Info(message);
            //test
            //logger.Debug("执行任务!!!!!!!!!!!!!!!");
            //using (MysqlDbContext dbcontext = new MysqlDbContext())
            //{
            //    try
            //    {
            //        logger.DebugFormat("test!");
            //        var dd = dbcontext.t_Interface.ToList();
            //        logger.Debug(dd.Count);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error("test", ex);
            //    }
            //}
        }

        protected override void OnStop()
        {
            try
            {
                scheduler.Shutdown();
                logger.Info("Quartz服务成功终止");
            }
            finally { }
        }
        protected override void OnPause()
        {
            scheduler.PauseAll();
        }

        protected override void OnContinue()
        {
            scheduler.ResumeAll();
        }
        public void Working()
        {
            TcpListernerThread tmptcp = new TcpListernerThread();

            TcpListernerThread.mIP = ConfigurationManager.AppSettings["ServerIP"];
            TcpListernerThread.mPort = ConfigurationManager.AppSettings["ServerPort"];


            var tmpType = "RFID采集服务";//Program._sysType.Equals("0") ? "入库" : "出库";
            AllMsg("******************[ " + tmpType + " ]************************");
            AllMsg("*_* *_* *_* *_* *_*_______________#_# #_# #_# #_# #_# #_# ");
            AllMsg("     *_* 监听服务成功启动.IP:" + TcpListernerThread.mIP + ", Port:" + TcpListernerThread.mPort);        
            AllMsg("******************[ " + tmpType + " ]************************");
            AllMsg("*_* *_* *_* *_* *_*_______________#_# #_# #_# #_# #_# #_# ");


            Thread tr = new Thread(tmptcp.GetMessage);
            tr.IsBackground = true;
            tr.Start();

        }

        public void SocketStop()
        {
            lock (TcpListernerThread.locker)//锁        
            {
                TcpListernerThread.s_bolWork = false;
            }
            if (TcpListernerThread.ms != null)
            {
                TcpListernerThread.mThread.Abort();
                TcpListernerThread.ms.Dispose();
            }
        }

    }
}
