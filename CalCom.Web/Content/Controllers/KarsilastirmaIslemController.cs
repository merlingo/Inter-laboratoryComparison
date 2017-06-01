using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calcom.Domain;
using CalCom.Web.Models;
using System.Data.SqlClient;
using System.Web.Security;
using System.Security.Principal;

namespace CalCom.Web.Controllers
{
    public class KarsilastirmaIslemController : Controller
    {
        public KarsilastirmaIslemController()
        {

        }
        public ActionResult Islemler()
        {
            return View();
        }
       
        //!! -- KARŞILAŞTIRMA BAŞLATMA MODÜLÜ --> !!
        public ActionResult BaslatSec()
        {
            //repository ile veritabanına bağlan
            Repository<Karsilastirmalar> karRep = new Repository<Karsilastirmalar>(new CalComEntities());

            var karlar = karRep.GetAll().Select(m => m).Where(kr => kr.Testler.Count == 0 ? kr.Testler.Count == 0 : (kr.Testler.Select(t => new { t.BaslangicZamani, t.Durum }).
                OrderBy(te => te.BaslangicZamani).First().Durum == "Kapali")).GroupBy(xc => xc.Konu);

            BaslatilacakTestListesi tkl = new BaslatilacakTestListesi();
           

            foreach (var kkonu in karlar)
            {
                TestKonu kn = new TestKonu();
                
                 string[] kler =   kkonu.Key.Split(' ');
                 kn.konu = kler[0];

                 for (int k = 1; k < kler.Length;k++ )
                     kn.konu = kn.konu + "_" + kler[k];
                foreach(var kar in kkonu)
                {
                    kn.basTestler.Add(new BaslatilacakTest { id = kar.Id, kacDefa = kar.Testler.Count, OA = kar.OlcumAletleri, olcumAraligi = new double[2] { kar.OlcumAraligiBas, kar.OlcumAraligiSon }, Tür = kar.Tur });
                }
                tkl.tkler.Add(kn);
            }
            tkl.count = karRep.GetAll().Count;
            return View(tkl);


        }
        
        public ActionResult Baslat(BaslatilacakTestListesi btl)
        {
            int im =0;
            CalComEntities db = new CalComEntities();
            //viewden seçilen testler btl ile alındı
            Repository<BaslatilacakTestler> basBekTestRep = new Repository<BaslatilacakTestler>(db);
            Repository<Karsilastirmalar> karRep = new Repository<Karsilastirmalar>(db);

            for (int i = 0; i < btl.tkler.Count; i++)
            {
                foreach (var kar in btl.tkler[i].basTestler)
                {

                    if (kar.secildiMi)
                    {
                        
                        object[] prt = {new SqlParameter(){ ParameterName="@karid", Value= kar.id }, new SqlParameter(){ ParameterName="@siraNo", Value=im}  } ;
                       basBekTestRep.Insert("Id",prt );
                        im++;
                    }
                }
            }
         
            BaslatilacakTestler bt = basBekTestRep.GetAll().Select(k => k).First();
           Karsilastirmalar karin = karRep.Get(bt.karid);
           bt.Karsilastirmalar = karin;
            BasBekleyenTestler bbt = new BasBekleyenTestler { karid=bt.karid, Karsilastirmalar = bt.Karsilastirmalar };
            BaslatViewModel bvm = new BaslatViewModel();
            bvm.bbt = bbt;
            basBekTestRep.Delete(bt);

            return View(bvm);
        }
        [HttpPost]
        public ActionResult Baslat(BaslatViewModel bvm)
        {
            CalComEntities db = new CalComEntities();
            //viewden seçilen testler btl ile alındı
            Repository<BasBekleyenTestler> basBekTestRep = new Repository<BasBekleyenTestler>(db);
            Repository<BaslatilacakTestler> baslatilacakTestRep = new Repository<BaslatilacakTestler>(db);

        
               // basBekTestRep.Add(bvm.bbt);
            object[] prt = { new SqlParameter() { ParameterName = "@karid", Value = bvm.bbt.karid },
                                   new SqlParameter() { ParameterName = "@baslangicZamani", Value = bvm.bbt.BaslangicZamani },
                               new SqlParameter() { ParameterName = "@BitisZamani", Value = bvm.bbt.BitisZamani },
                             new SqlParameter() { ParameterName = "@OlcumZamani", Value = bvm.bbt.OlcumZamani },
                             new SqlParameter() { ParameterName = "@OlcumBitisZamani", Value = bvm.bbt.OlcumBitisZamani },
                             new SqlParameter() { ParameterName = "@Fiyat", Value = bvm.bbt.Fiyat },
                         //      new SqlParameter() { ParameterName = "@ProtokolNo", Value = bvm.bbt.ProtokolNo },
                               new SqlParameter() { ParameterName = "@OlcumSuresi", Value = bvm.bbt.OlcumSuresi },
                                                                new SqlParameter() { ParameterName = "@HesaplamaYontemi", Value = bvm.bbt.HesaplamaYontemi },
                                 new SqlParameter() { ParameterName = "@GerekenLabSayisi", Value = bvm.bbt.GerekenLabSayisi },
                               };
                basBekTestRep.Insert( "Id",prt);
                BaslatilacakTestler bt = baslatilacakTestRep.GetAll().Select(k => k).FirstOrDefault();
                if (bt != null)
                {
                    BasBekleyenTestler bbt = new BasBekleyenTestler { karid = bt.karid, Karsilastirmalar = bt.Karsilastirmalar };
                   foreach(var bbtr in basBekTestRep.GetAll())
                   {
                        bvm.QueOncekiTest.Enqueue(new OncekiTest() { id=bbtr.Id , isim=bbtr.Karsilastirmalar.OlcumAletleri.Isim});
                     
                   }
                    bvm.bbt = bbt;
                    baslatilacakTestRep.Delete(bt);
                    return View(bvm);
                }
                else
                    return RedirectToAction("KesinBaslat");
        }
        public ActionResult KesinBaslat()
        {
            Repository<BasBekleyenTestler> basBekRep = new Repository<BasBekleyenTestler>(new CalComEntities());
            KesinBaslatViewModel kbvm = new KesinBaslatViewModel();   
            foreach(var bkr in basBekRep.GetAll())
            {
                kbvm.ozetler.Add(new BaslatOzet() { isim=bkr.Karsilastirmalar.OlcumAletleri.Isim, baslangicZaman = bkr.BaslangicZamani, bitisZaman=bkr.BitisZamani, fiyat=bkr.Fiyat, gerekenLabSayisi=bkr.GerekenLabSayisi, olcumSuresi=bkr.OlcumSuresi  });
            }
            //tüm KarBekleyenTestler alınır, sayfada gösterilir ve hepsi birden kesinleştirilir.
            return View(kbvm);
        }
        public ActionResult BaslatBilgilendirme()
        { 
            CalComEntities db= new CalComEntities();
            Repository<BasBekleyenTestler> basBekRep = new Repository<BasBekleyenTestler>(db);
            Repository<Testler> TestRep = new Repository<Testler>(db);
            int[] result = new int[basBekRep.GetAll().Count];
            Test test;
            foreach (var bkr in basBekRep.GetAll())
            {
                test = new Test(bkr);
                test.testBaslat();
                basBekRep.Delete(bkr);
            }


            return View();

        }
        //!! <-- END OF BAŞLATMA MODÜLÜ -- !!


