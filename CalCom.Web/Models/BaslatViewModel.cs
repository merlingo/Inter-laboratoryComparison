using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class BaslatViewModel
    {
        public BasBekleyenTestler bbt { get; set; }
        public Queue<OncekiTest> QueOncekiTest { get; set; }
        public BasDurum bd;
        public BaslatViewModel()
        {
            bbt = new BasBekleyenTestler();
            QueOncekiTest = new Queue<OncekiTest>();
        }
    }
    public class OncekiTest
    {
        public int id { get; set; }
        public string isim { get; set; }

        public OncekiTest()
        {

        }
    }
}