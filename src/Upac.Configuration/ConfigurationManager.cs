namespace Upac.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ConfigurationManager
    {
        #region Fields

        private static DocumentTypeAliasesSection _documentTypeAliasesSection;
        private static PropertyAliasesSection _propertyAliasesSection;
        private static SettingsSection _settingsSection;

        #endregion Fields

        #region Properties

        public static DocumentTypeAliasesSection DocumentTypeAliases
        {
            get
            {
                return (_documentTypeAliasesSection = _documentTypeAliasesSection ?? (System.Configuration.ConfigurationManager.GetSection("upac2/documentTypeAliases") as DocumentTypeAliasesSection));
            }
        }

        public static PropertyAliasesSection PropertyAliases
        {
            get
            {
                return (_propertyAliasesSection = _propertyAliasesSection ?? (System.Configuration.ConfigurationManager.GetSection("upac2/propertyAliases") as PropertyAliasesSection));
            }
        }

        public static SettingsSection Settings
        {
            get
            {
                return (_settingsSection = _settingsSection ?? (System.Configuration.ConfigurationManager.GetSection("upac2/settings") as SettingsSection));
            }
        }

        #endregion Properties
    }
}