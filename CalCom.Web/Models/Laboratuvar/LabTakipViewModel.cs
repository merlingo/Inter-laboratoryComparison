using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.Laboratuvar
{
    public class LabTakipViewModel
    {
        public List<LabOzet> Lablar;
        public LabTakipViewModel()
        {
            Lablar= new List<LabOzet>();
        }
    }

    public class LabOzet
    {
        public string LabAdi { get; set; }
        public string Yonetici { get; set; }
        public string CalismaAlani { get; set; }
        public string Sehir { get; set; }
        public int LabId { get; set; }
    }
    public class LabinTestleri
    {
          public List<LabTest> Testler;
          public LabinTestleri()
        {
            Testler = new List<LabTest>();
        }
    }
    public class LabTest
    {
        //test isimleri, başlama tarihi ve labın ölçüm tarihi
        public int TestId { get; set; }
        public string TestIsmi { get; set; }
        public DateTime BaslamaTarihi { get; set; }
        public DateTime OlcumTarihi { get; set; }
    }
}