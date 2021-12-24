namespace Adilet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseContex : DbContext
    {
        public DatabaseContex()
            : base("name=DatabaseContex")
        {
        }

        public virtual DbSet<Duyrular> Duyrular { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Makale> Makale { get; set; }
        public virtual DbSet<Mesajlar> Mesajlar { get; set; }
        public virtual DbSet<Metin> Metin { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }
        public virtual DbSet<Yorum> Yorum { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Etiket>()
        //        .HasMany(e => e.Makale)
        //        .WithMany(e => e.Etiket)
        //        .Map(m => m.ToTable("MakaleEtiket").MapLeftKey("EtiketId").MapRightKey("MakaleId"));
        //}
    }
}
