namespace Upac.GoogleSiteSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Settings
    {
        #region Fields

        public static string GoogleSiteSearchApiKey = System.Configuration.ConfigurationManager.AppSettings["Upac.GoogleSiteSearch.GoogleSiteSearchApiKey"];

        #endregion Fields
    }
}