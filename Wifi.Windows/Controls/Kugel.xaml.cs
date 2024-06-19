using System.Windows;
using System.Windows.Controls;

namespace WIFI.Windows.Controls
{
    /// <summary>
    /// Eine Ganzzahl in einem 
    /// eingefärbten Kreis
    /// </summary>
    public partial class Kugel : UserControl
    {
        /// <summary>
        /// Initialisiert ein neues Kugel Objekt
        /// </summary>
        public Kugel()
        {
            InitializeComponent();
        }

        #region Bindbare Zahl Eigenschaft
        /// <summary>
        /// Ruft den Wert dieser Kugel 
        /// ab oder legt diesen fest
        /// </summary>
        public int? Zahl
        {
            get => this.GetValue(Kugel.ZahlProperty) as int?;
            set => this.SetValue(Kugel.ZahlProperty, value);
        }
        /// <summary>
        /// Veröffentlicht die Zahl Eigenschaft 
        /// für die WPF Datenbindung
        /// </summary>
        public static readonly
            DependencyProperty ZahlProperty
            = DependencyProperty
            .Register(
                "Zahl",
                typeof(int?),
                typeof(Kugel));

        #endregion Bindbare Zahl Eigenschaft
    }
}
