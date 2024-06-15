using WIFI.Buchandlung.Client.Models;

namespace WIFI.Buchandlung.Client.ViewModels
{
    public class PersonenKarteiViewModel : WIFI.Windows.ViewModel
    {

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
    }
}
