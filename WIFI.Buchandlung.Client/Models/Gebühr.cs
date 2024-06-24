using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Beschreibt einen Gebühr eintrag
    /// </summary>
    public class Gebühr
    {
        /// <summary>
        /// Ruft die Laufende Nummer des Eintrags in der DB ab oder legt diese fest
        /// </summary>
        public int LfdNr;
        /// <summary>
        /// Ruft das Datum ab wann diese Gebühr benutzt werden soll ab oder legt diese fest
        /// </summary>
        public DateTime GültigAb;
        /// <summary>
        /// Ruft den Tagesstrafsatz ab oder legt diese fest
        /// </summary>
        public decimal Strafgebühr;
        /// <summary>
        /// Ruft den Ersatzfaktor ab der für eine Neubeschaffungs verrechnet wird ab oder legt diese fest
        /// </summary>
        public double ErsatzgebührFaktor;
        /// <summary>
        /// Ruft die anzahl der Gebührenfreien Tagen ab oder legt diese fest
        /// </summary>
        public int GebührenFreieTage;
    }
}
