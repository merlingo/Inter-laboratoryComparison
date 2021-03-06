//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calcom.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Karsilastirmalar
    {
        public Karsilastirmalar()
        {
            this.BasBekleyenTestler = new HashSet<BasBekleyenTestler>();
            this.ParametreDegerleri = new HashSet<ParametreDegerleri>();
            this.Testler = new HashSet<Testler>();
        }
    
        public int Id { get; set; }
        public int OAId { get; set; }
        public string Konu { get; set; }
        public double OlcumAraligiBas { get; set; }
        public double OlcumAraligiSon { get; set; }
        public Nullable<int> InputSayisi { get; set; }
        public Nullable<int> FonksiyonNo { get; set; }
        public string Tur { get; set; }
        public string RehberDöküman { get; set; }
        public string ÖlçümVeriTipi { get; set; }
        public Nullable<System.DateTime> EklenmeTarihi { get; set; }
    
        public virtual ICollection<BasBekleyenTestler> BasBekleyenTestler { get; set; }
        public virtual OlcumAletleri OlcumAletleri { get; set; }
        public virtual ICollection<ParametreDegerleri> ParametreDegerleri { get; set; }
        public virtual ICollection<Testler> Testler { get; set; }
    }
}
