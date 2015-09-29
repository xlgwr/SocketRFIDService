namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("anxinwh.m_products")]
    public partial class m_products
    {
        [Key]
        [StringLength(48)]
        public string prdct_no { get; set; }

        [Required]
        [StringLength(32)]
        public string prdct_nm { get; set; }

        [Required]
        [StringLength(255)]
        public string prdct_abbr { get; set; }

        [Required]
        [StringLength(255)]
        public string depot_no { get; set; }

        [Required]
        [StringLength(255)]
        public string prdct_type { get; set; }

        [Required]
        [StringLength(255)]
        public string unit { get; set; }

        [StringLength(255)]
        public string remark { get; set; }

        [StringLength(255)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [StringLength(255)]
        public string adduser { get; set; }

        public short status { get; set; }
    }
}
