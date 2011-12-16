namespace Upac.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Upac.Configuration.Elements;

    public class SettingsSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("imageGenUrl")]
        public StringElement ImageGenUrl
        {
            get { return (StringElement)base["imageGenUrl"]; }
            set { base["imageGenUrl"] = value; }
        }

        [ConfigurationProperty("setWidthOnRichTextEditorViaTemplateAlias")]
        public BoolElement SetWidthOnRichTextEditorViaTemplateAlias
        {
            get { return (BoolElement)base["setWidthOnRichTextEditorViaTemplateAlias"]; }
            set { base["setWidthOnRichTextEditorViaTemplateAlias"] = value; }
        }

        #endregion Properties
    }
}