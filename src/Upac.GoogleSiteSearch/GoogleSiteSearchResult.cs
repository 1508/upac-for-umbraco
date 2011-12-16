namespace Upac.GoogleSiteSearch
{
    public class GoogleSiteSearchResult
    {
        #region Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the headline.
        /// </summary>
        /// <value>The headline.</value>
        public virtual string Headline
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public virtual string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public virtual string Url
        {
            get;
            set;
        }

        #endregion Properties
    }
}