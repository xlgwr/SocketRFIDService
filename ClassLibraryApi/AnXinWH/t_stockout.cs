namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_stockout
    {
        [Key]
        [StringLength(32)]
        public string stockout_id { get; set; }

        public DateTime? stockout_date { get; set; }

        [StringLength(16)]
        public string user_no { get; set; }

        [StringLength(16)]
        public string pickup_user { get; set; }

        [StringLength(32)]
        public string pickup_card { get; set; }

        [StringLength(32)]
        public string pickup_mobile { get; set; }

        public short? status { get; set; }

        [Required]
        [StringLength(256)]
        public string remark { get; set; }

        [Required]
        [StringLength(16)]
        public string adduser { get; set; }

        [Required]
        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime addtime { get; set; }

        public DateTime updtime { get; set; }
    }
}
