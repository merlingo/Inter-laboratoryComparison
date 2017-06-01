using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class LaboratuvarKayitlariViewModel
    {
        public List<LaboratuvarKayitListeElemanı> LabList;
        public LaboratuvarKayitlariViewModel()
        {
            LabList = new List<LaboratuvarKayitListeElemanı>();
        }
    }
    public class LaboratuvarKayitListeElemanı
    {
        //lab adı - Yönetici - Çalışma Alanı - Telefon - Mail - Amblem - Kayıt Onay Durumu
        public int Id { get; set; }
        public String Isim { get; set; }
        public String Yonetici { get; set; }
        public String CalismaAlani { get; set; }
        public long Tel { get; set; }
        public String Mail { get; set; }
        public String AmblemPath { get; set; }
        public bool KayitliMi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public String idOlustur()
        {
            return Id + "_" + KayitliMi;
        }

       
    }
}