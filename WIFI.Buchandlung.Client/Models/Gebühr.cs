using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    public class Gebühr
    {
        public int LfdNr;
        public DateTime GültigAb;
        public decimal Strafgebühr;
        public double ErsatzgebührFaktor;
        public int GebührenFreieTage;
    }
}
