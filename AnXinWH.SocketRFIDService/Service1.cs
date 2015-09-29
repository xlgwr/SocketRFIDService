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
                #region satrtAutoGetXml job

                //IJobDetail AutoGetXml_job = JobBuilder.Create<AutoGetJob>().WithIdentity("autoGetXMLjob", "autoGetXMLGroup").Build();

                //ITrigger AutoGetXml_trigger = TriggerBuilder.Create()
                //    .WithIdentity("autoGetXMLTrigger", "autoGetXMLGroup")
                //    .StartAt(runTime)
                //    .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                //    .Build();


                //// Tell quartz to schedule the job using our trigger
                //scheduler.ScheduleJob(AutoGetXml_job, AutoGetXml_trigger);
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

            AllMsg("**********TcpListener监听服务成功启动.IP:" + TcpListernerThread.mIP + ",Port:" + TcpListernerThread.mPort);

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
        }

    }
}