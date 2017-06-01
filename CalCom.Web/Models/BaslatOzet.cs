using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class BaslatOzet
    {

       public string isim { get; set; }
       public Nullable<DateTime> baslangicZaman { get; set; }
       public Nullable<DateTime> bitisZaman { get; set; }
       public Nullable<decimal> fiyat { get; set; }
       public Nullable<int> olcumSuresi { get; set; }
       public Nullable<int> gerekenLabSayisi { get; set; }
    }
}