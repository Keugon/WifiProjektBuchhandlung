using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung
{
    /// <summary>
    /// Stellt einen Dienst bereit
    /// zum Verwalten der Positionen
    /// von Anwendungsfenstern
    /// </summary>
    public class FensterManager : AppObjekt
    {
        #region Win32Wrapper
        private const int SM_CMONITORS = 80;
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        /// <summary>
        /// Gibt eine gewünschte Information
        /// über die Bildschirmkonfiguration
        /// zurück.
        /// </summary>
        /// <param name="info">Eine Ganzzahl mit der
        /// die Rückgabe gesteuert wird</param>
        /// <returns></returns>
        /// <remarks>Details im Handbuch 
        /// https://learn.microsoft.com/de-de/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        /// </remarks>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static  int GetSystemMetrics(int info);

        /// <summary>
        /// Ruft eine Zusatzinformation zum
        /// unterscheiden der Fensternamen 
        /// bezogen auf die Bildschirm 
        /// konfiguration ab.
        /// </summary>
        /// <remarks>Die Information ist nicht
        /// gecached weil sich zur Laufzeit die 
        /// Konfiguration ändern kann</remarks>
        private string Monitorschlüssel
            => $"_M{GetSystemMetrics(SM_CMONITORS)}" +
            $"_{GetSystemMetrics(SM_CXSCREEN)}" +
            $"x{GetSystemMetrics(SM_CYSCREEN)}";
        

        #endregion Win32Wrapper

        #region Enthaltene Daten
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Daten.Fensterinfos _Liste = null!;

        /// <summary>
        /// Ruft die verwalteten Fensterinfos ab
        /// </summary>
        public Daten.Fensterinfos Liste
        {
            get
            {
                if (this._Liste == null)
                {
                    this._Liste = this.Öffnen();
                }

                return this._Liste;
            }
        }

        #endregion Enthaltene Daten

        #region Zum Benutzen des Dienstes

        /// <summary>
        /// Fügt dem FensterManager ein neues
        /// Fenster hinzu oder aktualisiert die Daten
        /// </summary>
        /// <param name="fenster">Ein Fensterinfo Objekt
        /// mit den Daten eines Fensters</param>
        /// <remarks>Als Schlüssel wird die Name-Eigenschaft
        /// vom Fensterinfo Objekt benutzt</remarks>
        public void Hinterlegen(Daten.Fensterinfo fenster)
        {
            //Zum Unterscheiden unterschiedlicher 
            //Bildschirmkonfiguartionne (2024.3.01)
            fenster.Name += this.Monitorschlüssel;
            // Gibt's das Fenster schon?
            var AlteInfo = this.Liste
                .Find(f => f.Name == fenster.Name);

            // Wenn nein, hinzufügen und fertig
            if (AlteInfo == null)
            {
                this.Liste.Add(fenster);
            }
            else
            {
                // Wenn ja, die Daten aktualisieren

                // Den Zustand immer
                AlteInfo.Zustand = fenster.Zustand;

                // Die Positionsdaten nur, wenn
                // neuere vorhanden sind

                // Original - herkömmliche Binärentscheidung
                if (fenster.Links != null)
                {
                    AlteInfo.Links = fenster.Links;
                }

                // Verbesserung - Binärentscheidung ?: als Funktion
                AlteInfo.Oben 
                    = fenster.Oben.HasValue ? fenster.Oben : AlteInfo.Oben;

                // Falls-Null-Operator ??
                AlteInfo.Breite= fenster.Breite ?? AlteInfo.Breite;
                AlteInfo.Höhe = fenster.Höhe ?? AlteInfo.Höhe;

            }
        }

        /// <summary>
        /// Gibt die Positionsdaten für 
        /// das gewünschte Fenster zurück
        /// </summary>
        /// <param name="fensterName">Der Schlüssel
        /// des Fensters, von dem die Positionsdaten
        /// benötigt werden</param>
        /// <returns>Null, falls das Fenster
        /// nicht vorhanden ist</returns>
        public Daten.Fensterinfo? Abrufen(string fensterName)
        {
            //Seit (2024.3.0.1) wird die Bildschirm
            //konfiguration berücksichtigt
            fensterName += this.Monitorschlüssel;
            //Weil in der Oberfläche der
            //interne Name nie sichtbar ist, case-sensitiv
            return this.Liste.Find(f => f.Name == fensterName);
        }

        #endregion Zum Benutzen des Dienstes

        #region Zum Speichern und Wiederherstellen

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Controller.FensterdatenController _Controller = null!;

        /// <summary>
        /// Ruft den Dienst zum Schreiben
        /// und Lesen der verwalteten Fensterposition ab
        /// </summary>
        /// <remarks>Es wird ein Xml Serialisierer benutzt</remarks>
        private Controller.FensterdatenController Controller
        {
            get
            {
                if (this._Controller == null)
                {
                    this._Controller = this.Kontext
                        .Produziere<Anwendung.Controller.FensterdatenController>();
                }

                return this._Controller;
            }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private string _Standardpfad = null!;

        /// <summary>
        /// Ruft den vollständigen Dateinamen
        /// zum Speichern und Öffnen der
        /// verwalteten Fenster ab.
        /// </summary>
        /// <remarks>Er befindet sich im 
        /// Lokalen Datenpfad des Benutzerprofils
        /// und wird um den Firmennamen, den
        /// Produktnamen und die Version aus
        /// der Anwendung ergänzt</remarks>
        public string Standardpfad
        {
            get
            {
                if (this._Standardpfad == null)
                {
                    this._Standardpfad
                        = System.IO.Path.Combine(

                            this.LokalerDatenpfad
                            , "Fensterpositionen.xml"

                            );
                }

                return this._Standardpfad;
            }
        }

        /// <summary>
        /// Schreibt die verwalteten Fenster
        /// in eine Datei.
        /// </summary>
        /// <remarks>Als Speicherort wird der Standardpfad benutzt</remarks>
        public void Speichern()
        {
            try
            {
                this.Controller.Schreiben(
                    this.Standardpfad, 
                    this.Liste);
            }
            catch (System.Exception ex)
            {
                this.OnFehlerAufgetreten(ex);
            }
        }

        /// <summary>
        /// Gibt die Liste mit den
        /// verwalteten Fenstern aus
        /// der mit Speichern erstellten
        /// Datei zurück
        /// </summary>
        /// <returns>Den Inhalt der gespeicherten Fenster
        /// oder eine leere Liste, wenn die
        /// Datei nicht vorhanden war</returns>
        /// <remarks>Als Speicherort wird der
        /// Standardpfad benutzt</remarks>
        protected Daten.Fensterinfos Öffnen()
        {
            Daten.Fensterinfos Ergebnis = null!;

            try
            {
                Ergebnis = this.Controller
                    .Lesen(this.Standardpfad);
            }
            catch (System.Exception ex)
            {
                // Damit wir nicht wiederholt
                // in den Fehler laufen
                Ergebnis = new Daten.Fensterinfos();

                // Sollte es sich um einen neuen
                // Benutzer handelt, tritt dieser
                // Fehler beim ersten Start auf
                var BeimErstenMalIgnorieren
                    = new System.Exception(
                        "Beim ersten Mal ignorieren - wahrscheinlich" +
                        "eine neue Benutzerin oder Benutzer.", ex);

                this.OnFehlerAufgetreten(BeimErstenMalIgnorieren);
            }

            return Ergebnis;
        }

        #endregion Zum Speichern und Wiederherstellen
    }
}
