namespace Upac.CampaignMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Settings
    {
        #region Fields

        public static string ApiClientId = System.Configuration.ConfigurationManager.AppSettings["Upac.CampaignMonitor.ApiClientId"];
        public static string ApiKey = System.Configuration.ConfigurationManager.AppSettings["Upac.CampaignMonitor.ApiKey"];

        #endregion Fields
    }
}