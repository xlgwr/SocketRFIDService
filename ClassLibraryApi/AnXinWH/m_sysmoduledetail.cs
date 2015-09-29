namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_sysmoduledetail")]
    public partial class m_sysmoduledetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(16)]
        public string mod_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short opr_code { get; set; }

        public int sort { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? updtime { get; set; }
    }
}
