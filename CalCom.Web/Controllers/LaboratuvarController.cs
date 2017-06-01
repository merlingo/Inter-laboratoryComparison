using Calcom.Domain;
using Calcom.Domain.Entities;
using CalCom.Web.Models.Laboratuvar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CalCom.Web.Controllers
{ 
    //Laboratuvar - Takvim Gereksinimleri:
    // 1- Takvimde atanmış tarih seçildiginde one ekran gelecek arka kararacak
    // 2- Takvim görünümünden liste görünümüne geçilebilecek
    // 3- Takvim türkçe olacak
    // 4- Başlıklar düzenlenecek
    [Authorize]
    public class LaboratuvarController : Controller
    {
        //
        // GET: /Laboratuvar/
        public ActionResult Index()
        {

            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Laboratuvarlar lab = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname));
            LaboratuvarViewModel lvm = new LaboratuvarViewModel();
            lvm.lab = lab;
            List<TestDate> tds = new List<TestDate>();
            foreach (var k in lab.Olcumler)
            {
                tds.Add(new TestDate { kisaIsim = "BAS", Tarih=(DateTime)k.Testler.BaslangicZamani , Type="baslangic"});
                tds.Add(new TestDate { kisaIsim = "BIT", Tarih = (DateTime)k.Testler.BitisZamani, Type = "bitis" });
              
            }
            lvm.takvim = new Takvim(tds.ToArray());
            foreach (var k in lab.Olcumler)
            {
                lvm.takvim.OlcumZamanlari.Add(new OlcumDate { basTarih = (DateTime)k.OlcumBaslangicZamani, bitTarih = (DateTime)k.OlcumSonZaman, GLabName = k.GeciciIsim, LabId = lab.Id, OlcumId = k.Id });
            }
            if (lab.LabinReferansCihazlari.Count != 0)
            {
                foreach (var lrc in lab.LabinReferansCihazlari)
                {
                    lvm.refler.Add(lrc.ReferansCihazlar);
                }
            }
            return View(lvm);
        }

        public ActionResult RCEklePartial()
        {
            return PartialView(new ReferansCihazlar());
        }
        [HttpPost]
        public ActionResult RCEkle(ReferansCihazlar rc)
        {
            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            //yeni ref cihaz ekleme
            Repository<ReferansCihazlar> RefRep = new Repository<ReferansCihazlar>(db);

            int refid = RefRep.Add(new ReferansCihazlar { Konu = rc.Konu, Imalatci = rc.Imalatci, SeriNo = rc.SeriNo, Sinif = rc.Sinif }).Id;
            //ref cihazı laba baglama
            Repository<LabinReferansCihazlari> lrcRep = new Repository<LabinReferansCihazlari>(db);
            int lrcid = lrcRep.Add(new LabinReferansCihazlari { LabId = labid, RefId = refid }).Id;
           
            return RedirectToAction("Index");
        }
        public ActionResult ProfilGuncelle()
        {

            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Laboratuvarlar lab = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname));

            return View(lab);
        }
        [HttpPost]
        public ActionResult ProfilGuncelle(Laboratuvarlar lab)
        {
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(new CalComEntities());
            labRep.Update(lab, lab.Id);
            return RedirectToAction("Index");
        }
       
        public ActionResult ReferansCihazListesi()
        {
            return View();
        }
        public ActionResult ReferansCihazEkle()
        {
            return View();
        }
        public ActionResult ReferansCihazSil()
        {
            return View();
        }
        public ActionResult ReferansCihazGüncelle()
        {
            return View();
        }
        public ActionResult RefCihazSilUyari()
        {
            return View();
        }
        public ActionResult LabListesi()
        {
            return View();
        }
        public ActionResult LabArama()
        {
            return View();
        }


        //Admin
        public ActionResult Takip()
        {
            Repository<Laboratuvarlar> LabRep = new Repository<Laboratuvarlar>(new CalComEntities());
            LabTakipViewModel ltvm = new LabTakipViewModel();
            foreach(var l in LabRep.GetAll())
            {
                ltvm.Lablar.Add(new LabOzet { CalismaAlani=l.CalismaAlani, LabAdi=l.Isim, Sehir=l.Sehir, Yonetici=l.Yonetici, LabId=l.Id });

            }

            return View(ltvm);
        }

        public ActionResult OlcumPartial(string Labid)
        {
            // seçilen laboratuvarın dahil olduğu tüm testleri listeleme --> test isimleri, başlama tarihi ve labın ölçüm tarihi
            LabinTestleri lt = new LabinTestleri();
            Repository<Olcumler> OlcRep = new Repository<Olcumler>(new CalComEntities());
            int lbid = Int32.Parse(Labid);
            foreach(Olcumler olc in OlcRep.FindAll(x=>x.LabId== lbid)){
                lt.Testler.Add(new LabTest { TestId = olc.TestId, TestIsmi = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim, BaslamaTarihi = (DateTime)olc.Testler.BaslangicZamani, OlcumTarihi = (DateTime)olc.OlcumBaslangicZamani });
            }

            return PartialView(lt);
        }

        //Admin 
        public ActionResult ProfilAdmin(int LabId)
        {
            Repository<Laboratuvarlar> LabRep = new Repository<Laboratuvarlar>(new CalComEntities());
            Laboratuvarlar lab = LabRep.Get(LabId);
            LaboratuvarViewModel lvm = new LaboratuvarViewModel();
            lvm.lab = lab;
            List<TestDate> tds = new List<TestDate>();
            foreach (var k in lab.Olcumler)
            {
                tds.Add(new TestDate { kisaIsim = "BAS", Tarih = (DateTime)k.Testler.BaslangicZamani, Type = "baslangic" });
                tds.Add(new TestDate { kisaIsim = "BIT", Tarih = (DateTime)k.Testler.BitisZamani, Type = "bitis" });

            }
            lvm.takvim = new Takvim(tds.ToArray());
            foreach (var k in lab.Olcumler)
            {
                lvm.takvim.OlcumZamanlari.Add(new OlcumDate { basTarih = (DateTime)k.OlcumBaslangicZamani, bitTarih = (DateTime)k.OlcumSonZaman, GLabName = k.GeciciIsim, LabId = lab.Id, OlcumId = k.Id });
            }
            foreach (var lrc in lab.LabinReferansCihazlari)
            {
                lvm.refler.Add(lrc.ReferansCihazlar);
            }
            return View(lvm);
           
        }
        //Admin
        public ActionResult SistemKullanimi()
        {
            return View();
        }

        public ActionResult MesajKutusu()
        {
            MesajKutusuViewModel mkvm = new MesajKutusuViewModel();
            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Laboratuvarlar lab = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname));
            mkvm.Mesajlar =(lab.Mesajlar.ToList());
            return View(mkvm);
        }
        public ActionResult MesajPartial(string mId)
        {
            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Laboratuvarlar lab = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname));
            string mesaj = lab.Mesajlar.Single(x => x.Id == Int32.Parse(mId)).Mesaj;
            return PartialView(model:mesaj);
        }
        public ActionResult MesajAt()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MesajAt(string title,string content)
        {
            CalComEntities db = new CalComEntities();
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity did = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(did, roles);
            string labname = authTicket.Name; //name olarak id'yi ver ki veritabanından id ile bulalım

            //lab idi yi alma
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Laboratuvarlar lab = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname));

            Repository<Mesajlar> MesRep = new Repository<Mesajlar>(db);
            MesRep.Insert("Id",new object[] { new SqlParameter() { ParameterName = "@LabId", Value = lab.Id },
                 new SqlParameter() { ParameterName = "@Konu", Value = title },
                  new SqlParameter() { ParameterName = "@Tur", Value = true },
                new SqlParameter() { ParameterName = "@MesajTarihi", Value = DateTime.Today },
                new SqlParameter() { ParameterName = "@Mesaj", Value = content }
            });
            ViewBag.Uyarı = "Mesajınız Gönderilmiştir.";
            return View();
        }
        public ActionResult AdminMesaj()
        {
            Repository<Mesajlar> MesRep = new Repository<Mesajlar>(new CalComEntities());
            MesajKutusuViewModel mkvm = new MesajKutusuViewModel();
            mkvm.Mesajlar = MesRep.GetAll().ToList();
            return View(mkvm);
        }
        public ActionResult AdminMesajPartial(string mId)
        {
            CalComEntities db = new CalComEntities();
            Repository<Mesajlar> MesRep = new Repository<Mesajlar>(db);
            string mesaj = MesRep.Get(Int32.Parse(mId)).Mesaj;

            return PartialView(model: mesaj);
        }
     
    }
}
