using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models.Laboratuvar
{
    public class MesajKutusuViewModel
    {
       public List<Mesajlar> Mesajlar;
        public MesajKutusuViewModel()
        {
            Mesajlar = new List<Mesajlar>();
        }
    }
    
}