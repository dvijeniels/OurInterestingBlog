namespace Adilet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Duyrular")]
    public partial class Duyrular
    {
        [Key]
        public int DuyuruId { get; set; }

        [StringLength(100)]
        public string DuyuruAd { get; set; }

        [StringLength(2000)]
        public string Duyuruicerik { get; set; }

        [StringLength(100)]
        public string DuyuruResim { get; set; }
    }
}
