namespace WIFI.Anwendung.Controller;

/// <summary>
/// Stellt ein Dienst zum Lesen
/// und Schreiben von Fensterpositionen
/// der Anwendung bereit
/// </summary>
/// <remarks>Es wird die Xml Serialisierung benutzt</remarks>
internal class FensterdatenController
    : Generisch.XmlController<Daten.Fensterinfos>
{

}

