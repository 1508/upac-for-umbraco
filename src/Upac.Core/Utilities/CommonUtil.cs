namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    using umbraco.presentation.nodeFactory;

    using Upac.Core.Configuration;
    using Upac.Core.Extensions;

    public static class CommonUtil
    {
        #region Methods

        public static int ConvertToIntSafe(string input, int defaultValue)
        {
            int outInt;
            bool converted = Int32.TryParse(input, out outInt);
            if (converted)
            {
                return outInt;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a value from the form request.
        /// </summary>
        /// <param name="key">The request-key</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value or defaultValue if value is empty or null</returns>
        public static T GetFormValue<T>(string key, T defaultValue)
        {
            if (HttpContext.Current == null)
            {
                return defaultValue;
            }
            return GetValueFromNameValueCollection(key, defaultValue, HttpContext.Current.Request.Form);
        }

        /// <summary>
        /// Gets a value from the querystring.
        /// </summary>
        /// <param name="key">The request-key</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value or defaultValue if value is empty or null</returns>
        public static T GetQueryStringValue<T>(string key, T defaultValue)
        {
            if (HttpContext.Current == null)
            {
                return defaultValue;
            }
            return GetValueFromNameValueCollection(key, defaultValue, HttpContext.Current.Request.QueryString);
        }

        public static string GetReadableFileSizeViaBytes(long Bytes)
        {
            if (Bytes >= 1073741824)
            {
                Decimal size = Decimal.Divide(Bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (Bytes >= 1048576)
            {
                Decimal size = Decimal.Divide(Bytes, 1048576);
                return String.Format("{0:##.#} MB", size);
            }
            else if (Bytes >= 1024)
            {
                Decimal size = Decimal.Divide(Bytes, 1024);
                return String.Format("{0:##} KB", size);
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                Decimal size = Bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

        public static string GetSetting(string path)
        {
            return string.Empty;
            // TODO
            //return ConfigurationManager.CurrentSiteSettings.GetSetting(path);
        }

        public static T GetSetting<T>(string path, T defaultValue)
        {
            return defaultValue;
            // TODO
            //return ConfigurationManager.CurrentSiteSettings.GetSetting(path, defaultValue);
        }

        /// <summary>
        /// Gets the value from name value collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns></returns>
        public static T GetValueFromNameValueCollection<T>(string key, T defaultValue, NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                return defaultValue;
            }

            string value = nameValueCollection[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            Type type = typeof(T);
            if (type == typeof(Boolean))
            {
                if (value == "1" || value.ToLower() == "true")
                {
                    return (T)(object)(true);
                }
                if (value == "0" || value.ToLower() == "false")
                {
                    return (T)(object)(false);
                }
                return defaultValue;
            }
            else if (type == typeof(Int32))
            {
                int defaultInt = (Int32)Convert.ChangeType(defaultValue, type);
                return (T)Convert.ChangeType(CommonUtil.ConvertToIntSafe(value, defaultInt), type);
            }
            return (T)Convert.ChangeType(value, type);
        }

        #endregion Methods
    }
}