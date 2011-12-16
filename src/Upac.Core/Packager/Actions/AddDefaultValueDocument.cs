namespace Upac.Core.Packager.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using log4net;

    using umbraco.BusinessLogic;
    using umbraco.cms.businesslogic.packager.standardPackageActions;
    using umbraco.cms.businesslogic.property;
    using umbraco.cms.businesslogic.web;
    using umbraco.interfaces;

    public class AddDefaultValueDocument : IPackageAction
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(AddDefaultValueDocument));

        private List<Document> defaultValueFolders;
        private umbraco.BusinessLogic.User user = new User(0);

        #endregion Fields

        #region Methods

        public static string GetAttributeValueFromNode(XmlNode node, string attributeName)
        {
            return (node.Attributes[attributeName] == null) ? string.Empty : node.Attributes[attributeName].InnerText;
        }

        public string Alias()
        {
            return "AddDefaultValueDocument";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            log.Info("AddDefaultValueDocument action started");
            Log.AddSynced(LogTypes.Debug, 0, -1, "AddDefaultValueDocument action started");
            XmlNodeList nodes = xmlData.SelectNodes("Document");
            if (nodes != null)
            {
                foreach (XmlNode nodeDocument in nodes)
                {
                    Add(nodeDocument);
                }
            }
            log.Info("AddDefaultValueDocument action ended");
            Log.AddSynced(LogTypes.Debug, 0, -1, "AddDefaultValueDocument action ended");
            return true;
        }

        public List<Document> GetDefaultValuesFolders()
        {
            // Lazy load
            if (defaultValueFolders != null)
            {
                return defaultValueFolders;
            }
            defaultValueFolders = new List<Document>();

            string configurationContainerAlias = "ConfigurationContainer";
            Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("configurationContainerAlias: {0}", configurationContainerAlias));

            Document[] rootDocuments = Document.GetRootDocuments();
            Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("rootDocuments count: {0}", rootDocuments.Length));

            foreach (Document rootDocument in rootDocuments)
            {
                Document[] children = rootDocument.Children;
                if (children == null)
                {
                    Log.AddSynced(LogTypes.Debug, 0, -1, "children == null");
                }
                else if (children.Length == 0)
                {
                    Log.AddSynced(LogTypes.Debug, 0, -1, "children.Length == 0");
                }
                else
                {
                    Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("children count: {0}", children.Length));

                    Document configurationContainer = children.FirstOrDefault(child => child.ContentType.Alias == configurationContainerAlias);
                    if (configurationContainer != null)
                    {
                        Log.AddSynced(LogTypes.Debug, 0, -1, "configurationContainer found!");
                        Document defaultValuesContainer = configurationContainer.Children.FirstOrDefault(
                        child =>
                        child.ContentType.Alias == "ConfigurationDefaultValuesContainer");
                        if (defaultValuesContainer != null)
                        {
                            Log.AddSynced(LogTypes.Debug, 0, -1, "defaultValuesContainer found!");
                            defaultValueFolders.Add(defaultValuesContainer);
                        }
                        else
                        {
                            Log.AddSynced(LogTypes.Debug, 0, -1, "defaultValuesContainer not found!");

                        }
                    }
                    else
                    {
                        Log.AddSynced(LogTypes.Debug, 0, -1, "configurationContainer not found!");
                    }
                }

            }
            return defaultValueFolders;
        }

        public XmlNode SampleXml()
        {
            string sampleXml = "" +
                "<Action runat=\"install\" undo=\"false\" alias=\"AddDefaultValueDocument\">" +
                "	<Document DocumentTypeAlias=\"NewsModule\">" +
                "		<Properties>" +
                "			<Property name=\"mainMacro\">" +
                "				<![CDATA[<?UMBRACO_MACRO macroalias=\"Modules-News-DocumentType-NewsModule\" />]]>" +
                "			</Property>" +
                "		</Properties>" +
                "	</Document>" +
                "</Action>";
            return helper.parseStringToXmlNode(sampleXml);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        private void Add(XmlNode nodeDocument)
        {
            string documentTypeAlias = GetAttributeValueFromNode(nodeDocument, "DocumentTypeAlias");
            if (!string.IsNullOrEmpty(documentTypeAlias))
            {
                log.InfoFormat("Trying to add new '{0}' default value document", documentTypeAlias);
                Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("Trying to add new '{0}' default value document", documentTypeAlias));
                List<Document> defaultValuesFolders = GetDefaultValuesFolders();
                foreach (Document defaultValuesFolder in defaultValuesFolders)
                {
                    // Check if it's already exist
                    Document newDoc = defaultValuesFolder.Children.FirstOrDefault(
                    child =>
                    child.ContentType.Alias == documentTypeAlias);
                    if (newDoc != null)
                    {
                        log.ErrorFormat("Document type '{0}' already exist in default value document '{1}'", documentTypeAlias, defaultValuesFolder.Id);
                        Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("Document type '{0}' already exist in default value document '{1}'", documentTypeAlias, defaultValuesFolder.Id));
                    }
                    else
                    {
                        DocumentType dt = DocumentType.GetByAlias(documentTypeAlias);
                        if (dt == null)
                        {
                            log.ErrorFormat("Document type '{0}' do not exist in the umbraco site", documentTypeAlias);
                        }
                        else
                        {
                            newDoc = Document.MakeNew(documentTypeAlias, dt, user, defaultValuesFolder.Id);
                            log.InfoFormat("Document type created with the id: {0}", newDoc.Id);
                            XmlNodeList properties = nodeDocument.SelectNodes("Properties/Property");
                            if (properties != null && properties.Count > 0)
                            {
                                foreach (XmlNode dataNode in properties)
                                {
                                    string dataValue = dataNode.InnerXml;
                                    dataValue = dataValue.Replace("<![CDATA[", string.Empty);
                                    dataValue = dataValue.Replace("]]>", string.Empty);

                                    if (!string.IsNullOrEmpty(dataValue))
                                    {
                                        string alias = dataNode.Attributes["name"].Value;
                                        Property property = newDoc.getProperty(alias);
                                        if (property != null)
                                        {
                                            log.InfoFormat("Adding value to the property: {0}", alias);
                                            property.Value = dataValue;
                                        }
                                    }
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