using System.ComponentModel;

namespace WIFI.Windows
{
    /// <summary>
    /// Stellt Basisfunktionalitäten für
    /// ein MVVM ViewModel bereit
    /// </summary>
    public abstract class ViewModel
        : WIFI.Anwendung.AppObjekt,
        System.ComponentModel.INotifyPropertyChanged
    {
        #region FürFortschrittsvisualisierung
        /// <summary>
        /// Ruft die IstBeschäftigt Stufe ab
        /// oder legt diese Fest
        /// </summary>
        private int IstBeschäftigtZähler { get; set; }
        /// <summary>
        /// Ruft einen Wahrheitswert ab 
        /// oder legt diesen, über den der
        /// Aktuelle zustand festgestellt werden 
        /// kann
        /// </summary>
        public bool IstBeschäftigt
        {
            get => this.IstBeschäftigtZähler > 0;
            set
            {
                if (value)
                {
                    this.IstBeschäftigtZähler++;

                }
                else
                {
                    this.IstBeschäftigtZähler--;
                    if (this.IstBeschäftigtZähler < 0)
                    {
                        this.IstBeschäftigtZähler = 0;
                    }
                }
                this.OnPropertyChanged();
            }
        }

        #endregion FürFortschrittsvisualisierung

        #region WPF über Änderungen informieren

        /// <summary>
        /// Wird ausgelöst, wenn sich der Inhalt
        /// einer Eigenschaft geändert hat
        /// </summary>
        public event PropertyChangedEventHandler?
            PropertyChanged = null;

        /// <summary>
        /// Löst das Ereignis PropertyChanged aus
        /// </summary>
        /// <param name="e">Ereignisdaten mit
        /// dem Namen der geänderten Eigenschaft</param>
        protected virtual void OnPropertyChanged(
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            var BehandlerKopie = this.PropertyChanged;
            if (BehandlerKopie != null)
            {
                BehandlerKopie(this, e);
            }
        }

        /// <summary>
        /// Löst das Ereignis PropertyChanged aus
        /// </summary>
        /// <param name="nameEigenschaft">optional; Die Bezeichnung
        /// der Eigenschaft, deren Inhalt geändert wurde</param>
        /// <remarks>Fehlt der Name der Eigenschaft,
        /// wird der Name vom Aufrufer eingesetzt</remarks>
        protected virtual void OnPropertyChanged(
            [System.Runtime.CompilerServices.CallerMemberName]
            string nameEigenschaft = null!)
        {
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(
                    nameEigenschaft));
        }


        #endregion WPF über Änderungen informieren

        #region Hilfsmethoden zum Vorbereiten der Views

        /// <summary>
        /// Übergibt die aktuellen Größendaten
        /// an den Fensterdienst der Infrastruktur
        /// </summary>
        /// <param name="fenster">Ein WPF Fenster
        /// dessen Koordinaten an den Fensterdienst
        /// übergeben werden sollen</param>
        /// <remarks>Der Name des Fensters wird
        /// als Schlüssel benutzt</remarks>
        protected void PositionHinterlegen(
            System.Windows.Window fenster)
        {
            // Neues Infoobjekt initialisieren
            var Info = new WIFI.Anwendung.Daten.Fensterinfo();

            // Infoobjekt konfigurieren
            //  immer den Namen und den Zustand
            Info.Name = fenster.Name;
            Info.Zustand = (int)fenster.WindowState;

            // Größenangaben nur, wenn das Fenster
            // im Normalzustand ist, weil bei Minimiert
            // und bei Maximiert ungültige Daten vorliegen
            if (fenster.WindowState
                == System.Windows.WindowState.Normal)
            {
                Info.Links = (int)fenster.Left;
                Info.Oben = (int)fenster.Top;
                Info.Breite = (int)fenster.Width;
                Info.Höhe = (int)fenster.Height;
            }

            // Das Infoobjekt an den Dienst übergeben
            this.Kontext.Fenster.Hinterlegen(Info);
        }

        /// <summary>
        /// Stellt mit dem Fensterdienst der
        /// Infrastruktur gespeicherte Zustände
        /// und Größen wiederher
        /// </summary>
        /// <param name="fenster">Ein WPF Fenster,
        /// von dem die alte Größe benötigt wird</param>
        /// <remarks>Der Name vom Fenster dient als Schlüssel</remarks>
        protected void PositionWiederherstellen(
            System.Windows.Window fenster)
        {
            // Gibt's Positionsdaten?
            var AlteDaten = this.Kontext.Fenster
                    .Abrufen(fenster.Name);

            // Wenn ja...
            if (AlteDaten != null)
            {
                //  Links, Oben, Breite und Höhe nur,
                //  wenn alte Daten vorhanden sind
                fenster.Left = AlteDaten.Links ?? fenster.Left;
                fenster.Top = AlteDaten.Oben ?? fenster.Top;
                fenster.Width = AlteDaten.Breite ?? fenster.Width;
                fenster.Height = AlteDaten.Höhe ?? fenster.Height;

                //  Den Zustand Minimiert als
                //  Normal betrachten, weil sonst
                //  das Fenster übersehen wird
                if ((System.Windows.WindowState)AlteDaten.Zustand
                    == System.Windows.WindowState.Maximized)
                {
                    fenster.WindowState = System.Windows.WindowState.Maximized;
                }
                else
                {
                    //Damit niemand das Fenster übersieht (seit Windows 3)
                    fenster.WindowState = System.Windows.WindowState.Normal;
                }
            }
        }

        #endregion Hilfsmethoden zum Vorbereiten der Views
    }
}
