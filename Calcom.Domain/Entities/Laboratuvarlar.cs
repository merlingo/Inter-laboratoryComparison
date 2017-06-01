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
    
    public partial class Laboratuvarlar
    {
        public Laboratuvarlar()
        {
            this.IslemGecmisleri = new HashSet<IslemGecmisleri>();
            this.LabinReferansCihazlari = new HashSet<LabinReferansCihazlari>();
            this.Mesajlar = new HashSet<Mesajlar>();
            this.Olcumler = new HashSet<Olcumler>();
        }
    
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Yonetici { get; set; }
        public string CalismaAlani { get; set; }
        public Nullable<long> VergiDairesiNo { get; set; }
        public Nullable<long> Telefon { get; set; }
        public Nullable<long> Faks { get; set; }
        public Nullable<long> Gsm { get; set; }
        public string Sehir { get; set; }
        public string Bolge { get; set; }
        public string Adres { get; set; }
        public byte[] Amblem { get; set; }
        public string Sifre { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<System.DateTime> OnayTarihi { get; set; }
    
        public virtual ICollection<IslemGecmisleri> IslemGecmisleri { get; set; }
        public virtual ICollection<LabinReferansCihazlari> LabinReferansCihazlari { get; set; }
        public virtual ICollection<Mesajlar> Mesajlar { get; set; }
        public virtual ICollection<Olcumler> Olcumler { get; set; }
    }
}