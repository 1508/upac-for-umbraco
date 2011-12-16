namespace Upac.Core.Packager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using umbraco.cms.businesslogic.web;

    public class NodeSettingInstaller : NodeInstaller
    {
        #region Constructors

        public NodeSettingInstaller(XmlDocument doc)
            : base(doc)
        {
        }

        public NodeSettingInstaller(string filename)
            : base(filename)
        {
        }

        public NodeSettingInstaller(string assemblyName, string fileName)
            : base(assemblyName, fileName)
        {
        }

        #endregion Constructors

        #region Methods

        public void Install()
        {
            Document[] websites = Document.GetRootDocuments();

            foreach (Document website in websites)
            {
                if (website != null && website.ContentType.Alias == "Folder - Siteroot")
                {
                    Document[] rootDocChildren = website.Children;
                    foreach (Document configurationContainers in rootDocChildren)
                    {
                        if (configurationContainers != null && configurationContainers.ContentType.Alias == "Configuration Container")
                        {
                            Document[] settingsChildren = configurationContainers.Children;
                            foreach (Document settings in settingsChildren)
                            {
                                if (settings != null && settings.Text == "Settings")
                                {
                                    base.InstallNodesInDocument(settings);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}