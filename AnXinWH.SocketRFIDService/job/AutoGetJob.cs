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

namespace AnXinWH.SocketRFIDService.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class AutoGetJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            logger.DebugFormat("*********开始运行job:{0}.下次job时间：{1}.", context.JobDetail.Key, context.NextFireTimeUtc);

            try
            {
                using (var db=new MysqlDbContext())
                {
                    var tmpcont = db.m_users.Count();
                    logger.DebugFormat("********************{0} 个用户.",tmpcont);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("######运行出错误：{0}", ex.Message);
            }
        }
    }
}