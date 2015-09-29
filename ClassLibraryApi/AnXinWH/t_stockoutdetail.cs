namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.t_stockoutdetail")]
    public partial class t_stockoutdetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string stockout_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string out_item_no { get; set; }

        [Required]
        [StringLength(30)]
        public string cash_no { get; set; }

        [Required]
        [StringLength(10)]
        public string item_no { get; set; }

        [StringLength(48)]
        public string prdct_no { get; set; }

        [Required]
        [StringLength(30)]
        public string pc { get; set; }

        public float? pqty { get; set; }

        public float qty { get; set; }

        public float nwet { get; set; }

        public float gwet { get; set; }

        [Required]
        [StringLength(20)]
        public string quanlity { get; set; }

        public short? status { get; set; }

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
