namespace WIFI.Buchandlung.Client.Models
{
    public class Personen : System.Collections.Generic.List<Person>
    {

    }
    public class Person : WIFI.Anwendung.Daten.GuidDatenObjekt
    {
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public string? Telefonnummer { get; set; }
        public string? Email { get; set; }
        public string? Straße { get; set; }
        public string? Ort { get; set; }
        public int? Plz { get; set; }
        public string? AusweisNr { get; set; }
        public override string ToString()
        {
            return $"Info About Person\n Vorname: {Vorname}";
        }
    }

}
