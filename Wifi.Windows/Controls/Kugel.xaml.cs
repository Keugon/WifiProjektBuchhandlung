using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
