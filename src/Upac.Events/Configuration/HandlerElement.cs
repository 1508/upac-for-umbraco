namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class HandlerElement : ConfigurationElement
    {
        #region Constructors

        public HandlerElement(Boolean enabled, String method, String type)
        {
            this.Enabled = enabled;
            this.Method = method;
            this.Type = type;
        }

        public HandlerElement()
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

        public string Key
        {
            get { return Method + "." + Type; }
        }

        [ConfigurationProperty("method", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Method
        {
            get
            {
                return (string)this["method"];
            }
            set
            {
                this["method"] = value;
            }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        #endregion Properties
    }
}