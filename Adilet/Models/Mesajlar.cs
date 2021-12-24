namespace Adilet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mesajlar")]
    public partial class Mesajlar
    {
        [Key]
        public int MesajId { get; set; }

        [StringLength(50)]
        public string MesajGonderen { get; set; }

        [StringLength(50)]
        public string MesajBaslik { get; set; }

        [StringLength(50)]
        public string MesajMail { get; set; }

        [StringLength(1000)]
        public string Mesajicerik { get; set; }
    }
}
