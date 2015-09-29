namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_parameter")]
    public partial class m_parameter
    {
        [Key]
        [StringLength(16)]
        public string paramkey { get; set; }

        [StringLength(16)]
        public string paramvalue { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public short? paramtype { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [StringLength(16)]
        public string depot_no { get; set; }
    }
}
