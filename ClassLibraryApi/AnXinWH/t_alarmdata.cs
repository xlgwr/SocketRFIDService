namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_alarmdata
    {
        [Key]
        [StringLength(255)]
        public string recd_id { get; set; }

        [Required]
        [StringLength(255)]
        public string alarm_type { get; set; }

        [StringLength(255)]
        public string depot_no { get; set; }

        [StringLength(255)]
        public string cell_no { get; set; }

        public DateTime begin_time { get; set; }

        public DateTime? over_time { get; set; }

        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }


        [StringLength(255)]
        public string remark { get; set; }

        public short status { get; set; }

        [StringLength(255)]
        public string adduser { get; set; }

        [StringLength(255)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
