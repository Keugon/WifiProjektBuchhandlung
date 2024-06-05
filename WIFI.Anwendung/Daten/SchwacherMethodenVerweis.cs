using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Anwendung.Daten
{
    /// <summary>
    /// Stellt eine typsichere Liste für Schwacher
    /// MethodenVerweis Objekte bereit.
    /// </summary>
    public class SchwacherMethodenVerweisListe
        : System.Collections.Generic.List<SchwacherMethodenVerweis>
    {
        /// <summary>
        /// Fürt alle hinterlegten Methoden aus, wenn die Besitzer
        /// noch existieren.
        /// </summary>
        public void AlleAufrufen()
        {
            foreach (var m in this)
            {
                m.Methode?.Invoke();
            }
        }
        /// <summary>
        /// ruft die Anzahl der Methoden 
        /// ab wo die Besitzer nicht mehr
        /// existieren
        /// </summary>
        public int ToteBesitzer => (
            from m in this  
            where m.Methode == null 
            select m)
            .Count();
        
           
        
    }
    /// <summary>
    /// Kapselt die Speicheradresse einer Methode ohne 
    /// die GarbageCollection am entfernen des Besitzers
    /// zu verhindern
    /// </summary>
    /// <remarks>Notwendig um in bstimmten Situationen 
    /// Speicherlöcher zu verhindern.
    /// (Ergänzt in 2024.4.0.0)</remarks>
    public class SchwacherMethodenVerweis : System.Object
    {
        /// <summary>
        /// Initialisiert ein neues SchwacherMethodenVerweis
        /// Objekt
        /// </summary>
        /// <param name="methode">Die Speicheradresse der Methode,
        /// die ohne die GarbageCollection zu blockieren
        /// gekapselt werden soll</param>
        public SchwacherMethodenVerweis(System.Action methode)
        {
            this._Methode = new System.WeakReference(methode);
        }
        /// <summary>
        /// Internes Feld für die Eigenschaft, 
        /// ohne die GarbageCollection am 
        /// entfernen des Besitzter Objekts zu hindern
        /// </summary>
        private System.WeakReference _Methode;
        /// <summary>
        /// Ruft die gekapselte Methode ab
        /// </summary>
        /// <remarks>Null falls das Besitzer
        /// objekt nicht mehr existiert</remarks>
        public System.Action? Methode
        {
            get => this._Methode.IsAlive ? 
                this._Methode.Target as System.Action 
                : null;
        }
    }
}
