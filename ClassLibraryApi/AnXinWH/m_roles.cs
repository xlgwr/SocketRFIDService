namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_roles
    {
        [Key]
        [StringLength(32)]
        public string role_id { get; set; }

        [Required]
        [StringLength(32)]
        public string role_nm { get; set; }

        [Required]
        [StringLength(16)]
        public string depot_no { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

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
