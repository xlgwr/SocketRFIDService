namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_stockoutsign
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string cash_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string cash_code { get; set; }

        [StringLength(16)]
        public string user_no { get; set; }

        [StringLength(30)]
        public string pickIdentity { get; set; }

        public short? status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
