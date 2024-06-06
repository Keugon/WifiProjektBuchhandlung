using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client.Models
{
    /// <summary>
    /// Stellt einen Dienst zum Lesen und Schreiben
    /// von Daten
    /// aus einer SQL Server Datenbank bereit
    /// </summary>
    /// <remarks>Der ConnectionString wird aus der
    /// Infrastruktur bezogen</remarks>
    public class SqlServerController
        : WIFI.Anwendung.AppObjekt
    {
        /// <summary>
        /// Gibt die Unterstützten Länder zurück.
        /// </summary>
        /// <param name="sprachcode">Das CulturInfoKürzel
        /// für die Lokalisierung</param>
        /// <returns>Liste mit Länder aus der
        /// DatenQuelle</returns>
        public Task<WIFI.Buchandlung.Client.Models.ArtikelListe> HoleArtikelListeAsync()
        {
            /*
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<WIFI.Lotto.Daten.Länder>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                var Länder = new Daten.Länder();

                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("HoleLänder", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;

                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@sprache", sprachcode);
                //Damit das RDBMS die sql Anweisung nicht jedes Mals
                //analysiert, nur einmal und cachen ("Ausführungsplan = "1")
                Befehl.Prepare();
                //Grundsatz "Öffne Spät- schließe früh"
                Verbindung.Open();
                //Für Inser, Update und Delet
                //Befehl.ExecuteNonQuery();
                //Drittens - ein Datenobjekt für SELECT
                using var Daten
                    = Befehl.ExecuteReader(
                        System.Data.CommandBehavior
                        .CloseConnection);
                //Die Daten vom Reader in unsere 
                //Datentransferobjekte "mappen"
                while (Daten.Read())
                {
                    Länder.Add(new Lotto.Daten.Land
                    {
                        ID = (System.Guid)Daten["ID"],
                        Iso2 = (string)Daten["ISO2"],
                        Name = (string)Daten["Name"]
                    });
                }
                this.Kontext.Log.Erstellen
                ($"{this} hat {Länder.Count} unterstützte Länder gefunden");

                //Zum Testen von Multithreading in 
                //Debug Version "Schlafen"

#if DEBUG
                System.Threading.Thread.Sleep(2000);
#endif
                this.Kontext.Log.EndeMelden();
                return Länder;
            });
*/
            return null!;
        }
        /// <summary>
        /// Holt die Passenden Personne aus 
        /// der Datenbank mit den angegeben parameter
        /// </summary>
        /// <returns>Liste von Personen</returns>
        public Personen HolePersonenAsync()
        {
            Personen personen = new Personen()
            {
                new Person()
                {
                    Vorname ="Florian",
                    Nachname= "Jemand",
                    Adresse="Irgendwo",
                    Email= "florian@email.at",
                    Telefonnummer = 0900666666
                },
                 new Person()
                {
                    Vorname ="Markus",
                    Nachname= "AuchJemand",
                    Adresse="IrgendIrgendwo",
                    Email= "florian@guugle.at",
                    Telefonnummer = 0200666666
                }
            };


            return personen;
        }
        /// <summary>
        /// Neuen Artikel in der Datenbank anlegen
        /// </summary>
        public void ArtikelAnlegen()
        {

        }

    }
}
