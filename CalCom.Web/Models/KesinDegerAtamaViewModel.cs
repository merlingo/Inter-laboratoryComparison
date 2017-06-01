using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class KesinDegerAtamaViewModel
    {
        public int testid { get; set; }
        public string yontem { get; set; }
        public bool atamaDurum { get; set; }
        public List<KesinDegerAtamalari> Atamalar { get; set; }

        public KesinDegerAtamaViewModel() {
            Atamalar = new List< KesinDegerAtamalari>();
        }
    }

    public class KesinDegerAtamalari
    {
        public int? id { get; set; }
        public int Onid { get; set; }
        public string OlcumNoktasi{get;set;}
        public string ParametreDegeri { get; set; }
        public Nullable<double> KesinDeger { get; set; }
        public Nullable<double> Belirsizlik { get; set; }

    }
}