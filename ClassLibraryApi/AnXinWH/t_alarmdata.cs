namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.t_alarmdata")]
    public partial class t_alarmdata
    {
        [Key]
        [StringLength(255)]
        public string recd_id { get; set; }

        [Required]
        [StringLength(255)]
        public string alarm_type { get; set; }

        [Required]
        [StringLength(255)]
        public string depot_no { get; set; }

        [Required]
        [StringLength(255)]
        public string cell_no { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime? over_time { get; set; }

        public short? remark { get; set; }

        public short status { get; set; }

        [StringLength(255)]
        public string adduser { get; set; }

        [StringLength(255)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
