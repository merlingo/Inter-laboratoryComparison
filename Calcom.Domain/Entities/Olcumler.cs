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
    
    public partial class Olcumler
    {
        public Olcumler()
        {
            this.OlcumdekiReferansCihaz = new HashSet<OlcumdekiReferansCihaz>();
            this.OlcumSonuclari = new HashSet<OlcumSonuclari>();
        }
    
        public int Id { get; set; }
        public int TestId { get; set; }
        public int LabId { get; set; }
        public string GeciciIsim { get; set; }
        public string GeciciSifre { get; set; }
        public Nullable<decimal> Sicaklik { get; set; }
        public Nullable<decimal> Nem { get; set; }
        public Nullable<decimal> Basinc { get; set; }
        public Nullable<System.DateTime> OlcumBaslangicZamani { get; set; }
        public Nullable<System.DateTime> OlcumSonZaman { get; set; }
        public Nullable<decimal> StandartSapma { get; set; }
        public Nullable<double> SabitSonuc { get; set; }
        public Nullable<double> OzelSonuc { get; set; }
        public string Durum { get; set; }
    
        public virtual ICollection<OlcumdekiReferansCihaz> OlcumdekiReferansCihaz { get; set; }
        public virtual Laboratuvarlar Laboratuvarlar { get; set; }
        public virtual Testler Testler { get; set; }
        public virtual ICollection<OlcumSonuclari> OlcumSonuclari { get; set; }
    }
}