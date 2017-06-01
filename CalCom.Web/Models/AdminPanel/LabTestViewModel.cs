using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class LabTestViewModel
    {
        public List<LabTest> labTest;
        public List<Oge> LabList;
        public List<Oge> TestList;
        public LabTestViewModel()
        {
            labTest = new List<LabTest>();
            LabList = new List<Oge>();
            TestList = new List<Oge>();
        }
    }
    public class LabTest
    {
        public int id { get; set; }
        public int labid { get; set; }
        public int testid { get; set; }
        public String lab { get; set; }
        public String kar { get; set; }
        public DateTime olcumTarihB { get; set; }
        public DateTime olcumTarihS { get; set; }

        public String simdiMi()
        {
            if(olcumTarihB > DateTime.Now) return "info";
            else if ( DateTime.Now > olcumTarihS) return "error";
            else return "success";
        }
    }
    public class Oge
    {
        public int id { get; set; }
        public string isim { get; set; }
    }
}