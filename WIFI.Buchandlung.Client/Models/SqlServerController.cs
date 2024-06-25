using System.Transactions;

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
        /// <returns>Liste von Artikeln</returns>
        public Task<ArtikelListe> HoleArtikelAsync(string suchParameter)
        {
            //Todo ggf Refactor auf eine Überladene Methode anstatt optionalen parameter
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<ArtikelListe>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                var Rückmeldung = new ArtikelListe();
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
                        ID = (Guid)Daten["ID"],
                        Bezeichnung = (string)Daten["Bezeichnung"],
                        Beschaffungspreis = (decimal)Daten["Beschaffungspreis"]
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
        /// Gibt eine Liste von InventarGegenstände aus der Datebank zurück
        /// </summary>
        /// <param name="suchParameter">SuchParameter nach Artikel.Bezeichnung</param>
        /// <param name="inventarNr">(Optional) Sucht nach InventarNr</param>
        /// <returns>Liste von InventarGegenstände</returns>
        public Task<InventarGegenstände> HoleInventarGegenständeAsync(string suchParameter, int? inventarNr = null!)
        {
            //Todo ggf Refactor auf eine Überladene Methode anstatt optionalen parameter
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<InventarGegenstände>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                InventarGegenstände Rückmeldung = new InventarGegenstände();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("InventarGegenstandSuche", Verbindung);
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
                    Rückmeldung.Add(new InventarGegenstand
                    {
                        InventarNr = (int)Daten["InventarNr"],
                        Bezeichnung = (string)Daten["Bezeichnung"],
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
        /// Gibt eine Liste von InventarGegenstände aus der Datebank zurück
        /// </summary>
        /// <param name="artikelGuid">GUID des Artikels dessen 
        /// InventarGegenstände geholt werden sollen</param>
        /// <returns>Liste von InventarGegenstände</returns>
        public Task<InventarGegenstände> HoleInventarGegenständeAsync(Guid artikelGuid)
        {
            //Todo ggf Refactor auf eine Überladene Methode anstatt optionalen parameter
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<InventarGegenstände>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                InventarGegenstände Rückmeldung = new InventarGegenstände();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("InventarGegenstandSuche", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@ArtikelGUID", artikelGuid);

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
                    Rückmeldung.Add(new InventarGegenstand
                    {
                        InventarNr = (int)Daten["InventarNr"],
                        Bezeichnung = (string)Daten["Bezeichnung"],
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
        /// Legt einen neuen InventarGegenstände in der Datenbank an oder überschreibt ihn
        /// </summary>
        /// <param name="artikelZumAnlegen">Artikel der bei dem InventarGegenstand hinterlegt werden soll</param><remarks>
        /// Es wird auf InventarNr geprüft wenn ein Update eines InventarGegenstände durchgeführt werden soll,
        /// danach auf Artikel GUID falls ein neuer InventarGegenstände mit vorhandenen Artikel angelegt wird,
        /// zuletzt wird ein neuer InventarGegenstände mit neuen Artikel angelegt</remarks>
        /// <returns>
        /// 1 = InventarGegenstände updated via InventarNr, 
        /// 2 = InventarGegenstände und mit angegeben Artikel wurde angelegt,
        /// 3 = Neuer InventarGegenstände und neuer Artikel wurde angelegt</returns>
        public Task<int> InventarGegenstandAnlegen(InventarGegenstand artikelZumAnlegen)
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
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("InventarGegenstandSpeichern", Verbindung);
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
                        RückgabeDatum = Daten["RückgabeDatum"] == DBNull.Value ? (DateTime?)null : (DateTime)Daten["RückgabeDatum"],
                        RückgabeZustand = Daten["RückgabeZustand"] == DBNull.Value ? (string?)null : (string)Daten["RückgabeZustand"],
                        Strafbetrag = Daten["Strafbetrag"] == DBNull.Value ? (decimal?)null : (decimal)Daten["Strafbetrag"],
                        StrafbetragBemerkung = Daten["StrafBetragBemerkung"] == DBNull.Value ? (string?)null : (string)Daten["StrafBetragBemerkung"]
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
        /// Gibt die Entlehnungen von Personen zurück deren "Gebüh bereits abgelaufen sind
        /// </summary>
        /// <param name="gebührenFreieTage">(Optional)Es kan eine bestimmte anzahl an Tagen
        /// vom Ausleih Datum ab angegeben werden sont gilt der 
        /// wert des aktuell gültigen Gebührensatzes</param>
        /// <returns>Liste von Entlehnungen</returns>
        public Task<Entlehnungen> HoleÜberfälligeEntlehnungenAsync(int? gebührenFreieTage = null)
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
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("EntlehnungÜberfällig", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@GebührenFreieTage", gebührenFreieTage);
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
                        ID = (System.Guid)Daten["IDEntlehnung"],
                        InventarNr = (int)Daten["InventarNr"],
                        Ausleiher = (System.Guid)Daten["AusleiherNr"],
                        AusleihDatum = (DateTime)Daten["AusleihDatum"],
                        AusleiherDaten = new Person
                        {
                            ID = (System.Guid)Daten["ID"],
                            Vorname = (string)Daten["Vorname"],
                            Nachname = (string)Daten["Nachname"],
                            Telefonnummer = (string)Daten["TelNr"],
                            Email = (string)Daten["Email"],
                            Straße = (string)Daten["Straße"],
                            Ort = (string)Daten["Ort"],
                            Plz = (int)Daten["PLZ"],
                            AusweisNr = (string)Daten["AusweisNr"]
                        }
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
        /// Gibt die letzte Entlehnung des Angegebenen InventarNr zurück
        /// </summary>
        /// <param name="inventarNr">inventarNr des InventarGegenstands 
        /// dessen letzte Entlehung gebrauchtr wird</param>
        /// <returns>Eine Entlehnung zum InventarGegenstand</returns>
        public Task<Entlehnung> HoleEntlehnungAsync(int inventarNr)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<Entlehnung>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                var Rückmeldung = new Entlehnung();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("EntlehnungSuche", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@InventarNr", inventarNr);
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
                if (Daten.Read())
                {
                    Rückmeldung = new Entlehnung
                    {
                        ID = (System.Guid)Daten["ID"],
                        InventarNr = (int)Daten["InventarNr"],
                        Ausleiher = (System.Guid)Daten["AusleiherNr"],
                        AusleihDatum = (DateTime)Daten["AusleihDatum"],
                        RückgabeDatum = Daten["RückgabeDatum"] == DBNull.Value ? (DateTime?)null : (DateTime)Daten["RückgabeDatum"],
                        RückgabeZustand = Daten["RückgabeZustand"] == DBNull.Value ? (string?)null : (string)Daten["RückgabeZustand"],
                        Strafbetrag = Daten["Strafbetrag"] == DBNull.Value ? (decimal?)null : (decimal)Daten["Strafbetrag"],
                        StrafbetragBemerkung = Daten["StrafBetragBemerkung"] == DBNull.Value ? (string?)null : (string)Daten["StrafBetragBemerkung"]
                    };
                }
                /*Kein return nur daten
                Rückmeldung = (int)rückmeldungParameter.Value;
                */
                this.Kontext.Log.EndeMelden();
                return Rückmeldung;
            });

        }
        /// <summary>
        /// Gibt eine Liste von Zustands-Objekten zurück
        /// </summary>
        /// <returns></returns>
        public Task<Zustände> HoleZuständeAsync()
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<Zustände>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                Zustände Rückmeldung = new Zustände();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("HoleZustände", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;

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
                    Rückmeldung.Add(new Zustand
                    {
                        ID = (int)Daten["ID"],
                        Bezeichnung = (string)Daten["Bezeichnung"]
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
        /// Legt eine neue Entlehnung in der Datenbank an
        /// </summary>
        /// <param name="EntlehnungZumAnlegen">
        /// Entlehnungs objekt das zum anlegen benutzt werden soll,
        /// RückgabeDatum,Zustand,strafbetrag,strafbetrag bemerkung werden zum anlegen nicht benötigt und sollten NULL sein</param>
        /// <returns>1 für angelegt 2 für updated</returns>
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
                Befehl.Parameters.AddWithValue("@ausleihDatum", EntlehnungZumAnlegen.AusleihDatum!.Value);
                Befehl.Parameters.AddWithValue("@rückgabeDatum", EntlehnungZumAnlegen.RückgabeDatum!.HasValue ? EntlehnungZumAnlegen.RückgabeDatum!.Value : null);
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
        /// <summary>
        /// Gibt die Aktuelle Gebühr zurück
        /// </summary>
        /// <param name="suchDatum">(Optional)Ein Datum von dem 
        /// Aus die gültige Gebühr zurückgegeben werden soll</param>
        /// <returns>Gebühr</returns>
        public Task<Gebühr> HoleAktuelleGebührAsync(DateTime? suchDatum = null)
        {
            //Das Holen als TAP Thread Laufen lassen
            return System.Threading.Tasks.Task<Gebühr>.Run(() =>
            {
                this.Kontext.Log.StartMelden();
                //Für das Ergebnis
                var Rückmeldung = new Gebühr();
                //Erstens - ein Verbindungsobjekt 
                using var Verbindung = new Microsoft.Data.SqlClient.SqlConnection(this.Kontext.Verbindungszeichenfolge);
                //Zweitens - ein Befehlsobjekt
                //(Reicht für Insert, Update und Delet)
                using var Befehl = new Microsoft.Data.SqlClient.SqlCommand("AktuelleGebühren", Verbindung);
                //Mitteilen das wir kein SQL direkt haben
                Befehl.CommandType = System.Data.CommandType.StoredProcedure;
                //Damit wir SQL Injection sicher sind..
                Befehl.Parameters.AddWithValue("@SuchDatum", suchDatum);
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
                if (Daten.Read())
                {
                    Rückmeldung = new Gebühr
                    {
                        LfdNr = (int)Daten["LfdNr"],
                        GültigAb = (DateTime)Daten["GültigAb"],
                        Strafgebühr = (decimal)Daten["Strafgebühr"],
                        ErsatzgebührFaktor = (double)Daten["ErsatzgebührFaktor"],
                        GebührenFreieTage = (int)Daten["GebührenFreieTage"]
                    };
                }
                /*Kein return nur daten
                Rückmeldung = (int)rückmeldungParameter.Value;
                */
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

        //Todo(Datenbank) Typ muss zu Artikel wandern da dieser auch für jeden InventargGegenstand gleich sein wird!
        //Todo(InventarGegenstände) Ändern auf der ArtikelSuche seite mittels kontext menü und popout wie PersonenKarteiÖffnen
        //Todo(Artikel) ggf(Ongün)Ändern auf der ArtikelSuche seite mittels kontext menü und popout wie PersonenKarteiÖffnen
        //Todo(ArtikelSuche) bei nicht finden Vorschläge entweder direkt in Artikel oder in separate table aber warscheinlich in Artikel mit Typ wunsch
        //Todo(ArtikelAnlegen) (Ongün) Zustand und Typ Listen von der Datenbank ziehen und in Dropdownlisten umsetzten
        //Todo(Bestands/Austands) Liste ggf(Ongün)
        //Todo(CSV) MahnungsListe


    }
}
