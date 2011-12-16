namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.Events.Configuration;

    [TestFixture]
    public class UpacEventsTest
    {
        #region Methods

        [Test]
        public void LoadConfiguration()
        {
            EventsSection settings = Upac.Events.Configuration.ConfigurationManager.Settings;
            Assert.IsNotNull(settings);
            bool enabled = settings.Enabled;
            Assert.IsTrue(enabled);

            EventCollection collection = settings.Events;
            Assert.IsNotNull(collection);
            int count = collection.Count;
            Assert.AreEqual(2, count);

            EventElement element = collection[0];
            Assert.IsTrue(element.Enabled);
            Assert.AreEqual("New", element.TargetEvent);
            Assert.AreEqual("umbraco.cms.businesslogic.web.Document, cms", element.TargetType);
            Assert.AreEqual(element.TargetType + "." + element.TargetEvent, element.Key);

            Assert.AreEqual(2, element.Handlers.Count);
            HandlerElement handler = element.Handlers[0];
            Assert.IsTrue(handler.Enabled);

            element = collection[1];
            Assert.IsFalse(element.Enabled);
        }

        #endregion Methods
    }
}