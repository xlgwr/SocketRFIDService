namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_classinfo")]
    public partial class m_classinfo
    {
        [Key]
        [StringLength(16)]
        public string cls_no { get; set; }

        [Required]
        [StringLength(128)]
        public string infoval { get; set; }

        [StringLength(256)]
        public string infoval2 { get; set; }

        [StringLength(256)]
        public string infoval3 { get; set; }

        public int cls_typno { get; set; }

        public int sort { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? updtime { get; set; }

        public DateTime? addtime { get; set; }
    }
}
