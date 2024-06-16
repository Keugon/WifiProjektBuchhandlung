using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;
using System.Windows;

namespace WIFI.Buchandlung.Client.ViewModels
{
    public class PersonenKarteiViewModel : WIFI.Windows.ViewModel
    {
        #region Lokale Eigenschaft DatenManager     
        /// <summary>
        /// Lokales Eigenschaft für 
        /// den DatenManager aus den AnwendungsViewModel
        /// </summary>
        public DatenManager? DatenManager { get; set; }
        #endregion Lokale Eigenschaft DatenManager

        #region Befehle
        public Befehl ArtikelAusleihenCommand
            => new Befehl(p =>
            Ausleihen(
                artikelZumAusleihen: ArtikelZumAusleihen,
                entlehnungZumAnlegen: EntlehnungZumAnlegen
                ));
        #endregion Befehle
        #region Bindings

        /// <summary>
        /// Internes feld für die Eigenschaft
        /// </summary>
        private Person _AktuellePerson = null!;
        /// <summary>
        /// Ruft die Person ab die auf der Oberfläche 
        /// dargestellt werden soll oder legt diese fest
        /// </summary>
        public Person AktuellePerson
        {
            get => this._AktuellePerson;
            set
            {
                this._AktuellePerson = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Entlehnungen _Entlehnungen = null!;
        /// <summary>
        /// Ruft die Entlehnungen der
        /// aktuell angezeigten Person ab
        /// </summary>
        public Entlehnungen EntlehnungenListe
        {
            get
            {
                if (this._Entlehnungen == null)
                {
                    this._Entlehnungen = this.DatenManager!.SqlServerController.HoleEntlehnungenAsync(this.AktuellePerson.ID).Result;
                }
                return this._Entlehnungen;
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Artikel _ArtikelZumAusleihen = null!;
        /// <summary>
        /// Ruft den Artikel der Ausgeliehen
        /// werden soll ab oder legt in fest
        /// </summary>
        public Artikel ArtikelZumAusleihen
        {
            get
            {
                if (this._ArtikelZumAusleihen == null)
                {
                    this._ArtikelZumAusleihen = new Artikel();
                }
                return this._ArtikelZumAusleihen;
            }
            set
            {
                this._ArtikelZumAusleihen = value;
                OnPropertyChanged();
            }
        }
        private Entlehnung _EntlehnungZumAnlegen = null!;
        public Entlehnung EntlehnungZumAnlegen
        {
            get
            {
                if (this._EntlehnungZumAnlegen == null)
                {
                    this._EntlehnungZumAnlegen = new Entlehnung();
                }
                return this._EntlehnungZumAnlegen;
            }
            set => this._EntlehnungZumAnlegen = value;
        }
        #endregion Bindings
        #region Methode
        /// <summary>
        /// Erstellt eine Entlehnung mit der 
        /// eingegeben InventarNr auf der Aktuellen Person
        /// </summary>
        /// <param name="artikelZumAusleihen">
        /// Artikel objekt mit der auszuleihenden InventarNr</param>
        public void Ausleihen(
            Artikel artikelZumAusleihen,
            Entlehnung entlehnungZumAnlegen)
        {
            //artikel Bezeichnung nach InventarNr abrufen
            //wenn nicht vorhanden Meldung Falsche InventarNr und Return; 
            try
            {
                artikelZumAusleihen.Bezeichnung = this.DatenManager!.SqlServerController
                .HoleArtikelListeAsync(
                    suchParameter: "",
                    inventarNr: artikelZumAusleihen.InventarNr.ToString()!)
                .Result[0].Bezeichnung;
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
                MessageBox
                    .Show($"Angegebene InventarNr: " +
                    $"{artikelZumAusleihen.InventarNr.ToString()}" +
                    $" konnte nicht gefunden werden!");
                return;
            }
            //Vor Entlehnung den Titel anzeigen!
            var result = MessageBox
                .Show(
                messageBoxText: $"Artikel mit Bezeichnung: {artikelZumAusleihen.Bezeichnung} ausleihen?",
                caption: "Bestätigen", MessageBoxButton.YesNo);
            //Wenn mit Ja bestägt Entlehnung anlegen sonst beenden!
            if (result == MessageBoxResult.Yes)
            {
                //Entlehnung Anlegen falls verfügbar
                try
                {
                    entlehnungZumAnlegen.ID = Guid.NewGuid();
                    entlehnungZumAnlegen.InventarNr = artikelZumAusleihen.InventarNr;
                    entlehnungZumAnlegen.Ausleiher = this.AktuellePerson.ID;
                    entlehnungZumAnlegen.AusleihDatum = DateTime.Today;
                    int rückmeldung = this.DatenManager!.SqlServerController
                        .EntlehnungAnlegen(entlehnungZumAnlegen).Result;
                    System.Diagnostics.Debug.WriteLine($"Rückmeldung aus dem Personne Anlegen:{rückmeldung}");
                    if (rückmeldung == 2)
                    {
                        MessageBox.Show("Artikel wurde ausgeliehen.");
                    }
                    this.ArtikelZumAusleihen = null!;
                    this.EntlehnungZumAnlegen = null!;
                }

                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine($"{ex.Message}");
                }
            }
        }
        #endregion
    }
}
