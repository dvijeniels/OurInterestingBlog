using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Adilet.ViewModel;
using System.IO;
using Adilet.Models;

namespace ShopSite.Controllers
{
    public class HomeController : Controller
    {
       // mvcblogDB db = new mvcblogDB();
        DatabaseContex db = new DatabaseContex();
        public ActionResult Index()
        {
            List<Makale> makaleler = db.Makale.OrderByDescending(s => s.MakaleId).Take(5).ToList();
            return View(makaleler);
        }

        public ActionResult Kategoriler()
        {
            List<Kategori> Kategoriler = db.Kategori.ToList();
            return PartialView(Kategoriler);
        }
        public ActionResult KategoriMakale(int ? id)
        {
            Kategori kat = db.Kategori.Where(s => s.KategoriId == id).SingleOrDefault();
            if(kat==null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.kategori = kat.KategoriAdi;
            List<Makale> makaleler = db.Makale.Where(s=>s.KategoriId==id).ToList();
            if (makaleler.Count==0)
            {
                ViewBag.kayityok = "Henüz bu kategoriye ait makale yoktur";
            }
            return View(makaleler);
        }
        public ActionResult MakaleDetay(int id)
        {
            Makale makale = db.Makale.Where(s => s.MakaleId == id).SingleOrDefault();
            if (makale==null)
            {
                return RedirectToAction("Index");
            }
            makale.Okunma += 1;
            db.SaveChanges();
            return View(makale);
        }
        public PartialViewResult Yorumlar(int makaleId)
        {
            List<Yorum> yorumlar = db.Yorum.Where(s => s.MakaleId == makaleId).ToList();
            return PartialView(yorumlar);
        }
        public JsonResult YorumYap(string yorum ,int MakaleId)
        {
            int uyeID=Convert.ToInt32(Session["uyeId"].ToString());
            Yorum yeni = new Yorum();
            yeni.MakaleId = MakaleId;
            yeni.Icerik = yorum;
            yeni.UyeId = uyeID;
            yeni.Tarih = DateTime.Now;
            db.Yorum.Add(yeni);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult YorumSil(int yorumId)
        {
            Yorum yor = db.Yorum.Where(s => s.YorumId == yorumId).SingleOrDefault();
            if(yor!=null)
            {
                db.Yorum.Remove(yor);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult BlogAra(string deger)
        {
            List<Makale> makaleler = db.Makale.Where(s => s.Baslik.Contains(deger) || s.Icerik.Contains(deger)).ToList();
            if(makaleler.Count==0)
            {
                ViewBag.kayityok = "Aradığınız Değere Göre Makale Bulunmamaktadır";
            }
            else
            {
                ViewBag.kayityok = "Aradığınız Değere Göre: " + makaleler.Count+ " Makale Bulundu";
            }
            ViewBag.deger = deger;
            return View(makaleler);
        }
        public ActionResult OturumAc(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult OturumAc(UyeModel model,string returnUrl)
        {
            Uye uye = db.Uye.Where(m => m.KullaniciAdi == model.KullaniciAdi && m.Sifre == model.Sifre).SingleOrDefault();
            if (uye!=null)
            {
                Session["uyeOturum"] = true;
                Session["uyeId"] = uye.UyeId;
                Session["uyeKadi"] = uye.KullaniciAdi;
                Session["uyeAdmin"] = uye.YetkiId;
                Session["uyeFoto"] = uye.Foto;
                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                else
                { 
                    return Redirect(returnUrl); 
                }
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı veya Şifre Geçersiz!";
                return View();
            }
            
        }
        public ActionResult OturumKapat(string returnUrl)
        {
            Session.Abandon();
            return Redirect(returnUrl);
        }
        public ActionResult UyeOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UyeOl(UyeModel model)
        {
            if (db.Uye.Where(m=>m.KullaniciAdi==model.KullaniciAdi).Count()>0)
            {
                ViewBag.hata = "Girilen Kullanıcı Zaten Kayıtlıdır";
                return View();
            }
            Uye yeni = new Uye();
            if(model.Foto!=null && model.Foto.ContentLength>0)
            {
                string uzanti = Path.GetExtension(model.Foto.FileName).ToLower();
                string dosya = Guid.NewGuid().ToString();
                if (uzanti != ".jpeg" && uzanti != ".jpg" && uzanti != ".png") 
                {
                    ModelState.AddModelError("Foto", "Dosya uzantısı  JPG, JPEG veya PNG olmalıdır!");
                    return View(model);
                }
                string dosyaAdi = dosya + uzanti;
                model.Foto.SaveAs(Server.MapPath("~/Content/UyeFoto/" + dosyaAdi));

                yeni.AdSoyad = model.AdSoyad;
                yeni.Email = model.Email;
                yeni.KullaniciAdi = model.KullaniciAdi;
                yeni.Sifre = model.Sifre;
                yeni.Foto = dosyaAdi;
                db.Uye.Add(yeni);
                db.SaveChanges();
            }
            
            Uye uye = db.Uye.OrderByDescending(m => m.UyeId).FirstOrDefault();
            Session["uyeOturum"] = true;
            Session["uyeId"] = uye.UyeId;
            Session["uyeKadi"] = uye.KullaniciAdi;
            //if (db.Uye.Where(m => m.YetkiId == model.YetkiId).Count() > 0)
                Session["uyeAdmin"] = uye.YetkiId;
            Session["uyeFoto"] = uye.Foto;
            return RedirectToAction("Index");
        }
        public ActionResult UyeDetay(int id)
        {
            Uye uye = db.Uye.Where(s => s.UyeId == id).SingleOrDefault();
            if (uye==null)
            {
                 return RedirectToAction("Index");
            }
            return View(uye);
        }
        public ActionResult SonEklenenler()
        {
            List<Makale> makaleler = db.Makale.OrderByDescending(s=>s.MakaleId).Take(5).ToList();
            return PartialView(makaleler);
        }
        public ActionResult MakaleEkle()
        {
            MakaleModel model = getModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult MakaleEkle(MakaleModel model)
        {
            if (Session["uyeOturum"]==null)
            {
                RedirectToAction("OturumAc");
            }
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
        public ActionResult MakaleDuzenle(int ? id)
        {
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
            int uyeId = Convert.ToInt32(Session["uyeId"].ToString());
            Makale makale = db.Makale.Where(m => m.MakaleId == id && m.UyeId==uyeId).SingleOrDefault();
            if (makale == null)
            {
                return RedirectToAction("UyeDetay/"+uyeId);
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
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
            int uyeId = Convert.ToInt32(Session["uyeId"].ToString());
            Makale makale = db.Makale.Where(m => m.MakaleId == model.MakaleId).SingleOrDefault();
            if (makale == null)
            {
                return RedirectToAction("UyeDetay/"+uyeId);
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
            if (Session["uyeOturum"] == null)
            {
                RedirectToAction("OturumAc");
            }
            int uyeId = Convert.ToInt32(Session["uyeId"].ToString());

            Makale makale = db.Makale.Where(m => m.MakaleId == id && m.UyeId==uyeId).SingleOrDefault();
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
            return RedirectToAction("UyeDetay/" + uyeId);
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
        public ActionResult Duyurular()
        {
            List<Duyrular> Duyurular = db.Duyrular.ToList();
            return PartialView(Duyurular);
        }
        public ActionResult Hakkimizda()
        {
            List<Metin> metin = db.Metin.ToList();
            return View(metin);
        }
        public ActionResult Mesaj()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Mesaj(MesajModel model)
        {
            Mesajlar mesaj = new Mesajlar();
            mesaj.MesajBaslik = model.MesajBaslik;
            mesaj.MesajGonderen = model.MesajGonderen;
            mesaj.Mesajicerik = model.Mesajicerik;
            mesaj.MesajMail = model.MesajMail;
            db.Mesajlar.Add(mesaj);
            db.SaveChanges();
            ViewBag.sonuc = "Mesajınız başarıyla iletildi";
            return View();
        }
    }
}