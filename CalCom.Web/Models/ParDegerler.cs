using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class ParDegerler
    {
        public List<ParametreDegerleri> olcListesi{get;set;}
         public ParDegerler()
        {
            olcListesi = new List<ParametreDegerleri>();
        }
    }
}