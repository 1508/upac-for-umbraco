namespace Upac.NantTasks
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using NAnt.Core;
    using NAnt.Core.Attributes;
    using NAnt.Core.Types;

    [TaskName("parsexmlincludes")]
    public class ParseXmlIncludes : Task
    {
        #region Properties

        [TaskAttribute("property", Required = true)]
        [StringValidator(AllowEmpty = true)]
        public string Property
        {
            get; set;
        }

        [TaskAttribute("file", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo XmlFile
        {
            get; set;
        }

        [TaskAttribute("xpath", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string XPath
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the XML reading task.
        /// </summary>
        protected override void ExecuteTask()
        {
            Log(Level.Info, string.Format("Parse includes in file {0} and will return the result based on the xpath {1}", XmlFile.FullName, XPath));

            // ensure the specified xml file exists
            if (!XmlFile.Exists)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "The XML file '{0}' does not exist.", XmlFile.FullName), Location);
            }

            XmlDocument document = LoadDocument(XmlFile.FullName);

            XmlNodeList nodes = document.SelectNodes("//Include");

            if (nodes == null || nodes.Count == 0)
            {

            }
            else
            {
                Log(Level.Info, string.Format("Found {0} includes in file {1}", nodes.Count, XmlFile.FullName));

                foreach (XmlNode includeNode in nodes)
                {
                    XmlAttribute srcAttribute = includeNode.Attributes["src"];

                    if (srcAttribute == null)
                    {
                        Log(Level.Warning, string.Format("Could not find src attribute on include node {0}", FindXPath(includeNode)));
                        continue;
                    }

                    string srcAttributeValue = srcAttribute.Value;

                    if (string.IsNullOrEmpty(srcAttributeValue))
                    {
                        Log(Level.Warning, string.Format("Could not find value on src node in include node {0}", FindXPath(includeNode)));
                        continue;
                    }

                    XmlAttribute xpathAttribute = includeNode.Attributes["xpath"];

                    if (xpathAttribute == null)
                    {
                        Log(Level.Warning, string.Format("Could not find xpath attribute on include node {0}", FindXPath(includeNode)));
                        continue;
                    }

                    string xpathToNodesInIncludeFile = xpathAttribute.Value;

                    if (string.IsNullOrEmpty(srcAttributeValue))
                    {
                        Log(Level.Warning, string.Format("Could not find value on xpath node in include node {0}", FindXPath(includeNode)));
                        continue;
                    }

                    string fileToInclude = Path.Combine(XmlFile.DirectoryName, srcAttributeValue);
                    Log(Level.Info, "--- Start ---");
                    Log(Level.Info, string.Format("Include file found '{0}', perform xpath {1} on include file", fileToInclude, xpathToNodesInIncludeFile));

                    FileInfo includeFileInfo = new FileInfo(fileToInclude);
                    if (!includeFileInfo.Exists)
                    {
                        throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                            "The include XML file '{0}' does not exist.", includeFileInfo.FullName), Location);
                    }

                    XmlDocument includeDocument = LoadDocument(includeFileInfo.FullName);
                    XmlNodeList nodeList = includeDocument.SelectNodes(xpathToNodesInIncludeFile);

                    if (nodeList != null)
                    {
                        Log(Level.Info, string.Format("----- nodeList.Count: {0} ---", nodeList.Count));
                        foreach (XmlNode child in nodeList)
                        {
                            XmlNode importNode = document.ImportNode(child, true);
                            Log(Level.Info, "------ Node imported --- ");
                                                        includeNode.ParentNode.InsertBefore(importNode, includeNode);
                                                }
                    }
                    includeNode.ParentNode.RemoveChild(includeNode);

                    Log(Level.Info, "--- End ---");

                }
            }
            XmlNode node = document.SelectSingleNode(XPath);
            Properties[Property] = node.InnerXml;
        }

        static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }

        static string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)node);
                        builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        /// <summary>
        /// Expands project properties in the string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string ExpandProps(string result)
        {
            return result;
            if (Properties == null)
            {
                return result;
            }
            return Properties.ExpandProperties(result, null);
        }

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">The file name of the file to load the XML document from.</param>
        /// <returns>
        /// A <see cref="XmlDocument">document</see> containing
        /// the document object representing the file.
        /// </returns>
        private XmlDocument LoadDocument(string fileName)
        {
            XmlDocument document = null;

            try
            {
                document = new XmlDocument();
                document.Load(fileName);
                return document;
            }
            catch (Exception ex)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Can't load XML file '{0}'.", fileName), Location,
                    ex);
            }
        }

        #endregion Methods
    }
}