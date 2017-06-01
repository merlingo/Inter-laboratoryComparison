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
    
    public partial class OlcumSonuclari
    {
        public OlcumSonuclari()
        {
            this.PerformanceScores = new HashSet<PerformanceScores>();
        }
    
        public int Id { get; set; }
        public int OlcumId { get; set; }
        public int OlcumNoktalariId { get; set; }
        public Nullable<double> Deger { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<double> Belirsizlik { get; set; }
    
        public virtual Olcumler Olcumler { get; set; }
        public virtual OlcumNoktalari OlcumNoktalari { get; set; }
        public virtual ICollection<PerformanceScores> PerformanceScores { get; set; }
    }
}
