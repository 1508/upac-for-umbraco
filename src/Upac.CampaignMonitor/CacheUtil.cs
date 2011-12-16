namespace Upac.CampaignMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Caching;

    using log4net;

    public static class CacheUtil
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(CacheUtil));

        #endregion Fields

        #region Methods

        public static bool Exist(string key)
        {
            return (HttpRuntime.Cache[key] != null);
        }

        public static T Get<T>(string key)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format("Get from cache. Key: {0}", key));
            }
            T data = default(T);
            if (HttpRuntime.Cache[key] != null)
            {
                data = (T)HttpRuntime.Cache[key];
            }
            return data;
        }

        public static void Insert<T>(string key, T data, int minutes)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format("Insert into cache. Key: {0}", key));
                log.Debug(string.Format("Items in cache: {0}", HttpContext.Current.Cache.Count));
            }
            HttpRuntime.Cache.Insert(key, data, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
        }

        #endregion Methods
    }
}