        //!! -- KARŞILAŞTIRMA EKLEME MODÜLÜ --> !!
        public ActionResult KarsilastirmaEkle()
        {

            return View();
        }
        
        public ActionResult OABilgileri()
        {
            Repository<OlcumAletleri> OARep = new Repository<OlcumAletleri>(new CalComEntities());
            List<OlcumAletleri> OAlar =  OARep.GetAll().ToList();
            OlcumAletiViewModel oavm = new OlcumAletiViewModel();
            oavm.OAlar = OAlar;
            
            return View(oavm);
        }
        public PartialViewResult OAPartial(int oaid)
        {
            Repository<OlcumAletleri> OARep = new Repository<OlcumAletleri>(new CalComEntities());

            return PartialView( OARep.Get(oaid) );
        }

        public ActionResult KarsilastirmaBilgileri(int oaid)
        {
            CalComEntities db = new CalComEntities();
           
            Repository<Karsilastirmalar> KarRep = new Repository<Karsilastirmalar>(db);
            KarRep.Insert("Id",new object[] { new SqlParameter() { ParameterName = "@OAId", Value = oaid }, new SqlParameter() { ParameterName = "@Konu", Value = "" }, new SqlParameter() { ParameterName = "@OlcumAraligiBas", Value = -1 }, new SqlParameter() { ParameterName = "@OlcumAraligiSon", Value = -1 } });
            Karsilastirmalar kar = KarRep.GetAll().Select(x => x).Where(z => z.OAId == oaid && z.Konu == "").First();
            return View(kar);
        }
        [HttpPost]
        public ActionResult KarsilastirmaBilgileri(OlcumAletleri oa)
        {
            CalComEntities db = new CalComEntities();
            Repository<OlcumAletleri> OARep = new Repository<OlcumAletleri>(db);
            int oaid = OARep.Add(oa).Id;
            Repository<Karsilastirmalar> KarRep = new Repository<Karsilastirmalar>(db);
          int karid = KarRep.Add( new Karsilastirmalar{ OAId=oaid, Konu="", OlcumAraligiBas=-1, OlcumAraligiSon=-1  }).Id;
              //KarRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@OAId", Value = oaid }, new SqlParameter() { ParameterName = "@Konu", Value = "" }, new SqlParameter() { ParameterName = "@OlcumAraligiBas", Value = -1 }, new SqlParameter() { ParameterName = "@OlcumAraligiSon", Value = -1 } });
            Karsilastirmalar kar = KarRep.Get(karid);
            return View(kar);
        }
        [HttpGet]
        public ActionResult ÖlçümDeğerleri(Karsilastirmalar kar)
        {
            CalComEntities db = new CalComEntities();
            Repository<Karsilastirmalar> KarRep = new Repository<Karsilastirmalar>(db);
          Karsilastirmalar kr =  KarRep.Update(kar, kar.Id);
          OlcumDegerleriViewModel odvm = new OlcumDegerleriViewModel();
            //düzenli mi düzensiz mi olduğunu dropdownbox ile seçsin, seçime göre partial açılsın. partial da karşılaştırma veriKarmasasi nı
            //dropdown daki değer yapsın.
          odvm.karid = kar.Id;
          List<SelectListItem> dropDownList = new List<SelectListItem>();
          dropDownList.Add(new SelectListItem { Text = "Düzenli - Kare", Value = "Kare" });
            dropDownList.Add(new SelectListItem { Text="Düzensiz - İçiçe", Value="Icice" });
            odvm.dropDownList = dropDownList;
            return View(odvm);
        }
      
