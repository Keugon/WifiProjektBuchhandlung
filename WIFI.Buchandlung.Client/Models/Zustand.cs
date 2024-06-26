namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt die Zustände der Artikel bereit
    /// </summary>
    /// <remarks>Die Liste ist für die Datenbindung
    /// vorbereitet, d.h. das CollectionChanged Ereignis
    /// ist implementiert</remarks>
    public class Zustände : System.Collections.Generic.List<Zustand>
    {
    }

    /// <summary>
    /// Stellt die Daten für die Zustände
    /// der Artikel bereit
    /// </summary>
    public class Zustand :System.Object
    {
        public int? ID { get; set; }
        public string? Bezeichnung { get; set; }

    }
}
