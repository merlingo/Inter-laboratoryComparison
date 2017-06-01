using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcom.Domain.Entities
{

    public class Gecici
    {
        public string gname { get; set; }
        public string gsifre { get; set; }

        public Gecici()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            gname = new string(
                Enumerable.Repeat(chars, 3)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            gsifre = new string(
    Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)])
              .ToArray());

        }
        public Gecici(string name , string sifre)
        {
            gname = name;
            gsifre = sifre;
        }
    }
}
