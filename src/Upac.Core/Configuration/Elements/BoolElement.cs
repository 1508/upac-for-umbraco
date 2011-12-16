namespace Upac.Core.Configuration.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class BoolElement : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("value", IsRequired = false, DefaultValue = true)]
        public Boolean Value
        {
            get
            {
                return (Boolean) this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }

        #endregion Properties
    }
}