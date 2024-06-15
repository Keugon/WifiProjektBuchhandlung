namespace WIFI.Anwendung.Daten;

/// <summary>
/// Stellt ein typsicheres dynamisches 
/// Array für Fensterinfo - Objekte bereit
/// </summary>
public class Fensterinfos
    : System.Collections.Generic.List<Fensterinfo>
{

}

/// <summary>
/// Beschreibt die Größe, die Position
/// und den Zustand eines Anwendungsfensters
/// </summary>
public class Fensterinfo : DatenObjekt
{
    /// <summary>
    /// Ruft den Schlüssel für diese Fensterinfo
    /// ab oder legt diesen fest
    /// </summary>
    [InToString]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den Fensterzustand (Normal,
    /// Maximiert bzw. Minimiert) ab
    /// oder legt diesen fest
    /// </summary>
    public int Zustand { get; set; }

    /// <summary>
    /// Ruft die linke Position
    /// des Fensters ab oder legt
    /// diese fest
    /// </summary>
    /// <remarks>Null, wenn kein
    /// Wert vorliegt</remarks>
    public int? Links { get; set; }

    /// <summary>
    /// Ruft die obere Position
    /// des Fensters ab oder legt
    /// diese fest
    /// </summary>
    /// <remarks>Null, wenn kein
    /// Wert vorliegt</remarks>
    public int? Oben { get; set; }

    /// <summary>
    /// Ruft die Breite
    /// des Fensters ab oder legt
    /// diese fest
    /// </summary>
    /// <remarks>Null, wenn kein
    /// Wert vorliegt</remarks>
    public int? Breite { get; set; }
    /// <summary>
    /// Ruft die Höhe
    /// des Fensters ab oder legt
    /// diese fest
    /// </summary>
    /// <remarks>Null, wenn kein
    /// Wert vorliegt</remarks>
    public int? Höhe { get; set; }
    /*
        /// <summary>
        /// Gibt einen Text zurück,
        /// der das aktuelle Fensterinfo beschreibt
        /// </summary>
        public override string ToString()
        {
            return $"{this.GetType().Name}(Name=\"{this.Name}\")";
        }
    */
}
