using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIFI.Anwendung.Daten;

namespace WIFI.Anwendung
{
    /// <summary>
    /// Stellt einne Dienst zum Verwalten 
    /// eines Anwendungsprotokolls bereit
    /// </summary><remarks>
    /// seit 2024.5.2.0 wird
    /// Für WPF wird die Threadsicherheit gewährleistet 
    /// wenn ein Dispatcher eingestellt wird.</remarks>
    public class ProtokollManager : AppObjekt
    {
        #region Daten
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Daten.Protokolleinträge _Einträge = null!;
        /// <summary>
        /// Ruf die Liste mit den
        /// Protokollzeilen ab
        /// </summary>
        public Daten.Protokolleinträge Einträge
        {
            get
            {
                if (this._Einträge == null)
                {
                    this._Einträge
                        = new Daten.Protokolleinträge();
                }
                return this._Einträge;
            }
        }
        #endregion Daten

        #region NeueEinträge
        /// <summary>
        /// Fügt dem Protokoll eine neue
        /// Zeile hinzu.
        /// </summary>
        /// <param name="eintrag">Protokolleintrag Objekt zum Hinzufügen.</param>
        /// <remarks>Die Threadsicherheit ist nicht gewährleistet, 
        /// außer es ist ein WPF Dispatcher vorhanden</remarks>
        public void Erstellen(Daten.Protokolleintrag eintrag)
        {
            this.Erstellen(eintrag, rekursiv: false);
        }
        /// <summary>
        /// Fügt dem Protokoll eine neue
        /// Zeile hinzu.
        /// </summary>
        /// <param name="eintrag">Protokolleintrag Objekt zum Hinzufügen.</param>
        /// <param name="rekursiv">für interne Zwecke</param>
        /// <remarks>Die Threadsicherheit ist nicht gewährleistet, 
        /// außer es ist ein WPF Dispatcher vorhanden</remarks>
        private void Erstellen(Daten.Protokolleintrag eintrag, bool rekursiv)
        {
            if (this.Dispatcher != null && !rekursiv)
            {
                //Wenn ein Dispatcher vorhanden ist
                //diesen bitte , die Methode Threadsicher
                //auszuführen.
                this.Dispatcher.GetType()
                    .GetMethod(
                    "Invoke",
                    new Type[] { typeof(System.Action) })?
                    .Invoke(
                        this.Dispatcher,
                        new object[] {
                            () => this.Erstellen(eintrag,rekursiv:true)
                        });
            }
            else
            {
                this.Einträge.Add(eintrag);
                //2024.3.1.0
                if (eintrag.Typ == Daten.ProtokolleintragTyp.Fehler)
                {
                    this.EnthältFehler = true;
                }
                //2024.4.0.0
                if (this.HatRückrufe)
                {
                    this.Rückrufe.AlleAufrufen();
#if DEBUG
                    //Fals rückrufe ohne besitzer vorhanden sind 
                    //aufstörung schalten
                    if (this.Rückrufe.ToteBesitzer > 0)
                    {
                        //Damit keine Rekursion auftritt
                        //wird hier nicht mit this.Erstellen 
                        //gearbeitet
                        this.Einträge.Add(new Protokolleintrag
                        {
                            Text = $"{this} hat rückrufe ohne besitzer!\n" +
                            $"Unbedingt nicht mehr benötigte Rückrufe stornieren!",
                            Typ = ProtokolleintragTyp.Fehler
                        });
                        //Damit das Ereignis ausgelöst wird
                        this.EnthältFehler = true;
                    }
#endif
                }
                this.Speichern(eintrag);
            }
        }
        /// <summary>
        /// Fügt dem Protokoll am Ende einen
        /// normalen Eintrag hinzu
        /// </summary>
        /// <param name="text">Die Information die als normaler 
        /// Eintrag hinzugefügt werden soll</param>
        /// <remarks>Die Threadsicherheit ist nicht gewährleistet</remarks>
        public void Erstellen(string text)
        {
            this.Erstellen(new Protokolleintrag
            {
                Text = text,
                Typ = Daten.ProtokolleintragTyp.Normal
            });
        }
        /// <summary>
        /// Fügt dem Protokoll am Ende einen
        /// Eintrag hinzu
        /// </summary>
        /// <param name="text">Die Information die für
        /// Eintrag benutzt werden soll</param>
        /// <param name="typ">ProtkolleintragTyp Bestimmt
        /// die Art des Eintrags</param>
        /// <remarks>Die Threadsicherheit ist nicht gewährleistet</remarks>
        public void Erstellen(string text, Daten.ProtokolleintragTyp typ)
        {
            this.Erstellen(new Daten.Protokolleintrag
            {
                Text = text,
                Typ = typ
            });
        }

