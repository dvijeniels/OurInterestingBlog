using Adilet.Models;
using Adilet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopSite.Auth;
using System.IO;
using System.Web.Helpers;

namespace ShopSite.Controllers
{
    public class AdminController : BaseController
    {
        DatabaseContex db = new DatabaseContex();
        public ActionResult AdminIndex()
        {
            List<Kategori> kategoriler = db.Kategori.ToList();
            ViewBag.katSay = kategoriler.Count;
            List<Makale> makaleler = db.Makale.ToList();
            ViewBag.makaleSay = makaleler.Count;
            List<Uye> uyeler = db.Uye.ToList();
            ViewBag.uyeSay = uyeler.Count;
            List<Yorum> yorumlar = db.Yorum.ToList();
            ViewBag.yorumSay = yorumlar.Count;
            List<Duyrular> duyurular = db.Duyrular.ToList();
            ViewBag.duyuruSay = duyurular.Count;
            List<Mesajlar> mesaj = db.Mesajlar.ToList();
            ViewBag.mesajSay = mesaj.Count;
            return View();
        }
        public ActionResult Kategoriler(int? id)
        {
            if (id == 1)
            {
                ViewBag.hata = "Kategori üzerinde makale kayıtlı olduğu için silinemez!";
            }
            if (id == 2)
            {
                ViewBag.sonuc = "Kategori Silindi!";
            }
            List<Kategori> kategoriler = db.Kategori.ToList();
            return View(kategoriler);
        }
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Where(m => m.KategoriAdi == model.KategoriAdi).Count() > 0)
            {
                ViewBag.hata = "Girilen kategori zaten kayıtlıdır tekrar deneyin!";
            }
            else
            {
                Kategori kat = new Kategori();
                kat.KategoriAdi = model.KategoriAdi;
                db.Kategori.Add(kat);
                db.SaveChanges();
                ViewBag.sonuc = "Kategori Eklendi";
            }
            return View();
        }
        public ActionResult KategoriDuzenle(int id)
        {
            Kategori kat = db.Kategori.Where(m => m.KategoriId == id).SingleOrDefault();
            if (kat == null)
            {
                return RedirectToAction("Kategoriler");
            }
            KategoriModel model = new KategoriModel();
            model.KategoriId = kat.KategoriId;
            model.KategoriAdi = kat.KategoriAdi;

            return View(model);
        }
        [HttpPost]
        public ActionResult KategoriDuzenle(KategoriModel model)
        {
            Kategori kat = db.Kategori.Where(m => m.KategoriId == model.KategoriId).SingleOrDefault();
            if (db.Kategori.Where(m => m.KategoriAdi == model.KategoriAdi).Count() > 0)
            {
                ViewBag.hata = "Girilen kategori zaten kayıtlıdır tekrar deneyin!";
            }
            else
            {
                kat.KategoriAdi = model.KategoriAdi;
                db.SaveChanges();
                ViewBag.sonuc = "Kategori Düzenlendi";
            }
            return View(model);
        }
        public ActionResult KategoriSil(int id)
        {
            if (db.Makale.Where(m => m.KategoriId == id).Count() > 0)
            {
                return RedirectToAction("Kategoriler/1");
            }
            Kategori kat = db.Kategori.Where(m => m.KategoriId == id).SingleOrDefault();
            if (kat != null)
            {
                db.Kategori.Remove(kat);
                db.SaveChanges();
                return RedirectToAction("Kategoriler/2");
            }
            return RedirectToAction("Kategoriler");
        }
        public ActionResult Makaleler(int? id)
        {
            if (id == 1)
            {
                ViewBag.sonuc = "Makale Silindi!";
            }
            List<Makale> makaleler = db.Makale.ToList();
            return View(makaleler);
        }

        public ActionResult MakaleEkle()
        {
            MakaleModel model = getModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult MakaleEkle(MakaleModel model)
        {
            int uyeId = Convert.ToInt32(Session["uyeId"].ToString());
            if (db.Makale.Where(m => m.Baslik == model.Baslik).Count() > 0)
            {
                ViewBag.hata = "Girilen başlık zaten kayıtlıdır!";
                model = getModel();
                return View(model);
            }
            else
            {
                Makale makale = new Makale();
                if (model.Foto != null && model.Foto.ContentLength > 0)
                {
                    string dosya = Guid.NewGuid().ToString();
                    string uzanti = Path.GetExtension(model.Foto.FileName).ToLower();
                    if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                    {
                        ModelState.AddModelError("Resim", "Dosya uzantısı PNG, JPG VEYA JPEG olmalıdır");
                        return View(model);
                    }
                    string DosyaAdi = dosya + uzanti;
                    model.Foto.SaveAs(Server.MapPath("~/Content/BlogResim/" + DosyaAdi));
                    makale.Foto = DosyaAdi;
                    makale.Baslik = model.Baslik;
                    makale.KategoriId = model.KategoriId;
                    makale.Icerik = model.Icerik;
                    makale.Tarih = DateTime.Now;
                    makale.UyeId = uyeId;
                    makale.Okunma = 0;
                    db.Makale.Add(makale);
                    db.SaveChanges();
                    ViewBag.sonuc = "Makale Eklendi";
                    model = getModel();
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Foto", "Dosya seçim hatası");
                    model = getModel();
                    return View(model);
                }

            }
        }
        public ActionResult MakaleDuzenle(int id)
        {
            Makale makale = db.Makale.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return RedirectToAction("Makaleler");
            }
            MakaleModel model = new MakaleModel();
            model.MakaleId = makale.MakaleId;
            model.Baslik = makale.Baslik;
            model.Icerik = makale.Icerik;
            model.KategoriListe = (from kat in db.Kategori.ToList()
                                   select new SelectListItem
                                   {
                                       Selected = true,
                                       Text = kat.KategoriAdi,
                                       Value = kat.KategoriId.ToString()
                                   }).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult MakaleDuzenle(MakaleModel model)
        {
            Makale makale = db.Makale.Where(m => m.MakaleId == model.MakaleId).SingleOrDefault();
            if (makale == null)
            {
                return RedirectToAction("Makaleler");
            }
            else
            {
                if (model.Foto != null && model.Foto.ContentLength > 0)
                {
                    string dosya = Guid.NewGuid().ToString();
                    string uzanti = Path.GetExtension(model.Foto.FileName).ToLower();
                    if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                    {
                        ModelState.AddModelError("Resim", "Dosya uzantısı PNG, JPG VEYA JPEG olmalıdır");
                        return View(model);
                    }
                    string DosyaAdi = dosya + uzanti;
                    model.Foto.SaveAs(Server.MapPath("~/Content/BlogResim/" + DosyaAdi));
                    makale.Foto = DosyaAdi;
                    makale.Baslik = model.Baslik;
                    makale.Icerik = model.Icerik;
                    makale.KategoriId = model.KategoriId;
                    makale.Tarih = DateTime.Now;
                    db.SaveChanges();
                    ViewBag.sonuc = "Makale Düzenlendi";
                    model = getModel();
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Foto", "Dosya seçim hatası");
                    model = getModel();
                    return View(model);
                }
            }
        }
        public ActionResult MakaleSil(int id)
        {
            Makale makale = db.Makale.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return RedirectToAction("Makaleler");
            }
            if (System.IO.File.Exists(Server.MapPath("~/Content/BlogResim/" + makale.Foto)))
            {
                System.IO.File.Delete(Server.MapPath("~/Content/BlogResim/" + makale.Foto));
            }
            List<Yorum> yorumlar = db.Yorum.Where(m => m.MakaleId == makale.MakaleId).ToList();
            db.Yorum.RemoveRange(yorumlar);
            db.Makale.Remove(makale);
            db.SaveChanges();
            return RedirectToAction("Makaleler/1");
        }
        private MakaleModel getModel()
        {
            MakaleModel model = new MakaleModel();
            model.KategoriListe = (from kat in db.Kategori.ToList()
                                   select new SelectListItem
                                   {
                                       Selected = false,
                                       Text = kat.KategoriAdi,
                                       Value = kat.KategoriId.ToString()
                                   }).ToList();
            model.KategoriListe.Insert(0, new SelectListItem
            {
                Selected = true,
                Value = "",
                Text = "Seçiniz"
            });
            return model;
        }
        public ActionResult Uyeler(int? id)
        {
            if (id == 1)
            {
                ViewBag.sonuc = "Üye Silindi!";
            }
            List<Uye> uyeler = db.Uye.ToList();
            return View(uyeler);
        }
        public ActionResult UyeEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UyeEkle(UyeModel uyemodel)
        {
            if (db.Uye.Where(m => m.KullaniciAdi == uyemodel.KullaniciAdi).Count() > 0)
            {
                ViewBag.hata = "Girilen Kullanıcı Adı zaten kayıtlıdır!";
                return View(uyemodel);
            }
            else
            {
                Uye uye = new Uye();
                if (uyemodel.Foto != null && uyemodel.Foto.ContentLength > 0)
                {
                    string uzanti = Path.GetExtension(uyemodel.Foto.FileName).ToLower();
                    string dosya = Guid.NewGuid().ToString();
                    if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                    {
                        ModelState.AddModelError("Resim", "Dosya uzantısı PNG, JPG VEYA JPEG olmalıdır");
                        return View(uyemodel);
                    }
                    string DosyaAdi = dosya + uzanti;
                    uyemodel.Foto.SaveAs(Server.MapPath("~/Content/UyeFoto/" + DosyaAdi));
                    uye.Foto = DosyaAdi;
                    uye.KullaniciAdi = uyemodel.KullaniciAdi;
                    uye.AdSoyad = uyemodel.AdSoyad;
                    uye.Email = uyemodel.Email;
                    uye.Sifre = uyemodel.Sifre;
                    uye.YetkiId = uyemodel.YetkiId;
                    db.Uye.Add(uye);
                    db.SaveChanges();
                    ViewBag.sonuc = "Üye Eklendi";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Foto", "Dosya seçim hatası");
                    return View();
                }
            }
        }
        public ActionResult UyeDuzenle(int ? id)
        {
            Uye uye = db.Uye.Where(m => m.UyeId == id).SingleOrDefault();
            if (uye==null)
            {
                return RedirectToAction("Uyeler");
            }
            UyeModel modeluye = new UyeModel();
            modeluye.UyeId = uye.UyeId;
            modeluye.KullaniciAdi = uye.KullaniciAdi;
            modeluye.AdSoyad = uye.AdSoyad;
            modeluye.Email = uye.Email;
            modeluye.Sifre = uye.Sifre;
            modeluye.YetkiId = uye.YetkiId;
            return View(modeluye);
        }
        [HttpPost]
        public ActionResult UyeDuzenle(UyeModel model)
        {
            Uye uye = db.Uye.Where(m => m.UyeId == model.UyeId).SingleOrDefault();
            if (uye == null)
            {
                return RedirectToAction("Uyeler");
            }
            uye.UyeId = model.UyeId;
            uye.KullaniciAdi = model.KullaniciAdi;
            uye.AdSoyad = model.AdSoyad;
            uye.Email = model.Email;
            uye.Sifre = model.Sifre;
            uye.YetkiId = model.YetkiId;
            db.SaveChanges();
            ViewBag.sonuc = "Üye Düzenlendi";
            return View();
        }
        public ActionResult UyeSil(int ? id)
        {
            Uye uye = db.Uye.Where(m => m.UyeId == id).SingleOrDefault();
            if (uye == null)
            {
                return RedirectToAction("Uyeler");
            }
            if (System.IO.File.Exists(Server.MapPath("~/Content/UyeFoto/" + uye.Foto)))
            {
                System.IO.File.Delete(Server.MapPath("~/Content/UyeFoto/" + uye.Foto));
            }
            List<Yorum> yorumlar = db.Yorum.Where(m => m.UyeId == uye.UyeId).ToList();
            db.Yorum.RemoveRange(yorumlar);
            List<Makale> makaleler = db.Makale.Where(m => m.UyeId == uye.UyeId).ToList();
            db.Makale.RemoveRange(makaleler);
            db.Uye.Remove(uye);
            db.SaveChanges();
            return RedirectToAction("Uyeler/1");
        }
        public ActionResult Duyurular(int ? id)
        {
            if (id == 1)
            {
                ViewBag.sonuc = "Duyru silindi!";
            }
            List<Duyrular> duyuru = db.Duyrular.ToList();
            return View(duyuru);
        }
        public ActionResult DuyuruEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DuyuruEkle(DuyuruModel model)
        {
            if (db.Duyrular.Where(m => m.DuyuruAd == model.DuyuruAd).Count() > 0)
            {
                ViewBag.hata = "Girilen Duyuru zaten kayıtlıdır tekrar deneyin!";
                return View(model);
            }
            else
            {
                Duyrular duyuru = new Duyrular();
                if (model.DuyuruResim != null && model.DuyuruResim.ContentLength > 0)
                {
                    string dosya = Guid.NewGuid().ToString();
                    string uzanti = Path.GetExtension(model.DuyuruResim.FileName).ToLower();
                    if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                    {
                        ModelState.AddModelError("Resim", "Dosya uzantısı PNG, JPG VEYA JPEG olmalıdır");
                        return View(model);
                    }
                    string DosyaAdi = dosya + uzanti;
                    model.DuyuruResim.SaveAs(Server.MapPath("~/Content/DuyuruResim/" + DosyaAdi));
                    duyuru.DuyuruResim = DosyaAdi;
                    duyuru.DuyuruAd = model.DuyuruAd;
                    duyuru.Duyuruicerik = model.Duyuruicerik;
                    db.Duyrular.Add(duyuru);
                    db.SaveChanges();
                    ViewBag.sonuc = "Duyuru Eklendi";
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Foto", "Dosya seçim hatası");
                    return View(model);
                }
            }
        }
        public ActionResult DuyuruDuzenle(int id)
        {
            Duyrular duyru = db.Duyrular.Where(m => m.DuyuruId == id).SingleOrDefault();
            if (duyru == null)
            {
                return RedirectToAction("Duyurular");
            }
            DuyuruModel model = new DuyuruModel();
            model.DuyuruAd = duyru.DuyuruAd;
            model.Duyuruicerik = duyru.Duyuruicerik;

            return View(model);
        }
        [HttpPost]
        public ActionResult DuyuruDuzenle(DuyuruModel model)
        {
            Duyrular duyuru = db.Duyrular.Where(m => m.DuyuruId == model.DuyuruId).SingleOrDefault();
            if (db.Duyrular.Where(m => m.DuyuruAd == model.DuyuruAd).Count() > 0)
            {
                ViewBag.hata = "Girilen duyru zaten kayıtlıdır tekrar deneyin!";
            }
            else
            {
                duyuru.DuyuruAd = model.DuyuruAd;
                duyuru.Duyuruicerik = model.Duyuruicerik;
                db.SaveChanges();
                ViewBag.sonuc = "Duyru Düzenlendi";
            }
            return View(model);
        }
        public ActionResult DuyuruSil(int id)
        {
            Duyrular duyuru = db.Duyrular.Where(m => m.DuyuruId == id).SingleOrDefault();
            if (duyuru == null)
            {
                return RedirectToAction("Duyurular");
            }
            if (System.IO.File.Exists(Server.MapPath("~/Content/DuyuruResim/" + duyuru.DuyuruResim)))
            {
                System.IO.File.Delete(Server.MapPath("~/Content/DuyuruResim/" + duyuru.DuyuruResim));
            }
            db.Duyrular.Remove(duyuru);
            db.SaveChanges();
            return RedirectToAction("Duyurular/1");
        }
        public ActionResult Hakkimizda()
        {
            List<Metin> metin = db.Metin.ToList();
            return View(metin);
        }
        public ActionResult HakkimizdaDuzenle(int id)
        {
            Metin metin = db.Metin.Where(m => m.MetinId == id).SingleOrDefault();
            if (metin == null)
            {
                return RedirectToAction("Hakkimizda");
            }
            HakkimizdaModel model = new HakkimizdaModel();
            model.MetinId = metin.MetinId;
            model.MetinIcerik = metin.MetinIcerik;
            return View(model);
        }
        [HttpPost]
        public ActionResult HakkimizdaDuzenle(HakkimizdaModel model)
        {
            Metin metin = db.Metin.SingleOrDefault();
            if (model.Resim != null && model.Resim.ContentLength > 0)
            {
                string dosya = Guid.NewGuid().ToString();
                string uzanti = Path.GetExtension(model.Resim.FileName).ToLower();
                if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png")
                {
                    ModelState.AddModelError("Resim", "Dosya uzantısı PNG, JPG VEYA JPEG olmalıdır");
                    return View(model);
                }
                string DosyaAdi = dosya + uzanti;
                model.Resim.SaveAs(Server.MapPath("~/Content/BlogResim/" + DosyaAdi));
                metin.Resim = DosyaAdi;
                metin.MetinIcerik = model.MetinIcerik;
                db.SaveChanges();
                ViewBag.sonuc = "Hakkımızda sayfası düzenlendi";
                return View(model);
            }
            else
            {
                ModelState.AddModelError("Foto", "Dosya seçim hatası");
                return View(model);
            }
            
        }
        public ActionResult Mesajlar(int? id)
        {
            if (id == 1)
            {
                ViewBag.sonuc = "Mesaj silindi!";
            }
            List<Mesajlar> mesajlar = db.Mesajlar.ToList();
            return View(mesajlar);
        }
        public ActionResult MesajSil(int id)
        {
            Mesajlar mesaj = db.Mesajlar.Where(m => m.MesajId == id).SingleOrDefault();
            if (mesaj != null)
            {
                db.Mesajlar.Remove(mesaj);
                db.SaveChanges();
                return RedirectToAction("Mesajlar/1");
            }
            return RedirectToAction("Mesajlar");
        }
    }
}