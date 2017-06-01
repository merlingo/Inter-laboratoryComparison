using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class TakipViewModel
    {
        public string KarLabIsmi { get; set; }
        public Takvim takvim { get; set; }
        public KayitBilgileri kayitBilgileri;
        public  OlcumBilgileri olcumBilgileri;
        public TakipViewModel(InputValueViewModel s){
            kayitBilgileri = new KayitBilgileri();
            olcumBilgileri = new OlcumBilgileri(s);
    }
    }
    public class KayitBilgileri
    {

        public string GIsim { get; set; }
        public DateTime OlcumZamaniBas { get; set; }
        public DateTime OlcumZamaniBit { get; set; }
        public DateTime TestBas { get; set; }
        public double TestFiyat { get; set; }
    }
    public class OlcumBilgileri
    {
        public double Sicaklik { get; set; }
        public double Nem { get; set; }
        public double Basinc { get; set; }
      public  InputValueViewModel Sonuclar;
      public OlcumBilgileri(InputValueViewModel s)
      {
          Sonuclar = s;
      }

    }
}