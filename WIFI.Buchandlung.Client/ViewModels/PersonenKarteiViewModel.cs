using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;
using System.Windows;
using System;
using WIFI.Buchandlung.Client.Views;

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
        public Befehl EntlehnungRückgabeFensterÖffnenCommand => new Befehl(p => RückgabeFensterÖffnen((p as Entlehnung)!));
        public Befehl EntlehnungRückgabeCommand => new Befehl(p => Rückgabe(p as System.Windows.Window));
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
                    System.Diagnostics.Debug.WriteLine("Neue Entlehnungsliste geholt");
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

        public Entlehnung? ZurückGebenEntlehnung { get; set; }

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
                    inventarNr: artikelZumAusleihen.InventarNr)
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
        public void RückgabeFensterÖffnen(Entlehnung entlehnung)
        {
            if (entlehnung != null)
            {
                this.ZurückGebenEntlehnung = entlehnung;
                System.Diagnostics.Debug.WriteLine(entlehnung.ID.ToString());
                var EntlehnungRückgabeFenster = new EntlehnungRückgabe();
                EntlehnungRückgabeFenster.DataContext = this;
                EntlehnungRückgabeFenster.Show();
            }
        }
        public void Rückgabe(System.Windows.Window currentWindow)
        {
            //Todo faking Selected Index +1 bis
            //Listen vom Server geladen werden 
            int rückgabezustand = int.Parse(ZurückGebenEntlehnung!.RückgabeZustand!);
            rückgabezustand += 1;
            this.ZurückGebenEntlehnung.RückgabeZustand = rückgabezustand.ToString();
            this.ZurückGebenEntlehnung.RückgabeDatum = DateTime.Now;
            //Berechnung Strafbetrag
            this.ZurückGebenEntlehnung.Strafbetrag = StrafbetragBerechnen(this.ZurückGebenEntlehnung);
            try
            {
                //Update vom Entlehnungs eintrag auf der Datenbank
                this.DatenManager!.SqlServerController
                                .EntlehnungAnlegen(this.ZurückGebenEntlehnung!);
            }
            catch (Exception ex)
            {
                OnFehlerAufgetreten(ex);
            }
            //fenster schließen
            currentWindow.Close();
            this.EntlehnungenListe = null!;
            OnPropertyChanged(nameof(EntlehnungenListe));
        }
        public decimal StrafbetragBerechnen(Entlehnung entlehnungZumBerechnen)
        {
            //wenn 14Tage nicht überschritten und
            //der Zustand nicht 3-Unbenutzbar oder 4-Verloren ist werden keine Gebühren erhoben!
            //0,5€ pro tag über 14, 10€ bei übermässiger benutztung,bei verlust/Totalschaden Beschaffungswert x2
            decimal strafbetrag = 0;
            if ((entlehnungZumBerechnen.AusleihDatum - DateTime.Now)!.Value.Days > 14)
            {
                //über 14 tage seit dem ausleihen dadurch berechnen
                int tageüberschreitung = (entlehnungZumBerechnen.AusleihDatum - DateTime.Now)!.Value.Days;
                strafbetrag += Convert.ToDecimal(tageüberschreitung * 0.5);
            }
            if (entlehnungZumBerechnen.RückgabeZustand == "3" || entlehnungZumBerechnen.RückgabeZustand == "4")
            {
                decimal beschaffungsPreis;
                //holt den Beschaffungspreis des Artikels über der InventarNr der Entlehnung
                try
                {
                    beschaffungsPreis = this.DatenManager!.SqlServerController.HoleInventarGegenständeAsync(suchParameter: "", inventarNr: entlehnungZumBerechnen.InventarNr).Result[0].Beschaffungspreis!.Value;
                    //Fügt den zweifachen Beschaffungspreis hinzu
                    strafbetrag += (beschaffungsPreis * 2);
                }
                catch (Exception ex)
                {
                    OnFehlerAufgetreten(ex);
                }

            }
            return strafbetrag;
        }
        #endregion
    }
}
