namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_checkpoint")]
    public partial class m_checkpoint
    {
        [Key]
        [StringLength(32)]
        public string checkpointno { get; set; }

        [Required]
        [StringLength(16)]
        public string checktime { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public short status { get; set; }

        [StringLength(32)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [Required]
        [StringLength(16)]
        public string depot_no { get; set; }
    }
}
