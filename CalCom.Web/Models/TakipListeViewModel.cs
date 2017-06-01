using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class TakipListeViewModel
    {
        public List<Takipci> Takipler;
        public TakipListeViewModel()
        {
            Takipler = new List<Takipci>();
        }
    }

    public class Takipci
    {
        //olcum aleti ismi, karsılastırma konusu, olcum id 
        public string OlcumAleti { get; set; }
        public string KarKonu { get; set; }
        public int OlcId { get; set; }
        public string Durum { get; set; }
    }
}