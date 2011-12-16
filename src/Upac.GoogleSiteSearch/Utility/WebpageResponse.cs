namespace Upac.GoogleSiteSearch.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    using log4net;

    #region Enumerations

    /// <summary>
    /// 
    /// </summary>
    public enum ResponseEncodingType
    {
        /// <summary>
        ///
        /// </summary>
        ASCII,
        /// <summary>
        ///
        /// </summary>
        UTF8,
        /// <summary>
        ///
        /// </summary>
        UTF16,
        /// <summary>
        ///
        /// </summary>
        UTF32
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WebpageResponseStatusCode
    {
        /// <summary>
        ///
        /// </summary>
        TimedOut,
        /// <summary>
        ///
        /// </summary>
        GetResponseStreamTimeout,
        /// <summary>
        ///
        /// </summary>
        InvalidUrl,
        /// <summary>
        ///
        /// </summary>
        InvalidHttpStatuscode,
        /// <summary>
        ///
        /// </summary>
        RequestStreamError,
        /// <summary>
        ///
        /// </summary>
        Ok
    }

    #endregion Enumerations

    /// <summary>
    /// 
    /// </summary>
    internal class WebpageResponse
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(WebpageResponse));

        private MemoryStream _contentAsSteam = null;
        private string _contenType = string.Empty;
        private ResponseEncodingType _encodingType = ResponseEncodingType.ASCII;
        private Exception _exception = null;
        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;

        /// <summary>
        /// The page content read into a list of byte[8192] buffers
        /// </summary>
        private List<byte[]> _readBuffers = new List<byte[]>();
        private string _responseCharacterSet = string.Empty;
        private string _responseText = string.Empty;
        private WebpageResponseStatusCode _statusCode = WebpageResponseStatusCode.Ok;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the ReadBuffers as a memorystream.
        /// </summary>
        public MemoryStream ContentAsStream
        {
            get { return this._contentAsSteam; }
        }

        public string ContentType
        {
            get { return this._contenType; }
            set { this._contenType = value; }
        }

        public ResponseEncodingType EncodingType
        {
            get { return this._encodingType; }
            set { this._encodingType = value; }
        }

        public Exception Exception
        {
            get { return this._exception; }
            set { this._exception = value; }
        }

        public HttpStatusCode HttpStatusCode
        {
            get { return this.HttpStatusCode; }
            set { this._httpStatusCode = value; }
        }

        public List<byte[]> ReadBuffers
        {
            get { return this._readBuffers; }
        }

        public string ResponseCharacterSet
        {
            get { return this._responseCharacterSet; }
            set { this._responseCharacterSet = value; }
        }

        /// <summary>
        /// Gets the response text from the ReadBuffers using an Encoding retrieved 
        /// from the ResponseCharacterSet or the guessed EncodingType.
        /// </summary>
        public string ResponseText
        {
            get
            {
                if (this._readBuffers.Count > 0 && this._responseText == string.Empty)
                {
                    //// translate from bytes to text
                    StringBuilder strContent = new StringBuilder();
                    Encoding encoding = null;
                    try
                    {
                        encoding = Encoding.GetEncoding(this._responseCharacterSet.ToLower());
                    }
                    catch (Exception ex)
                    {
                        log.Error("Fatal error in WebpageResponse: ", ex);
                    }

                    foreach (byte[] buffer in this._readBuffers)
                    {
                        string tempString = string.Empty;
                        if (encoding == null)
                        {
                            switch (this._encodingType)
                            {
                                case ResponseEncodingType.ASCII:
                                    tempString = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                                    break;
                                case ResponseEncodingType.UTF8:
                                    tempString = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                                    break;
                                case ResponseEncodingType.UTF16:
                                    tempString = Encoding.Unicode.GetString(buffer, 0, buffer.Length);
                                    break;
                                case ResponseEncodingType.UTF32:
                                    tempString = Encoding.UTF32.GetString(buffer, 0, buffer.Length);
                                    break;
                            }
                        }
                        else
                        {
                            tempString = encoding.GetString(buffer, 0, buffer.Length);
                        }

                        strContent.Append(tempString);
                    }

                    this._responseText = strContent.ToString();
                }

                return this._responseText;
            }
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public WebpageResponseStatusCode StatusCode
        {
            get { return this._statusCode; }
            set { this._statusCode = value; }
        }

        #endregion Properties
    }
}