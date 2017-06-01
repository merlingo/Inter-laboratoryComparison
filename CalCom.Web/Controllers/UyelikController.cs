using Calcom.Domain;
using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CalCom.Web.Controllers
{
     [Authorize]
    public class UyelikController : Controller
    {
        //
        // GET: /Uyelik/

        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Giris(string labismi, string sifre, bool chk_Rememberme)
        {

            if (Membership.ValidateUser(labismi, sifre))
            {

                FormsAuthenticationTicket tkt;
                string cookiestr;
                HttpCookie ck;
                tkt = new FormsAuthenticationTicket(1, labismi, DateTime.Now,
                    DateTime.Now.AddYears(1), chk_Rememberme, labismi);
                cookiestr = FormsAuthentication.Encrypt(tkt);
                ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);

                if (chk_Rememberme)
                {
                    ck.Expires = tkt.Expiration;
                    ck.Path = FormsAuthentication.FormsCookiePath;
                }
                Response.Cookies.Add(ck);
                return RedirectToAction("Anasayfa", "Genel");
            }
            else
            {
                ViewBag.a = "boyle bir kullanici yok";
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult UyeOl()
        {
            string ip = GetIPAddress();
            if(ip.Equals(Session["user_ip"]))
            {
                 ViewBag.a = "Birden fazla kayıt yapılamaz. Lütfen kaydınızın onaylanma bilgisi için mailinizi kontrol ediniz.";
            return View("Giris");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UyeOl(string lab_isim,string yonetici, string sifre, string calısma_alani, string vergiDairesiNo, string telefon, string faks, string gsm, string mail, string sehir, string bolge, string adres)
        {
            KayitBekleyenLab lab = new KayitBekleyenLab { Isim = lab_isim, CalismaAlani = calısma_alani, Faks = Convert.ToInt64(faks), Gsm = Convert.ToInt64(gsm), Sifre = sifre, Telefon = Convert.ToInt64(telefon), VergiDairesiNo = Convert.ToInt32(vergiDairesiNo), email = mail, Sehir = sehir, Bolge = bolge, Adres = adres, Yonetici = yonetici };
            SendMail(mail);
            Repository<KayitBekleyenLab> labRep = new Repository<KayitBekleyenLab>(new CalComEntities());
            labRep.Add(lab);
            //nullable olanları da kontrol edecek miyiz?
            string ip = GetIPAddress();
            Session["user_ip"] = ip;
            ViewBag.a = "Kaydınız onaylanmıştır. Sifreniz ile sisteme giriş yapabilirsiniz.";
            return View("Giris");
        }
        public ActionResult KayitBekleyenLablar()
        {
            return View();
        }
        public PartialViewResult KayitOnayla()
        {
            return PartialView();
        }
         
        public ActionResult Cıkıs()
        {
            FormsAuthentication.SignOut();
            return View("Giris");
        }
        public static void SendMail(  string alanAdres)
        {
           MailMessage mail = new MailMessage("mrt.narr@gmail.com", alanAdres);
           SmtpClient client = new SmtpClient();
           client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
           client.DeliveryMethod = SmtpDeliveryMethod.Network;
           client.UseDefaultCredentials = false;
          
           client.Credentials = new System.Net.NetworkCredential("mrt.narr@gmail.com", "123456gjj");
           mail.Subject = "Kayıt";
           mail.Body = "Merhabalar \n sistem kaydınız alınmıştır. Yönetici tarafından onaylandıktan sonra yine mail atılacaktır. Bu bir otomatik mesajdır. Lütfen cevap yazmayınız. Teşekkürler";
           client.Send(mail);
        }
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
