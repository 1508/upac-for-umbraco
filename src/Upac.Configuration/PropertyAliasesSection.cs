namespace Upac.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Upac.Configuration.Elements;

    public class PropertyAliasesSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("hideFromNavigation", IsRequired = true, IsKey = true)]
        public StringElement HideFromNavigation
        {
            get { return (StringElement)base["hideFromNavigation"]; }
            set { base["hideFromNavigation"] = value; }
        }

        [ConfigurationProperty("date", IsRequired = true, IsKey = true)]
        public StringElement ImageGenUrl
        {
            get { return (StringElement)base["date"]; }
            set { base["date"] = value; }
        }

        #endregion Properties
    }
}