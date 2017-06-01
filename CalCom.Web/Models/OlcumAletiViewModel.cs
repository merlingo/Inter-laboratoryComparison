using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class OlcumAletiViewModel
    {
        public List<OlcumAletleri> OAlar { get; set; }
        [Required]
        public OlcumAletleri oa { get; set; }
        public OlcumAletiViewModel()
        {
            OAlar = new List<OlcumAletleri>();
            oa = new OlcumAletleri();
        }
    }
}