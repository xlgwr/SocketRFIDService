namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_syslogrecd
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int log_id { get; set; }

        [Column("operator")]
        [StringLength(32)]
        public string ooperator { get; set; }

        [StringLength(32)]
        public string message { get; set; }

        public short? type { get; set; }

        public short? result { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string mod_id { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        public DateTime? addtime { get; set; }

        [StringLength(255)]
        public string org_no { get; set; }
    }
}
