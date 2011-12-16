namespace Upac.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SearchManager
    {
        #region Fields

        public static GenericProviderCollection<SearchProvider> Collection;
        public static SearchProvider Defaultprovider;

        #endregion Fields

        #region Constructors

        static SearchManager()
        {
            Collection = ProviderHelper.LoadProviders("", out Defaultprovider);
        }

        #endregion Constructors

        #region Methods

        public static void Search(string q)
        {
            Defaultprovider.Search(q);
        }

        #endregion Methods
    }
}