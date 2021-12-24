using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopSite.Auth
{
    public class BaseController:Controller
    {
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (Session["uyeOturum"] == null)
            {
                  filterContext.Result = new RedirectResult("~/Home/OturumAc");
            }
            else
            {
                if (Session["uyeAdmin"].ToString() != "1" || Session["uyeAdmin"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Home/OturumAc");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}