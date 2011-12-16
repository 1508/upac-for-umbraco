namespace Upac.GoogleSiteSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public static class GoogleSiteSearchUtil
    {
        #region Methods

        public static XmlDocument GoogleSiteSearchToXmlDocument(GoogleSiteSearch search, List<GoogleSiteSearchResult> results)
        {
            /*
            <Search>
                <NextResultPageUrl></NextResultPageUrl>
                <PreviousResultPageUrl></PreviousResultPageUrl>
                <NumberOfHits></NumberOfHits>
                <Results>
                    <Result>
                        <Description></Description>
                        <Headline></Headline>
                        <Language></Language>
                        <Url></Url>
                    </Result>
                <Results>
            </Search>
            */
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Search/>");

            XmlNode newNode = doc.CreateElement("NextResultPageUrl");
            newNode.InnerText = search.NextResultPageUrl;
            doc.FirstChild.AppendChild(newNode);

            newNode = doc.CreateElement("PreviousResultPageUrl");
            newNode.InnerText = search.PreviousResultPageUrl;
            doc.FirstChild.AppendChild(newNode);

            newNode = doc.CreateElement("NumberOfHits");
            newNode.InnerText = search.NumberOfHits;
            doc.FirstChild.AppendChild(newNode);

            XmlNode resultsNode = doc.CreateElement("Results");
            doc.FirstChild.AppendChild(resultsNode);

            foreach (GoogleSiteSearchResult searchResult in results)
            {
                XmlNode resultNode = doc.CreateElement("Result");
                resultsNode.AppendChild(resultNode);

                newNode = doc.CreateElement("Description");
                newNode.InnerText = searchResult.Description;
                resultNode.AppendChild(newNode);

                newNode = doc.CreateElement("Headline");
                newNode.InnerText = searchResult.Headline;
                resultNode.AppendChild(newNode);

                newNode = doc.CreateElement("Language");
                newNode.InnerText = searchResult.Language;
                resultNode.AppendChild(newNode);

                newNode = doc.CreateElement("Url");
                newNode.InnerText = searchResult.Url;
                resultNode.AppendChild(newNode);

            }
            return doc;
        }

        #endregion Methods
    }
}