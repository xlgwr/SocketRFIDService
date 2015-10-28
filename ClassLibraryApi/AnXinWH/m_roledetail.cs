namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_roledetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string role_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(16)]
        public string mod_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short opr_code { get; set; }
    }
}
