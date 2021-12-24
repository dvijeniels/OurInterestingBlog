using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adilet.ViewModel
{
    public class HakkimizdaModel
    {
        public int MetinId { get; set; }

        [Required(ErrorMessage = "İçerik giriniz")]
        [Display(Name = "İçerik")]
        [AllowHtml]
        public string MetinIcerik { get; set; }

        [Required(ErrorMessage = "Resim seçiniz")]
        [Display(Name = "Resim")]
        public HttpPostedFileBase Resim { get; set; }
    }
}