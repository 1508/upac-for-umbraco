using System.Collections.Specialized;
using System.Net.Mail;

namespace Upac.Core.Mail.Provider
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Web.Configuration;

    using Upac.Core.Configuration.Elements;
    using Upac.Core.Diagnostics;

    public static class MailSenderManager
    {
        public static MailSenderProviderBase DefaultProvider
        {
            get
            {
                MailSenderManager.InitializeProviders();
                return StaticArticles._defaultProvider;
            }
        }

        private static void InitializeProviders()
        {
            AuthorSection section = AuthorSection.Current;

            //Make sure that there is a custom section, and that the providers exist; if not setup, throw an error
            if (section == null || string.IsNullOrEmpty(section.DefaultProvider) || section.Providers.Count == 0)
                throw new ProviderException("The author section hasn't been setup correctly");

            //Instantiate the providers collection to store the collection with
            _providers = new ArticleProviderCollection();

            //Instantiate the providers collection using the helper method defined in the framework
            ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(ArticleProvider));

            //Get the default provider to use
            _defaultProvider = _providers[section.DefaultProvider];

            //If the default provider is null, throw an error
            if (_defaultProvider == null)
                throw new ProviderException("The default provider couldn't be instantiated");
        }

    }
}