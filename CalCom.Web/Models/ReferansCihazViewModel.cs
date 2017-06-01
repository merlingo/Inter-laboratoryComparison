using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class ReferansCihazViewModel
    {
        public int olcId { get; set; }
        public ReferansCihazlar rc { get; set; } //editor template yaz
    
     public List<ReferansCihazlar> OAlar { get; set; }

     public ReferansCihazViewModel()
        {
            OAlar = new List<ReferansCihazlar>();
            rc = new ReferansCihazlar();
        }
    }


}