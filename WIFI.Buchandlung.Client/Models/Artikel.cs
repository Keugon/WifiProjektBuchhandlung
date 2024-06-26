using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine Typsicher auflistung von Artikel objekten bereit
    /// </summary>
    public class ArtikelListe : System.Collections.Generic.List<Artikel>
    {
    }
    /// <summary>
    /// Beschreibt anlegbare Artike eg Bücher/spiele
    /// </summary>
    public class Artikel : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        /// <summary>
        /// Ruft die Bezeichnung ab oder legt diese fest
        /// </summary>
        public string? Bezeichnung { get; set; }
        /// <summary>
        /// Ruft den Beschaffungspreis ab oder legt diesen fest
        /// </summary>
        public decimal? Beschaffungspreis { get; set; }
        /// <summary>
        /// Ruft die Inventargegenstände des Artikels ab oder legt diese fest
        /// </summary>
        public InventarGegenstände? InventarGegenstände { get; set; }

    }
}
