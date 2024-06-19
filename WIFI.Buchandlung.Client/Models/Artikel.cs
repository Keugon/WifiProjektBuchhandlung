using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    public class Artikel :WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        public string? Bezeichnung { get; set; }
        public decimal? Beschaffungspreis { get; set; }
    }
}
