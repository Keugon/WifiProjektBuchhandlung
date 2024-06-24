namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine Typsichere auflistung von InventarGegenstand Objekten bereit
    /// </summary>
    public class InventarGegenstände : System.Collections.Generic.List<InventarGegenstand>
    {

    }
    /// <summary>
    /// Beschreibt einen InventarGegenstand
    /// </summary>
    public class InventarGegenstand : WIFI.Buchandlung.Client.Models.Artikel
    {
        /// <summary>
        /// Ruft die Inventar Nummer ab oder legt diese Fest
        /// </summary>
        public int InventarNr { get; set; }
        //Todo Typ und Zustand sind hier String zwecks der RückgabeFensterÖffnen
        //vom server aber beim hinsenden eigentlich String fehler in der logic? 

        /// <summary>
        /// Ruft den Typ ab oder legt diese Fest
        /// </summary>
        public string? Typ { get; set; }
        /// <summary>
        /// Ruft den Zustand ab oder legt diese Fest
        /// </summary>
        public string? Zustand { get; set; }
        /// <summary>
        /// Ruft die Entlehung zum InventarGegenstand ab oder legt diese Fest
        /// </summary>
        public Entlehnung? Entlehnung { get; set; }

    }
}
