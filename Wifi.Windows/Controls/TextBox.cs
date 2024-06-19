using System.Windows;
using System.Windows.Input;

namespace WIFI.Windows.Controls
{
    /// <summary>
    /// Stellt ein Steuerelement zum 
    /// Anzeigen oder bearbeiten eines 
    /// unformatierten Textes bereit
    /// </summary>
    /// <remarks>Erweitert die
    /// System.Windows.Controls.TextBox
    /// um ein Wasserzeichen und weitere
    /// möglichkeiten</remarks>
    public class TextBox : System.Windows.Controls.TextBox
    {

        #region Wasserzeichen Information
        /// <summary>
        /// Veröffentlicht die WasserzeichenEigenschaft 
        /// für die Datenbindung.
        /// </summary>
        public static readonly DependencyProperty
            WasserzeichenProperty = DependencyProperty
            .Register(
                "Wasserzeichen",
                typeof(string),
                typeof(TextBox),
                new PropertyMetadata(
                    null,
                    TextBox.WasserzeichenGeändert));
        /// <summary>
        /// Rückruf Mehtode für die Datenbindung
        /// zum Aktualisiseren vom Hintergrund
        /// </summary>
        /// <param name="d">TextBox wo das 
        /// Wasserzeichen geändert wurde</param>
        /// <param name="e">Zusatzdaten</param>
        private static void WasserzeichenGeändert(
            System.Windows.DependencyObject d,
            System.Windows.DependencyPropertyChangedEventArgs e)
        {

            var AktuelleTextBox = d as TextBox;
            if (AktuelleTextBox != null && AktuelleTextBox.IsInitialized)
            {
                if (AktuelleTextBox.Text == string.Empty)
                {
                    AktuelleTextBox.Background
                        = AktuelleTextBox.ErzeugeWasserZeichenGrafik();

                }
                else
                {
                    AktuelleTextBox.Background = AktuelleTextBox.OriginalHintergrund;
                }
            }
            //var AktuelleTextBox = d as TextBox;
            //if(AktuelleTextBox?.Text == string.Empty)
            //{
            //    AktuelleTextBox.Background 
            //        = AktuelleTextBox
            //        .ErzeugeWasserZeichenGrafik();
            //}
            //else
            //{
            //    AktuelleTextBox!.Background = AktuelleTextBox.OriginalHintergrund;
            //}
        }
        /// <summary>
        /// Ruft den Text, der bei einer Leeren Textbox
        /// benutzt werden soll ab
        /// oder legt ihn fest.
        /// </summary>
        /// <remarks>Standart "Enter Here" oder eine
        /// entsprechende Lokalisierung</remarks>
        public string? Wasserzeichen
        {
            get => this.GetValue(WasserzeichenProperty) as string ?? UITexte.WasserzeichenStandard;
            set => this.SetValue(WasserzeichenProperty, value);
        }
        #endregion Wasserzeichen Information

        #region Wasserzeichen Logik
        /// <summary>
        /// Ruft die Background Eigenschaft dieser
        /// Textbox beim erstellen ab
        /// oder legt diese fest
        /// </summary>
        /// <remarks>
        /// wird zum entfernen des Wasserzeichens benötigt
        /// </remarks>
        private System.Windows.Media.Brush? OriginalHintergrund { get; set; }
        /// <summary>
        /// Löst das Initialized Erreignis aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        /// <remarks>Wird um das cachen vom OriginalHintergrund
        /// erweitert</remarks>
        protected override void OnInitialized(EventArgs e)
        {

            base.OnInitialized(e);
            this.OriginalHintergrund = this.Background;
            if (this.Text == string.Empty)
            {
                this.Background = this.ErzeugeWasserZeichenGrafik();
            }
        }
        /// <summary>
        /// Gibt ein Visual Brush objekt für
        /// einen Label mit dem Wasserzeichen 
        /// zurück
        /// </summary>
        /// <returns></returns>
        protected virtual System.Windows.Media.Brush
            ErzeugeWasserZeichenGrafik()
        {
            var l = new System.Windows.Controls.Label();
            var b = new System.Windows.Media.VisualBrush(l);

            l.Content = this.Wasserzeichen;
            l.Width = this.Width;
            l.Height = this.Height;
            l.VerticalAlignment = VerticalAlignment.Stretch;
            l.VerticalContentAlignment = VerticalAlignment.Center;
            //20240502 Hr. Draxler
            //Hier muss der Original Hintergrund benutzt
            //werden
            //Das ist der Background vom Label nicht der Textbox selbst?
            //l.Background = this.OriginalHintergrund;
            l.FontStyle = System.Windows.FontStyles.Italic;
            l.Foreground = System.Windows.SystemColors.ControlDarkBrush;
            l.Padding = new Thickness(0);
            b.AlignmentX = 0;
            // b.AlignmentY = 0;
            b.Stretch = System.Windows.Media.Stretch.None;


            return b;
        }
        /// <summary>
        /// löst das TextChanged Ereignis aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        /// <remarks>Wird um das Ein/Abschalten 
        /// des Wasserzeichens ergänzt</remarks>
        protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (this.Text == string.Empty)
            {
                this.Background = this.ErzeugeWasserZeichenGrafik();
            }
            else
            {
                this.Background = this.OriginalHintergrund;
            }
        }
        #endregion Wasserzeichen Logik

        #region Mit Eingabe zum nächsten Feld
        /// <summary>
        /// Löst das Key Up Ereignis aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        /// <remarks>Verschiebt beim los lassen der Eingabe Taste
        /// den Focus auf das nächste Steuerelement
        /// in der Reihenfolge</remarks>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.Enter)
            {
                this.MoveFocus(
                    new TraversalRequest(
                        FocusNavigationDirection
                        .Next));
            }
        }

        #endregion Mit Eingabe zum nächsten Feld

        #region Beim Aktivieren den Inhalt makieren
        /// <summary>
        /// Löst das GotKeyboardFocus Ereignis aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        /// <remarks>Wird um das makieren des 
        /// gesammten Inhalts erweitert</remarks>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            this.SelectAll();
        }
        /// <summary>
        /// Löst das Rooted OnMouseDown Ereignis aus
        /// </summary>
        /// <param name="e">Zusatzdaten</param>
        /// <remarks>Falls diese Textbox noch nicht
        /// hat, den Focus einstellen und
        /// die Weitergabe vom Ereignis beenden</remarks>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (!this.IsFocused)
            {
                this.Focus();
                this.SelectAll();
                e.Handled = true;
            }
        }

        #endregion Beim Aktivieren den Inhalt makieren
    }
}
