using System.Windows;
using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;

namespace WIFI.Buchandlung.Client.ViewModels
{
    /// <summary>
    /// Stellt das Viewmodel zum Anlegen von Artikel/InventarGegenstände bereit
    /// </summary>
    public class ArtikelAnlegenViewModel : WIFI.Windows.ViewModel
    {
        #region Befehle
        /// <summary>
        /// CanExecute:Prüft zusätzlich ob alle notwendigen
        /// Textfelder nicht leer sind und gibt den Button frei
        /// </summary>
        public Befehl ArtikelAnlegenCommand
            => new Befehl(p => ArtikelnAnlegen((p as System.Windows.Window)!), p =>
            {

                if (Tools.General
                .AreStringsValid(
                    ArtikelZumAnlegen.Bezeichnung!,
                    ArtikelZumAnlegen.Beschaffungspreis.ToString()!,
                    ArtikelZumAnlegen.Typ!,
                    ArtikelZumAnlegen.Zustand!
                    ))
                {
                    return true;
                }
                return false;
            });
        //Bug ggf (ongün) Button kan angeklickt werden ohne inhalt in den Boxen
        #endregion  Befehle

        #region ArtikelBinding





        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private InventarGegenstand _ArtikelZumAnlegen = null!;
        /// <summary>
        /// Ruft das DatentransferObjekt ab zum Anlegen eines Artikels ab
        /// </summary>
        public InventarGegenstand ArtikelZumAnlegen
        {

            get
            {
                if (this._ArtikelZumAnlegen == null)
                {
                    this._ArtikelZumAnlegen = new InventarGegenstand();
                    this._ArtikelZumAnlegen.ID = Guid.NewGuid();
                }
                return this._ArtikelZumAnlegen;
            }
            set
            {
                this._ArtikelZumAnlegen = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Zustand _SelectedZustand = null!;
        /// <summary>
        /// Ruft den Ausgewählten Zustand ab oder diesen in 
        /// </summary>
        public Zustand SelectedZustand
        {

            get => this._SelectedZustand;
            set
            {
                this.ArtikelZumAnlegen.Zustand = value.ID.ToString();
            }
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Typ _SelectedTyp = null!;
        /// <summary>
        /// Ruft den ausgewählten Typ oder legt fest
        /// </summary>
        public Typ SelectedTyp
        {
            get => this._SelectedTyp;
            set
            {
                this.ArtikelZumAnlegen.Typ = value.ID.ToString();
            }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Zustände _Zustände = null!;

        /// <summary>
        /// Ruft die Zustände der
        /// Artikel ab oder legt diese fest
        /// </summary>
        public Zustände ZustandsListe
        {
            get
            {
                if (this._Zustände == null)
                {
                    this._Zustände
                        = this.DatenManager!.SqlServerController
                        .HoleZuständeAsync().Result;
                    System.Diagnostics.Debug.WriteLine("Zustandsliste geholt");
                }
                return this._Zustände;
            }
            set
            {
                this._Zustände = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Typen _Typen = null!;

        /// <summary>
        /// Ruft die Typen der
        /// Artikel ab oder legt diese fest
        /// </summary>
        public Typen TypenListe
        {
            get
            {
                if (this._Typen == null)
                {
                    this._Typen
                        = this.DatenManager!.SqlServerController
                        .HoleTypenAsync().Result;
                    System.Diagnostics.Debug.WriteLine("Typenliste geholt");
                }
                return this._Typen;
            }
            set
            {
                this._Typen = value;
                OnPropertyChanged();
            }
        }


        #endregion ArtikelBinding

        #region Lokale Eigenschaft DatenManager     
        /// <summary>
        /// Lokales Eigenschaft für 
        /// den DatenManager aus den AnwendungsViewModel
        /// </summary>
        public DatenManager? DatenManager { get; set; }
        #endregion Lokale Eigenschaft DatenManager

        #region Methoden
        /// <summary>
        /// Legt einen neuen Artigel in der Datenbank an
        /// </summary>
        public void ArtikelnAnlegen(System.Windows.Window currentWindow)
        {
            try
            {
                //Todo faking Selected Index +1 bis
                //Listen vom Server geladen werden 
                //int ArtikelZustand = int.Parse(ArtikelZumAnlegen.Zustand!);
                //ArtikelZustand += 1;
                //ArtikelZumAnlegen.Zustand = ArtikelZustand.ToString();
                //int ArtikelTyp = int.Parse(ArtikelZumAnlegen.Typ!);
                //ArtikelTyp += 1;
                //ArtikelZumAnlegen.Typ = ArtikelTyp.ToString();
                int rückmeldung = this.DatenManager!.SqlServerController
                    .InventarGegenstandAnlegen(ArtikelZumAnlegen).Result;
                System.Diagnostics.Debug.WriteLine($"Rückmeldung aus dem Artikel Anlegen:{rückmeldung}");
                if (rückmeldung == 2)
                {
                    MessageBox.Show("Artikel wurde Angelegt");
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
            //fenster schließen
            currentWindow.Close();
        }
        #endregion Methoden
    }
}
