namespace Upac.Core.Diagnostics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using log4net;

    public class PagePerformanceHttpModule : IHttpModule
    {
        #region Fields

        static ILog log = LogManager.GetLogger(typeof(PagePerformanceHttpModule));

        private DateTime startTime;

        #endregion Fields

        #region Methods

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(ContextBeginRequest);
            context.EndRequest += new EventHandler(ContextEndRequest);
        }

        void ContextBeginRequest(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
        }

        void ContextEndRequest(object sender, EventArgs e)
        {
            string requestUrl = HttpContext.Current.Request.Path;
            string[] ignorePaths = Upac.Core.Configuration.ConfigurationManager.UpacSettings.PagePerformance.IgnorePaths;
            foreach (string ignorePath in ignorePaths)
            {
                if (requestUrl.StartsWith(ignorePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }

            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                requestUrl = string.Concat(requestUrl, "?", HttpContext.Current.Request.QueryString);
            }

            int duration = (int)DateTime.Now.Subtract(startTime).TotalMilliseconds;
            if (log.IsWarnEnabled && duration > Upac.Core.Configuration.ConfigurationManager.UpacSettings.PagePerformance.WarnWhenMillisecondsIsGreatherThan)
            {
                log.WarnFormat("Duration ms: {0}\nPath: {1}", duration, requestUrl);
            }
            else if (log.IsInfoEnabled && duration > Upac.Core.Configuration.ConfigurationManager.UpacSettings.PagePerformance.InfoWhenMillisecondsIsGreatherThan)
            {
                log.InfoFormat("Duration ms: {0}\nPath: {1}", duration, requestUrl);
            }
        }

        #endregion Methods
    }
}