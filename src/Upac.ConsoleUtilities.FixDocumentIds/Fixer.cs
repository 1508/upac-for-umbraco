namespace Upac.ConsoleUtilities.FixDocumentIds
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class Fixer
    {
        #region Fields

        private int nextId = 1599;
        private string umbracoPackageFile;

        #endregion Fields

        #region Constructors

        public Fixer(string xmlpath)
        {
            umbracoPackageFile = xmlpath;
        }

        #endregion Constructors

        #region Methods

        public void Fix()
        {
            FileStream fsRead = new FileStream(umbracoPackageFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fsRead);
            fsRead.Dispose();

            XmlNode rootNode = xmldoc.SelectSingleNode("umbPackage/Documents/DocumentSet/node");
            if (rootNode != null)
            {
                FixNode(rootNode, -1, "-1", 1, 1);
                FileStream fsWrite = new FileStream(umbracoPackageFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                xmldoc.Save(fsWrite);
                fsWrite.Dispose();
            }
        }

        private void FixNode(XmlNode node, int parentID, string parentPath, int level, int sortOrder)
        {
            nextId++;

            XmlAttributeCollection xmlAttributeCollection = node.Attributes;
            int nodeId = nextId;
            string path = parentPath + "," + nodeId;

            Console.WriteLine(string.Concat("Fixing node: ", xmlAttributeCollection["nodeName"].Value));
            Console.WriteLine(string.Concat("   path: ", parentPath));
            Console.WriteLine(string.Concat("   old id: ", xmlAttributeCollection["id"].Value));
            Console.WriteLine(string.Concat("   new id: ", nodeId));

            xmlAttributeCollection["id"].Value = nodeId.ToString();
            xmlAttributeCollection["parentID"].Value = parentID.ToString();
            xmlAttributeCollection["level"].Value = level.ToString();
            xmlAttributeCollection["sortOrder"].Value = sortOrder.ToString();
            xmlAttributeCollection["path"].Value = path;

            XmlNodeList children = node.SelectNodes("node");
            if (children != null && children.Count > 0)
            {
                int childSortOrder = 0;
                foreach (XmlNode child in children)
                {
                    childSortOrder++;
                    FixNode(child, nodeId, path, level + 1, childSortOrder);
                }
            }
        }

        #endregion Methods
    }
}