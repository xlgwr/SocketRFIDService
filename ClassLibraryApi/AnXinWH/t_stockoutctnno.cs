namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_stockoutctnno
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string stockout_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(48)]
        public string prdct_no { get; set; }

        public float pqty { get; set; }

        public float qty { get; set; }

        public float? nwet { get; set; }

        public float? gwet { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
