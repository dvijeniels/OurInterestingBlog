namespace Adilet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Metin")]
    public partial class Metin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MetinId { get; set; }

        [StringLength(3000)]
        public string MetinIcerik { get; set; }

        [StringLength(300)]
        public string Resim { get; set; }
    }
}
