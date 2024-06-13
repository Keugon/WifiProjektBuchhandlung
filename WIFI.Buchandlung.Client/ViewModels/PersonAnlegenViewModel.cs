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
        /// <summary>
        /// CanExecute:Prüft zusätzlich ob alle notwendigen
        /// Textfelder nicht leer sind und gibt den Button frei
        /// </summary>
        public Befehl PersonAnlegenCommand 
            => new Befehl(p => PersonAnlegen(), p =>
        {

            if (Tools.General
            .AreStringsValid(
                Vorname, 
                Nachname, 
                PLZ, 
                Ort, 
                Straße, 
                TelNr, 
                Email, 
                AusweisNr))
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
        public DateTime GeburtsTag { get; set; } = DateTime.Today;

        #endregion Bindings

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
        public void PersonAnlegen()
        {
            //Check für über mindestens 8 Jahre alt
            
            if ((DateTime.Today.Year - this.GeburtsTag.Year) < 8)
            {
                MessageBox
                    .Show(
                    $"Das Angegebene Alter: " +
                    $"{(DateTime.Today.Year - this.GeburtsTag.Year)}" +
                    $" ist niedriger als das Mindestalter von 8 Jahren!");
                return;
            }
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
        #endregion Methoden
        
        
    }
}
