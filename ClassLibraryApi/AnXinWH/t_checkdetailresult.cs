namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_checkdetailresult
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string check_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string out_item_no { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(48)]
        public string prdct_no { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(96)]
        public string rfid_no { get; set; }

        [StringLength(50)]
        public string receiptNo { get; set; }

        public float? qty { get; set; }

        public float? nwet { get; set; }

        public float? gwet { get; set; }

        [StringLength(30)]
        public string cell_no { get; set; }

        [Required]
        [StringLength(255)]
        public string remark { get; set; }

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
