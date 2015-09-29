namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.t_terminaalarm")]
    public partial class t_terminaalarm
    {
        [Key]
        [StringLength(32)]
        public string AlarmNo { get; set; }

        public short AlarmType { get; set; }

        [Required]
        [StringLength(10)]
        public string TerminalNo { get; set; }

        public DateTime AlarmDate { get; set; }

        public int AlarmFlag { get; set; }

        [StringLength(256)]
        public string AlarmReason { get; set; }

        [StringLength(256)]
        public string Remark { get; set; }

        [StringLength(32)]
        public string UpdUserNo { get; set; }

        public DateTime? UpdDateTime { get; set; }
    }
}
