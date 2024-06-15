// Zum Aktivieren der WIFI Erweiterungen
using WIFI.Anwendung.Werkzeuge;

namespace WIFI.Anwendung;

/// <summary>
/// Unterstützt sämtliche 
/// WIFI Anwendungsobjekte
/// mit der Infrastruktur und 
/// weiteren Basisfunktionalitäten
/// </summary>
public abstract class AppObjekt : System.Object
{
    #region Infrastruktur

    /// <summary>
    /// Ruft das Objekt mit der Infrastruktur
    /// einer WIFI Anwendung ab oder legt
    /// dieses fest
    /// </summary>
    public Infrastruktur Kontext { get; set; } = null!;

    #endregion Infrastruktur

    #region FehlerAufgetreten-Ereignis

    // Dritter Schritt (oder erster bei System.EventHandler)
    // -> Das Ereignis deklarieren

    /// <summary>
    /// Wird ausgelöst, wenn im Objekt
    /// ein Problem eingetreten ist
    /// </summary>
    /// <remarks>Die Ursache befindet sich
    /// in den Ereignisdaten</remarks>
    public event FehlerAufgetretenEventHandler
        FehlerAufgetreten = null!;

    // Vierter Schritt (oder zweiter Schritt bei System.EventHandler)
    // -> Die Ereignisauslöser Methode

    /// <summary>
    /// Löst das Ereignis FehlerAufgetreten aus
    /// </summary>
    /// <param name="ex">Die Ausnahme, die
    /// den Fehler beschreibt</param>
    protected virtual void OnFehlerAufgetreten(System.Exception ex)
    {
        this.OnFehlerAufgetreten(
            new FehlerAufgetretenEventArgs(ex)
            );
    }

    /// <summary>
    /// Löst das Ereignis FehlerAufgetreten aus
    /// </summary>
    /// <param name="e">Ereignisdaten mit der Ursache</param>
    protected virtual void OnFehlerAufgetreten(
        FehlerAufgetretenEventArgs e)
    {
        // Wegen des Multithreadings...
        // Wir blockieren die Speicheradresse,
        // damit das Objekt von der Garbage Collection
        // nicht entfernt wird...
        var BehandlerKopie = this.FehlerAufgetreten;

        // Falls ein Ereignis-Behandler vorhanden ist
        if (BehandlerKopie != null)
        {
            // aufrufen...
            BehandlerKopie(this, e);
        }
    }

    #endregion FehlerAufgetreten-Ereignis

    #region Speicherorte

    /// <summary>
    /// Anwendungsweiter Cache für die Eigenschaft
    /// </summary>
    private static string _Anwendungspfad = null!;

    /// <summary>
    /// Ruft die vollständige Pfadangabe ab,
    /// aus der die Anwendung gestartet wurde
    /// </summary>
    protected string Anwendungspfad
    {
        get
        {
            if (AppObjekt._Anwendungspfad == null)
            {
                AppObjekt._Anwendungspfad =
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetEntryAssembly()!
                        .Location)!;
            }

            return AppObjekt._Anwendungspfad;
        }
    }

    /// <summary>
    /// Anwendungsweiter Cache für
    /// die Eigenschaft
    /// </summary>
    private static string _Datenpfad = null!;

    /// <summary>
    /// Ruft das Verzeichnis ab, wo
    /// ungefragt mit dem Serverprofil synchronisierte
    /// Daten gespeichert werden dürfen
    /// </summary>
    /// <remarks>Die Existenz des
    /// Verzeichnis wird sichergestellt</remarks>
    //20240410 Hr. Draxler: Todo - nicht erlaubte Zeichen prüfen
    protected string Datenpfad
    {
        get
        {
            if (AppObjekt._Datenpfad == null)
            {
                AppObjekt._Datenpfad
                    = System.IO.Path.Combine(
                        System.Environment.GetFolderPath(
                        Environment.SpecialFolder.
                            ApplicationData)

                        , this.HoleFirmenname()
                        , this.HoleProdukt()
                        , this.HoleVersion()

                        );
            }

            // Die Existenz sicherstellen
            // (falls ein Cleaner das Verzeichnis entfernt hat
            // oder es ein Neubenutzer ist)
            System.IO.Directory
                .CreateDirectory(AppObjekt._Datenpfad);

            return AppObjekt._Datenpfad;
        }
    }

    /// <summary>
    /// Anwendungsweiter Cache für
    /// die Eigenschaft
    /// </summary>
    private static string _LokalerDatenpfad = null!;

    /// <summary>
    /// Ruft das Verzeichnis ab, wo
    /// ungefragt Computer bezogene
    /// Daten gespeichert werden dürfen
    /// </summary>
    /// <remarks>Die Existenz des
    /// Verzeichnis wird sichergestellt</remarks>
    //20240404 Hr. Draxler: Todo - nicht erlaubte Zeichen prüfen
    protected string LokalerDatenpfad
    {
        get
        {
            if (AppObjekt._LokalerDatenpfad == null)
            {
                AppObjekt._LokalerDatenpfad
                    = System.IO.Path.Combine(
                        System.Environment.GetFolderPath(
                        Environment.SpecialFolder.
                            LocalApplicationData)

                        , this.HoleFirmenname()
                        , this.HoleProdukt()
                        , this.HoleVersion()

                        );
            }

            // Die Existenz sicherstellen
            // (falls ein Cleaner das Verzeichnis entfernt hat
            // oder es ein Neubenutzer ist)
            System.IO.Directory
                .CreateDirectory(AppObjekt._LokalerDatenpfad);

            return AppObjekt._LokalerDatenpfad;
        }
    }

    #endregion Speicherorte

    #region Internetzugriffe
    /// <summary>
    /// Internes Singleton Feld für die Eigenschaft
    /// </summary>
    private static System.Net.Http.HttpClient _HttpClient = null!;
    /// <summary>
    /// Ruft den Anwendungsweiten Dienst zum
    /// Arbeiten mit dem HTTPInternetProtokoll ab
    /// </summary>
    /// <remarks>
    /// Das Accept Language Header Feld ist
    /// auf die aktuelle Anwendungssprache 
    /// voreingestellt</remarks>
    protected System.Net.Http.HttpClient HttpClient
    {
        get
        {
            if (AppObjekt._HttpClient == null)
            {
                AppObjekt._HttpClient
                    = new System.Net.Http.HttpClient();
                this.Kontext.Log.Erstellen(
                    $"{this} hat den Dienst für HTTP " +
                    $"aufrufe initialisiert");

                AppObjekt._HttpClient.DefaultRequestHeaders
                    .Add(
                    "Accept-Language",
                    this.Kontext.Sprachen.AktuelleSprache.Code);
                this.Kontext.Log.Erstellen(
                    $"Die Sprache der Internet Antworten wude auf " +
                    $"{this.Kontext.Sprachen.AktuelleSprache.Code} " +
                    $"festgelegt");
            }
            return AppObjekt._HttpClient;
        }
    }

    #endregion  Internetzugriffe
}
