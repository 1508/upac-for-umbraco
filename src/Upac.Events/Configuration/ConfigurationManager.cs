namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ConfigurationManager
    {
        #region Fields

        private static EventsSection eventsSection;

        #endregion Fields

        #region Properties

        public static EventsSection Settings
        {
            get
            {
                return (eventsSection = eventsSection ?? (System.Configuration.ConfigurationManager.GetSection("umbracoevents") as EventsSection));
            }
        }

        #endregion Properties
    }
}