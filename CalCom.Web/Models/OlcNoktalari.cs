using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class OlcNoktalari
    {
        public List<OlcumNoktalari> olcListesi { get; set; }
        public int toplamOlc { get; set; }
        public OlcNoktalari()
        {
            olcListesi = new List<OlcumNoktalari>();
        }
    }
}