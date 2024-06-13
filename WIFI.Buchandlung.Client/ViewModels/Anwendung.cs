using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIFI.Windows;
using WIFI.Buchandlung.Client;
using WIFI.Buchandlung.Client.Views;
using WIFI.Buchandlung.Client.Models;
using System.Collections.ObjectModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WIFI.Buchandlung.Client.ViewModels
{
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
        public Befehl MenüPunktAnzeigenCommand => new Befehl(p => MenüPunktAnzeigen(p as string));
        public Befehl PersonenSucheCommand => new Befehl(p => PersonenSuche());
        public Befehl ArtikelSucheCommand => new Befehl(p => ArtikelSuche(p as string));
        /// <summary>
        /// CanExecute:Prüft zusätzlich ob alle notwendigen
        /// Textfelder nicht leer sind und gibt den Button frei
        /// </summary>
        public Befehl ArtikelAnlegenCommand => new Befehl(p => ArtikelAnlegen(), p =>
        {

            if (Tools.General.AreStringsValid(ArtikelBezeichnung,ArtikelBeschaffungspreis.ToString()))
            {
                return true;
            }
            return false;
        });
        public Befehl PersonenKarteiÖffnenCommand => new Befehl(p => PersonenKarteiÖffnen(p));
        public Befehl PersonAnlegenCommand => new Befehl(p => PersonAnlegenÖffnen());
        #endregion Commands

        #region Bindings
        #region Artikel Anlegen Bindings
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private string _ArtikelBezeichnung = null!;
        /// <summary>
        /// Ruft den in der Datenbank neuen 
        /// anzulegenden Artikelnamen ab oder legt diesen Fest
        /// </summary>
        public string ArtikelBezeichnung
        {
            get => this._ArtikelBezeichnung;
            set
            {
                this._ArtikelBezeichnung = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private decimal _ArtikelBeschaffungspreis;
        /// <summary>
        /// Ruft den in der Datenbank neuen 
        /// anzulegenden Artikelbeschaffungspreis ab oder legt diesen Fest
        /// </summary>
        public decimal ArtikelBeschaffungspreis
        {
            get => this._ArtikelBeschaffungspreis;
            set
            {
                this._ArtikelBeschaffungspreis = value;
                OnPropertyChanged();
            }
        }
        #endregion Artikel Anlegen Bindings
        #region Artikel Suche Bindings
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
                this._PersonBezeichnungSuche= value;
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
        private Person _SelectedPerson = null!;
        /// <summary>
        /// Ruft ab oder legt die Selectierten 
        /// PersonenListen Eintrag fest
        /// </summary>
        public Person SelectedPerson
        {
            get
            {
                if (this._SelectedPerson == null)
                {
                    this._SelectedPerson = new Person();
                }
                return this._SelectedPerson;
            }
            set
            {
                this._SelectedPerson = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private PersonenKarteiViewModel _PersonenKarteiVM = null!;
        /// <summary>
        /// Ruft das ViewModel für den PersonAnlegenView ab
        /// </summary>
        public PersonenKarteiViewModel PersonenKarteiVM
        {
            get
            {
                if (this._PersonenKarteiVM == null)
                {
                    this._PersonenKarteiVM = this.Kontext.Produziere<PersonenKarteiViewModel>();
                }
                return this._PersonenKarteiVM;
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
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private System.Windows.Controls.Control _AktuelleView = null!;
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
        /// <param name="viewToDisplay"></param>
        public void MenüPunktAnzeigen(string? viewName)
        {

            switch (viewName)
            {
                case nameof(ArtikelAnlegen):
                    AktuelleView = new Views.ArtikelAnlegen();
                    break;
                case nameof(ArtikelSuche):
                    AktuelleView = new Views.ArtikelSuche();
                    break;
                               case nameof(PersonenSuche):
                    AktuelleView = new Views.PersonenSuche();
                    break;
            }
        }
        /// <summary>
        /// Startet eine suche in der Datenbank nach Personen die den Suchbegriff enthalten
        /// </summary>
        /// <param name="name">Name der Gesucht wird</param>
        public void ArtikelSuche(string? name)
        {
            try
            {
                this.ArtikelListe = this.DatenManager.SqlServerController
                        .HoleArtikelListeAsync(this.ArtikelBezeichungSuche).Result;
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
            }

        }
        /// <summary>
        /// Legt einen neuen Artigel in der Datenbank an
        /// </summary>
        public void ArtikelAnlegen()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Rückmeldung aus der SQL Artikel Anlegen:{this.DatenManager.SqlServerController
                    .ArtikelAnlegen(
                        Guid.NewGuid(),
                        bezeichnung: ArtikelBezeichnung,
                        beschaffungspreis: ArtikelBeschaffungspreis
                        ).Result}");
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
        }
        /// <summary>
        /// Startet eine suche in der Datenbank nach Personen die den Suchbegriff enthalten
        /// </summary>
        /// <param name="name">Name der Gesucht wird</param>
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
                PersonenKarteiFenster.DataContext = this.PersonenKarteiVM;
                this.PersonenKarteiVM.AktuellePerson = (person as Person)!;
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
        #endregion Methoden
    }
}