        /// <summary>
        /// Erstellt im Protokoll eine Eintrag dass
        /// eine methode zu laufen beginnt
        /// </summary>
        /// <param name="aufruferName">Optional; Falls nicht angegeben 
        /// wird automatisch der Name vom aufrufer benutzt</param>
        public void StartMelden(
            [System.Runtime.CompilerServices.CallerMemberName]
        string aufruferName = null!)
        {
            //Den Namen vom Besitzer des Aufrufers 
            //ermitteln
            var AufrufListe = new System.Diagnostics.StackTrace(1);
            var Besitzer = AufrufListe.GetFrame(0)!.GetMethod()!.DeclaringType!.FullName;
            this.Erstellen($"{Besitzer}.{aufruferName}() startet ...");


        }

        /// <summary>
        /// Erstellt im Protokoll eine Eintrag dass
        /// eine methode beendet ist.
        /// </summary>
        /// <param name="aufruferName">Optional; Falls nicht angegeben 
        /// wird automatisch der Name vom aufrufer benutzt</param>
        public void EndeMelden(
            [System.Runtime.CompilerServices.CallerMemberName]
        string aufruferName = null!)
        {
            //Den Namen vom Besitzer des Aufrufers 
            //ermitteln
            var AufrufListe = new System.Diagnostics.StackTrace(1);
            var Besitzer = AufrufListe.GetFrame(0)!.GetMethod()!.DeclaringType!.FullName;
            this.Erstellen($"{Besitzer}.{aufruferName}() Endet.");

        }
        #endregion NeueEinträge

