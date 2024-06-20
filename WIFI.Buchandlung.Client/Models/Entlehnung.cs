namespace WIFI.Buchandlung.Client.Models
{
    public class Entlehnungen : System.Collections.Generic.List<Entlehnung>
    {

    }
    public class Entlehnung : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        public int? InventarNr { get; set; }
        public Guid? Ausleiher { get; set; }
        public DateTime? AusleihDatum { get; set; }
        public DateTime? RückgabeDatum { get; set; }
        public string? RückgabeZustand { get; set; }
        public decimal? Strafbetrag { get; set; }
        public string? StrafbetragBemerkung { get; set; }
    }
}
