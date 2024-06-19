namespace WIFI.Anwendung.Werkzeuge
{
    /// <summary>
    /// Stellt Erweiterungsmethoden zum Abrufen
    /// von Assembly Einstellungen bereit
    /// </summary>
    public static class AssemblyInfos : System.Object
    {
        /// <summary>
        /// Gibt die Einstellung vom 
        /// AssemblyCompanyAttribute zurück
        /// </summary>
        /// <param name="fürObjekt">Verweis auf
        /// das Objekt, wo diese Erweiterung benutzt wird</param>
        public static string HoleFirmenname(this object fürObjekt)
        {
            // Wir müssen das AssemblyCompanyAttribute
            // von der Komponente holen, die zum Starten
            // benutzt wurde
            var Einstellungen
                = System.Reflection.Assembly
                    .GetEntryAssembly()!
                        .GetCustomAttributes(
                            typeof(System.Reflection.
                                AssemblyCompanyAttribute),
                        inherit: false);

            // Falls es vorhanden ist,
            // die Einstellung zurückgeben
            if (Einstellungen.Length > 0)
            {
                return ((System.Reflection.AssemblyCompanyAttribute)
                        Einstellungen[0]).Company;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gibt die Einstellung vom 
        /// AssemblyProductAttribute zurück
        /// </summary>
        /// <param name="fürObjekt">Verweis auf
        /// das Objekt, wo diese Erweiterung benutzt wird</param>
        public static string HoleProdukt(this object fürObjekt)
        {
            // Wir müssen das AssemblyProductAttribute
            // von der Komponente holen, die zum Starten
            // benutzt wurde
            var Einstellungen
                = System.Reflection.Assembly
                    .GetEntryAssembly()!
                        .GetCustomAttributes(
                            typeof(System.Reflection.
                                AssemblyProductAttribute),
                        inherit: false);

            // Falls es vorhanden ist,
            // die Einstellung zurückgeben
            if (Einstellungen.Length > 0)
            {
                return ((System.Reflection.AssemblyProductAttribute)
                        Einstellungen[0]).Product;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gibt die Einstellung vom 
        /// Assembly Version zurück
        /// </summary>
        /// <param name="fürObjekt">Verweis auf
        /// das Objekt, wo diese Erweiterung benutzt wird</param>
        /// <remarks>Es wird die Haupt- und Subnummer geliefert</remarks>
        public static string HoleVersion(this object fürObjekt)
        {
            var Version = System.Reflection.Assembly
                .GetEntryAssembly()!.GetName().Version!;

            return $"{Version.Major}.{Version.Minor}";
        }

        /// <summary>
        /// Gibt die Einstellung vom 
        /// AssemblyCopyrightAttribute zurück
        /// </summary>
        /// <param name="fürObjekt">Verweis auf
        /// das Objekt, wo diese Erweiterung benutzt wird</param>
        public static string HoleCopyright(this object fürObjekt)
        {
            // Wir müssen das AssemblyCopyrightAttribute
            // von der Komponente holen, die zum Starten
            // benutzt wurde
            var Einstellungen
                = System.Reflection.Assembly
                    .GetEntryAssembly()!
                        .GetCustomAttributes(
                            typeof(System.Reflection.
                                AssemblyCopyrightAttribute),
                        inherit: false);

            // Falls es vorhanden ist,
            // die Einstellung zurückgeben
            if (Einstellungen.Length > 0)
            {
                return ((System.Reflection.AssemblyCopyrightAttribute)
                        Einstellungen[0]).Copyright;
            }

            return string.Empty;
        }
    }
}
