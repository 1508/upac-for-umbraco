namespace Upac.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Linq;
    using System.Text;

    public abstract class SearchProvider : ProviderBase
    {
        #region Fields

        private static SearchProvider provider = null;
        private static GenericProviderCollection<SearchProvider> providers = ProviderHelper.LoadProviders<SearchProvider> ("SearchProvider", out provider);

        #endregion Fields

        #region Methods

        public abstract void Search(string q);

        #endregion Methods
    }
}