namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using umbraco;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Configuration;
    using Upac.Core.Extensions;

    public static class DateUtil
    {
        #region Methods

        /// <summary>
        /// Returns the RFC822 datetime format for the specified isodate.
        /// RFC822 should be used when generating rss feeds etc
        /// </summary>
        /// <param name="dateTime">The dateTime to convert to RFC822 format</param>
        /// <returns>RFC822 datetime format</returns>
        public static string CreateRfc822Date(DateTime dateTime)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');

            if (offset < 0)
            {

                int i = offset * -1;

                timeZone = "-" + i.ToString().PadLeft(2, '0');

            }
            return dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), CultureInfo.GetCultureInfo("en-US"));
        }

        public static string FormatDate(string isodate, string key)
        {
            bool failed = false;
            string format = GetFormatViaKey(key, out failed);
            if (failed)
            {
                return format;
            }
            return library.FormatDateTime(isodate, format);
        }

        public static string FormatDate(DateTime dateTime, string key)
        {
            string setting = CommonUtil.GetSetting(string.Concat("Date format/", key));
            if (setting == String.Empty)
            {
                return String.Concat("Date format key not found: ", key);
            }
            return dateTime.ToString(setting);
        }

        public static string FormatDateLong(string isodate)
        {
            return FormatDate(isodate, "dateFormatLong");
        }

        public static string FormatDateLong(DateTime dateTime)
        {
            return FormatDate(dateTime, "dateFormatLong");
        }

        public static string FormatDateShort(string isodate)
        {
            return FormatDate(isodate, "dateFormatShort");
        }

        public static string FormatDateShort(DateTime dateTime)
        {
            return FormatDate(dateTime, "dateFormatShort");
        }

        public static string FormatDateTime(string isodate)
        {
            return FormatDate(isodate, "dateTimeFormat");
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return FormatDate(dateTime, "dateTimeFormat");
        }

        public static string FormatTime(string isodate)
        {
            return FormatDate(isodate, "timeFormat");
        }

        public static string FormatTime(DateTime dateTime)
        {
            return FormatDate(dateTime, "timeFormat");
        }

        public static string ToIsoDate(DateTime dateTime)
        {
            return dateTime.ToString("s");
        }

        private static string GetFormatViaKey(string key, out bool failed)
        {
            string cacheKey = string.Format("{0}#key:{1}", CacheUtil.GetCacheKey("FormatDate"), key); //"CacheUtil.GetCacheKey("FormatDate");
            if (CacheUtil.Exist(cacheKey))
            {
                failed = false;
                return CacheUtil.Get<string>(cacheKey);
            }

            Node homeNode = UpacContext.Current.HomeNode;
            if (homeNode == null)
            {
                failed = true;
                return "FormatDate failed: Not in upac context";
            }

            Node node = homeNode.GetDescendantViaDocumentTypePath("ConfigurationContainer/ConfigurationDateTime");
            if (node == null)
            {
                failed = true;
                return "FormatDate failed: Could not find config node ConfigurationContainer/ConfigurationDateTime";
            }

            string format = node.GetPropertyValue(key);
            if (string.IsNullOrEmpty(format))
            {
                failed = true;
                return String.Concat("Date format key not found: ", key);
            }
            CacheUtil.Insert(cacheKey, format, 10, true);
            failed = false;
            return format;
        }

        #endregion Methods
    }
}