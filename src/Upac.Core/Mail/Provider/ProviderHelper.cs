using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Upac.Core.Mail.Provider
{
    class ProviderHelper
    {
        /// <summary>
        /// Helper method for populating a provider collection 
        /// from a Provider section handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GenericProviderCollection<T> LoadProviders<T>(string sectionName, out T provider) where T : ProviderBase
        {
            // Get a reference to the provider section
            ProviderSectionHandler section = (ProviderSectionHandler)WebConfigurationManager.GetSection(sectionName);

            // Load registered providers and point _provider
            // to the default provider
            GenericProviderCollection<T> providers = new GenericProviderCollection<T>();
            ProvidersHelper.InstantiateProviders(section.Providers, providers, typeof(T));

            provider = providers[section.DefaultProvider];
            if (provider == null)
                throw new ProviderException(
                    string.Format(
                          "Unable to load default '{0}' provider",
                                sectionName));

            return providers;
        }
    }
}
