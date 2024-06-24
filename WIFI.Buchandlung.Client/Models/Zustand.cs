namespace WIFI.Buchandlung.Client.Models
{
    public class Zustände : System.Collections.Generic.List<Zustand>
    {

    }
    public class Zustand : WIFI.Anwendung.Daten.DatenObjekt
    {
        public int? ID { get; set; }
        public string? Bezeichnung { get; set; }
    }
}