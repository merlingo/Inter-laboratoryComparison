using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class YonetViewModel
    {
        public List<Yonetici> Takipler;
        public YonetViewModel()
        {
            Takipler = new List<Yonetici>();
        }
    }

    public class Yonetici
    {
        //olcum aleti ismi, karsılastırma konusu, olcum id 
        public string OlcumAleti { get; set; }
        public string KarKonu { get; set; }
        public int OlcId { get; set; }
        public double OlcumAraligiBas { get; set; }
        public double OlcumAraligiBit { get; set; }
        public string Nerede { get; set; }
        public string ProtocolNo { get; set; } // bu var ile rehber dokumanın kendisini gönder
        public int KatilanLab { get; set; }
    }
}