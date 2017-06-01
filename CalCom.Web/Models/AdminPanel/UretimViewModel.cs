using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class UretimViewModel
    {
        public List<OlcumAletiListeVerisi> OlcumAletiListesi { get; set; }
        public List<KarsilastirmaUretimi> KarList { get; set; }
        public List<LaboratuvarKayitListeElemanı> LabList { get; set; }
        public UretimViewModel()
        {
            OlcumAletiListesi = new List<OlcumAletiListeVerisi>();
            KarList = new List<KarsilastirmaUretimi>();
            LabList = new List<LaboratuvarKayitListeElemanı>();
        }
    }
}