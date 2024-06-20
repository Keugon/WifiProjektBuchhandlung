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
                    this._Entlehnungen 
                        = this.DatenManager!.SqlServerController
                        .HoleEntlehnungenAsync(this.AktuellePerson.ID).Result;
                }
                return this._Entlehnungen;
            }
            set
            {
                this._Entlehnungen = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private InventarGegenstand _ArtikelZumAusleihen = null!;
        /// <summary>
        /// Ruft den Artikel der Ausgeliehen
        /// werden soll ab oder legt in fest
        /// </summary>
        public InventarGegenstand ArtikelZumAusleihen
        {
            get
            {
                if (this._ArtikelZumAusleihen == null)
                {
                    this._ArtikelZumAusleihen = new InventarGegenstand();
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
            InventarGegenstand artikelZumAusleihen,
            Entlehnung entlehnungZumAnlegen)
        {
            //Todo es darf nur möglich sein Artikel deren
            //InventarNr nicht bereits ausgeliehen sind auszuleihen!
            //artikel Bezeichnung nach InventarNr abrufen
            //wenn nicht vorhanden Meldung Falsche InventarNr und Return; 
            try
            {
                artikelZumAusleihen.Bezeichnung 
                    = this.DatenManager!.SqlServerController
                .HoleInventarGegenständeAsync(
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
            //auf RückgabeDatum prüfen wenn noch nicht
            //vorhanden dann ist der Artikel noch ausgeliehen
            try
            {
                Entlehnung zumAusleihen = this.DatenManager!.SqlServerController
                                .HoleEntlehnungAsync(
                                    inventarNr: artikelZumAusleihen.InventarNr
                                    ).Result;
                if (zumAusleihen.RückgabeDatum == null)
                {
                    //Meldung InventarGegenstand augeliehen
                    MessageBox
                    .Show($"Angegebene InventarNr: " +
                    $"{artikelZumAusleihen.InventarNr.ToString()}" +
                    $" wird vorraussichtlich am: " +
                    $"{zumAusleihen.AusleihDatum!.Value.AddDays(14)}" +
                    $" zurückgegeben!");
                    return;
                }
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
            }
            //Vor Entlehnung den Titel anzeigen!
            var result = MessageBox
                .Show(
                messageBoxText: $"Artikel mit Bezeichnung: " +
                $"{artikelZumAusleihen.Bezeichnung} ausleihen?",
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
                    entlehnungZumAnlegen.AusleihDatum = DateTime.Now;
                    int rückmeldung = this.DatenManager!.SqlServerController
                        .EntlehnungAnlegen(entlehnungZumAnlegen).Result;
                    System.Diagnostics.Debug.WriteLine(
                        $"Rückmeldung aus dem Personne Anlegen:{rückmeldung}");
                    if (rückmeldung == 2)
                    {
                        MessageBox.Show("Artikel wurde ausgeliehen.");
                    }
                    this.ArtikelZumAusleihen = null!;
                    this.EntlehnungZumAnlegen = null!;
                    this.EntlehnungenListe = null!;
                    OnPropertyChanged(nameof(EntlehnungenListe));
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
