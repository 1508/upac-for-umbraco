namespace Upac.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using umbraco.presentation.nodeFactory;

    using Upac.Core.Data;
    using Upac.Core.Extensions;
    using Upac.Core.Utilities;

    public class LinkManager
    {
        #region Methods

        public static string GetNodeUrl(Node node, string alternateTemplate)
        {
            return GetNodeUrl(node, alternateTemplate, false);
        }

        public static string GetNodeUrl(Node node, string alternateTemplate, bool includeDomain)
        {
            // If node is null, just return an empty.String
            if (node == null)
            {
                return string.Empty;
            }

            // Check to see if the node is a redirect node.
            if (node.NodeTypeAlias == Configuration.ConfigurationManager.DocumentTypeAliases.Redirect.Value)
            {
                // Is it an internel link, then change node.
                int umbracoRedirectNodeId = node.GetPropertyValue("umbracoRedirect", -1);
                if (umbracoRedirectNodeId > -1)
                {
                    node = UmbracoUtil.GetNode(umbracoRedirectNodeId);
                    if (node == null)
                    {
                        return string.Empty;
                    }
                }
                else // check to see if we should do an external link
                {
                    string externalUrl = node.GetPropertyValue("externalLink", string.Empty);
                    if (externalUrl != string.Empty)
                    {
                        return externalUrl;
                    }
                }
            }

            bool umbracoUseDirectoryUrls = System.Configuration.ConfigurationManager.AppSettings["umbracoUseDirectoryUrls"] == "true";
            string url = string.Empty;

            string umbracoUrlAlias = node.GetPropertyValue("umbracoUrlAlias");
            if (umbracoUrlAlias != string.Empty && alternateTemplate == string.Empty)
            {
                if (umbracoUseDirectoryUrls == false && umbracoUrlAlias.EndsWith(".aspx") == false) // Running the site with .aspx extension
                {
                    url = "/" + umbracoUrlAlias + ".aspx";
                }
                else // Running the site with directory urls
                {
                    url = "/" + umbracoUrlAlias;
                }
            }
            else
            {
                int nodeLevel = node.Level();
                if (nodeLevel <= 2) // If we are on the frontpage of the site. (or below)
                {
                    url = "/";
                }
                else
                {
                    url = umbraco.library.NiceUrl(node.Id);
                    if (alternateTemplate != string.Empty)
                    {
                        if (umbracoUseDirectoryUrls) // Running the site with-out .aspx extension
                        {
                            url += "/" + alternateTemplate;
                        }
                        else // Running the site with directory urls
                        {
                            url = url.Replace(".aspx", "/" + alternateTemplate + ".aspx");
                        }
                    }
                }
            }

            if (includeDomain)
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] != null)
                    {
                        string domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                        string protocol = "http://";
                        if (HttpContext.Current.Request.ServerVariables["HTTPS"] != null && HttpContext.Current.Request.ServerVariables["HTTPS"] == "ON")
                        {
                            protocol = "https://";
                        }
                        url = string.Concat(protocol, domain, url);
                    }
                }
            }
            return url;
        }

        public static string GetNodeUrl(Node node)
        {
            return GetNodeUrl(node, string.Empty);
        }

        public static string GetNodeUrl(int nodeId)
        {
            return GetNodeUrl(nodeId, string.Empty);
        }

        public static string GetNodeUrl(int nodeId, string alternateTemplate)
        {
            return GetNodeUrl(nodeId, alternateTemplate, false);
        }

        public static string GetNodeUrl(int nodeId, string alternateTemplate, bool includeDomain)
        {
            Node node = Upac.Core.Utilities.UmbracoUtil.GetNode(nodeId);
            return GetNodeUrl(node, alternateTemplate, includeDomain);
        }

        public static string GetNodeUrl(string nodeId)
        {
            return GetNodeUrl(nodeId, string.Empty);
        }

        public static string GetNodeUrl(string nodeId, string alternateTemplate)
        {
            return GetNodeUrl(nodeId, alternateTemplate, false);
        }

        public static string GetNodeUrl(string nodeId, string alternateTemplate, bool includeDomain)
        {
            Node node = Upac.Core.Utilities.UmbracoUtil.GetNode(nodeId);
            return GetNodeUrl(node, alternateTemplate, includeDomain);
        }

        #endregion Methods
    }
}