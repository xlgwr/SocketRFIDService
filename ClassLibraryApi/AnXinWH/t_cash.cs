namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_cash
    {
        [Key]
        [StringLength(30)]
        public string cash_no { get; set; }

        public DateTime? cash_date { get; set; }

        [StringLength(16)]
        public string user_no { get; set; }

        [StringLength(32)]
        public string user_nm { get; set; }

        [StringLength(50)]
        public string custorder { get; set; }

        [StringLength(50)]
        public string contacter { get; set; }

        [StringLength(50)]
        public string tel { get; set; }

        [StringLength(50)]
        public string mobile { get; set; }

        [StringLength(30)]
        public string pickidentity { get; set; }

        [StringLength(30)]
        public string checkcode { get; set; }

        [StringLength(50)]
        public string carrrier { get; set; }

        [StringLength(50)]
        public string tanspotno { get; set; }

        [StringLength(30)]
        public string vendorcode { get; set; }

        [StringLength(10)]
        public string coo { get; set; }

        public short? status { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
