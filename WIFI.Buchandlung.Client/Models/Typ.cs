namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine typsichere Auflistung von Typ Objekten bereit.
    /// </summary>
    public class Typen : System.Collections.Generic.List<Typ>
    {

    }
    /// <summary>
    /// Beschreibt einen Typ
    /// </summary>
    public class Typ : WIFI.Anwendung.Daten.DatenObjekt
    {
        /// <summary>
        /// Ruft das ID ab oder legt es fest
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// Ruft die Bezeichnung ab oder legt diese fest
        /// </summary>
        public string? Bezeichnung { get; set; }
    }
}