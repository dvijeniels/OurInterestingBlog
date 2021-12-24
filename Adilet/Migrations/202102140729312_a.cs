namespace Adilet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Duyrular",
                c => new
                    {
                        DuyuruId = c.Int(nullable: false, identity: true),
                        DuyuruAd = c.String(maxLength: 100),
                        Duyuruicerik = c.String(maxLength: 2000),
                        DuyuruResim = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DuyuruId);
            
            CreateTable(
                "dbo.Kategori",
                c => new
                    {
                        KategoriId = c.Int(nullable: false, identity: true),
                        KategoriAdi = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.KategoriId);
            
            CreateTable(
                "dbo.Makale",
                c => new
                    {
                        MakaleId = c.Int(nullable: false, identity: true),
                        Baslik = c.String(maxLength: 500),
                        Icerik = c.String(),
                        Foto = c.String(maxLength: 500),
                        Tarih = c.DateTime(),
                        KategoriId = c.Int(),
                        UyeId = c.Int(),
                        Okunma = c.Int(),
                    })
                .PrimaryKey(t => t.MakaleId)
                .ForeignKey("dbo.Kategori", t => t.KategoriId)
                .ForeignKey("dbo.Uye", t => t.UyeId)
                .Index(t => t.KategoriId)
                .Index(t => t.UyeId);
            
            CreateTable(
                "dbo.Uye",
                c => new
                    {
                        UyeId = c.Int(nullable: false, identity: true),
                        KullaniciAdi = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Sifre = c.String(maxLength: 20),
                        AdSoyad = c.String(maxLength: 50),
                        Foto = c.String(maxLength: 250),
                        YetkiId = c.Int(),
                    })
                .PrimaryKey(t => t.UyeId)
                .ForeignKey("dbo.Yetki", t => t.YetkiId)
                .Index(t => t.YetkiId);
            
            CreateTable(
                "dbo.Yetki",
                c => new
                    {
                        YetkiId = c.Int(nullable: false, identity: true),
                        Yetki = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.YetkiId);
            
            CreateTable(
                "dbo.Yorum",
                c => new
                    {
                        YorumId = c.Int(nullable: false, identity: true),
                        Icerik = c.String(maxLength: 500),
                        UyeId = c.Int(),
                        MakaleId = c.Int(),
                        Tarih = c.DateTime(),
                    })
                .PrimaryKey(t => t.YorumId)
                .ForeignKey("dbo.Makale", t => t.MakaleId)
                .ForeignKey("dbo.Uye", t => t.UyeId)
                .Index(t => t.UyeId)
                .Index(t => t.MakaleId);
            
            CreateTable(
                "dbo.Mesajlar",
                c => new
                    {
                        MesajId = c.Int(nullable: false, identity: true),
                        MesajGonderen = c.String(maxLength: 50),
                        MesajBaslik = c.String(maxLength: 50),
                        MesajMail = c.String(maxLength: 50),
                        Mesajicerik = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.MesajId);
            
            CreateTable(
                "dbo.Metin",
                c => new
                    {
                        MetinId = c.Int(nullable: false),
                        MetinIcerik = c.String(maxLength: 3000),
                        Resim = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.MetinId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Yorum", "UyeId", "dbo.Uye");
            DropForeignKey("dbo.Yorum", "MakaleId", "dbo.Makale");
            DropForeignKey("dbo.Uye", "YetkiId", "dbo.Yetki");
            DropForeignKey("dbo.Makale", "UyeId", "dbo.Uye");
            DropForeignKey("dbo.Makale", "KategoriId", "dbo.Kategori");
            DropIndex("dbo.Yorum", new[] { "MakaleId" });
            DropIndex("dbo.Yorum", new[] { "UyeId" });
            DropIndex("dbo.Uye", new[] { "YetkiId" });
            DropIndex("dbo.Makale", new[] { "UyeId" });
            DropIndex("dbo.Makale", new[] { "KategoriId" });
            DropTable("dbo.Metin");
            DropTable("dbo.Mesajlar");
            DropTable("dbo.Yorum");
            DropTable("dbo.Yetki");
            DropTable("dbo.Uye");
            DropTable("dbo.Makale");
            DropTable("dbo.Kategori");
            DropTable("dbo.Duyrular");
        }
    }
}
