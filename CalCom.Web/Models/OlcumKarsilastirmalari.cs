using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class OlcumKarsilastirmalari
    {
        public List<OlcumKar> oklar;
        public OlcumKarsilastirmalari()
        {
            oklar = new List<OlcumKar>();
        }  
    }
    public class OlcumKar
    {
        public string olcumAdi { get; set; }
        public bool uygunMu { get; set; }
        public int testid { get; set; }
    }
}