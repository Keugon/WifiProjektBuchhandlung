namespace WIFI.Buchandlung.Client.Models
{
    public class ArtikelListe : System.Collections.Generic.List<Artikel>
    {

    }
    public class Artikel : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        public string? Bezeichnung { get; set; }
        public int? InventarNr { get; set; }
        public decimal? Beschaffungspreis { get; set; }
        //Todo Typ und Zustand sind hier String zwecks der Rückgabe
        //vom server aber beim hinsenden eigentlich String fehler in der logic? 
        public string? Typ { get; set; }
        public string? Zustand { get; set; }

    }
}
