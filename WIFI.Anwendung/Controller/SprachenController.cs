using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung.Controller
{
    /// <summary>
    /// Stellt einen Dienst zum 
    /// Lesen und Schreiben von
    /// Anwendungssprachen bereit
    /// </summary>
    internal class SprachenController 
        : Generisch.XmlController<Daten.Sprachen>
    {

        #region Standardsprachen aus den Ressourcen

        /// <summary>
        /// Gibt die Sprachen aus der Sprachen Xml Datei
        /// in den Ressourcen zurück.
        /// </summary>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Laden ein Fehler war</exception>
        public Daten.Sprachen HoleAusRessourcen()
        {
            // Ergebnisliste initialisieren
            var Sprachen = new Daten.Sprachen();

            // Die Ressourcedatei in ein XmlDocument laden
            var XmlDok = new System.Xml.XmlDocument();
            XmlDok.LoadXml(Properties.Resources.Sprachen);

            // Für jedes Element des Wurzelknotens
            foreach (System.Xml.XmlNode e in XmlDok.DocumentElement!.ChildNodes)
            {
                // die Attribute in ein neues Spracheobjekt
                // füllen (mappen) und dieses dem Ergebnis hinzufügen
                Sprachen.Add(
                    new Daten.Sprache
                    {
                        Code = e.Attributes!["code"]!.Value,
                        Name = e.Attributes!["name"]!.Value
                    });
            }
            
            // Ergebnisliste zurückgeben
            return Sprachen;

            // Hier ist automatisch XmlDok = null;
        }

        #endregion Standardsprachen aus den Ressourcen

    }
}
