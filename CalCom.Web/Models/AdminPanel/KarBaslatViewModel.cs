using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class KarBaslatViewModel
    {
        //bas bekleyen kar listesi, test listesi, 
        public List<KarBas> karbaslist { get; set; }
        public List<BasBekleyenTestler> bbtler;

        public KarBaslatViewModel()
        {
            karbaslist = new List<KarBas>();
            bbtler = new List<BasBekleyenTestler>();
        }
    }
    public class KarBas
    {
        public int id { get; set; }
        public OlcumAletleri OA { get; set; }
        public int kacDefa { get; set; }
        public double[] olcumAraligi { get; set; }
        public string Tür { get; set; }
        public bool secildiMi { get; set; }
    }
}