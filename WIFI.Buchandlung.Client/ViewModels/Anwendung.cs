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
        public Befehl PersonenSucheCommand => new Befehl(p => PersonenSuche(p as string));
        public Befehl PersonenKarteiÖffnenCommand => new Befehl(p => PersonenKarteiÖffnen(p));

        #endregion Commands
        #region Bindings

        private ObservableCollection<Person> _PersonenListe = null!;
        public ObservableCollection<Person> PersonenListe
        {
            get
            {
                if (this._PersonenListe == null)
                {
                    this._PersonenListe = new ObservableCollection<Person>();
                }
                return this._PersonenListe;
            }
            set
            {
                this._PersonenListe = value;
                OnPropertyChanged();
            }
        }
        private Person _SelectedPerson = null!;
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
        private PersonenKarteiViewModel  _PersonenKarteiVM = null!;
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
            

        #endregion
        #region Artikel Manager
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private ArtikelManager _ArtikelManager = null!;
        /// <summary>
        /// Ruft den Dienst zum Verarbeiten von Aktikel bereit
        /// </summary>
        public ArtikelManager ArtikelManager
        {
            get
            {
                if(this._ArtikelManager == null)
                {
                    this._ArtikelManager = this.Kontext.Produziere<ArtikelManager>();
                }
                return this._ArtikelManager;
            }
        }
        #endregion Artikel Manager
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
                case nameof(PersonAnlegen):
                    AktuelleView = new Views.PersonAnlegen();
                    break;
                    case nameof(PersonenSuche):
                    AktuelleView = new Views.PersonenSuche();
                    break;

            }

        }
        public void PersonenSuche(string? name)
        {
            this.PersonenListe = new ObservableCollection<Person>(this.ArtikelManager.SqlServerController.HolePersonenAsync());
        }
        public void PersonenKarteiÖffnen(object? person)
        {
            if(person is WIFI.Buchandlung.Client.Models.Person)
            {
                System.Diagnostics.Debug.WriteLine(person.ToString());
                var PersonenKarteiFenster = new PersonenKarteiView();
                PersonenKarteiFenster.DataContext = this.PersonenKarteiVM;
                this.PersonenKarteiVM.AktuellePerson = (person as Person)!;
                PersonenKarteiFenster.Show();
            }
        }
    }
}
