namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_syslogrecd
    {
        [Key]
        [Column(Order = 0)]
        public string log_id { get; set; }

        public string operatorid { get; set; }

        public string message { get; set; }

        public Int16 type { get; set; }

        public Int16 result { get; set; }

        [Key]
        [Column(Order = 1)]
        public string mod_id { get; set; }

        public string adduser { get; set; }

        public DateTime addtime { get; set; }

        public string org_no { get; set; }
    }
}
