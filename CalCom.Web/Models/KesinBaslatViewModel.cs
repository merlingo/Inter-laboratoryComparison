using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    public class KesinBaslatViewModel
    {
        public List<BaslatOzet> ozetler;
        public KesinBaslatViewModel()
        {
            ozetler = new List<BaslatOzet>();
        }
    }
}