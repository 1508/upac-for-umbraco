namespace Upac.GoogleSiteSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    public class GoogleSiteSearchXsltExtension
    {
        #region Methods

        public static XPathNodeIterator CreateEmptyIterator()
        {
            return new XmlDocument().CreateNavigator().Select("*");
        }

        public static XPathNodeIterator Search(string query, int pageSize, int pageNumber, string site)
        {
            GoogleSiteSearch search = new GoogleSiteSearch(query, site);
            List<GoogleSiteSearchResult> results = search.Search(pageNumber.ToString(), pageSize.ToString());
            XmlDocument document = GoogleSiteSearchUtil.GoogleSiteSearchToXmlDocument(search, results);
            XPathNavigator navigator = document.FirstChild.CreateNavigator();
            return navigator.Select("/");
        }

        #endregion Methods
    }
}