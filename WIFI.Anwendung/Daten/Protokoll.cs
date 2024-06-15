namespace WIFI.Anwendung.Daten
{

    /// <summary>
    /// Stellt eine Typsichere liste von
    /// Protokolleintrag Objekt bereit
    /// </summary>
    /// <remarks>Kann für die WPF Datenbindung
    /// benutzt werden(INotifyCollectionChanged)</remarks>
    public class Protokolleinträge
        : System.Collections.ObjectModel.ObservableCollection
        <Protokolleintrag>
    {

    }
    /// <summary>
    /// Ermöglicht das Unterscheiden
    /// von Protokolleinträgen
    /// </summary>
    /// <remarks>Beim Casten aufpassen dass
    /// System.Byte als grundlage benutzt ist. 
    /// damit beim Speichern von Einträgen in eine Datenbank
    /// weniger Speicher verschwendet wird.</remarks>
    public enum ProtokolleintragTyp : byte
    {
        /// <summary>
        /// Kennzeichnet eine Information
        /// </summary>
        Normal,
        /// <summary>
        /// Kennzeichnet eine Information der
        /// Beachtung geschenkt werden soll
        /// </summary>
        Warnung,
        /// <summary>
        /// Kennzeichnet eine Information
        /// die eine Ausnahme beschreibt
        /// </summary>
        Fehler
    }
    /// <summary>
    /// Stellt die Daten für eine Zeile 
    /// des Anwendungsprotokolls bereit
    /// </summary>
    public class Protokolleintrag : DatenObjekt
    {
        /// <summary>
        /// Ruft den Zeitstempel ab,
        /// wann dieser Eintrag erstellt wurde oder
        /// legt diesen fest.
        /// </summary>
        /// <remarks>Standartwert die Aktuellezeit</remarks>

        public System.DateTime Zeitpunkt { get; set; }
            = System.DateTime.Now;
        /// <summary>
        /// Ruft die Informaitions stufe dieses Eintrags
        /// ab oder legt diese fest
        /// </summary>
        /// <remarks>Standartwert Normal</remarks>
        [InToString(0)]
        public ProtokolleintragTyp Typ { get; set; }
        = ProtokolleintragTyp.Normal;
        /// <summary>
        /// Ruft die lesbare Information des Eintrags ab 
        /// oder legt diese fest.
        /// </summary>
        [InToString(1)]
        public string Text { get; set; } = string.Empty;


    }
}
