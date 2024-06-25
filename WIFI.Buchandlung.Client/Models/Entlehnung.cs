namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine Typsicher auflistung Entlehnung-Objekten bereit
    /// </summary>
    public class Entlehnungen : System.Collections.Generic.List<Entlehnung>
    {

    }
    /// <summary>
    /// Beschreibt eine Entlehung
    /// </summary>
    public class Entlehnung : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        /// <summary>
        /// Ruft die Inventarnummer ab oder legt diese fest
        /// </summary>
        public int? InventarNr { get; set; }
        /// <summary>
        /// Ruft die GUID des Ausleihers ab oder legt diese fest
        /// </summary>
        public Guid? Ausleiher { get; set; }
        /// <summary>
        /// Ruft das Ausleih Datum ab oder legt diese fest
        /// </summary>
        public DateTime? AusleihDatum { get; set; }
        /// <summary>
        /// Ruft das Rückgabe ab oder legt diese fest
        /// </summary>
        public DateTime? RückgabeDatum { get; set; }
        /// <summary>
        /// Ruft den Rückgabe Zustand ab oder legt diese fest
        /// </summary>
        public string? RückgabeZustand { get; set; }
        /// <summary>
        /// Ruft den Strafbetrag ab oder legt diese fest
        /// </summary>
        public decimal? Strafbetrag { get; set; }
        /// <summary>
        /// Ruft die Strafbetrag Bemerkung ab oder legt diese fest
        /// </summary>
        public string? StrafbetragBemerkung { get; set; }
        /// <summary>
        /// Ruft das Person Objekt zum dieser Entlehnung ab oder legt dieses fest
        /// </summary>
        public Person? AusleiherDaten { get; set; }
    }
}
