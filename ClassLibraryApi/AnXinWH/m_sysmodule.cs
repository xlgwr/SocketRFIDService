namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_sysmodule")]
    public partial class m_sysmodule
    {
        [Key]
        [StringLength(16)]
        public string mod_id { get; set; }

        [Required]
        [StringLength(32)]
        public string mod_nm { get; set; }

        [Required]
        [StringLength(16)]
        public string parentid { get; set; }

        [Required]
        [StringLength(256)]
        public string url { get; set; }

        public int iconic { get; set; }

        public int islast { get; set; }

        [StringLength(32)]
        public string version { get; set; }

        public short flag { get; set; }

        public short status { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }
    }
}
