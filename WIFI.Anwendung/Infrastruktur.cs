using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung;

/// <summary>
/// Stellt den Anwendungskontext
/// für eine WIFI Anwendung bereit
/// </summary>
/// <remarks>In der Infrastruktur
/// befinden sich Informationen,
/// die überall bekannt sein müssen.
/// Bei einer neuen WIFI Anwendung
/// mit einem Objekt dieser Klasse beginnen
/// und alle anderen Objekte der Anwendung
/// mit Produziere&lt;T&gt; erstellen,
/// damit diese Infrastruktur überall
/// bekannt ist.</remarks>
public class Infrastruktur : System.Object
{
    #region Protokolldienst
    /// <summary>
    /// Cache für Internes Feld
    /// </summary>
    private ProtokollManager _Log = null!;
    /// <summary>
    /// Ruft den Dienst zum Verwalten 
    /// des Anwendungsprotokoll ab
    /// </summary>
    public ProtokollManager Log
    {
        get
        {
            if (this._Log == null)
            {
                this._Log
                    = this.Produziere<ProtokollManager>();
                //Weil die produziere Methode nicht direkt 
                //auf das Protokoll zugreifen kann, wegen rekursion
                this._Log.Erstellen($"{this._Log} wurde Produziert");

            }
            return this._Log;
        }
    }

    #endregion Protokolldienst
    #region Sprachendienst

    /// <summary>
    /// Ruft den Dienst zum Verwalten
    /// der Anwendungssprachen ab
    /// </summary>
    public SprachenManager Sprachen
    {
        get
        {
            if (this._Sprachen == null)
            {
                this._Sprachen
                    = this.Produziere<SprachenManager>();
            }


            return this._Sprachen;
        }
    }

    /// <summary>
    /// Interner Cache für den Dienst
    /// zum Verwalten der Anwendungssprachen
    /// </summary>
    private SprachenManager _Sprachen = null!;

    #endregion Sprachendienst

    #region Objektfabrik
    /// <summary>
    /// Gibt ein initialisiertes Anwendungsobjekt zurück
    /// </summary>
    /// <param name="typ">Eine Type, der AppObjekt 
    /// erweitert und einen öffentlichen Konstruktor
    /// ohne Parameter besitzt</param>
    /// <returns>Ein Objekt mit eingestellter
    /// Infrastruktur</returns>
    /// <remarks>Ergänzt in 2024.5.2.0</remarks>
    public AppObjekt Produziere(System.Type typ)
    {
        //Die generisches Methode Produziere suchen
        var FabrikMethode 
            = this.GetType().GetMethod(
                "Produziere",new Type[] { })!;
        //Diese mit dem Typ aufrufen
        //und das Ergebniss zurückgeben
        return (FabrikMethode.MakeGenericMethod(typ).Invoke(this, null) as AppObjekt)!;
    }

    /// <summary>
    /// Gibt ein initialisiertes Anwendungsobjekt zurück
    /// </summary>
    /// <typeparam name="T">Eine Klasse, die AppObjekt 
    /// erweitert und einen öffentlichen Konstruktor
    /// ohne Parameter besitzt</typeparam>
    /// <returns>Ein Objekt mit eingestellter
    /// Infrastruktur</returns>
    public T Produziere<T>() where T : AppObjekt, new()
    {

        T NeuesObjekt = new T();

        // Die Infrastruktur an 
        // das neue Objekt übergeben
        NeuesObjekt.Kontext = this;

        // Nur für die Entwicklerversion
        // einen Protokolleintrag im Studio (Ausgabefenster),
        // dass ein Objekt produziert wurde
        // und einen Fehlerbehandler
#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"{NeuesObjekt} wurde produziert...");

        NeuesObjekt.FehlerAufgetreten += (sender, e) =>
                        System.Diagnostics.Debug.WriteLine(
                            $"FEHLER! {NeuesObjekt} hat " +
                            $"eine Ausnahme " +
                            $"\"{e.Ursache.Message}\" " +
                            $"ausgelöst!");
        //Einen Protokolleintrag erstellen
        //wenn wir nicht gerade selbst das protokoll sind
        if (NeuesObjekt is not WIFI.Anwendung.ProtokollManager)
        {
            this.Log.Erstellen($"{NeuesObjekt} wurde Produziert");
        }

#endif
        //Sollte das Anwendungsobjekt auf einen
        //Fehler aufgelaufen sein, diesem im 
        //Anwendungsprotokoll hinterlegen
        NeuesObjekt.FehlerAufgetreten += (sender, e) =>
                        this.Log.Erstellen(
                            $"FEHLER! {NeuesObjekt} hat " +
                            $"eine Ausnahme " +
                            $"\"{e.Ursache.Message}\" " +
                            $"ausgelöst!", Daten.ProtokolleintragTyp.Fehler);
        // TODO - hier weitere Produktionsschritte ergänzen

        return NeuesObjekt;
    }

