using Calcom.Domain;
using Calcom.Domain.Entities;
using CalCom.Web.Models;
using CalCom.Web.Models.AdminPanel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalCom.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        //
        // GET: /AdminPanel/

        public ActionResult Index()
        {
            return View();
        }
        // !--> URETIM MODUL BASLANGIC 
        public ActionResult Uretim()
        {
            CalComEntities db = new CalComEntities();
            Repository<OlcumAletleri> olcRep = new Repository<OlcumAletleri>(db);
            UretimViewModel oauvm = new UretimViewModel();
            oauvm.OlcumAletiListesi = new List<OlcumAletiListeVerisi>();
            foreach (var oa in olcRep.GetAll().ToList())
            {
                oauvm.OlcumAletiListesi.Add(new OlcumAletiListeVerisi { oaId = oa.Id, Isim = oa.Isim, Model = oa.Model, SeriNo = oa.SeriNo, KarsilastirmaSayisi = oa.Karsilastirmalar.Count, EklenmeTarihi = oa.EklenmeTarihi == null ? new DateTime() : (DateTime)oa.EklenmeTarihi });
            }
            Repository<Karsilastirmalar> kalRep = new Repository<Karsilastirmalar>(db);
            foreach (var oa in kalRep.GetAll().ToList())
            {
                oauvm.KarList.Add(new KarsilastirmaUretimi { Id = oa.Id, OlcumAleti = oa.OlcumAletleri.Isim, Konu = oa.Konu, OlcumAraligib = oa.OlcumAraligiBas, OlcumAraligis = oa.OlcumAraligiSon, AcilanTestSayisi = oa.Testler.Count, EklenmeTarihi = oa.EklenmeTarihi == null ? new DateTime() : (DateTime)oa.EklenmeTarihi, inputSayisi = oa.InputSayisi == null ? -1 : (int)oa.InputSayisi, RehberDokuman = oa.RehberDöküman });
            }
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Repository<KayitBekleyenLab> kLabRep = new Repository<KayitBekleyenLab>(db);
            LaboratuvarKayitlariViewModel lkvm = new LaboratuvarKayitlariViewModel();

            foreach (var lab in labRep.GetAll())
            {
                oauvm.LabList.Add(new LaboratuvarKayitListeElemanı { Id = lab.Id, Isim = lab.Isim, Yonetici = lab.Yonetici, CalismaAlani = lab.CalismaAlani, Tel = lab.Telefon == null ? -1 : (long)lab.Telefon, Mail = lab.email, AmblemPath = "~/App_Data/Amblemler/" + lab.Isim + ".jpg", KayitliMi = true });
            }
            foreach (var lab in kLabRep.GetAll())
            {
                oauvm.LabList.Add(new LaboratuvarKayitListeElemanı { Id = lab.Id, Isim = lab.Isim, Yonetici = lab.Yonetici, CalismaAlani = lab.CalismaAlani, Tel = lab.Telefon == null ? -1 : (long)lab.Telefon, Mail = lab.email, AmblemPath = "~/App_Data/Amblemler/" + lab.Isim + ".jpg", KayitliMi = false });
            }
            return View(oauvm);
        }
        public ActionResult OlcumAletiUretimi()
        {
            //ölçüm aleti listesi verilir ve seçilen ölçüm aleti için ayrıntılı bilgiler ve istatistiksel bilgiler gönderilir
            Repository<OlcumAletleri> olcRep = new Repository<OlcumAletleri>(new CalComEntities());
            OlcumAletiUretimiViewModel oauvm = new OlcumAletiUretimiViewModel();
            oauvm.OlcumAletiListesi = new List<OlcumAletiListeVerisi>();
            List<OlcumAletleri> listO = olcRep.GetAll().ToList();
            foreach (var oa in listO)
            {
                oauvm.OlcumAletiListesi.Add(new OlcumAletiListeVerisi { oaId=oa.Id, Isim=oa.Isim, Model=oa.Model, SeriNo=oa.SeriNo, KarsilastirmaSayisi=oa.Karsilastirmalar.Count,EklenmeTarihi=oa.EklenmeTarihi==null?new DateTime():(DateTime)oa.EklenmeTarihi });
            }
            return View(oauvm);
        }
        public PartialViewResult OlcumAletiBilgileri(int oaid)
        {
            //oaid si verilen olcum aletinin tüm bilgileri ve istatistiksel bilgileri döndürülecek
            Repository<OlcumAletleri> olcRep = new Repository<OlcumAletleri>(new CalComEntities());
            OlcumAletiBilgileri oab = new OlcumAletiBilgileri();
            oab.oa = olcRep.Get(oaid);
            return PartialView(oab);
        }

        public ActionResult KarsilastirmaUretimi()
        {
            Repository<Karsilastirmalar> olcRep = new Repository<Karsilastirmalar>(new CalComEntities());
            KarsilastirmaUretimiViewModel oauvm = new KarsilastirmaUretimiViewModel();
            List<Karsilastirmalar> listO = olcRep.GetAll().ToList();
            foreach (var oa in listO)
            {
                oauvm.KarList.Add(new KarsilastirmaUretimi {Id=oa.Id, OlcumAleti = oa.OlcumAletleri.Isim, Konu=oa.Konu, OlcumAraligib=oa.OlcumAraligiBas, OlcumAraligis=oa.OlcumAraligiSon, AcilanTestSayisi=oa.Testler.Count, EklenmeTarihi=oa.EklenmeTarihi==null?new DateTime():(DateTime)oa.EklenmeTarihi, inputSayisi=oa.InputSayisi==null?-1:(int)oa.InputSayisi, RehberDokuman=oa.RehberDöküman  });
            }
            return View(oauvm);
        }
        public PartialViewResult KarsilastirmaBilgileri(int karid)
        {
            //ayrıntılı karşılaştırma bilgileri -->karşılaştırma objesi - Karşılaştırma ölçüm bilgileri --> parametredeğerleri * ölçümnoktaları 
            Repository<Karsilastirmalar> olcRep = new Repository<Karsilastirmalar>(new CalComEntities());
            KarsilastirmaBilgileri kb = new KarsilastirmaBilgileri();
            kb.Kar = olcRep.Get(karid);
            return PartialView(kb);
        }
        public ActionResult LaboratuvarKayitlari()
        {
            //liste-->lab adı - Yönetici - Çalışma Alanı - Telefon - Mail - Amblem - Kayıt Onay Durumu
            CalComEntities db = new CalComEntities();
            Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
            Repository<KayitBekleyenLab> kLabRep = new Repository<KayitBekleyenLab>(db);
            LaboratuvarKayitlariViewModel lkvm = new LaboratuvarKayitlariViewModel();

            foreach (var lab in labRep.GetAll())
            {
                lkvm.LabList.Add(new LaboratuvarKayitListeElemanı { Id=lab.Id, Isim=lab.Isim, Yonetici=lab.Yonetici, CalismaAlani=lab.CalismaAlani, Tel=lab.Telefon==null?-1:(long)lab.Telefon, Mail=lab.email, AmblemPath="~/App_Data/Amblemler/"+lab.Isim+".jpg", KayitliMi=true});
            }
            foreach (var lab in kLabRep.GetAll())
            {
                lkvm.LabList.Add(new LaboratuvarKayitListeElemanı { Id = lab.Id, Isim = lab.Isim, Yonetici = lab.Yonetici, CalismaAlani = lab.CalismaAlani, Tel = lab.Telefon == null ? -1 : (long)lab.Telefon, Mail = lab.email, AmblemPath = "~/App_Data/Amblemler/" + lab.Isim + ".jpg", KayitliMi = false });
            }
            return View(lkvm);
        }
        public PartialViewResult LaboratuvarBilgileri(int id, bool k)
        {
            CalComEntities db = new CalComEntities();
            LaboratuvarBilgileri lb;
            if (k)
            {
                Repository<Laboratuvarlar> labRep = new Repository<Laboratuvarlar>(db);
                lb = new LaboratuvarBilgileri();
                lb.setLab(labRep.Get(id));
            }
            else
            {
                Repository<KayitBekleyenLab> labRep = new Repository<KayitBekleyenLab>(db);
                lb = new LaboratuvarBilgileri();
                lb.setLab(labRep.Get(id));
            }

            return PartialView(lb);
        }
        public ActionResult OlcumAletiEkle()
        {

            return PartialView(new OlcumAletleri());
        }
        [HttpPost]
        public ActionResult OlcumAletiEkle(OlcumAletleri oa)
        {
            Repository<OlcumAletleri> karRep = new Repository<OlcumAletleri>(new CalComEntities());
            karRep.Add(oa);
            return RedirectToAction("OlcumAletiUretimi");
        }

        public ActionResult KarsilastirmaYarat()
        {
            
            return View(new Karsilastirmalar());
        }

        //          !!!!!!EKSİK TAMAMLANMALI   -- ekranlarda process template düzeltilmeli ve oraya gönderilecek model düzenlenmeli            ! !!!!
        [HttpPost]
        public ActionResult KarsilastirmaYarat(Karsilastirmalar kar)
        {
            Repository<Karsilastirmalar> karRep = new Repository<Karsilastirmalar>(new CalComEntities());
            karRep.Add(kar);
            return RedirectToAction("KarsilastirmaUretimi");
        }

        public ActionResult LabKayitOnayi()
        {
            //Kayit Bekleyen labların listesi gönderilir
            Repository<KayitBekleyenLab> Kblrep = new Repository<KayitBekleyenLab>(new CalComEntities());
            LaboratuvarKayitlariViewModel lkvm = new LaboratuvarKayitlariViewModel();
            foreach (KayitBekleyenLab lab in Kblrep.GetAll())
            {
                lkvm.LabList.Add(new LaboratuvarKayitListeElemanı { Id = lab.Id, Isim = lab.Isim, Yonetici = lab.Yonetici, CalismaAlani = lab.CalismaAlani, Tel = lab.Telefon == null ? -1 : (long)lab.Telefon, Mail = lab.email, AmblemPath = "~/App_Data/Amblemler/" + lab.Isim + ".jpg", KayitTarihi=lab.KayitTarihi==null?new DateTime():(DateTime)lab.KayitTarihi });
            }
            return PartialView(lkvm);
        }

        //          !!!!!!EKSİK TAMAMLANMALI               ! !!!!
        [HttpPost]
        public ActionResult LabKayitOnayi(LaboratuvarKayitlariViewModel lab)
        {
            //Laboratuvar tablosuna eklenir ve labın mail adresine mail gönderilir
            return RedirectToAction("LaboratuvarKayitlari");
        }
        //<--! URETIM MODUL SON



        // !--> BASLANGIC MODUL BASI --> test yeni başlatıldığında durumu başlangıç olur, içerisinde hiç kayıt durumunda olcum yoksa durumu kalibrasyon, içerisinde hiç verigiriş durum yoksa testin durumu karşılaştırma ve raporlar oluşturulup gönderildiğinde arşiv olur.
        public ActionResult Baslangic()
        {
            //yeni başlatılan karşılaştırmalar ve kayıt olmuş ama onaylanmamış laboratuvarları en fazla olandan aza doğru 30 tane ama sayfalandırma olacak şekilde, ve labtestviewmodel verilecek.
            BaslangicViewModel bvm = new BaslangicViewModel();
            CalComEntities db = new CalComEntities();
            Repository<Olcumler> olcRep = new Repository<Olcumler>(db);
            Repository<Testler> testRep = new Repository<Testler>(db);
            foreach (var olc in olcRep.GetAll())
            {
                bvm.lbvm.labTest.Add(new LabTest { id = olc.Id, labid = olc.LabId, testid = olc.TestId, kar = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim, lab = olc.Laboratuvarlar.Isim, olcumTarihB = olc.OlcumBaslangicZamani == null ? new DateTime() : (DateTime)olc.OlcumBaslangicZamani, olcumTarihS = olc.OlcumSonZaman == null ? new DateTime() : (DateTime)olc.OlcumSonZaman });
                if (!bvm.lbvm.LabList.Exists(x => x.id == olc.LabId)) bvm.lbvm.LabList.Add(new Oge { id = olc.LabId, isim = olc.Laboratuvarlar.Isim });
                if (!bvm.lbvm.TestList.Exists(x => x.id == olc.TestId)) bvm.lbvm.TestList.Add(new Oge { id = olc.TestId, isim = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim });
            }
            foreach (var tst in testRep.GetAll().Select(x=>x).Where(y=>y.Durum.Equals("Baslangic")).OrderBy(t=>t.Olcumler.Count))
            {
                bvm.testler.Add(new BaslayanTest { TestId=tst.Id, Konu=tst.Karsilastirmalar.Konu, KayitliLab=tst.Olcumler.Count, OlcumAleti=tst.Karsilastirmalar.OlcumAletleri.Isim, YeniKayitLab=tst.Olcumler.Select(x=>x.Durum).Where(y=>y.Equals("Kayit")).Count() });
            }
            return View(bvm);
        }
        public ActionResult TestTakip()
        {
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            List<Testler> testList =TestRep.GetAll().ToList();
            return View(testList);
        }
        public ActionResult TestTakipBilgileri(int id)
        {
            Repository<Testler> TestRep = new Repository<Testler>(new CalComEntities());
            Testler t= TestRep.Get(id);
            return PartialView(t);
        }
      
        public ActionResult LabTestAnalizi()
        {
            //lab test matrixi gösterilir --> yukarıda test ismi aşağıda lab gösterilir, hücrelerde ise ölçüm tarihi. Eğer hücre seçilirse lab ve test bilgileri gelir, lab seçilirse lab ve test listesi vice versa...
            CalComEntities db = new CalComEntities();
            Repository<Olcumler> olcRep = new Repository<Olcumler>(db);
            LabTestViewModel lbvm = new LabTestViewModel();
            foreach (var olc in olcRep.GetAll())
            {
                lbvm.labTest.Add(new LabTest {id=olc.Id, labid = olc.LabId, testid = olc.TestId, kar = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim, lab = olc.Laboratuvarlar.Isim, olcumTarihB = olc.OlcumBaslangicZamani == null ? new DateTime() : (DateTime)olc.OlcumBaslangicZamani, olcumTarihS = olc.OlcumSonZaman == null ? new DateTime() : (DateTime)olc.OlcumSonZaman });
                if (!lbvm.LabList.Exists(x => x.id == olc.LabId)) lbvm.LabList.Add(new Oge { id = olc.LabId, isim = olc.Laboratuvarlar.Isim });
                if (!lbvm.TestList.Exists(x => x.id == olc.TestId)) lbvm.TestList.Add(new Oge { id = olc.TestId, isim = olc.Testler.Karsilastirmalar.OlcumAletleri.Isim });
            }
            return View(lbvm);
        }
        public ActionResult ProtokolEkle()
        {
            return PartialView();
        }
        public ActionResult KarsilastirmaBaslat()
        {
            
            //Eğer BaslatilacakTestler ve BasBekleyenTest yoksa boyle olur varsa BaslatilacakKarsilastirmalar açilir
            CalComEntities db = new CalComEntities();
            Repository<BasBekleyenTestler> BasBekRep = new Repository<BasBekleyenTestler>(db);
            if (BasBekRep.GetAll().Count < 1)
            {
                Repository<Karsilastirmalar> karRep = new Repository<Karsilastirmalar>(db);
                KarBaslatViewModel kbvm = new KarBaslatViewModel();
                var karlar = karRep.GetAll().Select(m => m).Where(kr => kr.Testler.Count == 0 ? kr.Testler.Count == 0 : (kr.Testler.Select(t => new { t.BaslangicZamani, t.Durum }).
                    OrderBy(te => te.BaslangicZamani).First().Durum == "Arsiv"));
                foreach (Karsilastirmalar kar in karlar)
                {
                    kbvm.karbaslist.Add(new KarBas { id = kar.Id, kacDefa = kar.Testler.Count, OA = kar.OlcumAletleri, olcumAraligi = new double[2] { kar.OlcumAraligiBas, kar.OlcumAraligiSon }, Tür = kar.Tur });
                }
                return View(kbvm);
            }
            else
            {
                //--->BUG:  HATA verecek, düzelt
                return View("BaslatilacakKarsilastirmalar");
            }
        }
        
        public ActionResult BaslatilacakKarsilastirmalar(KarBaslatViewModel btl)
        {
            int im = 0;
            CalComEntities db = new CalComEntities();
            //viewden seçilen testler btl ile alındı
            Repository<Karsilastirmalar> karRep = new Repository<Karsilastirmalar>(db);
            Repository<BasBekleyenTestler> bbtRep = new Repository<BasBekleyenTestler>(db);
            BaslatilacakKarsilastirmalarViewModel bkvm = new BaslatilacakKarsilastirmalarViewModel();

             foreach (var kar in btl.karbaslist)
                {

                    if (kar.secildiMi)
                    {

                        //object[] prt = { new SqlParameter() { ParameterName = "@karid", Value = kar.id }, new SqlParameter() { ParameterName = "@siraNo", Value = im } };
                        bbtRep.Add(new BasBekleyenTestler { karid = kar.id, sira = im });
                        im++;
                    }
                }
             foreach (var bbt in bbtRep.GetAll())
             {
                 bkvm.BaslicakTestler.Add(new BaslicakTestler { id=bbt.Id, formDolumu=bbt.sira==0?0:1, isim=karRep.Get((int)bbt.karid).OlcumAletleri.Isim, karid=(int)bbt.karid, sira=(int)bbt.sira });
                 if (bbt.sira == 0)
                 {
                     bkvm.seciliSirasi = (int)bbt.sira;
                     bkvm.test = bbt;
                   //  basBekTestRep.Delete(bbt);

                 }
             }
            return View(bkvm);
        }
        public ActionResult TestForm(int karid)
        {
            CalComEntities db = new CalComEntities();
            Repository<BasBekleyenTestler> bbtRep = new Repository<BasBekleyenTestler>(db);
            BasBekleyenTestler bbt = bbtRep.GetAll().Select(y => y).Where(x => x.karid == karid).FirstOrDefault();
            bbt=bbt == null ? bbtRep.Add( new BasBekleyenTestler { karid = karid } ): bbt;

            return PartialView(bbt);
        }
        [HttpPost]
        public ActionResult BaslatilacakKarsilastirmalar(BaslatilacakKarsilastirmalarViewModel bkvm)
        {
            //ne lazım?= güncellencek BasBekleyenTest bilgileri, seçilen testin sirasi ve baslicak testler listesini alırız. secilen test sirasından bir sonrakini listeden ceker test olarak veririz.
            //BasBekleyenTest bilgilerini de günceller, denk gelen BaslatilacakTest verisini de sileriz.
            CalComEntities db = new CalComEntities();
            //viewden seçilen testler btl ile alındı
            Repository<BasBekleyenTestler> basBekTestRep = new Repository<BasBekleyenTestler>(db);
            //Alınanı kaydetme 
            basBekTestRep.Update(bkvm.test,bkvm.test.Id);
            //siradakini cekme
            BasBekleyenTestler bt = basBekTestRep.GetAll().Select(x => x).Where(y => y.sira == bkvm.test.sira + 1).FirstOrDefault();
            //eğer bt nullsa demekki son sıradadır ozaman baslatilacakTestlerden herhangi biri cekilmelidir. Eger yine yoksa demekki tüm test verileri girilmiş, ozaman onay sayfası açılmalıdır.
            bt = bt == null ? basBekTestRep.GetAll().Select(x=>x).Where(y=>y.BaslangicZamani==null).FirstOrDefault() : bt;
            if (bt == null)
            {
                KesinBaslatViewModel kbvm = new KesinBaslatViewModel();
                foreach(var t in basBekTestRep.GetAll()){
                    kbvm.ozetler.Add(new BaslatOzet { isim=t.Karsilastirmalar.OlcumAletleri.Isim, baslangicZaman=t.BaslangicZamani, bitisZaman=t.BitisZamani, fiyat=t.Fiyat, gerekenLabSayisi=t.GerekenLabSayisi, olcumSuresi=t.OlcumSuresi });
                }
                    //sirada başka test yok onay sayfası açılacak, yine tüm karşılaştırmalar liste halinde verilmeli
                return View("KarsilastirmaBaslatmaOnayi",kbvm);
            }
                    bkvm.test = bt;
                    bkvm.seciliSirasi =(int) bt.sira;
                    foreach (var bastest in bkvm.BaslicakTestler)
                    {
                        BasBekleyenTestler basbek = basBekTestRep.Get(bastest.id);
                        bastest.isim = basbek.Karsilastirmalar.OlcumAletleri.Isim;
                        bastest.karid = (int)basbek.karid;
                        bastest.sira = (int)basbek.sira;
                        bastest.formDolumu=basbek.sira == bt.sira?0:1;
                    }
            return View(bkvm);
        }
        public ActionResult BaslatOnay()
        {
            CalComEntities db = new CalComEntities();
            Repository<BasBekleyenTestler> basBekTestRep = new Repository<BasBekleyenTestler>(db);
            Repository<Testler> testRep = new Repository<Testler>(db);
            foreach (var bk in basBekTestRep.GetAll())
            {
                testRep.Add(new Testler { karid=bk.karid, BaslangicZamani=bk.BaslangicZamani, BitisZamani=bk.BitisZamani,
                 Durum="Baslangic", Fiyat=bk.Fiyat, GerekenLabSayisi=bk.GerekenLabSayisi, HesaplamaYontemi=bk.HesaplamaYontemi, OlcumBitisZamani=bk.OlcumBitisZamani, OlcumSuresi=bk.OlcumSuresi, OlcumZamani=bk.OlcumZamani
                });
                basBekTestRep.Delete(bk);

            }
            return View();
        }
        public ActionResult Takvimlendirme()
        {
            return View();
        }
        public ActionResult DavetAyarlari()
        {
            return PartialView();
        }
        //!<-- BASLANGIC MODUL SON

        // !--> KALİBRASYON MODUL BAŞI
        public ActionResult Kalibrasyon()
        {
            return View();
        }
        public ActionResult KalibrasyonTakvimi()
        {
            return View();
        }
        public ActionResult KarsilastirmaTakvimiSecme()
        {
            return PartialView();
        }
        public ActionResult Olcumler()
        {
            return View();
        }
        public ActionResult Sürecler()
        {
            return View();
        }
        public ActionResult ReferansDegerler()
        {
            return PartialView();
        }
        public ActionResult StandardSapma()
        {
            return PartialView();
        }
        public ActionResult KarsilastirmaProvasi()
        {
            return View();
        }
        // !<-- KALİBRASYON MODUL SON

        //!-->RAPORLAMA MODUL BAŞI
        public ActionResult Karşılaştırma()
        {
            return View();
        }
        public ActionResult OlcumSonuclari()
        {
            return View();
        }
        public ActionResult OlcumAleti()
        {
            return View();
        }
        public ActionResult İstatistikselHesaplamalar()
        {
            return PartialView();
        }
        public ActionResult Grafiklendirme()
        {
            return PartialView();
        }
        public ActionResult Raporlama()
        {
            return View();
        }
        //!<--RAPORLAMA MODUL SON

        //!-->ARŞİV MODUL BAŞI
        public ActionResult Arşiv()
        {
            return View();
        }
        //!<--ARŞİV MODUL SON

        //!-->FORMLAR MODUL BAŞI
        public ActionResult Formlar()
        {
            return View();
        }
        //!-->GenelAyar MODUL BAŞI
        public ActionResult GenelAyar()
        {
            return View();
        }
        //public ActionResult Haberler()
        //{
        //    return View();
        //}
      
        //!-->KarmasikYapilar MODUL BAŞI
        public ActionResult KarmasikYapilar()
        {
            return View();
        }
        public ActionResult SayfaYapilari()
        {
            return View();
        }
        public ActionResult KaynakKod()
        {
            return View();
        }
        public ActionResult Reklam()
        {
            return View();
        }
        public ActionResult Grafikler()
        {
            return View();
        }
        public ActionResult RaporAyari()
        {
            return View();
        }
        //!-->Editor MODUL BAŞI
        public ActionResult Editor()
        {
            return View();
        }
        public ActionResult RaporTaslagi()
        {
            return View();
        }
        public ActionResult OlcumAyar()
        {
            return View();
        }
        public ActionResult MailAyar()
        {
            return View();
        }
        public ActionResult DavetiyeAyar()
        {
            return View();
        }
        //!<--FORMLAR MODUL SON

        //!-->DOKÜMENTASYON
        public ActionResult Dokumentasyon()
        {
            return View();
        }
        public ActionResult Protokoller()
        {
            return View();
        }
        public ActionResult Standardlar()
        {
            return View();
        }
        public ActionResult Reklamlar()
        {
            return View();
        }
        public ActionResult Bildirimler()
        {
            return View();
        }
        public ActionResult BelgeTaslagi()
        {
            return View();
        }
        //!<--DÖKÜMENTASYON MODUL SON
    }
}
