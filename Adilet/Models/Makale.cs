namespace Adilet.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Makale")]
    public partial class Makale
    {
        public Makale()
        {
            Yorum = new HashSet<Yorum>();
        }

        public int MakaleId { get; set; }

        [StringLength(500)]
        public string Baslik { get; set; }

        public string Icerik { get; set; }

        [StringLength(500)]
        public string Foto { get; set; }

        public DateTime? Tarih { get; set; }

        public int? KategoriId { get; set; }

        public int? UyeId { get; set; }

        public int? Okunma { get; set; }

        public virtual Kategori Kategori { get; set; }

        public virtual Uye Uye { get; set; }

        public virtual ICollection<Yorum> Yorum { get; set; }

    }
}
