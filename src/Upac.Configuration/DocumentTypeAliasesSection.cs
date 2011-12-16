namespace Upac.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Upac.Configuration.Elements;

    public class DocumentTypeAliasesSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("configurationContainer", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationContainer
        {
            get { return (StringElement)base["configurationContainer"]; }
            set { base["configurationContainer"] = value; }
        }

        [ConfigurationProperty("configurationDateTime", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationDateTime
        {
            get { return (StringElement)base["configurationDateTime"]; }
            set { base["configurationDateTime"] = value; }
        }

        [ConfigurationProperty("configurationDefaultValuesContainer", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationDefaultValuesContainer
        {
            get { return (StringElement)base["configurationDefaultValuesContainer"]; }
            set { base["configurationDefaultValuesContainer"] = value; }
        }

        [ConfigurationProperty("configurationEmail", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationEmail
        {
            get { return (StringElement)base["configurationEmail"]; }
            set { base["configurationEmail"] = value; }
        }

        [ConfigurationProperty("configurationGoogle", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationGoogle
        {
            get { return (StringElement)base["configurationGoogle"]; }
            set { base["configurationGoogle"] = value; }
        }

        [ConfigurationProperty("configurationRss", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationRss
        {
            get { return (StringElement)base["configurationRss"]; }
            set { base["configurationRss"] = value; }
        }

        [ConfigurationProperty("configurationSearchEngineOptimization", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationSearchEngineOptimization
        {
            get { return (StringElement)base["configurationSearchEngineOptimization"]; }
            set { base["configurationSearchEngineOptimization"] = value; }
        }

        [ConfigurationProperty("redirect", IsRequired = true, IsKey = true)]
        public StringElement Redirect
        {
            get { return (StringElement)base["redirect"]; }
            set { base["redirect"] = value; }
        }

        #endregion Properties

        #region Other

        //

        #endregion Other
    }
}