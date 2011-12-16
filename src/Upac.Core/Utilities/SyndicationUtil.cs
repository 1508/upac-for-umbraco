namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Xml;

    using Upac.Core.Diagnostics;

    public static class SyndicationUtil
    {
        #region Methods

        // TODO catch if 404 etc happens
        public static XmlDocument FetchFeedAndNormaliseToAtom(string url)
        {
            string cacheKey = string.Format("SyndicationUtil.FetchFeedAndNormaliseToAtom::{0}", url);
            if (CacheUtil.Exist(cacheKey))
            {
                return CacheUtil.Get<XmlDocument>(cacheKey);
            }
            XmlDocument doc = null;

            Assert.EnsureStringValue(url, "url");

            SyndicationFeed feed = null;
            using (XmlTextReader r = new XmlTextReader(url))
            {
                feed = SyndicationFeed.Load(r);
            }

            if (feed == null)
            {
                return null;
            }

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmltextWriter = new XmlTextWriter(stringWriter);
            xmltextWriter.Formatting = Formatting.Indented;
            Atom10FeedFormatter formatter = feed.GetAtom10Formatter();
            formatter.WriteTo(xmltextWriter);
            doc = new XmlDocument();
            string xml = stringWriter.ToString();
            // We do not wan't namespaces on the first node
            // TODO Do this smarter
            xml = xml.Remove(0, xml.IndexOf(">"));
            xml = "<feed>" + xml;
            doc.LoadXml(xml);
            CacheUtil.Insert(cacheKey, doc, 5, false);
            return doc;
        }

        #endregion Methods
    }
}