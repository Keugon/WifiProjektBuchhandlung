namespace WIFI.Anwendung.Daten;

/// <summary>
/// Stellt ein typsicheres dynamisches
/// Array für Sprache - Objekte bereit
/// </summary>
public class Sprachen
    : System.Collections.Generic.List<Sprache>
{

}

/// <summary>
/// Beschreibt eine Anwendungssprache
/// </summary>
/// <remarks>Datentransfer</remarks>
public class Sprache : DatenObjekt
{
    /// <summary>
    /// Ruft das CultureInfo Kürzel
    /// der Sprache ab oder legt dieses fest
    /// </summary>
    [InToString]
    public string Code { get; set; } = string.Empty; //=""

    /// <summary>
    /// Ruft die lesbare Bezeichnung
    /// dieser Sprache ab oder legt
    /// diese fest.
    /// </summary>
    [InToString]
    public string Name { get; set; } = string.Empty;
    /*
        /// <summary>
        /// Gibt einen Text zurück,
        /// der die aktuelle Sprache beschreibt
        /// </summary>
        public override string ToString()
        {
            return $"{this.GetType().Name}(" +
                $"Code=\"{this.Code}\", " +
                $"Name=\"{this.Name}\")";
        }
    */
}
