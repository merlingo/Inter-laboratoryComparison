using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.AdminPanel
{
    public class BaslatilacakKarsilastirmalarViewModel
    {
        public List<BaslicakTestler> BaslicakTestler { get; set; }
        public BasBekleyenTestler test { get; set; }
        public int seciliSirasi { get; set; }
        public BaslatilacakKarsilastirmalarViewModel()
        {
            BaslicakTestler = new List<BaslicakTestler>();
        }
    }
    public class BaslicakTestler
    {
        public int id{get;set;}
        public int karid { get; set; }
        public string isim{get;set;}
        public int formDolumu{get;set;}
        public int sira { get; set; }
    }
}