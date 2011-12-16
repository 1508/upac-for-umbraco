namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.Core.Configuration;

    [TestFixture]
    public class ConfigV2Test
    {
        #region Methods

        [Test]
        public void TestDocumentTypeAliases()
        {
            DocumentTypeAliasesSection documentTypeAliases = ConfigurationManager.DocumentTypeAliases;
            Assert.IsNotNull(documentTypeAliases);

            Assert.AreEqual("ConfigurationContainer", documentTypeAliases.ConfigurationContainer.Value);
            Assert.AreEqual("ConfigurationDateTime", documentTypeAliases.ConfigurationDateTime.Value);
            Assert.AreEqual("ConfigurationDefaultValuesContainer", documentTypeAliases.ConfigurationDefaultValuesContainer.Value);
            Assert.AreEqual("ConfigurationEmail", documentTypeAliases.ConfigurationEmail.Value);
            Assert.AreEqual("ConfigurationGoogle", documentTypeAliases.ConfigurationGoogle.Value);
            Assert.AreEqual("ConfigurationRss", documentTypeAliases.ConfigurationRss.Value);
            Assert.AreEqual("ConfigurationSearchEngineOptimization", documentTypeAliases.ConfigurationSearchEngineOptimization.Value);
            Assert.AreEqual("Redirect", documentTypeAliases.Redirect.Value);
        }

        [Test]
        public void TestPropertyAliases()
        {
            PropertyAliasesSection propertyAliases = ConfigurationManager.PropertyAliases;
            Assert.IsNotNull(propertyAliases);

            Assert.AreEqual("date", propertyAliases.Date.Value);
            Assert.AreEqual("hideFromNavigation", propertyAliases.HideFromNavigation.Value);
        }

        [Test]
        public void TestSettings()
        {
            SettingsSection settings = ConfigurationManager.Settings;
            Assert.IsNotNull(settings);

            Assert.AreEqual("/umbraco/ImageGen.ashx", settings.ImageGenUrl.Value);
            Assert.AreEqual(true, settings.SetWidthOnRichTextEditorViaTemplateAlias.Value);
        }

        #endregion Methods
    }
}