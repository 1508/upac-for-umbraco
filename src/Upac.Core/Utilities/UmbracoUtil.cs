namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.XPath;

    using umbraco.cms.businesslogic.media;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Extensions;

    public static class UmbracoUtil
    {
        #region Methods

        public static Media GetMedia(int mediaId)
        {
            Media media = null;

            try
            {
                media = new Media(mediaId);
                if (media.nodeObjectType != Media._objectType)
                {
                    media = null;
                }
            }
            catch { }
            return media;
        }

        public static Media GetMedia(string mediaId)
        {
            int id = CommonUtil.ConvertToIntSafe(mediaId, -1);
            if (id > -1)
            {
                return GetMedia(id);
            }
            return null;
        }

        public static string GetMenuTitle(Node node)
        {
            if (node != null)
            {
                return node.GetPropertyValue("navigationTitle", node.Name);
            }
            return string.Empty;
        }

        public static Node GetNode(int nodeId)
        {
            Node node = null;
            try
            {
                node = new Node(nodeId);
            }
            catch (Exception)
            {
                return null;
            }
            if (node.Id == 0)
            {
                return null;
            }
            return node;
        }

        public static Node GetNode(string nodeId)
        {
            int id = CommonUtil.ConvertToIntSafe(nodeId, -1);
            if (id > -1)
            {
                return GetNode(id);
            }
            return null;
        }

        public static Node GetNode(XPathNodeIterator iterator)
        {
            // TODO cpa says: se om man kan lave logik lidt mere simpel.
            if (iterator.CurrentPosition > 0 && iterator.Current.SelectSingleNode("@isDoc") != null)
            {
                XPathNavigator navigator = iterator.Current.SelectSingleNode("@id");
                if (navigator != null)
                {
                    int nodeId = navigator.ValueAsInt;
                    return GetNode(nodeId);
                }
            }
            if (iterator.MoveNext() == false)
            {
                return null;
            }

            if (iterator.Current.SelectSingleNode("@isDoc") != null)
            {
                XPathNavigator navigator = iterator.Current.SelectSingleNode("@id");
                if (navigator != null)
                {
                    int nodeId = navigator.ValueAsInt;
                    return GetNode(nodeId);
                }
            }
            return null;
        }

        #endregion Methods
    }
}