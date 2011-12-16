namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.Providers;

    [TestFixture]
    public class ProviderTest
    {
        #region Methods

        [Test]
        public void TestProviders()
        {
            SearchManager.Search("asd");
            SearchManager.Search("asd");
            SearchManager.Search("asd");
            SearchManager.Search("asd");
            SearchManager.Search("asd");
            SearchManager.Search("asd");
            SearchManager.Search("asd");
        }

        #endregion Methods
    }
}