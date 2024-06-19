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
        /// Gibt eine Liste von Artikeln aus der Datebank zurück
        /// </summary>
        /// <param name="suchParameter">SuchParameter nach Artikel.Bezeichnung</param>
        /// <param name="inventarNr">(Optional) Sucht nach InventarNr</param>
        /// <returns>Liste von Artikeln</returns>
        public Task<ArtikelListe> HoleArtikelListeAsync(string suchParameter, string inventarNr = null!)
        {
            //Todo ggf Refactor auf eine Überladene Methode anstatt optionalen parameter
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
                //optional InventarNr
                Befehl.Parameters.AddWithValue("@InventarNr", inventarNr);
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
        public Task<Personen> HolePersonenAsync(string suchParameter)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<Personen>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                Personen Rückmeldung = new Personen();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("PersonenSuche", Verbindung);
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
                    Rückmeldung.Add(new Person
                    {
                        ID = (System.Guid)Daten["ID"],
                        Vorname = (string)Daten["Vorname"],
                        Nachname = (string)Daten["Nachname"],
                        Plz = (int)Daten["PLZ"],
                        Ort = (string)Daten["Ort"],
                        Straße = (string)Daten["Straße"],
                        Telefonnummer = (string)Daten["TelNr"],
                        Email = (string)Daten["Email"],
                        AusweisNr = (string)Daten["AusweisNr"],
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
        /// Neuen Artikel in der Datenbank anlegen
        /// </summary>
        public Task<int> ArtikelAnlegen(Artikel artikelZumAnlegen)
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
                Befehl.Parameters.AddWithValue("@IDguid", artikelZumAnlegen.ID);
                Befehl.Parameters.AddWithValue("@inventarNr", artikelZumAnlegen.InventarNr);
                Befehl.Parameters.AddWithValue("@bezeichnung", artikelZumAnlegen.Bezeichnung);
                Befehl.Parameters.AddWithValue("@beschaffungspreis", artikelZumAnlegen.Beschaffungspreis);
                Befehl.Parameters.AddWithValue("@zustand", artikelZumAnlegen.Zustand);
                Befehl.Parameters.AddWithValue("@typ", artikelZumAnlegen.Typ);
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
        /// <summary>
        /// Legt eine neue Person in der Datenbank an
        /// </summary>
        /// <returns>return 1 oder 2 für update
        /// oder neu angelegt</returns>
        public Task<int> PersonAnlegen(Person personZumAnlegen)
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
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("PersonSpeichern", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;

                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@IDguid", personZumAnlegen.ID);
                Befehl.Parameters.AddWithValue("@vorname", personZumAnlegen.Vorname);
                Befehl.Parameters.AddWithValue("@nachname", personZumAnlegen.Nachname);
                Befehl.Parameters.AddWithValue("@plz", personZumAnlegen.Plz);
                Befehl.Parameters.AddWithValue("@ort", personZumAnlegen.Ort);
                Befehl.Parameters.AddWithValue("@straße", personZumAnlegen.Straße);
                Befehl.Parameters.AddWithValue("@telNr", personZumAnlegen.Telefonnummer);
                Befehl.Parameters.AddWithValue("@email", personZumAnlegen.Email);
                Befehl.Parameters.AddWithValue("@ausweisNr", personZumAnlegen.AusweisNr);
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
        /// <summary>
        /// Gibt die Entlehnungen einer Person oder aller Personen zurück
        /// </summary>
        /// <param name="personID">(Optional)GUID einer Person 
        /// um die Enlehnungen auf diese zu beschränken</param>
        /// <returns>Liste von Entlehnungen</returns>
        public Task<Entlehnungen> HoleEntlehnungenAsync(Guid personID)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<Entlehnungen>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                Entlehnungen Rückmeldung = new Entlehnungen();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("EntlehnungSuche", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@SuchParameter", personID);
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
                    Rückmeldung.Add(new Entlehnung
                    {
                        ID = (System.Guid)Daten["ID"],
                        InventarNr = (int)Daten["InventarNr"],
                        Ausleiher = (System.Guid)Daten["AusleiherNr"],
                        AusleihDatum = (DateTime)Daten["AusleihDatum"],
                        RückgabeDatum = (DateTime)Daten["RückgabeDatum"],
                        RückgabeZustand = (string)Daten["RückgabeZustand"],
                        Strafbetrag = (decimal)Daten["Strafbetrag"],
                        StrafbetragBemerkung = (string)Daten["StrafBetragBemerkung"]
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
        /// Legt eine neue Person in der Datenbank an
        /// </summary>
        /// <returns>return 1 oder 2 für update
        /// oder neu angelegt</returns>
        public Task<int> EntlehnungAnlegen(Entlehnung EntlehnungZumAnlegen)
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
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("EntlehnungSpeichern", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;

                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@id", EntlehnungZumAnlegen.ID);
                Befehl.Parameters.AddWithValue("@inventarNr", EntlehnungZumAnlegen.InventarNr);
                Befehl.Parameters.AddWithValue("@ausleiherNr", EntlehnungZumAnlegen.Ausleiher);
                Befehl.Parameters.AddWithValue("@ausleihDatum", EntlehnungZumAnlegen.AusleihDatum.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Befehl.Parameters.AddWithValue("@rückgabeDatum", EntlehnungZumAnlegen.RückgabeDatum.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Befehl.Parameters.AddWithValue("@rückgabeZustand", EntlehnungZumAnlegen.RückgabeZustand);
                Befehl.Parameters.AddWithValue("@strafbetrag", EntlehnungZumAnlegen.Strafbetrag);
                Befehl.Parameters.AddWithValue("@strafbetragBemerkung", EntlehnungZumAnlegen.StrafbetragBemerkung);

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

        //Todo (Datenbank) Mit dem aktuellen Datenbank design Artikel-> Entlehnung kan ein Artikel
        //nicht mehrere InventarNr auffassen somit ist ein artikel immer einzigartig und
        //die möglichkeit zu identifizieren ob von einem Artikel mehrere im Inventar
        //vorhanden sind ist schwieriger. Dies zu ändern bräuchte ein Datenbank redesign
        //und aller abläufe ebenfalls!
        //Es wäre dan ca. 
        //+-------------+       +-------------+       +-------------+
        //|   Artikel   |       |   Inventar  |       | Entlehnungen|
        //+-------------+       +-------------+       +-------------+
        //| ArtikelID   |1----->| InventarNr  |1----->| EntleihID   |
        //| Bezeichnung |       | ArtikelID   |       | InventarNr  |
        //+-------------+       +-------------+       | PersonID    |
        //                                            | AusleihDatum|
        //                                            | RueckgabeDatum|
        //                                            +-------------+
        //Neuer kommentar zwecks testbranch mergent
        // nueer kommenatart vol 30
        //florianTest
        // Update check wegen der Sicherstellung der Funktion
        //commit 1

    }
}
