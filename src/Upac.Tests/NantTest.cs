namespace Upac.Tests
{
    using System.Xml;

    using NUnit.Framework;

    [TestFixture]
    public class NantTest
    {
        #region Fields

        private int nextId = 1599;

        #endregion Fields

        #region Methods

        [Test]
        public void TestNode()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\Solutions\INT 900\Upac\Latest\Source\Upac.Foundation\PackageFiles\Documents\DocumentPackage.xml");

            XmlNode rootNode = doc.SelectSingleNode("umbPackage/Documents/DocumentSet/node");
            FixNode(rootNode, -1, "-1", 1, 1);
        }

        private void FixNode(XmlNode node, int parentID, string parentPath, int level, int sortOrder)
        {
            nextId++;

            XmlAttributeCollection xmlAttributeCollection = node.Attributes;
            int nodeId = nextId;
            string path = parentPath + "," + nodeId;

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