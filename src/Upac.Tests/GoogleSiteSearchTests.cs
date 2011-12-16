namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.XPath;

    using NUnit.Framework;

    using Upac.GoogleSiteSearch;

    [TestFixture]
    public class GoogleSiteSearchTests
    {
        #region Methods

        [Test]
        public void Serach()
        {
            GoogleSiteSearch search = new GoogleSiteSearch("UNEP");
            List<GoogleSiteSearchResult> results = search.Search("2", "15");

            GoogleSiteSearch search2 = new GoogleSiteSearch("UNEP", "www.latincarbon.com");

            List<GoogleSiteSearchResult> results2 = search2.Search("1", "20");

            XPathNodeIterator iterator = GoogleSiteSearchXsltExtension.Search("UNEP", 10, 1, "www.latincarbon.com");
        }

        #endregion Methods
    }
}