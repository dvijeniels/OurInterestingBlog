using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adilet.ViewModel
{
    public class MesajModel
    {
        public int MesajId { get; set; }
        [Required(ErrorMessage = "Adınızı ve Soyadınızı giriniz")]
        [Display(Name = "Ad ve Soyad")]
        [StringLength(50, ErrorMessage = "Adınız ve Soyadınız en fazla 50 karakter olmalı!")]
        public string MesajGonderen { get; set; }

        [Required(ErrorMessage = "Konu giriniz")]
        [Display(Name = "Konu")]
        [StringLength(50, ErrorMessage = "Konu Adı en fazla 50 karakter olmalı!")]
        public string MesajBaslik { get; set; }

        [Required(ErrorMessage = "Email giriniz")]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Email en fazla 50 karakter olmalı!")]
        public string MesajMail { get; set; }

        [Required(ErrorMessage = "İçerik giriniz")]
        [Display(Name = "İçerik")]
        [AllowHtml]
        public string Mesajicerik { get; set; }
    }
}