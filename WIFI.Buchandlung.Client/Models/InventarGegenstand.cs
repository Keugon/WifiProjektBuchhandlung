namespace WIFI.Buchandlung.Client.Models
{
    public class InventarGegenstände : System.Collections.Generic.List<InventarGegenstand>
    {

    }
    public class InventarGegenstand : WIFI.Buchandlung.Client.Models.Artikel
    {
        
        public int InventarNr { get; set; }
        
        //Todo Typ und Zustand sind hier String zwecks der Rückgabe
        //vom server aber beim hinsenden eigentlich String fehler in der logic? 
        public string? Typ { get; set; }
        public string? Zustand { get; set; }
        public Entlehnung? Entlehnung { get; set; }

    }
}
