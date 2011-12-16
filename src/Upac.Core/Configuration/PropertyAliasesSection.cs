namespace Upac.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Upac.Core.Configuration.Elements;

    public class PropertyAliasesSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("date", IsRequired = true, IsKey = true)]
        public StringElement Date
        {
            get { return (StringElement)base["date"]; }
            set { base["date"] = value; }
        }

        [ConfigurationProperty("hideFromNavigation", IsRequired = true, IsKey = true)]
        public StringElement HideFromNavigation
        {
            get { return (StringElement)base["hideFromNavigation"]; }
            set { base["hideFromNavigation"] = value; }
        }

        #endregion Properties
    }
}