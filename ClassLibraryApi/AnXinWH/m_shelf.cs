namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_shelf")]
    public partial class m_shelf
    {
        [Key]
        [StringLength(16)]
        public string shelf_no { get; set; }

        [StringLength(32)]
        public string shelf_nm { get; set; }

        [StringLength(16)]
        public string depot_no { get; set; }

        [StringLength(16)]
        public string shelf_type { get; set; }

        [StringLength(128)]
        public string area { get; set; }

        [StringLength(128)]
        public string location { get; set; }

        [StringLength(258)]
        public string remark { get; set; }

        public short? status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
