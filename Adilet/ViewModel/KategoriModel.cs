using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Adilet.ViewModel
{
    public class KategoriModel
    {
        public int KategoriId { get; set; }
        [Required(ErrorMessage="Kategori giriniz")]
        [Display(Name = "Kategori")]
        [StringLength(50,ErrorMessage = "Kategori Adı en fazla 50 karakter olmalı!")]

        public string KategoriAdi { get; set; }
    }
}