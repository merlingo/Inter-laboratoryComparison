using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    //ViewModel --> testin tüm ölçümleri, parametre degerleri, ölçüm noktaları, kesin degerler
    public class OlcumSonuclariViewModel
    {
        public int TestId { get; set; }
        public List<ParametreDegerleri> ParDerler { get; set; }
        public string yontem { get; set; }
        public List<String> Lablar { get; set; }
        public List<LabOlcumu> Olcumler { get; set; }
        public string KarName { get; set; }
        public OlcumSonuclariViewModel()
        {
            ParDerler = new List<ParametreDegerleri>();
            Lablar = new List<String>();
            Olcumler = new List<LabOlcumu>();
        }
        public class LabOlcumu
        {
            public int Osid { get; set; }
            public int Olcid { get; set; }
            public double PerformanceScore { get; set; }
        }
    }
}