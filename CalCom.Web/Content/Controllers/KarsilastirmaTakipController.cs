using Calcom.Domain;
using Calcom.Domain.Entities;
using CalCom.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CalCom.Web.Controllers
{
    public class KarsilastirmaTakipController : Controller
    {
        //
        // GET: /KarsilastirmaTakip/

        public ActionResult Takip()
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

            //labın kayıtlı olduğu testleri alma
            Repository<Olcumler> OlcRep = new Repository<Olcumler>(db);
            List<Olcumler> olcumler = OlcRep.GetAll().Select(x => x).Where(y => y.LabId == labid && y.Testler.Durum=="Açık").ToList();
            
            //olcum aleti ismi, karsılastırma konusu, olcum id si, lerini liste şeklinde göstericez. -  viewModel oluşturulur
            TakipListeViewModel tvm = new TakipListeViewModel();
            foreach(var o in olcumler)
            { 
                tvm.Takipler.Add(new Takipci { KarKonu = o.Testler.Karsilastirmalar.Konu, OlcumAleti=o.Testler.Karsilastirmalar.OlcumAletleri.Isim, OlcId =o.Id, Durum=o.Durum });
            }
            return View(tvm);
        }

        public PartialViewResult TakipPartial(string olcId)
        {
            //takvim, kayıt bilgileri, ölçüm bilgileri
            int testid;
            CalComEntities db = new CalComEntities();
            Repository<Olcumler> OlcRep = new Repository<Olcumler>(db);
            int olcid = Int32.Parse(olcId);
            Olcumler olc = OlcRep.Get(olcid);
            testid = olc.TestId;
            Test t = new Test(testid);
            Takvim tak = t.TakvimOlustur();
            Karsilastirmalar kar = t.getTestler().Karsilastirmalar;
            TakipViewModel tvm;
            {
                int olcSayisi = 0;
                List<ParametreDegerleri> parDerler = kar.ParametreDegerleri.ToList();
                int[] olcSayilari = new int[parDerler.Count];
                int i = 0;
                foreach (var parDer in parDerler)
                {
                    olcSayilari[i] = parDer.OlcumNoktalari.Count;
                    olcSayisi = olcSayisi + olcSayilari[i];
                    i++;
                }
                 tvm = new TakipViewModel(new NestedInputValueViewModel(olcid, olcSayisi, olcSayilari));
                //HATA: Input value View model için ölçüm noktası verileri girilmesi gerekmektedir. Hata veriyor
                tvm.KarLabIsmi = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim;
                tvm.takvim = tak;
                tvm.kayitBilgileri = new KayitBilgileri { GIsim = olc.GeciciIsim, OlcumZamaniBas = (DateTime)olc.OlcumBaslangicZamani, OlcumZamaniBit = (DateTime)olc.OlcumSonZaman, TestBas = (DateTime)olc.Testler.BaslangicZamani, TestFiyat = (double)olc.Testler.Fiyat };
                if (olc.Durum == "VeriGiriş")
                {
                    tvm.olcumBilgileri.Basinc = (double)olc.Basinc;
                    tvm.olcumBilgileri.Nem = (double)olc.Nem;
                    tvm.olcumBilgileri.Sicaklik = (double)olc.Sicaklik;
                    int j = 0;
                    foreach(var parder in parDerler)
                         tvm.olcumBilgileri.Sonuclar.baslikPD.Add(parder.ParametreDegeri);
                    foreach (var OlcumSonc in olc.OlcumSonuclari)
                    {
                        //tvm.olcumBilgileri.Sonuclar.Add(OlcumSonc);
                       
                        tvm.olcumBilgileri.Sonuclar.olcumNoktasi.Add(OlcumSonc.OlcumNoktalari.OlcumNoktasi);
                        
                        ((NestedInputValueViewModel)tvm.olcumBilgileri.Sonuclar).sonuclar[j] = OlcumSonc;
                        j++;
                    }
                }
            }
            return PartialView(tvm);
        }
        //admin
        public ActionResult Yonet()
        {
            //açık olan test listesi gelir. Testi seçince kayıtlı olan tüm lablar listelenir. Yine birtanesinin ayrıntısına gitmek için buyutece tıklanır.
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            List<Testler> testler = TestRep.FindAll(x => x.Durum == "Açık").ToList();

            YonetViewModel tkv = new YonetViewModel();
            foreach (Testler t in testler)
            {
                tkv.Takipler.Add(new Yonetici { OlcumAleti=t.Karsilastirmalar.OlcumAletleri.Isim, OlcId=t.Id, ProtocolNo=t.ProtokolNo,  KatilanLab=t.Olcumler.Count,  KarKonu = t.Karsilastirmalar.Konu, Nerede=t.Karsilastirmalar.Tur, OlcumAraligiBas=t.Karsilastirmalar.OlcumAraligiBas, OlcumAraligiBit=t.Karsilastirmalar.OlcumAraligiSon });
            }

            return View(tkv);
        }
        //admin
        public PartialViewResult YonetPartial(string TestId)
        {
            CalComEntities db = new CalComEntities();
            Repository<Testler> TestRep = new Repository<Testler>(db);
            Dictionary<string, TakipViewModel> TestLabTakip = new Dictionary<string, TakipViewModel>();

            int testid = Int32.Parse(TestId);
            Testler testler = TestRep.Get(testid);

            TakipViewModel tvm ;
            foreach (var olc in testler.Olcumler)
            {
                Test t = new Test(testid);
                Takvim tak = t.TakvimOlustur();
                Karsilastirmalar kar = t.getTestler().Karsilastirmalar;
                int olcSayisi = 0;
                List<ParametreDegerleri> parDerler = kar.ParametreDegerleri.ToList();
                int[] olcSayilari = new int[parDerler.Count];
                int i = 0;
                foreach (var parDer in parDerler)
                {
                    olcSayilari[i] = parDer.OlcumNoktalari.Count;
                    olcSayisi = olcSayisi + olcSayilari[i];
                    i++;
                }
                tvm = new TakipViewModel(new NestedInputValueViewModel(olc.Id, olcSayisi, olcSayilari));

                tvm.KarLabIsmi = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim;
                tvm.takvim = tak;
                tvm.kayitBilgileri = new KayitBilgileri { GIsim = olc.GeciciIsim, OlcumZamaniBas = (DateTime)olc.OlcumBaslangicZamani, OlcumZamaniBit = (DateTime)olc.OlcumSonZaman, TestBas = (DateTime)olc.Testler.BaslangicZamani, TestFiyat = (double)olc.Testler.Fiyat };
                if (olc.Durum == "VeriGiriş")
                {
                    tvm.olcumBilgileri.Basinc = (double)olc.Basinc;
                    tvm.olcumBilgileri.Nem = (double)olc.Nem;
                    tvm.olcumBilgileri.Sicaklik = (double)olc.Sicaklik;
                    int j = 0;
                    foreach (var parder in parDerler)
                        tvm.olcumBilgileri.Sonuclar.baslikPD.Add(parder.ParametreDegeri);
                    foreach (var OlcumSonc in olc.OlcumSonuclari)
                    {
                        //tvm.olcumBilgileri.Sonuclar.Add(OlcumSonc);

                        tvm.olcumBilgileri.Sonuclar.olcumNoktasi.Add(OlcumSonc.OlcumNoktalari.OlcumNoktasi);

                        ((NestedInputValueViewModel)tvm.olcumBilgileri.Sonuclar).sonuclar[j] = OlcumSonc;
                        j++;
                    }
                }
                TestLabTakip.Add(olc.Laboratuvarlar.Isim, tvm);
            }
            return PartialView(TestLabTakip);
        }
        //admin
        public ActionResult Ayrinti(int Testid)
        { 
            // olcum aleti bilgileri, karşılaştırma bilgileri, test bilgileri, ölçüm bilgileri, test iptal etme, kayıtlı lab bilgileri, 
            Repository<Testler> TestRep = new Repository<Testler>( new  CalComEntities());
            Testler t = TestRep.Get(Testid);

            return View(t);
        }

    }
}
