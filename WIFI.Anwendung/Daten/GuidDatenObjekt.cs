namespace WIFI.Anwendung.Daten
{
    /// <summary>
    /// Stellt ein DatenTransferObjekt mit einer
    /// Weltweit eindeutigen 
    /// Identifikationsnummer bereit.
    /// </summary>
    public abstract class GuidDatenObjekt
        : WIFI.Anwendung.Daten.DatenObjekt
    {
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private System.Guid? _ID = null;
        /// <summary>
        /// Ruft den Weltweit eindeutigen
        /// Schlüssel für das Objekt ab
        /// oder legt diesen fest
        /// </summary>
        /// <remarks>Existiert seit 
        /// 2024.5.0.0</remarks>
        public System.Guid ID
        {
            get
            {
                if (this._ID == null)
                {
                    this._ID = System.Guid.NewGuid();
                }
                return this._ID.Value;
            }
            set => this._ID = value;
        }
    }
}
