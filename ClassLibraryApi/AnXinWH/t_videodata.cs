namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_videodata
    {
        [Key]
        [StringLength(32)]
        public string video_no { get; set; }

        public string message { get; set; }

        public float font_size { get; set; }

        public float font_type { get; set; }

        public short site { get; set; }

        public int Channel { get; set; }

        public string Version { get; set; }
        public short status { get; set; }
        public string remark { get; set; }

  
        public string adduser { get; set; }

        public DateTime? addtime { get; set; }
    }
}
