namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_devicerelation")]
    public partial class m_devicerelation
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string RelationNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string TerminalNo { get; set; }

        [StringLength(32)]
        public string Relation1 { get; set; }

        [StringLength(32)]
        public string Relation2 { get; set; }

        [StringLength(32)]
        public string Relation3 { get; set; }

        [StringLength(32)]
        public string Relation4 { get; set; }

        [StringLength(32)]
        public string Relation5 { get; set; }

        [StringLength(32)]
        public string Relation6 { get; set; }

        [StringLength(32)]
        public string Relation7 { get; set; }

        [StringLength(32)]
        public string Relation8 { get; set; }
    }
}
