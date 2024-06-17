using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    public class InventarArtikel : WIFI.Buchandlung.Client.Models.Artikel
    {
        public Guid Ausleiher { get; set; }
        public DateTime AusleihDatum { get; set; }
        public DateTime RückgabeDatum { get; set; }
        public string? RückgabeZustand { get; set; }
        public decimal Strafbetrag { get; set; }
        public string? StrafbetragBemerkung { get; set; }
    }
}
