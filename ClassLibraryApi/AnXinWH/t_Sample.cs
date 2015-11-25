namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Sample
    {
        [Key]
        [StringLength(96)]
        public string sampl_no { get; set; }

        public string rfid_no { get; set; }

        public DateTime sampl_time { get; set; }

        public string device_id { get; set; }

        public short status { get; set; }
        public string remark { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
