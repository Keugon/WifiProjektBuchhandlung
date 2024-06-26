namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Eine Typsichere Liste von Zustan-Objekten
    /// </summary>

    public class Zustände : System.Collections.Generic.List<Zustand>
    {

    }

    /// <summary>
    /// Beschreibt einen Zustand eines InventarGegenstands
    /// </summary>
    public class Zustand : WIFI.Anwendung.Daten.DatenObjekt
    {
        /// <summary>

        /// Die Nummerische Zuordnung des Zustands
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// Die Wörtliche Beschreibung des Zustands
        /// </summary>
        public string? Bezeichnung { get; set; }

    }
}