namespace Upac.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using SysConfiguration = System.Configuration.Configuration;
    using ConfigurationManager = System.Configuration.ConfigurationManager;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationTest
    {
        #region Fields

        private Upac.Core.Configuration.Elements.Settings settings;

        #endregion Fields

        #region Methods

        [SetUp]
        public void Init()
        {
            settings = Upac.Core.Configuration.ConfigurationManager.UpacSettings;
        }

        [Test]
        public void LoadConfiguration()
        {
            Assert.IsNotNull(settings);
        }

        #endregion Methods
    }
}