    #endregion Objektfabrik

    #region Fensterdienst

    /// <summary>
    /// Internes Feld für die Eigenschaft
    /// </summary>
    private FensterManager _Fenster = null!;

    /// <summary>
    /// Ruft den Dienst zum Verwalten
    /// der Anwendungsfenster ab
    /// </summary>
    public FensterManager Fenster
    {
        get
        {
            if (this._Fenster == null)
            {
                this._Fenster = this.Produziere<FensterManager>();
            }

            return this._Fenster;
        }
    }

    #endregion Fensterdienst

    #region Für Datenbankzugriff
    //Wegen des Designs zum Feststellen vom Anwendungspfad
    private class ZurUnterstützung : AppObjekt
    {
        public string BasisPfad => this.Anwendungspfad;
    }
    /// <summary>
    /// Internes Feld für die Eigenschaft
    /// </summary>
    private string _Verbindungszeichenfolge = string.Empty;
    /// <summary>
    /// Ruft den ConnectionString der Anwendungsdatenbank ab 
    /// oder legt diesen fest
    /// </summary>
    public string Verbindungszeichenfolge
    {
        get => this._Verbindungszeichenfolge;
        set => this._Verbindungszeichenfolge
            = VerbindungszeichenfolgePrüfen(value);

    }
    /// <summary>
    /// Gibt einen geprüften SQL Server ConnectionString 
    /// zurück
    /// </summary>
    /// <param name="einstellung">Die Verbindungszeichenfolge
    /// die geprüft werden soll</param>
    /// <returns>Relative Pfadangaben werden bezogen
    /// auf das Anwendungsverzeichniss absolut 
    /// eingestellt</returns>
    /// <exception cref="System.Exception">
    /// Tritt auf wenn Fehler in der Konfigurationsangabe vorliegt
    /// </exception>
    private string VerbindungszeichenfolgePrüfen(string einstellung)
    {
        //Ist eine Datenbankdatei angehängt?
        const string DateiKennung = "AttachDBfilename";
        if (einstellung.Contains(DateiKennung, StringComparison.InvariantCultureIgnoreCase))
        {
            //Wenn ja
            //Die Einstellung AttachDBfilename korrigieren
            var Einstellungen = einstellung.Split(';');
            var Datei = Einstellungen
                .Where(e => e.Contains(
                    DateiKennung,
                    StringComparison
                    .InvariantCultureIgnoreCase))
                .First().Split('=')[1];
            if (!System.IO.Path.IsPathRooted(Datei))
            {
                //Weil hier in der Infrastruktur 
                //der Anwendungspfad eines Appobjekts
                //nicht greifbar ist
                var Info = new ZurUnterstützung();
                var NeuerPfad
                    = System.IO.Path
                    .GetFullPath(
                        Datei
                        , Info.BasisPfad);
                //die alte Einstellung mit
                //dem neuen Pfad ersetzten
                einstellung = einstellung
                    .Replace(Datei,NeuerPfad);
            }
        }
        this.Log.Erstellen($"Datenbank Einstellung=\"({einstellung})\"");
        //unverändert zurückgeben
        return einstellung;
    }
    #endregion Für Datenbankzugriff
}
