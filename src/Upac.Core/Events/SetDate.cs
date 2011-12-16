namespace Upac.Core.Events
{
    using System;

    using log4net;

    using umbraco.cms.businesslogic.property;
    using umbraco.cms.businesslogic.web;

    using Upac.Core.Configuration;

    public static class SetDate
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(SetDate));

        #endregion Fields

        #region Methods

        public static void SetDateOnNew(Document sender, umbraco.cms.businesslogic.NewEventArgs e)
        {
            log.Info("SetDateOnNew start");
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format("Name: {0} | ID: {1}", sender.Text, sender.Id));
            }
            Property property = sender.getProperty(ConfigurationManager.PropertyAliases.Date.Value);
            log.Info("SetDateOnNew property found: " + property != null);
            if (property != null)
            {
                property.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            log.Info("SetDateOnNew end");
        }

        #endregion Methods
    }
}