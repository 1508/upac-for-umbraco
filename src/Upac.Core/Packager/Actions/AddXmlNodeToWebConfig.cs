namespace Upac.Core.Packager.Actions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Hosting;
    using System.Xml;

    using log4net;

    using umbraco.cms.businesslogic.packager.standardPackageActions;
    using umbraco.interfaces;

    public class AddXmlNodeToWebConfig : IPackageAction
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(AddXmlNodeToWebConfig));

        #endregion Fields

        #region Methods

        public string Alias()
        {
            return "AddXmlNodeToWebConfig";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
                string webConfigPath = HostingEnvironment.MapPath("/web.config");
                if (string.IsNullOrEmpty(webConfigPath))
                {
                    return false;
                }
                FileStream fsRead = new FileStream(webConfigPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlDocument webconfig = new XmlDocument();
                webconfig.Load(fsRead);

                fsRead.Dispose();

                bool changed = false;

                XmlNodeList nodes = xmlData.SelectNodes("Item");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (AddNode(webconfig, node))
                        {
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    FileStream fsWrite = new FileStream(webConfigPath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                    webconfig.Save(fsWrite);
                    fsWrite.Dispose();
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<Action runat=\"install\" undo=\"false\" alias=\"AddXmlNodeToWebConfig\">");
            sb.AppendLine("	<Item>");
            sb.AppendLine("		<XPath>/configuration/configSections</XPath>");
            sb.AppendLine("		<XPathCheckIfExist>/configuration/configSections/section[@name='log4net']</XPathCheckIfExist>");
            sb.AppendLine("		<XmlNode>");
            sb.AppendLine("			<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler, log4net\"/>");
            sb.AppendLine("		</XmlNode>");
            sb.AppendLine("	</Item>");
            sb.AppendLine("	<Item>");
            sb.AppendLine("		<XPath>/configuration</XPath>");
            sb.AppendLine("		<XPathCheckIfExist>/configuration/log4net</XPathCheckIfExist>");
            sb.AppendLine("		<InsertAfter>/configuration/configSections</InsertAfter>");
            sb.AppendLine("		<XmlNode>");
            sb.AppendLine("			<log4net configSource=\"App_Data\\Log4Net.config\" />");
            sb.AppendLine("		</XmlNode>");
            sb.AppendLine("	</Item>");
            sb.AppendLine("	<Item>");
            sb.AppendLine("		<XPath>/configuration/system.web/httpModules</XPath>");
            sb.AppendLine("		<XPathCheckIfExist>/configuration/system.web/httpModules/add[@name='UpacEventHttpModule']</XPathCheckIfExist>");
            sb.AppendLine("		<XmlNode>");
            sb.AppendLine("			<add name=\"UpacEventHttpModule\" type=\"Upac.Core.Events.EventHttpModule, Upac.Core\" />");
            sb.AppendLine("		</XmlNode>");
            sb.AppendLine("	</Item>");
            sb.AppendLine("	<Item>");
            sb.AppendLine("		<XPath>/configuration/system.webServer/modules</XPath>");
            sb.AppendLine("		<XPathCheckIfExist>/configuration/system.webServer/modules/add[@name='UpacEventHttpModule']</XPathCheckIfExist>");
            sb.AppendLine("		<XmlNode>");
            sb.AppendLine("			<add name=\"UpacEventHttpModule\" type=\"Upac.Core.Events.EventHttpModule, Upac.Core\" />");
            sb.AppendLine("		</XmlNode>");
            sb.AppendLine("	</Item>");
            sb.AppendLine("</Action>");
            return helper.parseStringToXmlNode(sb.ToString());
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            try
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool AddNode(XmlDocument webconfig, XmlNode xmlData)
        {
            XmlNode xpathNode = xmlData.SelectSingleNode("XPath");
            if (xpathNode == null)
            {
                log.Error("XPath node not found!");
                return false;
            }

            string xpath = xpathNode.InnerText;
            if (string.IsNullOrEmpty(xpath))
            {
                log.Error("XPath value not found!");
                return false;
            }

            XmlNode nodeToInsertNewNodeInto = webconfig.SelectSingleNode(xpath);
            if (nodeToInsertNewNodeInto == null)
            {
                log.Error("nodeToInsertNewNodeInto node not found via xpath!");
                return false;
            }

            XmlNode xmlNode = xmlData.SelectSingleNode("XmlNode");
            if (xpathNode == null)
            {
                log.Error("XmlNode node not found!");
                return false;
            }

            if (xmlNode.ChildNodes.Count == 0)
            {
                log.Error("No child node to XmlNode found!");
                return false;
            }

            if (xmlNode.ChildNodes.Count > 1)
            {
                log.Error("More than one child node found!");
                return false;
            }

            XmlNode checkIfExistXPathNode = xmlData.SelectSingleNode("XPathCheckIfExist");
            if (checkIfExistXPathNode != null && !string.IsNullOrEmpty(checkIfExistXPathNode.InnerText))
            {
                XmlNode nodeAlreadyExist = webconfig.SelectSingleNode(checkIfExistXPathNode.InnerText);
                if (nodeAlreadyExist != null)
                {
                    log.Error("New node already exist!");
                    return false;
                }
            }

            XmlNode newNode = webconfig.ImportNode(xmlNode.FirstChild, true);

            XmlNode insertAfterNode = null;

            XmlNode insertAfterNodeXpath = xmlData.SelectSingleNode("InsertAfter");
            if (insertAfterNodeXpath != null && !string.IsNullOrEmpty(insertAfterNodeXpath.InnerText))
            {
                insertAfterNode = webconfig.SelectSingleNode(insertAfterNodeXpath.InnerText);
                if (insertAfterNode == null)
                {
                    log.Error("insertAfterNode not found, but will insert the new node to XPath location!");
                }
            }

            if (insertAfterNode != null)
            {
                nodeToInsertNewNodeInto.InsertAfter(newNode, insertAfterNode);
            }
            else
            {
                nodeToInsertNewNodeInto.PrependChild(newNode);
            }
            return true;
        }

        #endregion Methods
    }
}