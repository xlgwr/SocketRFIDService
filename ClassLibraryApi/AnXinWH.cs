namespace ClassLibraryApi.AnXinWH
{
    using log4net;
    using MySql.Data.Entity;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Reflection;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class AnXinWH : DbContext
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        public AnXinWH()
            : base("name=MysqlDbConn")
        {
            //add log for EF gen SQL by xlg
            Database.Log = message => logger.DebugFormat(message.Replace("\n", " "));
        }

        public virtual DbSet<m_checkpoint> m_checkpoint { get; set; }
        public virtual DbSet<m_classinfo> m_classinfo { get; set; }
        public virtual DbSet<m_depot> m_depot { get; set; }
        public virtual DbSet<m_devicemodel> m_devicemodel { get; set; }
        public virtual DbSet<m_devicerelation> m_devicerelation { get; set; }
        public virtual DbSet<m_funcform> m_funcform { get; set; }
        public virtual DbSet<m_parameter> m_parameter { get; set; }
        public virtual DbSet<m_products> m_products { get; set; }
        public virtual DbSet<m_roledetail> m_roledetail { get; set; }
        public virtual DbSet<m_roles> m_roles { get; set; }
        public virtual DbSet<m_shelf> m_shelf { get; set; }
        public virtual DbSet<m_sysmodule> m_sysmodule { get; set; }
        public virtual DbSet<m_sysmoduledetail> m_sysmoduledetail { get; set; }
        public virtual DbSet<m_terminaldevice> m_terminaldevice { get; set; }
        public virtual DbSet<m_users> m_users { get; set; }
        public virtual DbSet<t_alarmdata> t_alarmdata { get; set; }
        public virtual DbSet<t_bespeak> t_bespeak { get; set; }
        public virtual DbSet<t_bespeakdetail> t_bespeakdetail { get; set; }
        public virtual DbSet<t_cash> t_cash { get; set; }
        public virtual DbSet<t_cashdetail> t_cashdetail { get; set; }
        public virtual DbSet<t_checkdetailresult> t_checkdetailresult { get; set; }
        public virtual DbSet<t_checkresult> t_checkresult { get; set; }
        public virtual DbSet<t_functioninfo> t_functioninfo { get; set; }
        public virtual DbSet<t_interface> t_interface { get; set; }
        public virtual DbSet<t_roles> t_roles { get; set; }
        public virtual DbSet<t_sampling> t_sampling { get; set; }
        public virtual DbSet<t_videodata> t_videodata { get; set; }
        public virtual DbSet<t_stock> t_stock { get; set; }
        public virtual DbSet<t_stockdetail> t_stockdetail { get; set; }
        public virtual DbSet<t_stockin> t_stockin { get; set; }
        public virtual DbSet<t_stockinctnno> t_stockinctnno { get; set; }
        public virtual DbSet<t_stockinctnnodetail> t_stockinctnnodetail { get; set; }
        public virtual DbSet<t_stockindetail> t_stockindetail { get; set; }
        public virtual DbSet<t_stockout> t_stockout { get; set; }
        public virtual DbSet<t_stockoutctnno> t_stockoutctnno { get; set; }
        public virtual DbSet<t_stockoutctnnodetail> t_stockoutctnnodetail { get; set; }
        public virtual DbSet<t_stockoutdetail> t_stockoutdetail { get; set; }
        public virtual DbSet<t_syslogrecd> t_syslogrecd { get; set; }
        public virtual DbSet<t_terminaalarm> t_terminaalarm { get; set; }
        public virtual DbSet<t_stockinsign> t_stockinsign { get; set; }
        public virtual DbSet<t_stockoutsign> t_stockoutsign { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //指定单数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
