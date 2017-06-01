using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public enum BasDurum
    {
        başlangıç,
        devam,
        son
    };
    public class BaslatilacakTestListesi
    {

        public int count { get; set; }
        public List<TestKonu> tkler { get; set; }
        public BasDurum bd;

        public BaslatilacakTestListesi()
        {
            tkler = new List<TestKonu>();
           
        }
    }
    public class TestKonu 
    {

        public string konu { get; set; }
        public List<BaslatilacakTest> basTestler { get; set; }
       
        public TestKonu()
       {
           basTestler = new  List<BaslatilacakTest>();
       }

    }
    public class BaslatilacakTest
    {
        public int id { get; set; }
        public OlcumAletleri OA { get; set; }
        public int kacDefa { get; set; }
        public double[] olcumAraligi { get; set; }
        public string Tür { get; set; }
        public bool secildiMi { get; set; }

    }
}