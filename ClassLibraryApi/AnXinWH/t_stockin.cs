namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_stockin
    {
        [Key]
        [StringLength(32)]
        public string stockin_id { get; set; }

        public DateTime? stockin_date { get; set; }

        [StringLength(16)]
        public string user_no { get; set; }

        public short? status { get; set; }

        [StringLength(30)]
        public string op_no { get; set; }

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
