using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;

namespace WIFI.Buchandlung.Client.ViewModels
{
    public class PersonAnlegenViewModel : WIFI.Windows.ViewModel
    {
        #region Befehle
        public Befehl PersonAnlegenCommand => new Befehl(p => PersonAnlegen());
        #endregion  Befehle
        #region Bindings

        public string Vorname { get; set; } = null!;
        public string Nachname { get; set; } = null!;
        public string PLZ { get; set; } = null!;
        public string Ort { get; set; } = null!;
        public string Straße { get; set; } = null!;
        public string TelNr { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string AusweisNr { get; set; } = null!;

        #endregion Bindings
        public DatenManager? DatenManager { get; set; }
        /// <summary>
        /// Legt einen neuen Artigel in der Datenbank an
        /// </summary>
        public void PersonAnlegen()
        {
            try
            {
                Guid newGuidOnDemand = Guid.NewGuid();
                System.Diagnostics.Debug.WriteLine($"Rückmeldung aus der SQL Artikel Anlegen:{this.DatenManager!.SqlServerController
                    .PersonAnlegen(
                    guid: newGuidOnDemand,
                    vorname: Vorname,
                    nachname: Nachname,
                    plz: int.Parse(PLZ),
                    ort: Ort,
                    straße:Straße,
                    telefonNr: int.Parse(TelNr),
                    email: Email,
                    ausweisNr: AusweisNr
                    
                    ).Result}");
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
        }
    }
}
