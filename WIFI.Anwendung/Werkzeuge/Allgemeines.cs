using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung.Werkzeuge
{
    /// <summary>
    /// Stellt diverse Erweiterungsmethoden
    /// zur Unterstützung der Entwicklung bereit
    /// </summary>
    public static class Allgemeines : System.Object
    {
        /// <summary>
        /// Ergänzt den Pfad um das gefundene Sprachkürzel
        /// </summary>
        /// <param name="pfad">Eine Verzeichnisangabe, wo
        /// die Existenz eines Unterverzeichnisses mit dem
        /// Sprachkürzel der aktuellen Anwendungssprache
        /// geprüft werden soll</param>
        /// <returns>Der Pfad mit dem Sprachkürzel oder
        /// der unveränderte Pfad, wenn keine Lokalisierung
        /// gefunden wurde</returns>
        /// <remarks>Es wird die "Fall-Back-Technik" benutzt</remarks>
        public static string HoleLokalisiertenOrdner(this string pfad)
        {
            // Das Kürzel der aktuellen Sprache ermitteln
            var Kultur = System.Globalization.CultureInfo
                .CurrentUICulture.Name;

            // Solange im Pfad kein Unterordner
            // mit diesem Kürzel enthalten ist,
            // die Subkultur entfernen und wieder prüfen.
            // Bis alles geprüft wurde
            while (!System.IO.Path.Exists(
                        System.IO.Path.Combine(pfad, Kultur))
                    && Kultur.Length > 0
                )
            {
                int LetzterBindestrich
                     = Kultur.LastIndexOf('-');
                if (LetzterBindestrich > -1)
                {
                    Kultur = Kultur
                        .Substring(0, LetzterBindestrich);
                }
                else
                {
                    //Es wurde alles geprüft
                    Kultur = string.Empty;
                }
            }

            return System.IO.Path.Combine(pfad, Kultur);
        }

    }
}
