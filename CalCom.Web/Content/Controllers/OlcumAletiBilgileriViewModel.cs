using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalCom.Web.Controllers
{
   public class OlcumAletiBilgileriViewModel
    {
        private int olcId;

        public OlcumAletiBilgileriViewModel(int olcId)
        {
            // TODO: Complete member initialization
            this.olcId = olcId;
        }

        public int getOlcId()
        {
            return olcId;
        }
        public Calcom.Domain.Entities.OlcumAletleri oa { get; set; }
    }
}
