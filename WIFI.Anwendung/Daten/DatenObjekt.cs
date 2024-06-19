namespace WIFI.Anwendung.Daten
{
    /// <summary>
    /// Kennzeichnet eine Eigenschaft 
    /// deren Inhalt im 
    /// ToString() Ergebnis angeführt werden soll
    /// </summary>
    /// <remarks>Mit Hilfe der Position Eigenschaft
    /// kan die Reihenfolge gesteuert werden
    /// sollte die Position nicht angegeben werden 
    /// befinden sich diese am Ende(int.maxValue)</remarks>
    public class InToStringAttribute : System.Attribute
    {
        /// <summary>
        /// Initialisiert ein neues leeres InToStringAttribute 
        /// objekt
        /// </summary>
        public InToStringAttribute()
        {

        }
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private int _Position = int.MaxValue;
        /// <summary>
        /// Ruft die Stelle ab wo die
        /// gekennzeichnete Eigenschaft im
        /// ToString Ergebniss platziert werden
        /// soll
        /// </summary>
        public int Position => this._Position;
        /// <summary>
        /// Initialisiert ein neues InToStringAttribute 
        /// mit Prioritäts angabe (ReihenFolge)
        /// </summary>
        /// <param name="position">Wird zum 
        /// Sortieren des ToString Ergebnisses benutzt
        /// </param>
        public InToStringAttribute(int position)
        {
            this._Position = position;
        }
    }
    /// <summary>
    /// Unterstützt semtliche WIFI DTOs
    /// mit einer Basislogik
    /// </summary>
    public abstract class DatenObjekt : System.Object
    {
        #region Zurunterstützung
        /// <summary>
        /// Stellt eine Typsichere auflistung
        /// von Eigenschaften bereit, deren Inhalt im ToString() 
        /// Ergebnis angeführt sein soll.
        /// </summary>
        private class InToStringEigenschaften
            : System.Collections.Generic
            .List<System.Reflection.PropertyInfo>
        {

        }
        /// <summary>
        /// Stellt eine typsichere Hashtable
        /// zum Sammeln der Eigenschaften für
        /// die ToString Ausgabe je Typ bereit
        /// </summary>
        private class InToStringWörterbuch
            : System.Collections.Generic.Dictionary<System.Type, InToStringEigenschaften>
        {

        }
        #endregion Zurunterstützung

        #region Analysebereich
        /// <summary>
        /// Interner Singelton Cache zum Sammeln der Analysergebnisse
        /// </summary>
        private static InToStringWörterbuch _AnalysierteTypen = null!;
        /// <summary>
        /// Ruft die Liste mit den bereis 
        /// bekannten Typen ab
        /// </summary>
        private InToStringWörterbuch AnalysierteTypen
        {
            get
            {
                if (DatenObjekt._AnalysierteTypen == null)
                {
                    DatenObjekt._AnalysierteTypen
                        = new InToStringWörterbuch();
                }
                return _AnalysierteTypen;
            }
        }
        /// <summary>
        /// Gibt für den Aktuellen Typ die Liste
        /// der Eigenschaften zurück die mit 
        /// InToStringAttribute gekennzeichnet sind
        /// </summary>
        /// <returns>Die Liste mit den PropertyInfo
        /// Objekten der gekennzeichneten Eigenschaften</returns>
        /// <remarks>Falls die Liste Leer ist
        /// sind keine Eigenschaften mit InToStringAttribute makiert
        /// </remarks>
        private InToStringEigenschaften FindeInToStingEigenschaft()
        {
            var GefundeneEigenschaften = new InToStringEigenschaften();

            //Zum Sammeln der Positions Einstellungen
            var EigenschaftenMitPosition
                = new System.Collections.Generic.Dictionary<
                    System.Reflection.PropertyInfo,
                    int>();

            foreach (var e in this.GetType().GetProperties())
            {
                //Zum Sortieren eine Eigenschaft
                //die InToStringAttribute besitzt
                //auch die Einstellung von InToStringAttribute
                //festellen und merken
                var Einstellungen = e.GetCustomAttributes(
                    typeof(
                    InToStringAttribute),
                    inherit: true);

                if (Einstellungen.Length > 0)
                {
                    int GewünschtePosition
                        = (Einstellungen[0] as InToStringAttribute)!
                        .Position;
                    //Reihenfolge des Findens
                    EigenschaftenMitPosition.Add(e, GewünschtePosition);
                }
            }
            //Entsprechen der Position-Einstellung Sortieren
            GefundeneEigenschaften.AddRange(from e in EigenschaftenMitPosition
                                            orderby e.Value
                                            select e.Key);


            return GefundeneEigenschaften;
        }

        #endregion Analysebereich
        /// <summary>
        /// Gibt eine lesbare Beschreibung dieses Subjekts 
        /// als text zurück
        /// </summary>
        /// <remarks>Das Ergebnis kann mit dem
        /// InToStringAttribute gesteuert werden</remarks>
        public override string ToString()
        {
            InToStringEigenschaften InToString = null!;
            //Ist der Typ bereits bekannt
            if (this.AnalysierteTypen.ContainsKey(this.GetType()))
            {
                InToString = this.AnalysierteTypen[this.GetType()];
            }
            else
            {
                //Wenn nein, analysieren und chachen
                InToString = this.FindeInToStingEigenschaft();
                this.AnalysierteTypen.Add(this.GetType(), InToString);
            }

            //sind InToStringEigenschaften vorhanden?
            if (InToString.Count > 0)
            {
                //Wenn Ja, dass ToString ergebniss bauen
                var Ergebnis = $"{this.GetType().Name}(";
                //Nur der Name vom Typ

                // var NewList = InToString.OrderBy(p => p.GetCustomAttributesData()[0].ConstructorArguments[0].Value);

                //die Namen der Eigenschaften
                //mit den aktuellen Werten
                foreach (var e in InToString)
                {
                    var Wert = e.GetValue(this);

                    //Textinformation unter Anführungszeichen
                    if (e.PropertyType == typeof(System.String) && Wert != null)
                    {
                        Ergebnis += $"{e.Name}=\"{Wert}\", ";
                    }
                    else
                    {
                        Ergebnis += $"{e.Name}={Wert}, ";
                    }
                }
                //Das In der Schleife angehängte ", " entfernen
                Ergebnis = Ergebnis.TrimEnd(',', ' ');
                Ergebnis += ")";

                //und Zurückgeben
                return Ergebnis;
            }
            //Wenn nein, der Standartrückgabewert
            return base.ToString()!;
        }
    }
}
