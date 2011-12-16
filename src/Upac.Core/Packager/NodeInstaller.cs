namespace Upac.Core.Packager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using log4net;

    using umbraco.BusinessLogic;
    using umbraco.cms.businesslogic.property;
    using umbraco.cms.businesslogic.web;

    public class NodeInstaller
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(NodeInstaller));

        private umbraco.BusinessLogic.User currentUser = umbraco.BasePages.UmbracoEnsuredPage.CurrentUser;
        XmlDocument doc = new XmlDocument();

        #endregion Fields

        #region Constructors

        public NodeInstaller(XmlDocument doc)
        {
            this.doc = doc;
        }

        public NodeInstaller(string filename)
        {
            doc.Load(filename);
        }

        public NodeInstaller(string assemblyName, string fileName)
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                Stream stream = assembly.GetManifestResourceStream(fileName);
                if (stream == null)
                {
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + assemblyName + "'");
                }

                XmlTextReader tr = new XmlTextReader(stream);
                doc.Load(tr);
            }
            catch (Exception e)
            {
                throw new Exception(assemblyName + ": " + e.Message);
            }
        }

        #endregion Constructors

        #region Properties

        public User CurrentUser
        {
            get { return currentUser; }
        }

        #endregion Properties

        #region Methods

        public void InstallNodesInDocument(Document document)
        {
            XmlNode node = doc.SelectSingleNode("node");
            InstallNodeInDocument(document, node);
            try
            {
                document.PublishWithChildrenWithResult(currentUser);
            }
            catch(Exception e)
            {
                log.Error("Could not publish", e);
            }
            umbraco.library.UpdateDocumentCache(document.Id);
            umbraco.library.RefreshContent();
        }

        private void InstallNodeInDocument(Document parentDocument, XmlNode node)
        {
            string name = node.Attributes["nodeName"].Value;
            string nodeTypeAlias = node.Attributes["nodeTypeAlias"].Value;
            DocumentType dt = DocumentType.GetByAlias(nodeTypeAlias);

            // Ensure we do not reinstall nodes
            Document document = parentDocument.Children.ToArray().SingleOrDefault(childDoc => childDoc.Text == name);
            if (document == null)
            {
                document = Document.MakeNew(name, dt, CurrentUser, parentDocument.Id);
                XmlNodeList dataNodes = node.SelectNodes("data");
                if (dataNodes != null && dataNodes.Count > 0)
                {
                    foreach (XmlNode dataNode in dataNodes)
                    {
                        // TODO this should handle if the first child is CDATA
                        string dataValue = dataNode.InnerText;
                        if (!string.IsNullOrEmpty(dataValue))
                        {
                            string alias = dataNode.Attributes["alias"].Value;
                            Property property = document.getProperty(alias);
                            if (property != null)
                            {
                                property.Value = dataValue;
                            }
                        }
                    }
                }
            }

            XmlNodeList nodes = node.SelectNodes("node");
            if (nodes != null && nodes.Count > 0)
            {
                foreach (XmlNode child in nodes)
                {
                    InstallNodeInDocument(document, child);
                }
            }
        }

        #endregion Methods
    }
}