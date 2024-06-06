using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    public class Personen : System.Collections.Generic.List<Person>
    {

    }
    public class Person
    {
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public int? Telefonnummer { get; set; }
        public string? Email { get; set; }
        public string? Adresse { get; set; }
        public override string ToString()
        {
            return $"Info About Person\n Vorname: {Vorname}";
        }
    }
    
}
