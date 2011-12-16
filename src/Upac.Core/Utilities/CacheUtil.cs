namespace Upac.Core.Utilities
{
    using System;
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

        public static string GetCacheKey(string preKey)
        {
            return string.Concat("SiteRootNodeId::", UpacContext.Current.HomeNode.Id, "::", preKey);
        }

        public static void Insert<T>(string key, T data, int minutes, bool removeOnUmbracoPublish)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(string.Format("Insert into cache. Key: {0}", key));
                log.Debug(string.Format("Items in cache: {0}", HttpContext.Current.Cache.Count));
            }
            CacheDependency cacheDependency = null;
            if (removeOnUmbracoPublish)
            {
                if (HttpContext.Current != null)
                {
                    cacheDependency = new CacheDependency(HttpContext.Current.Server.MapPath("/App_Data/umbraco.config"));
                }
                else
                {
                    string path = HttpRuntime.AppDomainAppPath;
                    log.Debug(string.Format("Could not insert into the cache since we depend on HttpContext.Current. Key: {0}", key));
                    return;
                }
            }
            HttpRuntime.Cache.Insert(key, data, cacheDependency, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
        }

        #endregion Methods
    }
}