namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.t_roles")]
    public partial class t_roles
    {
        [Key]
        [StringLength(10)]
        public string roleid { get; set; }

        [Required]
        [StringLength(32)]
        public string rolename { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public DateTime? addtime { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? updtime { get; set; }
    }
}
