namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_sampling
    {
        [Key]
        [StringLength(96)]
        public string rfid_no { get; set; }

        public float qty { get; set; }

        public float nwet { get; set; }

        public float gwet { get; set; }

        public float agwet { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
