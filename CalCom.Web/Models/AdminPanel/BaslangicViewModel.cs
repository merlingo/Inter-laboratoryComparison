using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class BaslangicViewModel
    {
        public LabTestViewModel lbvm;
        public List<BaslayanTest> testler;

        public BaslangicViewModel()
        {
            lbvm = new LabTestViewModel();
            testler = new List<BaslayanTest>();
        }
    }
    public class BaslayanTest
    {
        public int TestId { get; set; }
        public string Konu { get; set; }
        public string OlcumAleti { get; set; }
        public int KayitliLab { get; set; }
        public int YeniKayitLab { get; set; }

    }
}