        [HttpPost]
        public void KarisikParDegerOzet(ParametreDegerleri par, List<OlcumNoktalari> olcler)
        {
            //HATA: formdan buraya veriler geldikten sonra sayfa yenileniyor. 2 çözüm: ya yine aynı sayfayı döndürcez ActionResult olarak ki parça parça çalışan parametre değeri forumları sıkıntı çıkarır
            //, ya da ajax form kullanarak partial döndürecez.
            CalComEntities db = new CalComEntities();
            Repository<OlcumNoktalari> olcRep = new Repository<OlcumNoktalari>(db);
            Repository<ParametreDegerleri> parRep = new Repository<ParametreDegerleri>(db);
            foreach (var olc in olcler)
            {
                olcRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@ParDerId", Value = olc.ParDerId }, new SqlParameter() { ParameterName = "@OlcumNoktasi", Value = olc.OlcumNoktasi } });
            }
               parRep.Update(par, par.ParDerId);
            
            //return PartialView();
        }
        [HttpPost]
        public ActionResult EkleBilgilendirme_karisik(List<OlcumNoktalari> olcler, List<ParametreDegerleri> par)
        {
            //ekranda değişecek şey var ney --> hidden lar eklenmeli HATALI HATA-->LİSTEYE TÜM OLÇUMLER ALINAMIYOR. HER PARAMETRE İÇİN AYRI KAYDETME OLMALI, KAYIT OLAN YENİ PARTİAL AÇMALI ÖZET EKRANI İÇİNDE
            //HEPSİ BİTİNCE DE İLERLE YE BASILARAK KARŞILAŞTIRMA EKLENMELİ.
            CalComEntities db = new CalComEntities();
            Repository<OlcumNoktalari> olcRep = new Repository<OlcumNoktalari>(db);
            Repository<ParametreDegerleri> parRep = new Repository<ParametreDegerleri>(db);
            foreach (var olc in olcler)
            {
                olcRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@ParDerId", Value = olc.ParDerId }, new SqlParameter() { ParameterName = "@OlcumNoktasi", Value = olc.OlcumNoktasi } });
            }
            foreach (var p in par)
            {
                parRep.Update(p,p.ParDerId);
            }
            return View("EkleBilgilendirme");
        }
        public PartialViewResult ParDegerPartial(int sayi, int karid)
        {
            ParDegerler pd = new ParDegerler();
            for(int i=0;i<sayi;i++)
            {
                pd.olcListesi.Add(new ParametreDegerleri { karid = karid });
            }
            return PartialView(pd);
        }
        public PartialViewResult ParDegerPartial_Karisik(int sayi, int karid)
        {
            CalComEntities db = new CalComEntities();
            ParDegerler pd = new ParDegerler();
            Repository<ParametreDegerleri> parRep = new Repository<ParametreDegerleri>(db);

            for (int i = 0; i < sayi;i++ )
            {
                //parRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@karid", Value = karid }, new SqlParameter() { ParameterName = "@ParametreDegeri", Value = "xxx" } });
               pd.olcListesi.Add(  parRep.Add(new ParametreDegerleri { karid = karid }) );
            }
          
            return PartialView(pd);
        }
        public PartialViewResult OlcNoktaPartial_Karisik(int sayi, int parDerid, int sonOlcumNo)
        {
            //yeni OlcNoktalar model view ı eklenmeli
            OlcNoktalari pd = new OlcNoktalari();
            pd.toplamOlc = sonOlcumNo;
            for (int i = 0; i < sayi; i++)
            {
                pd.olcListesi.Add(new OlcumNoktalari { ParDerId = parDerid });
            }
            return PartialView(pd);
        }
        //public PartialViewResult OlcNoktaPartial(int sayi, int karid)
        //{
        //    OlcNoktalar pd = new OlcNoktalar();
        //    for (int i = 0; i < sayi; i++)
        //    {
        //        pd.olcListesi.Add(new OlcumNoktasi { karid = karid });
        //    }
        //    return PartialView(pd);
        //}
        // !! <--  KARŞILAŞTIRMA EKLEME SONU -- !!


        //!! -- TEST KAYIT MODÜLÜ --> !!
       
        public ActionResult TestSeç()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity id = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(id, roles);
            string labname = authTicket.Name;
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(new CalComEntities());
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            //durumu açık olan, labın kayıt yaptırmadığı(ölçüm tablosunda açık olan testin id si ve labın idsinde bir veri yoksa) testler alınır ve view'e gönderilir.
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            List<Testler> Testler = TestRep.GetAll().Select(z => z).Where(t => t.Durum == "Açık").Select(m => m).Where(k => k.Olcumler.Select(o => o).Where(Olc => Olc.LabId == labid).FirstOrDefault() == null).ToList<Testler>();
            
            return View(Testler);
        }
        public ActionResult TestKayit(int Id)
        {
            //id'si alınan test için takvim oluşturulur, bilgileri ve takvim gösterilir
            Test test = new Test(Id);
            Takvim tak = test.TakvimOlustur();
            return View(tak);

        }
        public PartialViewResult TarihSecimOnay(string seciliTarih, string id)
        {
            string[] dts = seciliTarih.Split('-');
            DateTime dt = new DateTime(Int32.Parse(dts[0]),Int32.Parse(dts[1]),Int32.Parse(dts[2]));
            int testId = Int32.Parse(id);
            Test t = new Test(testId);
           // t.testKayit();
            KayitOnayViewModel kovm = new KayitOnayViewModel();
            
            int olcumSuresi = (int)(t.getTestler().OlcumSuresi == null ? 2 : t.getTestler().OlcumSuresi);
            //olcum süresinin tam ortasına kaydı koymalı - 2'ye böl integer bolmesi, bas ve son olarak. 
            int bas = olcumSuresi / 2;
            //dt = new DateTime(dt.Year,dt.Month,dt.Day-bas);
            dt = dt.Subtract(new TimeSpan(1, 0, 0, 0));
            kovm.dBas = dt;
            for (int i = 0; i < olcumSuresi; i++)
            {
                //hafta sonlarını almama
                dt =  dt.AddDays(1);
                i=(dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday) ? i-1 : i;
            }
            kovm.dBit = dt;
            kovm.TestName = t.getTestler().Karsilastirmalar.OlcumAletleri.Isim;
            kovm.TestId = id;
            return PartialView(kovm);
        }
        //kayıt takvimi üztünde basılan tarihteki olayı gösterir
        public PartialViewResult KayitOlayGoster()
        {
            //olay türü, test id veya olcum lab GIsim i alır, o olay hakkındaki ayrıntılı bilgileri verir.
            //bu partial sayfada test bilgileri gösterilir; başlama tarihi, bilgileri, kayıt olan lablar ve ölçüm gün aralığı, karşılaştırma bilgileri
           
            return PartialView();
        }

