namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    using umbraco.cms.businesslogic.media;

    using Upac.Core.Extensions;

    public static class ImageUtil
    {
        #region Methods

        public static string ThumbnailUrl(int mediaId, int width)
        {
            return ThumbnailUrl(mediaId, width, -1);
        }

        public static string ThumbnailUrl(int mediaId, int width, int height)
        {
            return ThumbnailUrl(mediaId, width,height, true);
        }

        public static string ThumbnailUrl(int mediaId, int width, int height, bool constrain)
        {
            Media media = UmbracoUtil.GetMedia(mediaId);
            if (media != null)
            {
                string imagePath = media.GetPropertyValue("umbracoFile", string.Empty);
                return ThumbnailUrl(imagePath, width, height, constrain);
            }
            return string.Empty;
        }

        public static string ThumbnailUrl(string imagePath, int width)
        {
            return ThumbnailUrl(imagePath, width, -1);
        }

        public static string ThumbnailUrl(string imagePath, int width, int height)
        {
            return ThumbnailUrl(imagePath, width, height, true);
        }

        public static string ThumbnailUrl(string imagePath, int width, int height, bool constrain)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return string.Empty;
            }

            string fullPath = HttpContext.Current.Server.MapPath(imagePath);
            if (File.Exists(fullPath) == false)
            {
                return string.Empty;
            }

            if (width == -1 && height == -1)
            {
                return imagePath;
            }

            string imageGenUrl = Upac.Core.Configuration.ConfigurationManager.Settings.ImageGenUrl.Value;
            Diagnostics.Assert.EnsureStringValue(imageGenUrl, "imageGenUrl");

            StringBuilder sb = new StringBuilder();

            sb.Append(imageGenUrl);
            sb.Append("?image=");
            sb.Append(HttpUtility.UrlEncode(imagePath));

            if (width > -1)
            {
                sb.Append("&width=");
                sb.Append(width.ToString());
            }
            if (height > -1)
            {
                sb.Append("&height=");
                sb.Append(height.ToString());
            }

            if (width > -1 && height > -1)
            {
                sb.Append("&constrain=");
                sb.Append(constrain.ToString().ToLower());
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}