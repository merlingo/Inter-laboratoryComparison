using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class OlcumAletiUretimiViewModel
    {
        public List<OlcumAletiListeVerisi> OlcumAletiListesi { get; set; }
        

    }

    public class OlcumAletiListeVerisi
    {
        public int oaId { get; set; }
        public string Isim { get; set; }
        public String SeriNo { get; set; }
        public String Model { get; set; }
        public int KarsilastirmaSayisi { get; set; }
        public int TestSayisi { get; set; }
        public DateTime EklenmeTarihi { get; set; }
    }
}