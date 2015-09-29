namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_users")]
    public partial class m_users
    {
        [Key]
        [StringLength(16)]
        public string user_no { get; set; }

        [Required]
        [StringLength(32)]
        public string user_nm { get; set; }

        [Required]
        [StringLength(16)]
        public string depot_no { get; set; }

        [Required]
        [StringLength(64)]
        public string user_pwd { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        [Required]
        [StringLength(128)]
        public string role_id { get; set; }

        public short status { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [StringLength(16)]
        public string org_no { get; set; }
    }
}
