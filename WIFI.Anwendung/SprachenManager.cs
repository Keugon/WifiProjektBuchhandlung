using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung;

/// <summary>
/// Stellt einen Dienst zum
/// Verwalten der Anwendungssprachen bereit
/// </summary>
public class SprachenManager : AppObjekt
{
    #region Unterstützte Sprachen

    /// <summary>
    /// Stellt sicher, dass die Inhalte
    /// zur gewählten aktuellen Sprache passen
    /// </summary>
    public void Aktualisieren()
    {
        // Code der aktuellen Sprache merken
        var Sprachkürzel = this.AktuelleSprache.Code;

        // Den Cache der Liste leeren,
        // damit beim nächsten Lesen die
        // Ressource benutzt wird
        this._Liste = null!;

        // Mit dem gemerkten Code
        // die aktuelle Sprache neu initialisieren
        this.AktuelleSprache 
            = this.Liste.Find(s => s.Code == Sprachkürzel)!;
    }

    /// <summary>
    /// Interner Cache für die Liste
    /// der unterstützten Sprachen
    /// </summary>
    private Daten.Sprachen _Liste = null!;

    /// <summary>
    /// Ruft die unterstützten
    /// Anwendungssprachen ab
    /// </summary>
    /// <remarks>Seit 20240403 werden die
    /// Sprachen alphabetisch sortiert</remarks>
    public Daten.Sprachen Liste
    {
        get
        {
            if (this._Liste == null)
            {
                try
                {
                    //20240403 Sortierte Rückgabe
                    //this._Liste = this.Controller
                    //    .HoleAusRessourcen();

                    this._Liste = new Daten.Sprachen();
                    this._Liste.AddRange(
                        from s in this.Controller.HoleAusRessourcen() 
                        orderby s.Name 
                        select s);

                }
                catch (System.Exception ex)
                {
                    // Damit wir nicht wiederholt
                    // in den Fehler laufen, eine
                    // leere Liste zurückgeben
                    this._Liste = new Daten.Sprachen();

                    this.OnFehlerAufgetreten(ex);
                }
                finally
                {
                    // Läuft immer - mit und
                    // ohne Fehler. Benötigt man,
                    // wenn im catch ein throw ist
                }
            }

            return this._Liste;
        }
    }

    #endregion Unterstützte Sprachen

    #region Datencontroller

    /// <summary>
    /// Interner Cache (Feld) für die Eigenschaft
    /// </summary>
    private Controller.SprachenController _Controller = null!;

    /// <summary>
    /// Ruft den Dienst zum Lesen und Schreiben
    /// der Anwendungssprachen ab
    /// </summary>
    private Controller.SprachenController Controller
    {
        get
        {
            if (this._Controller == null)
            {
                this._Controller = this.Kontext
                    .Produziere<WIFI.Anwendung.Controller.SprachenController>();
            }

            return this._Controller;
        }
    }

    #endregion Datencontroller

    #region Aktuelle Sprache

    /// <summary>
    /// Stellt im Betriebssystem die
    /// gewünschte Sprache der Anwendung ein
    /// </summary>
    /// <param name="iso2code">Das 2stellige
    /// .Net CultureInfo Kürzel der gewünschten Sprache
    /// case-insensitiv</param>
    /// <remarks>Sollte die Sprache nicht gefunden werden,
    /// wird Englisch benutzt. Die Sprache wird
    /// auch in der Eigenschaft AktuelleSprache festgelegt.
    /// Die Sprache der Zahlenformatierung wird nicht
    /// geändert und weiterhin aus dem Betriebssystem bezogen</remarks>
    public void Festlegen(string iso2code)
    {
        // Das Objekt der gewünschten Sprache finden
        var GewünschteSprache = this.Liste
            .Find(s => s.Code.Equals(
                iso2code,
                StringComparison.InvariantCultureIgnoreCase)
            );
        if (GewünschteSprache == null)
        {
            GewünschteSprache = this.Liste
                .Find(s => s.Code.Equals(
                    "en",
                    StringComparison.InvariantCultureIgnoreCase)
            );
        }

        // Falls die gewünschte Sprache
        // eine andere als die aktuelle ist
        if (this.AktuelleSprache.Code != GewünschteSprache!.Code)
        {
            // im Betriebssytem die neue Sprache einstellen
            System.Globalization.CultureInfo.CurrentUICulture
                = new System.Globalization
                    .CultureInfo(GewünschteSprache.Code);

            // die Sprache zur aktuellen Sprache machen
            this.AktuelleSprache = GewünschteSprache; 
            
            // Ressourcen neu laden
            this.Aktualisieren();


        }
    }

    /// <summary>
    /// Internes Feld für die Eigenschaft
    /// </summary>
    private Daten.Sprache _AktuelleSprache = null!;

    /// <summary>
    /// Ruft die derzeit ausgewählte
    /// Anwendungssprache der Oberfläche ab oder 
    /// legt diese fest
    /// </summary>
    /// <remarks>Als Standard wird die
    /// Sprache aus dem Betriebssystem
    /// benutzt, falls diese nicht vorhanden
    /// ist, Englisch</remarks>
    public Daten.Sprache AktuelleSprache
    {
        get
        {
            //Falls noch keine Sprache vorhanden ist
            if (this._AktuelleSprache == null)
            {
                //Betriebssystemsprache
                var OS = System.Globalization.CultureInfo
                    .CurrentUICulture.TwoLetterISOLanguageName;
                //CASE-SENSTIV!!! (CS)
                /*
                this._AktuelleSprache
                    = this.Liste.Find(s => s.Code == OS)!;
                */
                //CASE-INSENSTIV!!! (CI)
                this._AktuelleSprache
                    = this.Liste.Find(
                        s => s.Code.Equals(
                                OS,
                                StringComparison.InvariantCultureIgnoreCase))!;

                //Falls diese nicht vorhanden ist,
                //Englisch benutzen
                if (this._AktuelleSprache == null)
                {
                    this._AktuelleSprache
                        = this.Liste.Find(
                            s => s.Code.Equals(
                                "en",
                                StringComparison.InvariantCultureIgnoreCase))!;
                }
            }

            return this._AktuelleSprache;
        }
        set => this._AktuelleSprache = value;
    }

    #endregion Aktuelle Sprache
}
