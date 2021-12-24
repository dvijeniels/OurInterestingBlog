using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adilet.ViewModel
{
    public class DuyuruModel
    {
        public int DuyuruId { get; set; }

        [Required(ErrorMessage = "Duyuru başlığı giriniz")]
        [Display(Name = "Başlık")]
        [StringLength(100, ErrorMessage = "Kategori Adı en fazla 100 karakter olmalı!")]
        public string DuyuruAd { get; set; }

        [Required(ErrorMessage = "Duyuru İçerik giriniz")]
        [Display(Name = "İçerik")]
        [AllowHtml]
        public string Duyuruicerik { get; set; }

        [Required(ErrorMessage = "Resim seçiniz")]
        [Display(Name = "Resim")]
        public HttpPostedFileBase DuyuruResim { get; set; }
    }
}