using System.Configuration;
using System.Data;
using System.Windows;
using WIFI.Anwendung;
using WIFI.Buchandlung.Client;



namespace WIFI.Buchandlung.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Ruft die WIFI Infrastruktur
        /// ab oder legt diese fest
        /// </summary>
        protected WIFI.Anwendung.Infrastruktur
            Kontext
        { get; set; } = null!;


        /// <summary>
        /// Löst das Startup-Ereignis aus
        /// </summary>
        /// <param name="e">Die Ereignisdaten
        /// mit den Startparametern aus der
        /// Eingabeaufforderung</param>
        /// <remarks>Wird um das Hochfahren
        /// der WIFI Infrastruktur erweitert</remarks>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Zuerst einmal das, was sonst passiert
            base.OnStartup(e);

            // Unsere Infrastruktur hochfahren
            this.Kontext = new WIFI.Anwendung.Infrastruktur();

            // Die zuletzt benutzte Sprache wiederherstellen
            this.Kontext.Sprachen.Festlegen(
                Client.Properties.Settings.Default.LetzterSprachcode);

            // Das ViewModel initialisieren
            var vm = this.Kontext.Produziere<ViewModels.Anwendung>();
            
            // Die Hauptfenster View als Oberfläche benutzen
            vm.Anzeigen<Views.Hauptfenster>();

        }

        /// <summary>
        /// Löst das Exit-Ereignis aus
        /// </summary>
        /// <param name="e">Die Ereignisdaten
        /// mit der Ursache des Beendens</param>
        /// <remarks>Wird um das Herunterfahren
        /// der WIFI Infrastruktur erweitert</remarks>
        protected override void OnExit(ExitEventArgs e)
        {
            // Zuerst einmal die eigene Infrastruktur
            // herunterfahren...

            WIFI.Buchandlung.Client.Properties.Settings.Default
                .LetzterSprachcode = this.Kontext.Sprachen
                    .AktuelleSprache.Code;
          WIFI.Buchandlung.Client.Properties.Settings.Default.Save();

            this.Kontext.Fenster.Speichern();

            // Zum Schluss noch das, was sonst passiert
            base.OnExit(e);
        }
    }

}
