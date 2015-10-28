namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_cashdetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string cash_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string item_no { get; set; }

        [StringLength(48)]
        public string prdct_no { get; set; }

        [StringLength(30)]
        public string pc { get; set; }

        [StringLength(10)]
        public string unit { get; set; }

        public float? qty { get; set; }

        public float? nwgt { get; set; }

        public float? gwgt { get; set; }

        [StringLength(30)]
        public string quanlity { get; set; }

        [StringLength(30)]
        public string ctnno { get; set; }

        [StringLength(30)]
        public string package { get; set; }

        [StringLength(96)]
        public string rfid_no { get; set; }

        [StringLength(50)]
        public string receiptNo { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public short? status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
