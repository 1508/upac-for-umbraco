namespace Upac.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Linq;
    using System.Text;

    public class GenericProviderCollection<T> : ProviderCollection
        where T : System.Configuration.Provider.ProviderBase
    {
        #region Indexers

        public new T this[string name]
        {
            get { return (T)base[name]; }
        }

        #endregion Indexers

        #region Methods

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is T))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }

        #endregion Methods
    }
}