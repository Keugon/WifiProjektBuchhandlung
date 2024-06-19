using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    public class InventarGegenstand :System.Object
    {
        public Artikel? Artikel { get; set; }
        public Entlehnung? Entlehnung { get; set; }
       
    }
}
