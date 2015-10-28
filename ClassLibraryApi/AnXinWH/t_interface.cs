namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_interface
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int recd_id { get; set; }

        [StringLength(128)]
        public string address { get; set; }

        public int? type { get; set; }

        [StringLength(16)]
        public string downtime { get; set; }

        public int? downtype { get; set; }

        [StringLength(128)]
        public string adjunct_address { get; set; }

        [StringLength(128)]
        public string adjunct_value { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public int? status { get; set; }
    }
}
