using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        public Task<ArtikelListe> HoleArtikelListeAsync(string suchParameter)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<ArtikelListe>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                ArtikelListe Rückmeldung = new ArtikelListe();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("ArtikelSuche", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@SuchParameter", suchParameter);
                /* kein Return Value nur daten
var rückmeldungParameter = new Microsoft.Data.SqlClient.SqlParameter("@Rückmeldung", System.Data.SqlDbType.Int)
{
Direction = System.Data.ParameterDirection.Output
};
Befehl.Parameters.Add(rückmeldungParameter);
*/
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
                    Rückmeldung.Add(new Artikel
                    {
                        ID = (System.Guid)Daten["ID"],
                        Bezeichnung = (string)Daten["Bezeichnung"],
                        InventarNr = (int)Daten["InventarNr"],
                        Beschaffungspreis = (decimal)Daten["Beschaffungspreis"],
                        Zustand = (string)Daten["Zustand"],
                        Typ = (string)Daten["Typ"]
                    });
                }
                /*Kein return nur daten
                Rückmeldung = (int)rückmeldungParameter.Value;
                */
                this.Kontext.Log.EndeMelden();
                return Rückmeldung;
            });
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
        public Task<int> ArtikelAnlegen(Guid guid, string bezeichnung, int inventarNr = 0, decimal beschaffungspreis = 0, int zustand = 1, int typ = 1)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<int>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                int Rückmeldung = 0;
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("ArtikelSpeichern", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;

                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@IDguid", guid);
                Befehl.Parameters.AddWithValue("@inventarNr", inventarNr);
                Befehl.Parameters.AddWithValue("@bezeichnung", bezeichnung);
                Befehl.Parameters.AddWithValue("@beschaffungspreis", beschaffungspreis);
                Befehl.Parameters.AddWithValue("@zustand", zustand);
                Befehl.Parameters.AddWithValue("@typ", typ);
                //Rückmeldung
                var rückmeldungParameter = new Microsoft.Data.SqlClient.SqlParameter("@Rückmeldung", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                Befehl.Parameters.Add(rückmeldungParameter);
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
                Rückmeldung = (int)rückmeldungParameter.Value;
                this.Kontext.Log.EndeMelden();
                return Rückmeldung;
            });
        }

    }
}
