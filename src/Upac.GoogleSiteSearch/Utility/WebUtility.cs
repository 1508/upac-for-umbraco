namespace Upac.GoogleSiteSearch.Utility
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Utility for performing common HTTP actions.
    /// </summary>
    internal static class WebUtility
    {
        #region Methods

        /// <summary>
        /// Requests a webpage and returns the result in form of a WebpageResponse object
        /// </summary>
        /// <param name="url">The url to the webpage</param>
        /// <param name="timeoutSeconds">Timeout in seconds</param>
        /// <returns>WebpageResponse object</returns>
        public static WebpageResponse GetWebpage(string url, int timeoutSeconds)
        {
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }

            WebpageResponse result = new WebpageResponse();

            //// Prepare the web page we will be asking for
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 100;
            }
            catch (Exception ex)
            {
                result.StatusCode = WebpageResponseStatusCode.InvalidUrl;
                result.Exception = ex;
                return result;
            }

            request.Timeout = timeoutSeconds * 1000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception exception)
            {
                result.StatusCode = WebpageResponseStatusCode.TimedOut;
                result.Exception = exception;
                return result;
            }

            result.HttpStatusCode = response.StatusCode;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.OK:
                    break;
                default:
                    result.StatusCode = WebpageResponseStatusCode.InvalidHttpStatuscode;
                    return result;
            }

            //// Get Encoding Type
            ResponseEncodingType encodingType = ResponseEncodingType.ASCII;
            if (response.CharacterSet.ToLower().Contains("utf-8"))
            {
                encodingType = ResponseEncodingType.UTF8;
            }
            else if (response.CharacterSet.ToLower().Contains("utf-16"))
            {
                encodingType = ResponseEncodingType.UTF16;
            }
            else if (response.CharacterSet.ToLower().Contains("utf-32"))
            {
                encodingType = ResponseEncodingType.UTF16;
            }
            else if (response.CharacterSet.ToLower().Contains("iso"))
            {
                encodingType = ResponseEncodingType.ASCII;
            }

            result.EncodingType = encodingType;
            result.ResponseCharacterSet = response.CharacterSet;

            //// Read data via the response stream
            Stream resStream = null;
            try
            {
                resStream = response.GetResponseStream();
            }
            catch (Exception ex)
            {
                result.StatusCode = WebpageResponseStatusCode.GetResponseStreamTimeout;
                result.Exception = ex;

                return result;
            }

            int count = 0;
            //// Read from the responsestream
            do
            {
                //// used on each read operation
                byte[] buf = new byte[8192];

                //// fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                //// make sure we read some data
                if (count != 0)
                {
                    if (count < 8192)
                    {
                        byte[] readBuf = new byte[count];
                        for (int i = 0; i < count; i++)
                        {
                            readBuf[i] = buf[i];
                        }

                        result.ReadBuffers.Add(readBuf);
                    }
                    else
                    {
                        result.ReadBuffers.Add(buf);
                    }
                }
            }
            while (count > 0);

            response.Close();
            resStream.Close();
            result.ContentType = response.ContentType;
            if (result.ContentType == string.Empty)
            {
                // TODO: Document this!
                result.ContentType.ToString();
            }

            //// Return the page source
            return result;
        }

        #endregion Methods
    }
}