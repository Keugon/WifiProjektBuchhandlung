namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine Typsichere Auflistung von Person-Objekten bereit
    /// </summary>
    public class Personen : System.Collections.Generic.List<Person>
    {

    }
    /// <summary>
    /// Beschreibt eine Person
    /// </summary>
    public class Person : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        /// <summary>
        /// Ruft den Vorname ab oder legt diese Fest
        /// </summary>
        public string? Vorname { get; set; }
        /// <summary>
        /// Ruft den Nachnamen ab oder legt diese Fest
        /// </summary>
        public string? Nachname { get; set; }
        /// <summary>
        /// Ruft die Telefonnummer ab oder legt diese Fest
        /// </summary>
        public string? Telefonnummer { get; set; }
        /// <summary>
        /// Ruft die Emailadresse ab oder legt diese Fest
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Ruft die Straße ab oder legt diese Fest
        /// </summary>
        public string? Straße { get; set; }
        /// <summary>
        /// Ruft den Ort ab oder legt diese Fest
        /// </summary>
        public string? Ort { get; set; }
        /// <summary>
        /// Ruft die Postleitzahl ab oder legt diese Fest
        /// </summary>
        public int? Plz { get; set; }
        /// <summary>
        /// Ruft die Ausweisnummer ab mit der auch das Geburtsdatum
        /// beim Anlegen kontrolliert wurde ab oder legt diese Fest
        /// </summary>
        public string? AusweisNr { get; set; }

    }

}
