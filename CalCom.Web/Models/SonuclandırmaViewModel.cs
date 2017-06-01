using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class SonuclandırmaViewModel
    {
        //public List<SonucViewModel> lsvm;
        public List<string> ParDer;
        public int TestId { get; set; }
        public SonuclandırmaViewModel()
        {
            //lsvm = new List<SonucViewModel>();
            ParDer = new List<string>();
        }
    }

    //public class SonucViewModel
    //{
    //    public int labid { get; set; }
    //    public string gLabName { get; set; }
    //    public string LabName { get; set; }
    //    public List<Sonuc> Sonuclar;
    //    public SonucViewModel()
    //    {
    //        Sonuclar = new List<Sonuc>();
    //    }

    //}

}