namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_stockdetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(48)]
        public string prdct_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(96)]
        public string rfid_no { get; set; }

        [StringLength(50)]
        public string receiptNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(16)]
        public string shelf_no { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string ctnno_no { get; set; }

        public float pqty { get; set; }

        public float qty { get; set; }

        public float nwet { get; set; }

        public float gwet { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
