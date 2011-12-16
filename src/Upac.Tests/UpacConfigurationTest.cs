using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Upac.Configuration;
using Upac.Configuration.Elements;

namespace Upac.Tests
{
	[TestFixture]
	public class UpacConfigurationTest
	{
		[Test]
		public void TestUpacConfiguration()
		{
			SettingsSection settings = Upac.Configuration.ConfigurationManager.Settings;
			Assert.IsNotNull(settings);

			BoolElement element = settings.SetWidthOnRichTextEditorViaTemplateAlias;
			Assert.IsNotNull(element);

			Assert.IsTrue(element.Value);

			settings = Upac.Configuration.ConfigurationManager.Settings;
		}
	}
}
