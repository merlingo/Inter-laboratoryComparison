using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcom.Domain.Entities
{
  public class Takvim
    {
      public int TestId { get; set; }
      public string TestName;
      public List<OlcumDate> OlcumZamanlari;
      public List<TestDate> TestZamanlari;
      public Takvim(TestDate[] td)
      {
          OlcumZamanlari = new List<OlcumDate>();

          TestZamanlari = new List<TestDate>();
          for (int i = 0; i <td.Count(); i++)
          {
              TestZamanlari.Add(td[i]);
          }
      }
      public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
      {
          // Unix timestamp is seconds past epoch
          System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
          dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
          return dtDateTime;
      }
      public  double DateTimeToUnixTimestamp(DateTime dateTime)
      {
          return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
      }
      public void OlcumEkle(OlcumDate od)
      {
          OlcumZamanlari.Add(od);
      }

      public List<DateTime> GecerliZamanlar()
      {
          List<DateTime> dtler = new List<DateTime>();
          //DateTime basTarih = TestZamanlari.Find(d=>d.Type=="baslangic").Tarih;
          //bugünden itiraben olsa daha mantıklı
          DateTime basTarih = DateTime.Today;
          DateTime sonTarih = TestZamanlari.Find(d=>d.Type=="son_olcum").Tarih;
          for (DateTime dt = basTarih.AddDays(1); dt < sonTarih; dt= dt.AddDays(1)) //basTarih' e eklenen bir sayısı baslangic zamanını değiştirebilir DKKAT!
              if (dt.DayOfWeek != DayOfWeek.Saturday || dt.DayOfWeek != DayOfWeek.Sunday)
                 dtler.Add(dt);

          return dtler;
      }


      public List<DateTime> GecerliZamanlar(DateTime d1, DateTime d2)
      {
          List<DateTime> dtler = new List<DateTime>();
         
          for (DateTime dt = d1.AddDays(1); dt < d2; dt =  dt.AddDays(1)) //basTarih' e eklenen bir sayısı baslangic zamanını değiştirebilir DKKAT!
              if (dt.DayOfWeek != DayOfWeek.Saturday || dt.DayOfWeek != DayOfWeek.Sunday)
                 dtler.Add(dt);

          return dtler;
      }

    }
  public class OlcumDate
  {
      public int OlcumId { get; set; }
      public int LabId { set; get; }
      public string GLabName { get; set; }
      public DateTime basTarih { get; set; }
      public DateTime bitTarih { get; set; }

  }

  public class TestDate
  {
      public string Type; //testin hangi tarihi; başlangıç - Olcum - Bitiş
      public DateTime Tarih { get; set; }
      public string kisaIsim { get; set; }
  }
}
