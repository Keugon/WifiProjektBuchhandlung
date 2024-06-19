namespace WIFI.Windows
{
    /// <summary>
    /// Kapselt eine Methode für
    /// die WPF Command Bindung
    /// </summary>
    public class Befehl
        : System.Object, System.Windows.Input.ICommand
    {
        /// <summary>
        /// Wird ausgelöst, wenn sich die
        /// Voraussetzung für CanExecute geändert hat
        /// </summary>
        /// <remarks>
        /// Die Bahandler methode wird an den
        /// Windows-BefehlsManager weitergegeben</remarks>
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                System.Windows.Input.CommandManager
                    .RequerySuggested += value;
            }
            remove
            {
                System.Windows.Input.CommandManager
                    .RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Internes Feld für die Methode
        /// zum Prüfen, ob der Befehl aktuell
        /// zulässig ist.
        /// </summary>
        private System.Predicate<object?>? IstBefehlErlaubt = null;

        /// <summary>
        /// Gibt einen Wahrheitswert zurück,
        /// der angibt, ob die Methode des
        /// Befehls aktuell zulässig ist oder nicht.
        /// </summary>
        /// <param name="parameter">Zusatzdaten der Datenbindung</param>
        /// <returns>True, wenn der Befehl gültig ist</returns>
        /// <remarks>Sollte keine Prüfmethode vorhanden sein,
        /// wird True als Standard zurückgegeben</remarks>
        public bool CanExecute(object? parameter)
        {
            return this.IstBefehlErlaubt == null ?
                true : this.IstBefehlErlaubt(parameter);
        }

        /// <summary>
        /// Internes Feld für die Speicheradresse
        /// der Methode, die in diesem Befehl versteckt ist
        /// </summary>
        private System.Action<object?> Befehlsmethode = null!;

        /// <summary>
        /// Führt die Methode dieses Befehls aus
        /// </summary>
        /// <param name="parameter">Zusatzdaten der Datenbindung</param>
        public void Execute(object? parameter)
        {
            this.Befehlsmethode(parameter);
        }

        /// <summary>
        /// Initialisiert einen neuen Befehl
        /// für die Command WPF Datenbindung
        /// </summary>
        /// <param name="befehlsmethode">Speicheradresse
        /// der Methode, die bei diesem Befehl
        /// ausgeführt werden soll</param>
        /// <remarks>Dieser Befehl ist immer gültig</remarks>
        public Befehl(System.Action<object?> befehlsmethode)
            : this(befehlsmethode, istBefehlErlaubt: null!)
        {

        }

        /// <summary>
        /// Initialisiert einen neuen Befehl
        /// für die Command WPF Datenbindung
        /// </summary>
        /// <param name="befehlsmethode">Speicheradresse
        /// der Methode, die bei diesem Befehl
        /// ausgeführt werden soll</param>
        /// <param name="istBefehlErlaubt">Speicheradresse
        /// der Methode, die prüft, ob der
        /// Befehl aktuell zulässig ist</param>
        public Befehl(
            System.Action<object?> befehlsmethode,
            System.Predicate<object?> istBefehlErlaubt)
        {
            this.Befehlsmethode = befehlsmethode;
            this.IstBefehlErlaubt = istBefehlErlaubt;
        }
    }
}
