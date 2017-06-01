using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcom.Domain.Entities
{
    //state pattern uygulanmıştır, Test in 2 durumu olabilir açık ve kapalı
   public class Test
    {
      
       public Repository<Testler> TestRep;
       CalComEntities db;
       Testler t;
       int ID;
       public Test(int id)
       {
           db = new CalComEntities();
           TestRep = new Repository<Testler>(db);
           t = TestRep.Get(id);
           ID = id;
          
       }
       public Test(BasBekleyenTestler bkr)
       {
           TestRep = new Repository<Testler>(new CalComEntities());
           TestRep.Insert("Id",new object[] { new SqlParameter() { ParameterName = "@karid", Value = bkr.karid },
                                   new SqlParameter() { ParameterName = "@baslangicZamani", Value = bkr.BaslangicZamani },
                               new SqlParameter() { ParameterName = "@BitisZamani", Value = bkr.BitisZamani },
                             new SqlParameter() { ParameterName = "@OlcumZamani", Value = bkr.OlcumZamani },
                             new SqlParameter() { ParameterName = "@Fiyat", Value = bkr.Fiyat },
                         new SqlParameter() { ParameterName = "@OlcumBitisZamani", Value = bkr.OlcumBitisZamani },

                         //      new SqlParameter() { ParameterName = "@ProtokolNo", Value = bkr.ProtokolNo },
                         new SqlParameter() { ParameterName = "@HesaplamaYontemi", Value = bkr.HesaplamaYontemi },
                               new SqlParameter() { ParameterName = "@OlcumSuresi", Value = bkr.OlcumSuresi },
                                 new SqlParameter() { ParameterName = "@GerekenLabSayisi", Value = bkr.GerekenLabSayisi } });
          
             t = TestRep.Find(x => x.karid == bkr.karid && x.BaslangicZamani == bkr.BaslangicZamani);
          
       }

       public void testBaslat()
       {
         
           ID = t.Id;
           t.Durum = "Açık";
           TestRep.Update(t, t.Id);
       }
       //teste kayıt yaptıracak laboratuvarın idsine ihtiyaç vardır
       public Gecici testKayit(int labid, DateTime dbas)
       {
           
           Gecici g = new Gecici();
           // labid ile olcumler tablosuna yeni veri eklenir. durumu kayıt olarak değiştirilir
           Repository<Olcumler> olcRep = new Repository<Olcumler>(db);
           olcRep.Insert("Id", new object[] { new SqlParameter() { ParameterName = "@TestId", Value = ID },
                new SqlParameter() { ParameterName = "@LabId", Value = labid },
                 new SqlParameter() { ParameterName = "@GeciciIsim", Value = g.gname },
                  new SqlParameter() { ParameterName = "@GeciciSifre", Value = g.gsifre },
                   new SqlParameter() { ParameterName = "@Durum", Value = "Kayit" },
                   new SqlParameter() { ParameterName = "@OlcumBaslangicZamani", Value = dbas },
                                      new SqlParameter() { ParameterName = "@OlcumSonZaman", Value = dbas.AddDays((double)t.OlcumSuresi) }


                                      }
                         );

           return g;
       }

       public void labOlcum()
       {
          
       }

       public void adminSonucla()
       {
          
       }

       public void adminYayinla()
       {
          
       }

      

       public Testler getTestler()
       {
           return t;
       }


       public Takvim TakvimOlustur()
       {
           Takvim takvim = new Takvim(new TestDate[] { new TestDate { Tarih = (DateTime)t.BaslangicZamani, Type = "baslangic", kisaIsim = "BAS" }, new TestDate { Tarih = (DateTime)t.BaslangicZamani, Type = "ilk_olcum", kisaIsim = "ILK" }, new TestDate { Tarih = ((DateTime)t.BitisZamani).Subtract(new TimeSpan(1,0,0,0)), Type = "son_olcum", kisaIsim = "SON" }, new TestDate { Tarih = (DateTime)t.BitisZamani, Type = "bitis", kisaIsim = "BIT" } });
           takvim.TestName = t.Karsilastirmalar.OlcumAletleri.Isim;
           takvim.TestId = ID;
           if(t.Olcumler.Count>0)
              foreach(Olcumler olc in t.Olcumler)
                  takvim.OlcumEkle(new OlcumDate { GLabName = olc.GeciciIsim, LabId = olc.LabId, OlcumId = t.Id, basTarih = (DateTime) olc.OlcumBaslangicZamani, bitTarih = (DateTime)olc.OlcumSonZaman });
           return takvim;
       }


       internal System.Data.Entity.DbContext getDb()
       {
           return db;
       }
    }
}
