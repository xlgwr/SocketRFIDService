namespace ClassLibraryApi.AnXinWH
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_terminaldevice
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string TerminalNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ModelNo { get; set; }

        [Required]
        [StringLength(32)]
        public string TerminalType { get; set; }

        [Required]
        [StringLength(50)]
        public string TerminalName { get; set; }

        [Required]
        [StringLength(16)]
        public string shelf_no { get; set; }

        public int ConnectFlag { get; set; }

        [Required]
        [StringLength(32)]
        public string SerialNoIPAddr { get; set; }

        public int ReadTime { get; set; }

        public int ReadInterval { get; set; }

        [StringLength(32)]
        public string param1 { get; set; }

        [StringLength(32)]
        public string param2 { get; set; }

        [StringLength(32)]
        public string param3 { get; set; }

        [StringLength(32)]
        public string param4 { get; set; }

        [StringLength(32)]
        public string param5 { get; set; }

        [StringLength(32)]
        public string param6 { get; set; }

        [StringLength(32)]
        public string param7 { get; set; }

        [StringLength(32)]
        public string param8 { get; set; }

        [StringLength(32)]
        public string param9 { get; set; }

        [StringLength(32)]
        public string param10 { get; set; }

        [StringLength(32)]
        public string param11 { get; set; }

        [StringLength(32)]
        public string param12 { get; set; }

        [StringLength(32)]
        public string param13 { get; set; }

        [StringLength(32)]
        public string param14 { get; set; }

        [StringLength(32)]
        public string param15 { get; set; }

        [StringLength(32)]
        public string param16 { get; set; }

        [StringLength(32)]
        public string param17 { get; set; }

        [StringLength(32)]
        public string param18 { get; set; }

        public DateTime ParamUpdTime { get; set; }

        public DateTime? TrmnUpdTime { get; set; }

        [StringLength(256)]
        public string TrmnRemark { get; set; }

        [Required]
        [StringLength(255)]
        public string CipherText { get; set; }

        public short TrmnStatus { get; set; }

        [StringLength(16)]
        public string adduser { get; set; }

        [StringLength(16)]
        public string upduser { get; set; }

        public DateTime? addtime { get; set; }

        public DateTime? updtime { get; set; }

        [StringLength(32)]
        public string UpdUserNo { get; set; }

        [Required]
        [StringLength(32)]
        public string depot_no { get; set; }
    }
}
