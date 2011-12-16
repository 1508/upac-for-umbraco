namespace Upac.Configuration.Elements
{
    using System;
    using System.Configuration;

    public class StringElement : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("value", IsRequired = false)]
        public String Value
        {
            get
            {
                return (String) base["value"];
            }
            set
            {
                base["value"] = value;
            }
        }

        #endregion Properties
    }
}