namespace Upac.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Upac.Core.Configuration.Elements;

    public class DocumentTypeAliasesSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("dtConfigurationContainer", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationContainer
        {
            get { return (StringElement)base["dtConfigurationContainer"]; }
            set { base["dtConfigurationContainer"] = value; }
        }

        [ConfigurationProperty("dtConfigurationDateTime", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationDateTime
        {
            get { return (StringElement)base["dtConfigurationDateTime"]; }
            set { base["dtConfigurationDateTime"] = value; }
        }

        [ConfigurationProperty("dtConfigurationDefaultValuesContainer", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationDefaultValuesContainer
        {
            get { return (StringElement)base["dtConfigurationDefaultValuesContainer"]; }
            set { base["dtConfigurationDefaultValuesContainer"] = value; }
        }

        [ConfigurationProperty("dtConfigurationEmail", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationEmail
        {
            get { return (StringElement)base["dtConfigurationEmail"]; }
            set { base["dtConfigurationEmail"] = value; }
        }

        [ConfigurationProperty("dtConfigurationGoogle", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationGoogle
        {
            get { return (StringElement)base["dtConfigurationGoogle"]; }
            set { base["dtConfigurationGoogle"] = value; }
        }

        [ConfigurationProperty("dtConfigurationRss", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationRss
        {
            get { return (StringElement)base["dtConfigurationRss"]; }
            set { base["dtConfigurationRss"] = value; }
        }

        [ConfigurationProperty("dtConfigurationSearchEngineOptimization", IsRequired = true, IsKey = true)]
        public StringElement ConfigurationSearchEngineOptimization
        {
            get { return (StringElement)base["dtConfigurationSearchEngineOptimization"]; }
            set { base["dtConfigurationSearchEngineOptimization"] = value; }
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