namespace Upac.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    using umbraco;
    using umbraco.cms.businesslogic.media;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Data;
    using Upac.Core.Utilities;

    public static class NodeExtensions
    {
        #region Methods

        public static Node Find(this Nodes nodes, string name)
        {
            foreach (Node node in nodes)
            {
                if (node.Name.ToLowerInvariant() == name.ToLowerInvariant())
                    return node;
            }
            return null;
        }

        public static Node GetDescendantViaDocumentTypePath(this Node node, string path)
        {
            Node returnNode = null;

            if (string.IsNullOrEmpty(path))
            {
                // If no path is provided, just return self
                return node;
            }

            XPathNavigator navigator = node.ToXPathNavigator();

            XPathNodeIterator nodeIterator = navigator.Select(path);
            if (nodeIterator.MoveNext())
            {
                XPathNavigator current = nodeIterator.Current;
                XPathNavigator idNavigator = current.SelectSingleNode("@id");
                if (idNavigator != null)
                {
                    int nodeId = idNavigator.ValueAsInt;
                    returnNode = UmbracoUtil.GetNode(nodeId);
                }
            }
            return returnNode;
        }

        public static string GetPropertyValue(this Node node, string alias)
        {
            return node.GetPropertyValue(alias, string.Empty);
        }

        public static T GetPropertyValue<T>(this Node node, string alias, T defaultValue)
        {
            var property = node.GetProperty(alias);

            if (property == null || string.IsNullOrEmpty(property.Value))
            {
                return defaultValue;
            }
            Type type = typeof(T);
            if (type == typeof(Boolean))
            {
                return (T)(object)(property.Value == "1");
            }
            return (T)Convert.ChangeType(property.Value, type);
        }

        public static T GetPropertyValue<T>(this Media media, string alias, T defaultValue)
        {
            var property = media.getProperty(alias);

            if (property == null || string.IsNullOrEmpty(property.Value.ToString()))
            {
                return defaultValue;
            }
            return (T)Convert.ChangeType(property.Value, typeof(T));
        }

        public static int Level(this Node node)
        {
            return node.Path.Split(new[] { ',' }).Length;
        }

        public static int[] Levels(this Node node)
        {
            string[] strings = node.Path.Split(new[] { ',' });
            int[] ints = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                ints[i] = Convert.ToInt32(strings[i]);
            }
            return ints;
        }

        public static List<Node> ToList(this Nodes nodes)
        {
            List<Node> list = new List<Node>(nodes.Count);
            foreach (Node node in nodes)
            {
                list.Add(node);
            }
            return list;
        }

        public static XPathNavigator ToXPathNavigator(this Node node)
        {
            XPathNavigator navigator = content.Instance.XmlContent.CreateNavigator();
            navigator.MoveToId(node.Id.ToString());
            return navigator;
        }

        public static XPathNodeIterator ToXPathNodeIterator(this Node node)
        {
            XPathNavigator navigator = content.Instance.XmlContent.CreateNavigator();
            navigator.MoveToId(node.Id.ToString());
            return navigator.Select(".");
        }

        #endregion Methods
    }
}