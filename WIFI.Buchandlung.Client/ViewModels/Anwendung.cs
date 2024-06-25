using WIFI.Buchandlung.Client.Models;
using WIFI.Buchandlung.Client.Views;
using WIFI.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace WIFI.Buchandlung.Client.ViewModels
{
    /// <summary>
    /// Haupt Viewmodel der Anwendung
    /// </summary>
    public class Anwendung : WIFI.Windows.ViewModel
    {
        #region Hauptview       
        /// <summary>
        /// Ruft einen Wahrheitswert ab
        /// oder legt diesen fest, mit dem
        /// gesteuert wird, ob beim
        /// Schließen der Anwendung nachgefragt 
        /// werden soll, ob man sicher ist.
        /// </summary>
        /// <remarks>Diese Einstellung wird
        /// über die Anwendungskonfiguration
        /// BeimBeendenFragen gesteuert.</remarks>
        public bool BeimBeendenFragen
        {
            get => Properties.Settings.Default.BeimBeendenFragen;

            // Sicherstellen, dass beim Beenden
            // die Einstellungen gespeichert werden (Exit-Ereignis)
            set => Properties.Settings.Default.BeimBeendenFragen = value;
        }
        /// <summary>
        /// Öffnet die Hauptoberfläche 
        /// der Anwendung
        /// </summary>
        /// <typeparam name="T">Ein WPF Fenster,
        /// das als Hauptfenster der Anwendung
        /// benutzt werden soll</typeparam>
        public void Anzeigen<T>()
            where T : System.Windows.Window, new()
        {
            var f = new T();
            Client.App.Current.MainWindow = f;

            // View und ViewModel verbinden
            f.DataContext = this;

            // Das Fenster vorbereiten
            this.Initialisiere(f);

            f.Show();
        }
        /// <summary>
        /// Bereitet die View für die Anzeige vor
        /// und bindet Ereignisse zur Kontrolle
        /// </summary>
        /// <param name="fenster">Ein WPF Fenster,
        /// das für die Anzeige vorbereitet werden soll</param>
        private void Initialisiere(System.Windows.Window fenster)
        {
            // Für den Fensterdienst
            // einen Namen für das Wiederfinden festlegen
            fenster.Name = fenster.GetType().Name;

            // Weil kein Load-Ereignis wie früher vorhanden ist,
            // nur ein Loaded, wo alles zu spät ist, direkt
            // hier den alten Zustand wiederherstellen
            this.PositionWiederherstellen(fenster);

            // Sollte das Fenster geschlossen werden,
            // noch rasch die Position hinterlegen
            fenster.Closing
                += (sender, e) =>
                {
                    // Nachfragen, ob sicher und
                    // eventuell abbrechen

                    if (this.BeimBeendenFragen)
                    {
                        var Antwort = System.Windows.MessageBox
                                .Show(
                                    Views.Texte.BeendenFrage,
                                    Views.Texte.Titel,
                                    System.Windows.MessageBoxButton.YesNo,
                                    System.Windows.MessageBoxImage.Question);

                        e.Cancel = Antwort == System.Windows
                            .MessageBoxResult.No;
                    }

                    this.PositionHinterlegen(fenster);
                };
        }
        #endregion Hauptview

        #region Commands
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl MenüPunktAnzeigenCommand => new Befehl(p => MenüPunktAnzeigen(p as string));
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl PersonenSucheCommand => new Befehl(p => PersonenSuche());
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl ArtikelSucheCommand => new Befehl(p => ArtikelSuche());
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl PersonenKarteiÖffnenCommand => new Befehl(p => PersonenKarteiÖffnen(p));
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl PersonAnlegenCommand => new Befehl(p => PersonAnlegenÖffnen());
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl ArtikelAnlegenCommand => new Befehl(p => ArtikelAnlegenÖffnen());
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl InventarGegenstandvonArtikelAnlegenCommand => new Befehl(p => ArtikelAnlegenÖffnen((p as Artikel)!));
        /// <summary>
        /// Bindbarer aufruf der Oberfläche
        /// </summary>
        public Befehl SpeichernInCSVCommand => new Befehl(p => SpeichernInCSV(Mahnungen));
        #endregion Commands

        #region Bindings            
        #region Artikel/Personen Suche Bindings
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private string _ArtikelBezeichnungSuche = null!;
        /// <summary>
        /// Ruft den den Artikelname ab mit dem
        /// in der Datenbank gesucht wird 
        /// ab oder legt diesen fest
        /// </summary>
        public string ArtikelBezeichungSuche
        {
            get => this._ArtikelBezeichnungSuche;
            set
            {
                this._ArtikelBezeichnungSuche = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private string _PersonBezeichnungSuche = null!;
        /// <summary>
        /// Ruft den den Vornamen ab mit dem nach Personen
        /// in der Datenbank gesucht wird 
        /// ab oder legt diesen fest
        /// </summary>
        public string PersonBezeichungSuche
        {
            get => this._PersonBezeichnungSuche;
            set
            {
                this._PersonBezeichnungSuche = value;
                OnPropertyChanged();
            }
        }
        #endregion
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Personen _PersonenListe = null!;
        /// <summary>
        /// Ruft ab oder legt die Liste an Personen die an der
        /// PersonenSuche seite angezeigt werden soll fest
        /// </summary>
        public Personen PersonenListe
        {
            get
            {
                if (this._PersonenListe == null)
                {
                    this._PersonenListe = new Personen();
                }
                return this._PersonenListe;
            }
            set
            {
                this._PersonenListe = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private ArtikelListe _ArtikelListe = null!;
        /// <summary>
        /// Ruft die zu Darstellende 
        /// Liste von Artikel aus der Datenbank 
        /// ab oder legt diese fest
        /// </summary>
        public ArtikelListe ArtikelListe
        {
            get
            {
                if (this._ArtikelListe == null)
                {
                    this._ArtikelListe = new ArtikelListe();
                }
                return this._ArtikelListe;
            }
            set
            {
                this._ArtikelListe = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private PersonAnlegenViewModel _PersonAnlegenVM = null!;
        /// <summary>
        /// Ruft das Viewmodel zum Anlegen einer neuen Person ab
        /// </summary>
        public PersonAnlegenViewModel PersonAnlegenVM
        {
            get
            {
                this._PersonAnlegenVM = this.Kontext.Produziere<PersonAnlegenViewModel>();
                this._PersonAnlegenVM.DatenManager = this.DatenManager;
                return this._PersonAnlegenVM;
            }
        }
        /// <summary>
        /// Ruft die Liste an Entlehnungen die überfällig(Gebühr.GebührenFreieTage) sind ab
        /// </summary>
        public Entlehnungen Mahnungen
        {
            get
            {
                try
                {
                    return this.DatenManager.SqlServerController.HoleÜberfälligeEntlehnungenAsync().Result;
                }
                catch (Exception ex)
                {
                    OnFehlerAufgetreten(ex);
                    //Falls die Abfrage scheitert
                    return new Entlehnungen();
                }
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>      
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private System.Windows.Controls.Control _AktuelleView = null!;
        //Todo Zuletzt angezeigte view oder standart view einstellbar
        /// <summary>
        /// Ruft die Aktuell Angezeigte View ab
        /// </summary>
        public System.Windows.Controls.Control AktuelleView
        {
            get
            {
                return this._AktuelleView;
            }
            set
            {
                this._AktuelleView = value;
                OnPropertyChanged();
            }
        }
        #endregion Bindings

        #region Daten Manager
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private DatenManager _DatenManager = null!;
        /// <summary>
        /// Ruft den Dienst zum Verarbeiten von Daten bereit
        /// </summary>
        public DatenManager DatenManager
        {
            get
            {
                if (this._DatenManager == null)
                {
                    this._DatenManager = this.Kontext.Produziere<DatenManager>();
                }
                return this._DatenManager;
            }
        }
        #endregion ADaten Manager

        #region Methoden             
        /// <summary>
        /// Methode um die Controls per Button Click zu wechseln
        /// </summary>
        /// <param name="viewName"></param>
        public void MenüPunktAnzeigen(string? viewName)
        {

            switch (viewName)
            {
                case nameof(ArtikelSuche):
                    AktuelleView = new Views.ArtikelSuche();
                    break;
                case nameof(PersonenSuche):
                    AktuelleView = new Views.PersonenSuche();
                    break;
                case nameof(MahnungenView):
                    AktuelleView = new Views.MahnungenView();
                    break;
            }
        }
        /// <summary>
        /// Startet eine suche in der Datenbank nach Personen die den Suchbegriff enthalten
        /// </summary>
        public void ArtikelSuche()
        {
            var tempArtikelListe = new ArtikelListe();
            try
            {
                tempArtikelListe = this.DatenManager.SqlServerController
                        .HoleArtikelAsync(this.ArtikelBezeichungSuche).Result;
                //Wenn suche in Artikel keine Ergenisse zurückgibt, Vorschlag eintragen?
                if (tempArtikelListe.Count == 0)
                {

                }
                foreach (Artikel artikel in tempArtikelListe)
                {
                    artikel.InventarGegenstände = this.DatenManager.SqlServerController.HoleInventarGegenständeAsync(artikel.ID).Result;
                    //hole entlehnung für die InventarNr
                    foreach (InventarGegenstand inventarGegenstand in artikel.InventarGegenstände)
                    {
                        inventarGegenstand.Entlehnung = this.DatenManager.SqlServerController.HoleEntlehnungAsync(inventarGegenstand.InventarNr).Result;
                    }

                }
                this.ArtikelListe = tempArtikelListe;
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
            }

        }
        /// <summary>
        /// Startet eine suche in der Datenbank nach Personen die den Suchbegriff enthalten
        /// </summary>
        public void PersonenSuche()
        {
            try
            {
                this.PersonenListe = this.DatenManager.SqlServerController
                        .HolePersonenAsync(this.PersonBezeichungSuche).Result;
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
            }

        }
        /// <summary>
        /// Öffnet die PersonenKartei des 
        /// gewählten PersonenListen eintrag
        /// </summary>
        /// <param name="person"></param>
        public void PersonenKarteiÖffnen(object? person)
        {
            if (person is WIFI.Buchandlung.Client.Models.Person)
            {

                System.Diagnostics.Debug.WriteLine(person.ToString());
                var PersonenKarteiFenster = new PersonenKarteiView();
                PersonenKarteiViewModel PersonenKarteiVM = new PersonenKarteiViewModel();
                PersonenKarteiFenster.DataContext = PersonenKarteiVM;
                PersonenKarteiVM.AktuellePerson = (person as Person)!;
                PersonenKarteiVM.DatenManager = this.DatenManager;
                PersonenKarteiFenster.Show();
            }
        }
        /// <summary>
        /// Öffnet das Fenster zum Anlegen einer neuen Person
        /// die mindestens 8 Jahre alt sein muss und das mit einem 
        /// Ausweis prüft
        /// </summary>
        public void PersonAnlegenÖffnen()
        {
            var PersonAnlegenFenster = new PersonAnlegenView();
            this.PersonAnlegenVM.DatenManager = this.DatenManager;
            PersonAnlegenFenster.DataContext = this.PersonAnlegenVM;

            PersonAnlegenFenster.Show();
        }
        /// <summary>
        /// Öffnet das Fenster zum Anlegen eines neuen Artikels
        /// </summary>
        public void ArtikelAnlegenÖffnen()
        {
            var ArtikelAnlegenFenster = new ArtikelAnlegenView();
            //singleton viewmodel
            ArtikelAnlegenViewModel artikelAnlegenViewM = new ArtikelAnlegenViewModel();
            ArtikelAnlegenFenster.DataContext = artikelAnlegenViewM;
            artikelAnlegenViewM.DatenManager = this.DatenManager;
            ArtikelAnlegenFenster.Show();
        }
        /// <summary>
        /// Öffnet das Fenster zum Anlegen eines neuen 
        /// Artikels/InventarGegenstands hier wird nur einer 
        /// neuer InventarGegenstand erzeugt anstatt einen 
        /// gänzlich neuen Artikel Anzulegen
        /// </summary>
        /// <param name="artikel">
        /// Der Artikel der von der Liste mittels 
        /// Kontextmenü gewählt wurde</param>
        public void ArtikelAnlegenÖffnen(Artikel artikel)
        {
            var ArtikelAnlegenFenster = new ArtikelAnlegenView();
            //singleton viewmodel
            ArtikelAnlegenViewModel artikelAnlegenViewM = new ArtikelAnlegenViewModel();
            ArtikelAnlegenFenster.DataContext = artikelAnlegenViewM;
            artikelAnlegenViewM.DatenManager = this.DatenManager;
            artikelAnlegenViewM.ArtikelZumAnlegen.ID = artikel.ID;
            artikelAnlegenViewM.ArtikelZumAnlegen.Bezeichnung = artikel.Bezeichnung;
            artikelAnlegenViewM.ArtikelZumAnlegen.Beschaffungspreis = artikel.Beschaffungspreis;
            ArtikelAnlegenFenster.Show();

        }
        /// <summary>
        /// Speichert das mitgegebene Objekt in eine CSV datei
        /// </summary>
        /// <param name="entlehnungenZumSpeichern">Entlehnungen Objekt das in eine CSV Datei geschrieben werden soll</param>
        public void SpeichernInCSV(Entlehnungen entlehnungenZumSpeichern)
        {
            string speicherPfad;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                speicherPfad = saveFileDialog.FileName;
                var csvInhalt = new StringBuilder();
                //Kopfzeile
                csvInhalt.AppendLine("InventarNr,AusleihDatum,Vorname,Nachname,TeleNr,Email,PLZ,Ort,Straße,AusweisNr");
                //Objekt inhalt in csv schreiben
                foreach (Entlehnung entlehnung in entlehnungenZumSpeichern)
                {
                    var zeile = string
                            .Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                            entlehnung.InventarNr,
                            entlehnung.AusleihDatum,
                            entlehnung.AusleiherDaten!.Vorname,
                            entlehnung.AusleiherDaten.Nachname,
                            entlehnung.AusleiherDaten.Telefonnummer,
                            entlehnung.AusleiherDaten.Email,
                            entlehnung.AusleiherDaten.Plz,
                            entlehnung.AusleiherDaten.Ort,
                            entlehnung.AusleiherDaten.Straße,
                            entlehnung.AusleiherDaten.AusweisNr
                            );
                    csvInhalt.AppendLine(zeile);
                }
                File.WriteAllText(speicherPfad,csvInhalt.ToString());
            }

        }
        #endregion Methoden
    }
}
