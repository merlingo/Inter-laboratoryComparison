using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class LaboratuvarBilgileri
    {
       public  Laboratuvarlar lab { get; set; }
         public void setLab(Laboratuvarlar l)
         {
             lab = l;
         }
         public void setLab(KayitBekleyenLab l)
         {
             lab = new LabAdaptor(l);
         }
    }

    public class LabAdaptor : Laboratuvarlar
    {
        KayitBekleyenLab klab;
        public LabAdaptor(KayitBekleyenLab kbl)
        {
            klab = kbl;
            this.Id=kbl.Id;       
             this.Isim =kbl.Isim;
        this.Yonetici =kbl.Yonetici;
         this.CalismaAlani=kbl.CalismaAlani; 
         this.VergiDairesiNo =kbl.VergiDairesiNo;
         this.Telefon=kbl.Telefon;
         this.Faks=kbl.Faks ;
         this.Gsm =kbl.Gsm;
        this.Sehir =kbl.Sehir;
         this.Bolge =kbl.Bolge;
        this.Adres =kbl.Adres;
         this.Amblem =kbl.Amblem;
         this.Sifre =kbl.Sifre;
         this.email=kbl.email;
        }
    }
}