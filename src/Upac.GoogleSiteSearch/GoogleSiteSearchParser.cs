namespace Upac.GoogleSiteSearch
{
    using System;
    using System.Xml;
    using System.Xml.XPath;

    using log4net;

    /// <summary>
    /// 
    /// </summary>
    public class GoogleSiteSearchParser : IDisposable
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(GoogleSiteSearchParser));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Parses the specified XML node.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <returns></returns>
        public virtual GoogleSiteSearchParserResult Parse(XmlNode xmlNode)
        {
            GoogleSiteSearchParserResult gssp = new GoogleSiteSearchParserResult();

            XmlNode searchResult = GetNode(xmlNode, "RES");

            if (searchResult == null || searchResult.ChildNodes.Count <= 0)
            {
                return gssp;
            }

            foreach (XmlNode r in searchResult.ChildNodes)
            {
                if (r.Name == "M")
                {
                    gssp.Hits = r.InnerText;
                    continue;
                }

                if (r.Name == "NB")
                {
                    XmlNode next = GetNode(r, "NU");
                    XmlNode previous = GetNode(r, "PU");

                    if (next != null)
                    {
                        gssp.NextUrl = next.InnerText;
                    }

                    if (previous != null)
                    {
                        gssp.PreviousUrl = previous.InnerText;
                    }

                    continue;
                }

                if (r.Name == "R")
                {
                    GoogleSiteSearchResult parsedResult = new GoogleSiteSearchResult()
                                                              {
                                                                  Url = GetNode(r, "U").InnerText,
                                                                  Headline = GetNode(r, "T").InnerText,
                                                                  Description = GetNode(r, "S").InnerText,
                                                                  Language = GetNode(r, "LANG").InnerText
                                                              };
                    gssp.Result.Add(parsedResult);
                }
            }

            return gssp;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        private static XmlNode GetNode(XmlNode parent, string nodeName)
        {
            XmlNode node = null;

            try
            {
                if (parent != null)
                {
                    node = parent.SelectSingleNode(nodeName);
                }
            }
            catch (XPathException xPathException)
            {
                log.Error("Xpath exception occured while parsing google site search xml.", xPathException);
            }

            return node;
        }

        #endregion Methods
    }
}