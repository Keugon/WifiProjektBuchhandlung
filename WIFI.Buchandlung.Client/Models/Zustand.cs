﻿namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt eine typsichere Auflistung von Zustand Objekten bereit
    /// </summary>
    /// <remarks>Die Liste ist für die Datenbindung
    /// vorbereitet, d.h. das CollectionChanged Ereignis
    /// ist implementiert</remarks>
    public class Zustände : System.Collections.Generic.List<Zustand>
    {

    }

    /// <summary>
    /// Beschreibt ein Zustand
    /// </summary>
    public class Zustand : WIFI.Anwendung.Daten.DatenObjekt
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