namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class EventsSection : ConfigurationSection
    {
        #region Properties

        // Create a "enabled" attribute.
        [ConfigurationProperty("enabled", DefaultValue = "true", IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }

        [ConfigurationProperty("events", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(EventCollection), AddItemName = "event", ClearItemsName = "clear", RemoveItemName = "remove")]
        public EventCollection Events
        {
            get
            {
                return (EventCollection)base["events"];
            }
        }

        #endregion Properties
    }
}