using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class KarsilastirmaUretimiViewModel
    {
        public List<KarsilastirmaUretimi> KarList;
        public KarsilastirmaUretimiViewModel()
        {
            KarList = new List<KarsilastirmaUretimi>();
        }

    }
    public class KarsilastirmaUretimi
    {
        public int Id { get; set; }
        public String OlcumAleti { get; set; }
        public String Konu { get; set; }
        public double OlcumAraligib { get; set; }
        public double OlcumAraligis { get; set; }
        public int AcilanTestSayisi { get; set; }
        public int inputSayisi { get; set; }
        public String RehberDokuman { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        
    }
}