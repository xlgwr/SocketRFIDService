namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_funcform")]
    public partial class m_funcform
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string formid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string functiontype { get; set; }

        [Required]
        [StringLength(64)]
        public string formname { get; set; }

        public int sortno { get; set; }

        public short frmstatus { get; set; }
    }
}
