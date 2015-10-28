namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_devicemodel
    {
        [Key]
        [StringLength(10)]
        public string modelno { get; set; }

        [StringLength(50)]
        public string modenm { get; set; }

        public int modeflag { get; set; }

        [Required]
        [StringLength(16)]
        public string param1 { get; set; }

        [Required]
        [StringLength(16)]
        public string param2 { get; set; }

        [Required]
        [StringLength(16)]
        public string param3 { get; set; }

        [Required]
        [StringLength(16)]
        public string param4 { get; set; }

        [Required]
        [StringLength(16)]
        public string param5 { get; set; }

        [Required]
        [StringLength(16)]
        public string param6 { get; set; }

        [Required]
        [StringLength(16)]
        public string param7 { get; set; }

        [Required]
        [StringLength(16)]
        public string param8 { get; set; }

        [Required]
        [StringLength(16)]
        public string param9 { get; set; }

        [Required]
        [StringLength(16)]
        public string param10 { get; set; }

        [Required]
        [StringLength(16)]
        public string param11 { get; set; }

        [Required]
        [StringLength(16)]
        public string param12 { get; set; }

        [Required]
        [StringLength(16)]
        public string param13 { get; set; }

        [Required]
        [StringLength(16)]
        public string param14 { get; set; }

        [Required]
        [StringLength(16)]
        public string param15 { get; set; }

        [Required]
        [StringLength(16)]
        public string param16 { get; set; }

        [Required]
        [StringLength(16)]
        public string param17 { get; set; }

        [Required]
        [StringLength(16)]
        public string param18 { get; set; }

        [StringLength(256)]
        public string modereamrk { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [StringLength(32)]
        public string adduser { get; set; }

        [StringLength(32)]
        public string upduser { get; set; }
    }
}
