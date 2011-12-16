namespace Upac.GoogleSiteSearch
{
    using System.Collections.Generic;

    using log4net;

    /// <summary>
    /// Represents a complete Google SiteSearch search result.
    /// </summary>
    public class GoogleSiteSearchParserResult
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(GoogleSiteSearchParserResult));

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GoogleSiteSearchParserResult class.
        /// </summary>
        public GoogleSiteSearchParserResult()
        {
            this.Result = new List<GoogleSiteSearchResult>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the number result hits.
        /// </summary>
        public string Hits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL to the next result page (if one exists).
        /// </summary>
        public string NextUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL to the previous result page (if one exists).
        /// </summary>
        public string PreviousUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of results found using the specified query.
        /// </summary>
        public List<GoogleSiteSearchResult> Result
        {
            get;
            set;
        }

        #endregion Properties
    }
}