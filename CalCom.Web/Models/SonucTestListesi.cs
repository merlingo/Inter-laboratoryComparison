using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class SonucTestListesi
    {
       public List<SonucTest> STler;
        public SonucTestListesi()
        {
            STler = new List<SonucTest>();
        }
    }

    public class SonucTest
    {
        public int VGLabSayisi { get; set; } 
        public int KytLabSayisi { get; set; }
        public string KarIsim { get; set; }
        public string Konu { get; set; }
        public int testId { get; set; }
        public string KesinDeger { get; set; }
        public bool KesinDegerDurum { get; set; }
    }
    public class SonucLab
    {
        public string Isim { get; set; }
        public string GIsim { get; set; }
        public DateTime OlcDate { get; set; }
        public string Durum { get; set; }
        public int OlcId { get; set; }
    }
}