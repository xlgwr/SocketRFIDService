namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_functioninfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string roleid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string formid { get; set; }

        public short rolestatus { get; set; }
    }
}
