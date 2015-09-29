namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_depot")]
    public partial class m_depot
    {
        [Key]
        [StringLength(8)]
        public string depot_no { get; set; }

        [Required]
        [StringLength(32)]
        public string depot_nm { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
