using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalCom.Web.Controllers
{
    //tüm karşılaştırmalar listelenir. karşılaştırmalar şimdiye kadar kaç kere başlatılmış, kaç lab katılmış, 
    [Authorize]
    public class ArsivController : Controller
    {
        //
        // GET: /Arsiv/

        public ActionResult Arsiv()
        {
            return View();
        }

    }
}
