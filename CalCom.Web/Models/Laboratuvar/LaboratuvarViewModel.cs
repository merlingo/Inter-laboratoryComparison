using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.Laboratuvar
{
    public class LaboratuvarViewModel
    {
        public Laboratuvarlar lab { get; set; }
        public Takvim takvim { get; set; }
        public List<ReferansCihazlar> refler;
        public LaboratuvarViewModel()
        {
            refler = new List<ReferansCihazlar>();
        }

    }
}