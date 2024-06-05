using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung.Controller.Generisch
{
    /// <summary>
    /// Stellt ein Dienst zum Lesen und
    /// Schreiben von Daten im Xml Format bereit
    /// </summary>
    /// <remarks>Es wird die Xml Serialisierung benutzt</remarks>
    public abstract class XmlController<T> : AppObjekt
    {
        /// <summary>
        /// Serialisiert die Daten in die 
        /// angegebene Datei
        /// </summary>
        /// <param name="pfad">Der vollständige Name
        /// der Datei, die zum Serialisieren benutzt werden soll</param>
        /// <param name="daten">Die Liste mit den Informationen,
        /// die im Xml Format geschrieben werden sollen</param>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Serialisieren ein Fehler war</exception>
        public void Schreiben(string pfad, T daten)
        {
            // Ein Objekt zum Schreiben der Daten
            // Damit unsere Umlaute richtig benutzt werden,
            // im UTF8 Zeichensatz
            using var Schreiber
                = new System.IO.StreamWriter(
                    pfad,
                    append: false,
                    System.Text.Encoding.UTF8);

            // Ein Xml Serializer Objekt
            var Xml = new System.Xml.Serialization
                .XmlSerializer(daten!.GetType());

            // Daten serialisieren
            Xml.Serialize(Schreiber, daten);

            Schreiber.Close();
            // wegen "using var Schreiber..." wird hier
            // das Dispose() aufgerufen

        }

        /// <summary>
        /// Gibt die deserialisierten Daten
        /// aus der Datei zurück
        /// </summary>
        /// <param name="pfad">Der vollständige Name
        /// der Datei, die zum Deserialisieren benutzt werden soll</param>
        /// <returns>Die Liste mit den Informationen,
        /// die sich in der Datei im Xml Format befinden</returns>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Serialisieren ein Fehler war</exception>
        public T Lesen(string pfad)
        {

            // Ein Objekt zum Lesen der Datei
            using var Leser = new System.IO.
                StreamReader(
                    pfad,
                    System.Text.Encoding.UTF8);

            // Ein Objekt zum Deserialisieren
            var Xml = new System.Xml.Serialization
                .XmlSerializer(typeof(T));

            // Deserialisierte Daten zurückgeben
            return (T)Xml.Deserialize(Leser)!;
            // wegen "using var Leser..." wird das Objekt
            // sauber geschlossen
        }
    }
}
