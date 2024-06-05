using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung;

// Erster Schritt für eigene Ereignisse
// -> Die Ereignisdaten-Klasse

/// <summary>
/// Stellt die Daten für das
/// FehlerAufgetreten Ereignis bereit
/// </summary>
/// <remarks>Solche Klassen müssen
/// System.EventArgs erweitern und
/// im Namen auf EventArgs enden</remarks>
public class FehlerAufgetretenEventArgs : System.EventArgs
{
    /// <summary>
    /// Initialisiert ein FehlerAufgetretenEventArgs Objekt
    /// </summary>
    /// <param name="ursache">Die Ausnahme,
    /// die den Fehler beschreibt</param>
    public FehlerAufgetretenEventArgs(System.Exception ursache)
    {
        this._Ursache = ursache;
    }

    /// <summary>
    /// Internes Feld für die Eigenschaft
    /// </summary>
    private System.Exception _Ursache = null!;

    /// <summary>
    /// Ruft das Ausnahme-Objekt ab
    /// mit dem der Fehler beschrieben ist
    /// </summary>
    public System.Exception Ursache => this._Ursache;
    /*
    {
        
        get
        {
            return this._Ursache;
        }
        

        get => this._Ursache;
    }
    */

}


// Zweiter Schritt für eigene Ereignisse
// -> Die Signatur der Methode, die
//    als Ereignis-Behandler erlaubt ist

/// <summary>
/// Stellt die Methode dar, die das
/// FehlerAufgetreten Ereignis behandelt.
/// </summary>
/// <param name="sender">Immer der erste Parameter.
/// Der Verweis auf das Objekt, von dem diese
/// Methode aufgerufen wird</param>
/// <param name="e">Immer der zweite Parameter.
/// Der Verweis auf das Objekt mit den Daten
/// für das Ereignis. Falls keine Daten 
/// geliefert werden, System.EventArgs alleine</param>
public delegate void FehlerAufgetretenEventHandler(
                        object sender,
                        FehlerAufgetretenEventArgs e);