        [HttpPost]
        public ActionResult Kaydet(int testId, DateTime dBas)
        {
            //id si alınan test için testKayıt çalıştırılır ve kullanıcıya Geçici Name ve Şifresi gösterilir. "test kaydınız başarıyla yapılmıştır. Seçtiğiniz tarihler arasında ölçüm aleti size gönderilecektir."

            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity id = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(id, roles);
            string labname = authTicket.Name;
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(new CalComEntities());
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            Test t = new Test(testId);
           Gecici g = t.testKayit(labid,dBas);


            return View(g);
        }
        // !! <-- TEST KAYIT SONU -- !!



        //!! -- ÖLÇÜM VERİ GİRİŞİ MODÜLÜ --> !!
        public ActionResult OlcumKarsilastirmalari()
        {
            //Laboratuvar için ölçüm aşamasında olan, kayıt olduğu karşılaştırmaları gördüğü ekran. Liste şeklinde tüm kayıtlı olduğu karşılaştırmalar gelir; ölçüm zamanı gelen karşılaştırma seçilebilir.

            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            CalComEntities db = new CalComEntities();
            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            FormsIdentity id = new FormsIdentity(authTicket);
            GenericPrincipal principal = new GenericPrincipal(id, roles);
            string labname = authTicket.Name;
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            List<Olcumler> olcumler = labRep.Get(labid).Olcumler.Select(y=>y).Where(z=>z.Durum=="Kayit").ToList<Olcumler>();

            OlcumKarsilastirmalari vgvm = new OlcumKarsilastirmalari();
           
            foreach(Olcumler olc in olcumler)
            {
                vgvm.oklar.Add(new OlcumKar() {  olcumAdi = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim, testid = olc.Id, uygunMu = (olc.OlcumBaslangicZamani < DateTime.Today && DateTime.Today < olc.OlcumSonZaman) });
            }

            return View(vgvm);
        }
        [HttpGet]
        public ActionResult SifreGir(int olcId)
        { Repository<Olcumler> olcRep = new Repository<Olcumler>(new CalComEntities());
           
            Olcumler olc= olcRep.Get(olcId);
            SifreGirViewModel sgvm = new SifreGirViewModel();
            sgvm.bb =new Gecici { gname=olc.GeciciIsim, gsifre=olc.GeciciSifre };
            return View(sgvm);

        }

        [HttpPost]
        public ActionResult SifreGir(int olcId,string id, string pass)
        {
            Repository<Olcumler> olcRep = new Repository<Olcumler>(new CalComEntities());
            Olcumler olc = olcRep.Get(olcId);
            OlcumAletleri oa = olc.Testler.Karsilastirmalar.OlcumAletleri;
            OlcumAletiBilgileriViewModel oabvm = new OlcumAletiBilgileriViewModel(olcId);
            oabvm.oa = oa;
                return View("OlcumAletiBilgileri",oabvm);
        }

        public ActionResult OlcumAletiBilgileri(bool a)
        {
            return View();
        }

        public ActionResult OrtamDegerleri(string olcId)
        {
            
            OrtamDegerleri od = new OrtamDegerleri();
            return View(od);
        }
        [HttpPost]
        public ActionResult ReferansCihaz(OrtamDegerleri od)
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


            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            Repository<Olcumler> olcRep = new Repository<Olcumler>(db);

            Olcumler olc = olcRep.Get(od.olcId);
            olc.Basinc = od.basinc;
            olc.Nem = od.nem;
            olc.Sicaklik = od.sicaklik;
            olcRep.Update(olc,od.olcId);
            //od u kaydet rcvm oluştur.
            
