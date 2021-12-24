using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adilet.ViewModel
{
    public class UyeModel
    {
        public int UyeId { get; set; }
        [Required(ErrorMessage="Kullanıcı Adınızı Giriniz")]
        [Display(Name="Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessage="E-Mail Giriniz")]
        [Display(Name="E-Mail Adresi")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifrenizi Giriniz")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
        [Required(ErrorMessage = "Adınızı ve Soyadınızı Giriniz")]
        [Display(Name = "Adınız ve Soyadınız")]
        public string AdSoyad { get; set; }
        [Required(ErrorMessage = "Foto Seçiniz")]
        [Display(Name = "Fotoğraf")]
        public HttpPostedFileBase Foto { get; set; }
        public Nullable<int> YetkiId { get; set; }
    }
}