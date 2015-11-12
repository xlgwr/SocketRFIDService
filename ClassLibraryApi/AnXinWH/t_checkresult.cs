namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_checkresult
    {
        [Key]
        [StringLength(128)]
        public string check_id { get; set; }

        public string check_date { get; set; }

        [StringLength(30)]
        public string bespeak_no { get; set; }

        [StringLength(16)]
        public string user_no { get; set; }

        [StringLength(32)]
        public string user_nm { get; set; }

        public short status { get; set; }

        [StringLength(255)]
        public string remark { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime addtime { get; set; }

        public DateTime updtime { get; set; }
    }
}
