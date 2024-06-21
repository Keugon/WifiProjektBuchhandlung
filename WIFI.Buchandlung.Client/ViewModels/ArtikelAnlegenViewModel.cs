﻿using System.Windows;
using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;

namespace WIFI.Buchandlung.Client.ViewModels
{
    public class ArtikelAnlegenViewModel : WIFI.Windows.ViewModel
    {
        #region Befehle
        /// <summary>
        /// CanExecute:Prüft zusätzlich ob alle notwendigen
        /// Textfelder nicht leer sind und gibt den Button frei
        /// </summary>
        public Befehl ArtikelAnlegenCommand
            => new Befehl(p => ArtikelnAnlegen(), p =>
            {

                if (Tools.General
                .AreStringsValid())
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
        public void ArtikelnAnlegen()
        {
            try
            {
                //Todo faking Selected Index +1 bis
                //Listen vom Server geladen werden 
                int ArtikelZustand = int.Parse(ArtikelZumAnlegen.Zustand!);
                ArtikelZustand += 1;
                ArtikelZumAnlegen.Zustand = ArtikelZustand.ToString();
                int ArtikelTyp = int.Parse(ArtikelZumAnlegen.Typ!);
                ArtikelTyp += 1;
                ArtikelZumAnlegen.Typ = ArtikelTyp.ToString();
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
        }
        #endregion Methoden
    }
}