        #region FehlerStatus
        /// <summary>
        /// Wird ausgelöst wenn sich die
        /// Eigenschaft EnthältFehler
        /// geändert hat
        /// </summary>
        public event System.EventHandler? EnthältFehlerGeändert;
        /// <summary>
        /// Löst das Ereignis EnthältFehlerGeändert 
        /// aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        protected virtual void OnEnthältFehlerGeändert(
            System.EventArgs e)
        {
            this.EnthältFehlerGeändert?.Invoke(this, e);
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private bool _EnthältFehler = false;
        /// <summary>
        /// Ruft True ab wenn im Protokoll
        /// Fehler einträge enthalten sind
        /// </summary>
        public bool EnthältFehler
        {
            get => this._EnthältFehler;
            private set
            {
                this._EnthältFehler = value;
                this.OnEnthältFehlerGeändert(System.EventArgs.Empty);
            }
        }

        #endregion FehlerStatus

        #region Rückrufe für Objekte die das Protokoll benutzen
        /// <summary>
        /// Ruft True ab wenn Rückrufe gebucht sind,
        /// sonst False
        /// </summary>
        /// <remarks>Damit bei keinen Rückrufen
        /// die Liste nicht initialisiert wird,
        /// wird mit dem Feld und nicht der Eigenschaft
        /// gearbeitet</remarks>
        protected bool HatRückrufe => this._Rückrufe != null;

        /// <summary>Entfernt eine Methode die
        /// ausgeführt wurde wenn ein neue Protokolleintrag
        /// erstellt wird
        /// hinterlegen einer Methode die 
        /// ausgeführt wird wenn ein neuer
        /// Protokolleintrag erstellt wurde
        /// </summary>
        /// <param name="methode">Speicheradresse der 
        /// Methode die bei einem neuen Eintrag von
        /// ProtokollManager nicht mehr aufgerufen werden soll</param>
        public void RückrufStornieren(System.Action methode)
        {
            if (this.Rückrufe.Remove((
                  from m in this.Rückrufe
                  where m.Methode == methode
                  select m)
                  .FirstOrDefault()!))
            {
                this.Erstellen($"{this}" +
                        $" hat einen Rückruf für " +
                        $"{methode.Target}(ID={methode.Target?.GetHashCode()} Storniert. " +
                        $"{this.Rückrufe.Count} Rückruf{(this.Rückrufe.Count != 1 ? "e" : "")} weiter vorhanden");
            }
            //Meine Hü rein nach der aufgaben stellung io aber es
            //wurde wärend dem wiederholen etwas dazu gebastelt
            //this.Erstellen($"Rückrufe übrig:{this.Rückrufe.Count}");

            //Meine Variante Funktioniert aber offensichtlich sehr viel mehr umständlich als ein LINQ
            /*
            SchwacherMethodenVerweisListe zuLöschendeVerweise = new SchwacherMethodenVerweisListe();
            foreach (SchwacherMethodenVerweis verweis in Rückrufe)
            {
                if(verweis.Methode == methode)
                {
                    zuLöschendeVerweise.Add(verweis);
                    this.Erstellen($"Es wurde {verweis} zur zu löschender liste hinzugefügt(ID={methode.Target?.GetHashCode()}");
                }
            }

            foreach (SchwacherMethodenVerweis item in zuLöschendeVerweise)
            {
                this.Rückrufe.Remove(item);
                this.Erstellen($"Es wurde {item} gelöscht(ID={methode.Target?.GetHashCode()} Rückrufe Übrig {this.Rückrufe.Count}");
            }
            */
        }

        /// <summary>
        /// Ermöglicht beim benutzen des Protokolls
        /// dass hinterlegen einer Methode die 
        /// ausgeführt wird wenn ein neuer
        /// Protokolleintrag erstellt wurde
        /// </summary>
        /// <param name="methode">Speicheradresse der 
        /// Methode die bei einem neuen Eintrag von
        /// ProtokollManager aufgerufen werden soll</param>
        public void RückrufBuchen(System.Action methode)
        {
            this.Rückrufe.Add(new SchwacherMethodenVerweis(methode)
                );

            this.Erstellen($"{this}" +
                $" hat einen {this.Rückrufe.Count}. Rückruf für " +
                $"{methode.Target}(ID={methode.Target?.GetHashCode()} gebucht"
                , ProtokolleintragTyp.Warnung);
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Daten.SchwacherMethodenVerweisListe _Rückrufe = null!;
        /// <summary>
        /// Ruft die Liste mit den Mehtoden ab, 
        /// die bei einem neuen Eintrag aufgerufen werden
        /// sollen
        /// </summary>
        protected Daten.SchwacherMethodenVerweisListe Rückrufe
        {
            get
            {
                if (this._Rückrufe == null)
                {
                    this._Rückrufe = new Daten.SchwacherMethodenVerweisListe();

                    this.Erstellen($"{this} hat die Liste für Rückrufmethoden" +
                        $"initialisisert", ProtokolleintragTyp.Warnung);
                }
                return this._Rückrufe;
            }
        }

        #endregion Rückrufe für Objekte die das Protokoll benutzen

        #region Zum Speichern der Einträge
        /// <summary>
        /// Bennent existierende Protokolldateien um
        /// , damit eine neue Protokolldatei erstellt wird.
        /// </summary>
        /// <remarks>Es wird am Ende eine Erweiterung ".1" bis
        /// ".4" benutzt, d.h vier Generationen werden aufgehoben.</remarks>
        private void Zusammenräumen()
        {
            this.StartMelden();
            //20240529 Hr. Draxler - optimierung
            // if(this.Pfad != null && this.Pfad != string.Empty) 
            if (!string.IsNullOrEmpty(this.Pfad))
            {
                const int Generationen = 4; //Todo Konfigurierbar mochn
                for (int i = Generationen; i > 0; i--)
                {
                    var NeuerName = $"{this.Pfad}.{i}";
                    var AlterName
                        = i > 1 ?
                        $"{this.Pfad}.{i - 1}" :
                        this.Pfad;
                    System.IO.File.Delete(NeuerName);

                    if (System.IO.File.Exists(AlterName))
                    {
                        System.IO.File.Move(AlterName, NeuerName);
                    }
                }
                this.Erstellen($"{this} hat das Alte Protokoll umbenant");

            }

            this.EndeMelden();
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private string _Pfad = string.Empty;
        /// <summary>
        /// Ruft den Vollständigen Namen der Datei
        /// ab in die die Einträge als unformatierter Text
        /// geschrieben werden sollen oder legt diesen Fest
        /// </summary>
        /// <remarks>Leer lassen, wenn keine Protokolldatei
        /// erstellt werden soll</remarks>
        public string Pfad
        {
            get => this._Pfad;
            set
            {
                this._Pfad = value;
                if (this._Pfad != string.Empty)
                {
                    //Eine Relative Pfad Angabe auf 
                    //das Anwendungsverzeichniss beziehen
                    if (!System.IO.Path.IsPathRooted(this._Pfad))
                    {
                        this._Pfad = System.IO.Path
                            .GetFullPath(
                            this._Pfad,
                            this.Anwendungspfad);
                    }
                    //Damit die Datenträger nicht überlaufen,
                    //alte versionne löschen.
                    this.Zusammenräumen();
                }

            }
        }
        /// <summary>
        /// Versucht den Eintrag in die Datei aus der
        /// Pfad Eigenschaft am Ende einzutragen
        /// </summary>
        /// <param name="eintrag">Protokolleintrag der 
        /// gespeichert werden soll</param>
        /// <remarks>Sollte das Speichern nach mehrmaligen
        /// versuch nicht gelingen wird die Protokollierung
        /// abgeschaltet.
        /// d.h. die Pfad Eigenschaft auf einene String.empty 
        /// eingestellt, als trennzeichen wird ein Tabulator benutzt
        /// damit die Datei in unterschiedlichen Spracheinstellungen 
        /// geöffnet werden kann</remarks>
        protected void Speichern(Daten.Protokolleintrag eintrag)
        {
            if (this._Pfad != string.Empty)
            {
                //Wegen Multithreading kann die Datei aktuell
                //gesperrt sein
                //Bis zu 10x probieren
                int versuche = 10;
                do
                {
                    try
                    {
                        using var Schreiber
                            = new System.IO.StreamWriter(
                                path: this.Pfad,
                                append: true,
                                encoding: System.Text.Encoding.UTF8);
                        //Damit wir nicht zwischen English
                        //und Deutsch unterscheiden müssen
                        //ein Tabolator als Trennzeichen
                        const string Zeilenmuster = "{0}\t{1}\t{2}";
                        Schreiber.WriteLine(
                            string.Format(
                                Zeilenmuster,
                                eintrag.Zeitpunkt,
                                eintrag.Typ,
                                //Auf keinen fall dürfen die 
                                //Trennzeichen im Text vorkommen
                                //durch leerzeichen ersetzten
                                eintrag.Text
                                .Replace('\t', ' ') //Kein Tabulator
                                .Replace('\r', ' ')//Keine Eingabetaste
                                .Replace('\n', ' ')//Keine Zeilenvorschübe
                                ));
                        versuche = 0;
                    }
                    catch (Exception)
                    {
                        //Ein bisschen warten
                        System.Threading.Thread.Sleep(100);
                        versuche--;
                        //20240523 Hr. Draxler
                        //die binärentscheidung fehlte
                        //d.h beim ersten problem wurde bereits abgeschaltet
                        if (versuche == 0)
                        {
                            //Offensichtlich funktionierts nicht
                            //deshalb die protokollierung abschalten
                            this._Pfad = string.Empty;
                        }
                    }
                } while (versuche > 0);
            }
        }
        #endregion Zum Speichern der Einträge

        #region Threadmanager aus WPF
        /// <summary>
        /// Ruft das Objekt für die Threadverwaltung 
        /// einer WPF Anwendung ab oder Legt dieses fest
        /// </summary>
        /// <remarks>
        /// Sollte ein Dispatcher vorhanden sein,
        /// wird die Threadsicherheit gewährleistet</remarks>
        public object? Dispatcher { get; set; }

        #endregion Threadmanager aus WPF
    }
}
