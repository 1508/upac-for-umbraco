namespace Upac.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class ProviderHelper
    {
        #region Methods

        /// <summary>
        /// Helper method for populating a provider collection 
        /// from a Provider section handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GenericProviderCollection<T> LoadProviders<T>(string sectionName, out T provider)
            where T : ProviderBase
        {
            // Get a reference to the provider section
            //ProviderSectionHandler section = (ProviderSectionHandler)WebConfigurationManager.GetSection(sectionName);

            // Load registered providers and point _provider
            // to the default provider
            GenericProviderCollection<T> providers = new GenericProviderCollection<T>();
            //ProvidersHelper.InstantiateProviders(section.Providers, providers, typeof(T));

            string xml = "<providers><clear /><add name=\"Default\" type=\"Upac.Providers.ConcretSearchProvider, Upac.Providers\" /></providers>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList nodes = doc.SelectNodes("providers/*");

            foreach (XmlNode node in nodes)
            {
                if (node.LocalName == "clear")
                {
                    providers.Clear();
                }
                else
                {
                    string type = node.SelectSingleNode("@type").InnerText;
                    Type targetType = System.Web.Compilation.BuildManager.GetType(type, true);
                    object obj2 = Activator.CreateInstance(targetType);
                    Type c = obj2.GetType();
                    Type type2 = typeof(T);
                    T provider2 = obj2 as T;
                    if (provider2 == null)
                    {
                        throw new InvalidOperationException("Provider definition must have a 'name' attribute. Xml: " + node.OuterXml);
                    }
                    string attribute = GetAttribute("name", node);
                    if (attribute.Length == 0)
                    {
                        throw new InvalidOperationException("Provider definition must have a 'name' attribute. Xml: " + node.OuterXml);
                    }
                    provider2.Initialize(attribute, GetAttributes(node));
                    providers.Add(provider2);
                }
            }

            provider = providers["Default"];

            return providers;

            //provider = providers[section.DefaultProvider];

            //if (provider == null)
            //    throw new ProviderException(
            //        string.Format(
            //              "Unable to load default '{0}' provider",
            //                    sectionName));
        }

        private static string GetAttribute(string name, XmlNode node)
        {
            if ((node != null) && (node.Attributes != null))
            {
                XmlNode node2 = node.Attributes[name];
                if (node2 != null)
                {
                    return node2.Value;
                }
            }
            return string.Empty;
        }

        private static NameValueCollection GetAttributes(XmlNode node)
        {
            NameValueCollection values = new NameValueCollection();
            if ((node != null) && (node.Attributes != null))
            {
                foreach (XmlNode node2 in node.Attributes)
                {
                    values.Add(node2.Name, node2.Value);
                }
            }
            return values;
        }

        //
        private static TCollection GetProviders<TProvider, TCollection>(List<XmlNode> nodes)
            where TProvider : ProviderBase
            where TCollection : ProviderCollection, new()
        {
            TCollection local = Activator.CreateInstance<TCollection>();
            foreach (XmlNode node in nodes)
            {
                if (node.LocalName == "clear")
                {
                    local.Clear();
                }
                else
                {
                    object obj2 = new ConcretSearchProvider();
                    Type c = obj2.GetType();
                    Type type2 = typeof(TProvider);
                    TProvider provider = obj2 as TProvider;
                    if (provider == null)
                    {
                        throw new InvalidOperationException("Provider definition must have a 'name' attribute. Xml: " + node.OuterXml);
                    }
                    string attribute = GetAttribute("name", node);
                    if (attribute.Length == 0)
                    {
                        throw new InvalidOperationException("Provider definition must have a 'name' attribute. Xml: " + node.OuterXml);
                    }
                    provider.Initialize(attribute, GetAttributes(node));
                    local.Add(provider);
                }
            }
            return local;
        }

        #endregion Methods
    }
}