namespace Upac.GoogleSiteSearch
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Xml;
    using System.Xml.XPath;

    using log4net;

    using Upac.GoogleSiteSearch.Utility;

    /// <summary>
    /// Represents and handles a single Google SiteSearch query.
    /// </summary>
    public class GoogleSiteSearch : IDisposable
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(GoogleSiteSearch));

        /// <summary>
        /// Google SiteSearch base url.
        /// </summary>
        private static string _googleBaseUrl = "http://www.google.com/cse";

        /// <summary>
        /// Customer unique SiteSearch ID.
        /// </summary>
        private string _googleCXNumber = string.Empty;

        /// <summary>
        /// Query to look up using Google SiteSearch.
        /// </summary>
        private string _q = string.Empty;

        /// <summary>
        /// Results found by the query.
        /// </summary>
        private List<GoogleSiteSearchResult> _results;

        /// <summary>
        /// Only seearch this site.
        /// </summary>
        private string _site = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GoogleSiteSearch class.
        /// </summary>
        /// <param name="query">Query to search for in the google site search index.</param>
        public GoogleSiteSearch(string query)
            : this(query, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GoogleSiteSearch class.
        /// </summary>
        /// <param name="query">Query to search for in the google site search index.</param>
        public GoogleSiteSearch(string query, string site)
        {
            this._q = query;
            this._results = new List<GoogleSiteSearchResult>();
            this._googleCXNumber = Settings.GoogleSiteSearchApiKey;
            this._site = site;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the URL to the next page in the search result (if one exists).
        /// </summary>
        public string NextResultPageUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of hits found using the specified query.
        /// </summary>
        public string NumberOfHits
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the URL to the previous page in the search result (if one exists).
        /// </summary>
        public string PreviousResultPageUrl
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Disposes the Google SiteSearch query.
        /// </summary>
        public virtual void Dispose()
        {
            if (this._results != null)
            {
                this._results.Clear();
                this._results = null;
            }
        }

        /// <summary>
        /// Performs the search, and returns the results in the range from the start
        /// and to the page size index.
        /// </summary>
        /// <param name="start">Index of where in the total result to return results.</param>
        /// <param name="pageSize">Number of results to return.</param>
        /// <returns>List of google site search results.</returns>
        /// <exception cref="System.NullReferenceException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Xml.XPath.XPathException"></exception>
        public virtual List<GoogleSiteSearchResult> Search(string start, string pageSize)
        {
            if (this._results.Count > 0)
            {
                this._results.Clear();
            }

            // Check whether or not a Google customer ID has been defined
            if (string.IsNullOrEmpty(this._googleCXNumber))
            {
                throw new NullReferenceException("Google SiteSearch exception: No CX number have been defined. Make sure the key is added to 'Site config/Search settings'.");
            }

            string response = this.DoSearch(this.FormatQueryString(this._googleCXNumber, this._q, start, pageSize, this._site));
            GoogleSiteSearchParserResult result = this.ParseResult(response);

            if (result != null)
            {
                this.NextResultPageUrl = result.NextUrl;
                this.PreviousResultPageUrl = result.PreviousUrl;
                this.NumberOfHits = result.Hits;
                this._results = result.Result;
            }

            return this._results;
        }

        /// <summary>
        /// Does the search.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        protected virtual string DoSearch(string url)
        {
            string responseData = string.Empty;

            // Attempt to get the search result using the created query
            try
            {
                WebpageResponse response = WebUtility.GetWebpage(url, 10);
                if (response.StatusCode == WebpageResponseStatusCode.Ok)
                {
                    responseData = response.ResponseText;
                }
            }
            catch (WebException wex)
            {
                log.Error("An error occured while trying to perform Google SiteSearch.", wex);
            }
            catch (NotSupportedException nse)
            {
                log.Error("An error occured while trying to perform Google SiteSearch.", nse);
            }

            return responseData;
        }

        /// <summary>
        /// Formats the query string.
        /// </summary>
        /// <param name="cx">The cx.</param>
        /// <param name="query">The query.</param>
        /// <param name="start">The start.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        protected virtual string FormatQueryString(string cx, string query, string start, string pageSize, string site)
        {
            if (!string.IsNullOrEmpty(site))
            {
                query = string.Format("{0} site:{1}", query, site);
            }

            string url = string.Format(_googleBaseUrl + "?cx={0}&client=google-csbe&q={1}&output=xml_no_dtd", cx, query);

            if (string.IsNullOrEmpty(pageSize))
            {
                pageSize = "10";
            }

            if (!string.IsNullOrEmpty(start))
            {
                url += "&start=" + start;
            }

            if (!string.IsNullOrEmpty(pageSize))
            {
                url += "&num=" + pageSize;
            }

            return url;
        }

        /// <summary>
        /// Parses the result.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected virtual GoogleSiteSearchParserResult ParseResult(string data)
        {
            XmlDocument xmlDocument = new XmlDocument();

            // Load the result into a friendly structure
            try
            {
                xmlDocument.LoadXml(data);
            }
            catch (XmlException xmlException)
            {
                log.Error("An error occured while loading the Google SiteSearch XML.", xmlException);
            }

            // Parse the result
            try
            {
                GoogleSiteSearchParserResult result;
                using (GoogleSiteSearchParser googleSiteSearchParser = new GoogleSiteSearchParser())
                {
                    result = googleSiteSearchParser.Parse(xmlDocument.SelectSingleNode("GSP"));
                }

                return result;
            }
            catch (XPathException xPathException)
            {
                log.Error("An error occured while parsing the Google SiteSearch XML response.", xPathException);
            }

            return null;
        }

        #endregion Methods
    }
}