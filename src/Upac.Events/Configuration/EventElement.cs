namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class EventElement : ConfigurationElement
    {
        #region Constructors

        public EventElement(Boolean enabled, String targetType, String targetEvent)
        {
            this.Enabled = enabled;
            this.TargetType = targetType;
            this.TargetEvent = targetEvent;
        }

        public EventElement()
        {
            // Attributes on the properties provide default values.
        }

        #endregion Constructors

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

        [ConfigurationProperty("handlers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HandlerCollection), AddItemName = "handler", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HandlerCollection Handlers
        {
            get
            {
                return (HandlerCollection)base["handlers"];
            }
        }

        public string Key
        {
            get { return TargetType + "." + TargetEvent; }
        }

        [ConfigurationProperty("targetEvent", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string TargetEvent
        {
            get
            {
                return (string)this["targetEvent"];
            }
            set
            {
                this["targetEvent"] = value;
            }
        }

        [ConfigurationProperty("targetType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string TargetType
        {
            get
            {
                return (string)this["targetType"];
            }
            set
            {
                this["targetType"] = value;
            }
        }

        #endregion Properties
    }
}