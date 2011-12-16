namespace Upac.Core.Packager.Actions
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Configuration;
    using System.Xml;

    using log4net;

    using umbraco.cms.businesslogic.packager.standardPackageActions;
    using umbraco.interfaces;

    public class AddConfigurationSection : IPackageAction
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(AddConfigurationSection));

        #endregion Fields

        #region Methods

        public string Alias()
        {
            return "UpacActionAddConfigurationSection";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                var sectionName = xmlData.SelectSingleNode("Section").Attributes["name"].Value;

                if (config.Sections[sectionName] == null)
                {
                    var assemblyName = xmlData.SelectSingleNode("Section").Attributes["assembly"].Value;
                    var typeName = xmlData.SelectSingleNode("Section").Attributes["type"].Value;
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly == null) return false;
                    var configSection = assembly.CreateInstance(typeName) as ConfigurationSection;
                    if (configSection == null) return false;
                    config.Sections.Add(sectionName, configSection);
                    configSection.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Minimal);
                }
                return true;
            }
            catch (Exception exception)
            {
                log.Error("Der er sket en fejl", exception);
                return false;
            }
        }

        public XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true\" alias=\"UpacActionAddConfigurationSection\"><Section name=\"\" assembly=\"\" type=\"\" /></Action>";
            return helper.parseStringToXmlNode(sample);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            try
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                var sectionName = xmlData.SelectSingleNode("Section").Attributes["name"].Value;

                if (config.Sections[sectionName] != null)
                {
                    config.Sections.Remove(sectionName);
                    config.Save(ConfigurationSaveMode.Minimal);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Methods
    }
}