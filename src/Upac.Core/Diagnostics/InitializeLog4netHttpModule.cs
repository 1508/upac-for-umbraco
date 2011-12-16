namespace Upac.Core.Diagnostics
{
    using System.Web;

    public class InitializeLog4NetHttpModule : IHttpModule
    {
        #region Fields

        static bool initialized = false;
        static object lockObject = new object();

        #endregion Fields

        #region Methods

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (!initialized)
            {
                lock (lockObject)
                {
                    if (!initialized)
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        initialized = true;
                    }
                }
            }
        }

        #endregion Methods
    }
}