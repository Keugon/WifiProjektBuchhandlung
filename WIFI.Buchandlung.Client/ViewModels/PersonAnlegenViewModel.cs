using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WIFI.Buchandlung.Client.Models;
using WIFI.Windows;

namespace WIFI.Buchandlung.Client.ViewModels
{
    public class PersonAnlegenViewModel : WIFI.Windows.ViewModel
    {
        #region Befehle
        public Befehl PersonAnlegenCommand => new Befehl(p => PersonAnlegen(), p =>
        {

            if (Tools.General.AreStringsValid(Vorname, Nachname, PLZ, Ort, Straße, TelNr, Email, AusweisNr))
            {
                return true;
            }
            return false;
        });
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
                int rückmeldung = this.DatenManager!.SqlServerController
                    .PersonAnlegen(
                    guid: newGuidOnDemand,
                    vorname: Vorname,
                    nachname: Nachname,
                    plz: int.Parse(PLZ),
                    ort: Ort,
                    straße: Straße,
                    telefonNr: TelNr,
                    email: Email,
                    ausweisNr: AusweisNr

                    ).Result;
                System.Diagnostics.Debug.WriteLine($"Rückmeldung aus dem Personne Anlegen:{rückmeldung}");
                if (rückmeldung == 2)
                {
                    MessageBox.Show("Person wurde Angelegt");
                }
            }

            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
        }
        
    }
}
