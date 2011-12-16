namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;

    using log4net;

    /// <summary>
    /// Utility class for handling OEmbed
    /// </summary>
    public class OEmbedUtil
    {
        #region Fields

        /// <summary>
        /// The log4net logger
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(OEmbedUtil));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Gets the O embed HTML.
        /// </summary>
        /// <param name="url">The OEmbed URL/Source.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns>The html or an error message</returns>
        public static string GetOEmbedHtml(string url, string maxWidth, string maxHeight)
        {
            StringBuilder output = new StringBuilder();
            if (!string.IsNullOrEmpty(url))
            {
                string jsonResponse = string.Empty;

                StringBuilder oohembedUrl = new StringBuilder();
                oohembedUrl.Append("http://oohembed.com/oohembed/?");
                if (!string.IsNullOrEmpty(maxWidth))
                {
                    int width = CommonUtil.ConvertToIntSafe(maxWidth, -1);
                    if (width > -1)
                    {
                        oohembedUrl.Append("maxwidth=");
                        oohembedUrl.Append(width);
                        oohembedUrl.Append("&");
                    }
                }

                if (!string.IsNullOrEmpty(maxHeight))
                {
                    int height = CommonUtil.ConvertToIntSafe(maxHeight, -1);
                    if (height > -1)
                    {
                        oohembedUrl.Append("maxheight=");
                        oohembedUrl.Append(height);
                        oohembedUrl.Append("&");
                    }
                }

                oohembedUrl.Append("url=");
                oohembedUrl.Append(HttpUtility.UrlEncode(url));

                string cacheKey = string.Format("Upac.Core.Utilities.OEmbed::{0}", oohembedUrl);
                Log.DebugFormat("Try to look in cache via cachekey: {0}", cacheKey);
                if (CacheUtil.Exist(cacheKey))
                {
                    Log.Debug("Cache gave a hit. Return html from cache");
                    return CacheUtil.Get<string>(cacheKey);
                }

                Log.Debug("No content in cache");

                System.Net.WebClient webClient = new System.Net.WebClient();

                try
                {
                    jsonResponse = webClient.DownloadString(oohembedUrl.ToString());
                }
                catch (System.Net.WebException exception)
                {
                    if (exception.Status != System.Net.WebExceptionStatus.ProtocolError)
                    {
                        Log.Error("Exception smidt", exception);
                        throw;
                    }

                    // If it's a ProtocolError.
                    // oembed throws a 404 status, if the resource could not be looked up.
                    output.Append("<a href=\"" + url + "\">" + url + "</a>");
                    output.Append("<br /><em>Error with embedding the source<br />oohembed source: " + oohembedUrl + "</em>");
                }

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
                    OEmbedResponse businessObject = scriptSerializer.Deserialize<OEmbedResponse>(jsonResponse);
                    string html = businessObject.GetHtml();
                    CacheUtil.Insert(cacheKey, html, 10, false);
                    output.Append(html);
                }
            }

            return output.ToString();
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// Wrapper for the OEmbed response
        /// </summary>
        private class OEmbedResponse
        {
            #region Properties

            /// <summary>
            /// Gets or sets the height.
            /// </summary>
            /// <value>The height.</value>
            public string Height
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the HTML.
            /// </summary>
            /// <value>The response-HTML</value>
            public string Html
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            /// <value>The title.</value>
            public string Title
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>The oembed response type</value>
            public string Type
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the URL.
            /// </summary>
            /// <value>The oembed URL.</value>
            public string Url
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public string Width
            {
                get; set;
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// Gets the HTML.
            /// </summary>
            /// <returns>The response html</returns>
            public string GetHtml()
            {
                if (this.Type == "photo")
                {
                    return "<img src=\"" + this.Url + "\" width=\"" + this.Width + "\" height=\"" + this.Height + "\" alt=\"" + HttpUtility.HtmlEncode(this.Title) + "\" />";
                }

                if (!string.IsNullOrEmpty(this.Html))
                {
                    return this.Html;
                }

                return string.Empty;
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}