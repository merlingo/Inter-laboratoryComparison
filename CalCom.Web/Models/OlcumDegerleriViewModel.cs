using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalCom.Web.Models
{
    public class OlcumDegerleriViewModel
    {
        public int karid { get; set; }
        public List<SelectListItem> dropDownList { get; set; }
    }
}