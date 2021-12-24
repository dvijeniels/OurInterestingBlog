using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adilet.ViewModel
{
    public class MakaleModel
    {
        public int MakaleId { get; set; }
        [Required(ErrorMessage = "Başlık giriniz")]
        [Display(Name = "Başlık")]
        [StringLength(50, ErrorMessage = "Başlık Adı en fazla 50 karakter olmalı!")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "İçerik giriniz")]
        [Display(Name = "İçerik")]
        [AllowHtml]
        public string Icerik { get; set; }

        [Required(ErrorMessage = "Resim seçiniz")]
        [Display(Name = "Resim")]
        public HttpPostedFileBase Foto { get; set; }

        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Kategori seçiniz")]
        [Display(Name = "Kategori")]
        public int KategoriId { get; set; }

        public List<SelectListItem> KategoriListe { get; set; }
        public int UyeId { get; set; }
        public int Okunma { get; set; }
    }
}