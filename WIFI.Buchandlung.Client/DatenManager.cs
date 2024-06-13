﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIFI.Buchandlung.Client
{
    public class DatenManager :WIFI.Anwendung.AppObjekt
    {
        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Models.SqlServerController _SqlServerController = null!;
        /// <summary>
        /// Stellt einen Dienst zum abrufen von Daten
        /// eines SQL Servers bereit
        /// </summary>
        public Models.SqlServerController SqlServerController
        {
            get
            {
                if(this._SqlServerController == null)
                {
                 this._SqlServerController =   this.Kontext.Produziere<Models.SqlServerController>();
                }
                return this._SqlServerController!;
            }
        }
    }
}