            //rcvm oluştur view e gönderecek
            ReferansCihazViewModel rcvm= new ReferansCihazViewModel();
            rcvm.olcId = od.olcId;
            rcvm.OAlar = labRep.Get(labid).LabinReferansCihazlari.Select(x => x.ReferansCihazlar).ToList();
            return View(rcvm);
        }
        public PartialViewResult RCPartial(int oaid)
        {
            Repository<ReferansCihazlar> refRep = new Repository<ReferansCihazlar>(new CalComEntities());
            ReferansCihazlar rc = refRep.Get(oaid);
            return PartialView(rc);
        }
        //Eğer rc seçilirse
        [HttpGet]
        public ActionResult VeriGiris(int oaid, int olcId)
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


            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            int labid = ((Laboratuvarlar)labRep.Find(l => l.Isim == labname)).Id;
            
            Repository<LabinReferansCihazlari> lrcRep = new Repository<LabinReferansCihazlari>(db);
            LabinReferansCihazlari lrc = lrcRep.Find(x=>(x.RefId == oaid && x.LabId ==labid));
            Repository<OlcumdekiReferansCihaz> OlcRep = new Repository<OlcumdekiReferansCihaz>(db);
            
            
        int orcid=  OlcRep.Add(new OlcumdekiReferansCihaz { LrcId = lrc.Id, OlcId = olcId }).Id;
              InputValueViewModel ivvm;
              Repository<OlcumSonuclari> OlcSonRep = new Repository<OlcumSonuclari>(db);
           
            //karşılaştırma tablosundan; olcum noktaları ve parametre degerlerini alıp viewe tablo oluşturulacak --> her parametre degeri icin olcum noktaları alınacak; bu alınan veriler olcumSonuç tablosuna eklenecek
            //ya da her par ve olc degeri için bir OlcumSonuc veriri oluştur ve onun listesini ver
            Repository<Olcumler> OlcumRep = new Repository<Olcumler>(db);
            Karsilastirmalar kar = OlcumRep.Get(olcId).Testler.Karsilastirmalar;
            //if (kar.ÖlçümVeriTipi.Equals("Duzenli"))
            //{
            //    ivvm = new SquareInputValueViewModel(olcId, kar.ParametreDegerleri.Count, kar.OlcumNoktasi.Count);
            //    foreach (ParametreDegerleri pd in kar.ParametreDegerleri)
            //    {
            //        ivvm.baslikPD.Add(pd.ParametreDegeri);
            //    }
            //    foreach (OlcumNoktasi on in kar.OlcumNoktasi)
            //    {
            //        ivvm.olcumNoktasi.Add(on.OlcumNoktasi1);
            //    }
            //    int i = 0, j = 0;
            //    foreach (ParametreDegerleri pd in kar.ParametreDegerleri)
            //    {
            //        j = 0;
            //        foreach (OlcumNoktasi on in kar.OlcumNoktasi)
            //        {
            //            ((SquareInputValueViewModel)ivvm).sonuclar[i, j] = new OlcumSonuc { OlcumId = olcId, OlcumNoktasiId = on.NoktaId, ParDerId = pd.ParDerId, Tarih = DateTime.Today };
            //            j++;
            //        }
            //        i++;

            //    }
            //}
            //else //if( kar.ÖlçümVeriTipi.Equals(Düzensiz))
            {
                int olcSayisi=0;
                List<ParametreDegerleri> parDerler = kar.ParametreDegerleri.ToList();
                int[] olcSayilari = new int[parDerler.Count]; // her parametre için farklı sayıda ölçüm noktası
                int i =0;
                foreach(var parDer in parDerler)
                {
                    olcSayilari[i] = parDer.OlcumNoktalari.Count;
                    olcSayisi = olcSayisi + olcSayilari[i];
                    i++;
                }
                //OlcSonRep.FindAll(x=>x.OlcumNoktalari.ParDerId==kar.ParametreDegerleri.s
                ivvm = new NestedInputValueViewModel(olcId,olcSayisi,olcSayilari);
                i=0;
                foreach (var parDer in parDerler)
                {
                    ivvm.baslikPD.Add(parDer.ParametreDegeri);
                    foreach(var on in parDer.OlcumNoktalari){
                        ((NestedInputValueViewModel)ivvm).sonuclar[i] = new OlcumSonuclari { OlcumNoktalariId = on.Id, OlcumId = olcId, Tarih = DateTime.Today };
                    i++;
                    ivvm.olcumNoktasi.Add(on.OlcumNoktasi);
                    }
                }
            }
            return View(ivvm);
        }
        //eğer yeni rc eklenirse
        [HttpPost]
        public ActionResult VeriGiris(ReferansCihazViewModel oa)
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
        
            int refid = RefRep.Add(new ReferansCihazlar { Konu=oa.rc.Konu, Imalatci=oa.rc.Imalatci, SeriNo=oa.rc.SeriNo, Sinif=oa.rc.Sinif }).Id;
          //ref cihazı laba baglama
            Repository<LabinReferansCihazlari> lrcRep = new Repository<LabinReferansCihazlari>(db);
            int lrcid = lrcRep.Add( new LabinReferansCihazlari{ LabId=labid, RefId=refid }).Id;
           
            //ref cihazı olcume baglama
            Repository<OlcumdekiReferansCihaz> OlcRep = new Repository<OlcumdekiReferansCihaz>(db);

            int orcid = OlcRep.Add(new OlcumdekiReferansCihaz { LrcId=lrcid, OlcId=oa.olcId }).Id;
          
            //karşılaştırma tablosundan; olcum noktaları ve parametre degerlerini alıp viewe tablo oluşturulacak --> her parametre degeri icin olcum noktaları alınacak; bu alınan veriler olcumSonuç tablosuna eklenecek
            //ya da her par ve olc degeri için bir OlcumSonuc veriri oluştur ve onun listesini ver
            Repository<Olcumler> or = new Repository<Olcumler>(db);
            int ti = or.Get(oa.olcId).Testler.Id;
            Repository<Testler> tr = new Repository<Testler>(db);
            Karsilastirmalar kar= tr.Get(ti).Karsilastirmalar;

            InputValueViewModel ivvm;
        //    if (kar.ÖlçümVeriTipi.Equals("Duzenli"))
        //    {
        //     ivvm = new SquareInputValueViewModel(oa.olcId, kar.ParametreDegerleri.Count, kar.OlcumNoktasi.Count);
        //    foreach(ParametreDegerleri pd in kar.ParametreDegerleri){
        //        ivvm.baslikPD.Add(pd.ParametreDegeri);
        //    }
        //    foreach (OlcumNoktasi on in kar.OlcumNoktasi)
        //    {
        //        ivvm.olcumNoktasi.Add(on.OlcumNoktasi1);
        //    }
        //    int i=0,j=0;
        //    foreach (ParametreDegerleri pd in kar.ParametreDegerleri)
        //    {
 
        //        foreach (OlcumNoktasi on in kar.OlcumNoktasi)
        //        {
        //          ((SquareInputValueViewModel) ivvm).sonuclar[i,j] = new OlcumSonuc { OlcumId=oa.olcId, OlcumNoktasiId = on.NoktaId, ParDerId = pd.ParDerId, Tarih = DateTime.Today };
        //           j++;
        //        }
        //        j = 0;
        //        i++;

        //    }
        //}
        //      else //if( kar.ÖlçümVeriTipi.Equals(Düzensiz))
            {
                int olcSayisi=0;
                List<ParametreDegerleri> parDerler = kar.ParametreDegerleri.ToList();
                int[] olcSayilari = new int[parDerler.Count];
                int i =0;
                foreach(var parDer in parDerler)
                {
                    olcSayilari[i] = parDer.OlcumNoktalari.Count;
                    olcSayisi = olcSayisi + olcSayilari[i];
                    i++;
                }
                ivvm = new NestedInputValueViewModel(oa.olcId, olcSayisi, olcSayilari);
                i=0;
                foreach (var parDer in parDerler)
                {
                    ivvm.baslikPD.Add(parDer.ParametreDegeri);
                    foreach (var on in parDer.OlcumNoktalari)
                    {
                        ((NestedInputValueViewModel)ivvm).sonuclar[i] = new OlcumSonuclari { OlcumNoktalariId = on.Id, OlcumId = oa.olcId, Tarih = DateTime.Today };
                        i++;
                        ivvm.olcumNoktasi.Add(on.OlcumNoktasi);
                    }
                }
            }
            
            return View(ivvm);
            //sonuçlandırılırken de ParametreDegerleri tablosuna 1 e 1 bağlı bir Sonuç tablosuna admin den alınan her değer
            //kaydedilir. Sonuç verileri sistem tarafından da hesaplanabilir.
        }
        [HttpPost]
        public ActionResult OlcumBilgilen(OlcumSonuclari[] sonuclar,int olcId)
        {
            CalComEntities db = new CalComEntities();
            Repository<OlcumSonuclari> OSRep = new Repository<OlcumSonuclari>(db);

            foreach (OlcumSonuclari sonuc in sonuclar)
            {
                
                OSRep.Insert("Id", new object[] {
                                     new SqlParameter() { ParameterName = "@OlcumNoktalariId", Value = sonuc.OlcumNoktalariId },
                 new SqlParameter() { ParameterName = "@OlcumId", Value = sonuc.OlcumId },
                  new SqlParameter() { ParameterName = "@Tarih", Value = sonuc.Tarih },
                new SqlParameter() { ParameterName = "@Deger", Value = sonuc.Deger } });
                
            }
            Repository<Olcumler> OlcRep = new Repository<Olcumler>(db);
            Olcumler olc = OlcRep.Get(olcId);

            olc.Durum = "VeriGiriş";
            OlcRep.Update(olc, olc.Id);
            ViewBag.Bilgi = "Ölçüm değerleri başarı ile kaydedilmiştir. En kısa zamanda sonuçlar açıklanacaktır";
            return View();
        }
        //[HttpPost]
        //public ActionResult OlcumBilgilenDuz(OlcumSonuc[] sonuclar, int olcId)
        //{ 
        //    CalComEntities db = new CalComEntities();
        //    Repository<OlcumSonuc> OSRep = new Repository<OlcumSonuc>(db);

        //    foreach (OlcumSonuc sonuc in sonuclar)
        //    {
                
        //        OSRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@OlcumNoktasiId", Value = sonuc.OlcumNoktasiId },
        //         new SqlParameter() { ParameterName = "@OlcumId", Value = sonuc.OlcumId },
        //          new SqlParameter() { ParameterName = "@Tarih", Value = DateTime.Today },
        //          new SqlParameter() { ParameterName = "@ParDerId", Value = sonuc.ParDerId},

        //        new SqlParameter() { ParameterName = "@Deger", Value = sonuc.Deger } });
                
        //    }
        //    Repository<Olcumler> OlcRep = new Repository<Olcumler>(db);
        //    Olcumler olc = OlcRep.Get(olcId);

        //    olc.Durum = "VeriGiriş";
        //    OlcRep.Update(olc, olc.Id);
        //    ViewBag.Bilgi = "Ölçüm değerleri başarı ile kaydedilmiştir. En kısa zamanda sonuçlar açıklanacaktır";
        //    return View();
        //}
        //<-- !! VERİ GİRİŞ SON <-- !!


        //!! -- ÖLÇÜM SONUÇLANDIRMA MODÜLÜ --> !!

        public ActionResult SonucListesi()
        {
            // Durumu VeriGiriş olan ölçümlerin; test ve laboratuvar listesi olacaktır. Kullanıcı birtanesini seçer. Testler olur seçince lablar ortaya çıkar XXXX
            //yeni --> devam eden tüm karşılaştırmalar listelenir. Listede durumları kayıt ve olcüm olan laboratuvar sayısı gösterilir. eğer kayıt 0 ise rengi belli eder ve
            // sonuçlandırmaya basılabilir. her test için Gerçek Değer butonu olur ve basılması durumunda gercek degeri atanır. Eger gercek degeri atanmamış ise sonuclandırma
            // ya basınca bunun uyarısı verilir ve gerçek degerin atanmasını ister. gerçek deger aynı ekranda partial olarak atanır ya da yeni ekran açılabilir.
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            SonucTestListesi stl = new SonucTestListesi();
            SonucTest st;
            foreach (Testler t in TestRep.GetAll())
            {
                //Count u 0 olanlar seçilmez
                st = new SonucTest
                {
                    KarIsim = t.Karsilastirmalar.OlcumAletleri.Isim,
                    Konu = t.Karsilastirmalar.Konu,
                    VGLabSayisi = t.Olcumler.Select(x => x).Where(y => y.Durum == "VeriGiriş").Count(),
                    KytLabSayisi = t.Olcumler.Select(x => x).Where(y => y.Durum == "Kayit").Count(),
                    testId = t.Id
                };
                st.KesinDeger= t.KesinDegerler.Count()>0?"Atandı":"Atanmadı";
                st.KesinDegerDurum = t.KesinDegerler.Count() > 0;
                stl.STler.Add(st);
            }
            
            return View(stl);
        }
        [HttpPost]
        public ActionResult SonucListesi(KesinDegerAtamaViewModel kdavm) //--> !!!!! KDVAM DOGRU ALINMIYOR, DICTIONARY İ LİSTE YAP VE LİSTE İLE ALMA YONTEMİNİ KULLAN !!!!
        {
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            Repository<KesinDegerler> KesDesRep = new Repository<KesinDegerler>(new CalComEntities());
            SonucTestListesi stl = new SonucTestListesi();
            SonucTest st;
            //kdavm değerleri veritabanına kayıt edilir --> varsa güncelleme    yoksa yeni ekleme.
            foreach(var kd in kdavm.Atamalar)
                if(kdavm.atamaDurum)
                    KesDesRep.Update(new KesinDegerler { Belirsizlik = kd.Belirsizlik, Deger = kd.KesinDeger, Testid = kdavm.testid, Yontem = kdavm.yontem, OlcumNoktalariId = kd.Onid, Id= (int)kd.id },(int)kd.id);
                else
            KesDesRep.Add(new KesinDegerler { Belirsizlik=kd.Belirsizlik, Deger=kd.KesinDeger, Testid=kdavm.testid, Yontem=kdavm.yontem, OlcumNoktalariId=kd.Onid });
            foreach (Testler t in TestRep.GetAll())
            {
                //Count u 0 olanlar seçilmez
                st = new SonucTest
                {
                    KarIsim = t.Karsilastirmalar.OlcumAletleri.Isim,
                    Konu = t.Karsilastirmalar.Konu,
                    VGLabSayisi = t.Olcumler.Select(x => x).Where(y => y.Durum == "VeriGiriş").Count(),
                    KytLabSayisi = t.Olcumler.Select(x => x).Where(y => y.Durum == "Kayit").Count(),
                    testId = t.Id
                };
                st.KesinDeger = t.KesinDegerler.Count() > 0 ? "Atandı" : "Atanmadı";
                st.KesinDegerDurum = t.KesinDegerler.Count() > 0;
                stl.STler.Add(st);
            }

            return View(stl);
        }
        public ActionResult KesinDegerGuncellemePartial(string testid)
        {
            //testin olcum noktalarini ve parderlerini getirir, herbiri için değer ve belirsizlik girilmesi beklenir, ya da seçilen yönteme göre diğer labların ortalamaları alınır ama kullanıcın istediği labların
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            int testId = Int32.Parse(testid);
            Karsilastirmalar kar = TestRep.Get(testId).Karsilastirmalar;

            KesinDegerAtamaViewModel kdavm = new KesinDegerAtamaViewModel();
            kdavm.testid = testId;
            //karın her parderi için onler modele eklenir
            foreach (var pd in kar.ParametreDegerleri)
            {
                foreach (var on in pd.OlcumNoktalari)
                    kdavm.Atamalar.Add(new KesinDegerAtamalari { Onid = on.Id, OlcumNoktasi = on.OlcumNoktasi, ParametreDegeri = pd.ParametreDegeri, Belirsizlik = on.KesinDegerler.Select(x => x).Where(y => y.Testid == testId).Single().Belirsizlik, KesinDeger = on.KesinDegerler.Select(x => x).Where(y => y.Testid == testId).Single().Deger, id = on.KesinDegerler.Select(x => x).Where(y => y.Testid == testId).Single().Id });
            }
            kdavm.atamaDurum = true; //DÜZELTİLMELİ --> kesin deger atanmamış ise null pointer exception verecek. Yukarıdaki 'add' satırını ikiye böl
            return PartialView(kdavm);
        }
        public ActionResult KesinDegerAtamaPartial(string testid)
        {
            //testin olcum noktalarini ve parderlerini getirir, herbiri için değer ve belirsizlik girilmesi beklenir, ya da seçilen yönteme göre diğer labların ortalamaları alınır ama kullanıcın istediği labların
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            int testId = Int32.Parse(testid);
            Karsilastirmalar kar = TestRep.Get(testId).Karsilastirmalar;

            KesinDegerAtamaViewModel kdavm = new KesinDegerAtamaViewModel();
            kdavm.testid = testId;
            //karın her parderi için onler modele eklenir
            foreach(var pd in kar.ParametreDegerleri){
                foreach(var on in pd.OlcumNoktalari)
                    kdavm.Atamalar.Add(new KesinDegerAtamalari { Onid = on.Id, OlcumNoktasi = on.OlcumNoktasi, ParametreDegeri = pd.ParametreDegeri});
            }
            kdavm.atamaDurum = false; //DÜZELTİLMELİ --> kesin deger atanmamış ise null pointer exception verecek. Yukarıdaki 'add' satırını ikiye böl
            return PartialView(kdavm);
        }
        public ActionResult KesinDegerGostermePartial(string testid)
        {
            //testin olcum noktalarini ve parderlerini getirir, herbiri için değer ve belirsizlik girilmesi beklenir, ya da seçilen yönteme göre diğer labların ortalamaları alınır ama kullanıcın istediği labların
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            int testId = Int32.Parse(testid);
            Karsilastirmalar kar = TestRep.Get(testId).Karsilastirmalar;

            KesinDegerAtamaViewModel kdavm = new KesinDegerAtamaViewModel();
            kdavm.testid = testId;
            KesinDegerler kd = TestRep.Get(testId).KesinDegerler.Select(x => x).FirstOrDefault();
            if (kd != null)
            {
                kdavm.yontem = kd.Yontem;
                //karın her parderi için onler modele eklenir
                foreach (var pd in kar.ParametreDegerleri)
                {
                    foreach (var on in pd.OlcumNoktalari)
                        kdavm.Atamalar.Add(new KesinDegerAtamalari { Onid = on.Id, OlcumNoktasi = on.OlcumNoktasi, ParametreDegeri = pd.ParametreDegeri, Belirsizlik = on.KesinDegerler.Select(x => x).Where(y => y.Testid == testId).Single().Belirsizlik, KesinDeger = on.KesinDegerler.Select(x => x).Where(y => y.Testid == testId).Single().Deger });
                }
            }
            else
                kdavm.yontem = "seçilmedi";
           
            return PartialView(kdavm);
        }
        public ActionResult OlcumSonucları(int testid)
        {
            //Her parametre değeri için tablo getirilir. Tablonun başlıkları ölç. noktaları olur rowlarda lab verileri bulunur, en üst
            //kesin değer olur. Her lab için performans score hesaplanır. Yönetici hesaplama yöntemini seçecektir.
            CalComEntities db = new CalComEntities();
            OlcumSonuclariViewModel osvm = new OlcumSonuclariViewModel();
            //ViewModel --> testin tüm ölçümleri, parametre degerleri, ölçüm noktaları, kesin degerler
            Repository<Testler> TestRep = new Repository<Testler>(db);
            Testler test =TestRep.Get(testid);
            osvm.ParDerler = test.Karsilastirmalar.ParametreDegerleri.ToList();

            bool alık = false;
            osvm.TestId = testid;
            //Bu teste dahil olan lablar
            OlcumSonuclariViewModel.LabOlcumu lo; 
            foreach (var olc in test.Olcumler)
            {
                osvm.Lablar.Add(olc.Laboratuvarlar.Isim);
                foreach(var on in olc.OlcumSonuclari){
                    lo = new OlcumSonuclariViewModel.LabOlcumu(); lo.Olcid = olc.Id;
                    if (on.PerformanceScores != null)
                    {

                        alık = true; break;
                    }
                    lo.Osid = on.Id;
                    osvm.Olcumler.Add(lo);

                }
            }
            if (alık)
            {  foreach (var olc in test.Olcumler)
            
                foreach (var os in olc.OlcumSonuclari)
                    osvm.Olcumler.Add(new OlcumSonuclariViewModel.LabOlcumu { Olcid = olc.Id, Osid = os.Id, PerformanceScore = (double)os.PerformanceScores.Select(x => x).Single().Deger });
                return View("HesaplamaSonuclari", osvm);
            }
               return View(osvm);
        }

      //HATA: Bu yöntem ile olmuyor çünkü her ölçüm sonucu için hesaplama yapılmalı ama tabloda labın tüm ölçüm sonuçları için hesaplama isteniyor
      //DÜZELTME: Yeni action ile çözülmeli, form sonucu hesaplama isimli action'a gelecek, orada hesaplama yapılacak ve her sonuç için skor gösterilcek
        //
        [HttpPost]
        public ActionResult HesaplamaSonuclari(OlcumSonuclariViewModel osvm)
        {
            CalComEntities db = new CalComEntities();
          //PErfScoreları kaydet, aynı tabloyu döndür, her tablo için grafik görünümü hazırla
            //Eğer yöntem z-score ise, grafikte z-score değeri gösterilir, yöntem en değeri ise grafikte ölçüm sonucu gösterilir.
            //Her ölçüm noktası için tablo oluşturulur, herbiri için başlık--> "ParDer" - "KarAdı" - "ÖlçümNoktası" Ölçüm Sonuçları
            //Her tablo için altına grafik gösterilmelidir.
            Repository<PerformanceScores> PerSkor = new Repository<PerformanceScores>(db);
            foreach(var ps in osvm.Olcumler){
            PerSkor.Add(new PerformanceScores { Deger=ps.PerformanceScore, OlcumSonuclariId=ps.Osid, Yontem=osvm.yontem });
            }

            
            Repository<Testler> TestRep = new Repository<Testler>(db);
            Testler test = TestRep.Get(osvm.TestId);
            osvm.ParDerler = test.Karsilastirmalar.ParametreDegerleri.ToList();

            //Bu teste dahil olan lablar
            OlcumSonuclariViewModel.LabOlcumu lo;
            foreach (var olc in test.Olcumler)
            {
                osvm.Lablar.Add(olc.Laboratuvarlar.Isim);
                foreach (var on in olc.OlcumSonuclari)
                {
                    lo = new OlcumSonuclariViewModel.LabOlcumu(); lo.Olcid = olc.Id;
                    lo.Osid = on.Id;
                    osvm.Olcumler.Add(lo);

                }
            }
                return View(osvm);
        }

       
        public ActionResult Yayınla(int TestId)
        {
            //tablo ve grafikler laboratuvarlara mail ve sistem üzerinden gönderilir.
            return View();
        }


        //<-- !! ÖLÇÜM SONUÇLANDIRMA SON <-- !!
    }
}
