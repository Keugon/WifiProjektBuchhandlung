using WIFI.Buchandlung.Client.Models;

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

        private Person _AktuellePerson = null!;

        public Person AktuellePerson
        {
            get => this._AktuellePerson;
            set
            {
                this._AktuellePerson = value;
                OnPropertyChanged();
            }
        }
        private Entlehnungen _Entlehnungen = null!;
        public Entlehnungen EntlehnungenListe
        {
            get
            {
                if (this._Entlehnungen == null)
                {
                    this._Entlehnungen = this.DatenManager!.SqlServerController.HoleEntlehnungenAsync(this.AktuellePerson.ID).Result;
                }
                return this._Entlehnungen;
            }
        }
    }
}
